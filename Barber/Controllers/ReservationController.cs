using Barber.Data;
using Barber.Data.models;
using Barber.Models;
using Barber.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ScheduleService _svc;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _um;

        public ReservationController(ScheduleService svc,
                                     ApplicationDbContext db,
                                     UserManager<ApplicationUser> um)
        {
            _svc = svc; _db = db; _um = um;
        }

        // 1. Calendar
        public async Task<IActionResult> Calendar(int? year, int? month, int? hairstyleId)
        {
            var today = DateTime.Today;
            int y = year ?? today.Year;
            int m = month ?? today.Month;

            // Ако искат да върнат назад преди сегашния месец — принудително оставяме на днешния
            if (new DateTime(y, m, 1) < new DateTime(today.Year, today.Month, 1))
            {
                y = today.Year;
                m = today.Month;
            }

            var monthStart = new DateTime(y, m, 1);
            var vm = new CalendarViewModel { MonthStart = monthStart };

            int daysInMonth = DateTime.DaysInMonth(y, m);
            for (int d = 1; d <= daysInMonth; d++)
            {
                var date = new DateTime(y, m, d);
                // блокирани минали дни
                if (date < today)
                {
                    vm.Days.Add(new CalendarDayViewModel
                    {
                        Date = date,
                        HasSchedule = false,
                        FreeSlotCount = 0
                    });
                    continue;
                }

                var slots = await _svc.GetAvailableSlotsAsync(date);
                vm.Days.Add(new CalendarDayViewModel
                {
                    Date = date,
                    HasSchedule = slots.Any(),
                    FreeSlotCount = slots.Count
                });
            }

            ViewBag.HairstyleId = hairstyleId ?? 0;
            return View(vm);
        }


        public async Task<IActionResult> Slots(DateTime date, int hairstyleId)
        {
            var slots = await _svc.GetAvailableSlotsAsync(date);
            return View(new SlotsViewModel { Date = date, AvailableSlots = slots, HairstyleId = hairstyleId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Confirm(DateTime date, TimeSpan time, int hairstyleId)
        {
            if (await _db.Appointments.AnyAsync(a => a.Date == date && a.Time == time))
            {
                TempData["Error"] = "Този час вече е зает.";
                return RedirectToAction("Slots", new { date, hairstyleId });
            }

            var user = await _um.GetUserAsync(User);
            _db.Appointments.Add(new Appointment
            {
                Date = date,
                Time = time,
                HairstyleId = hairstyleId,
                UserId = user.Id
            });
            await _db.SaveChangesAsync();
            return RedirectToAction("MyBookings");
        }

        // 4. My Bookings
        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            var uid = _um.GetUserId(User);
            var list = await _db.Appointments
                                .Include(a => a.Hairstyle)
                                .Where(a => a.UserId == uid)
                                .OrderBy(a => a.Date).ThenBy(a => a.Time)
                                .ToListAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> SlotsAjax(DateTime date, int hairstyleId)
        {
            var slots = await _svc.GetAvailableSlotsAsync(date);
            var vm = new SlotsViewModel
            {
                Date = date,
                AvailableSlots = slots,
                HairstyleId = hairstyleId
            };
            return PartialView("_SlotsPartial", vm);
        }

    }

}
