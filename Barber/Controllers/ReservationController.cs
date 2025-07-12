using Barber.Data;
using Barber.Data.models;
using Barber.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers
{
    [Route("Reservation")]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReservationController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /Reservation/Calendar
        [HttpGet("Calendar")]
        public async Task<IActionResult> Calendar(int? year, int? month)
        {
            var today = DateTime.Today;
            int y = year ?? today.Year;
            int m = month ?? today.Month;

            if (new DateTime(y, m, 1) < new DateTime(today.Year, today.Month, 1))
            {
                y = today.Year;
                m = today.Month;
            }

            var monthStart = new DateTime(y, m, 1);
            var vm = new CalendarViewModel
            {
                MonthStart = monthStart,
                Days = new List<CalendarDayViewModel>()
            };

            int daysInMonth = DateTime.DaysInMonth(y, m);
            for (int d = 1; d <= daysInMonth; d++)
            {
                var date = monthStart.AddDays(d - 1);
                int freeCount = 0;
                if (date >= today)
                {
                    freeCount = await _db.WorkingSchedules
                        .CountAsync(ws => ws.Date == date && ws.IsAvailable);
                }

                vm.Days.Add(new CalendarDayViewModel
                {
                    Date = date,
                    FreeSlotCount = freeCount
                });
            }

            return View(vm);
        }

        // GET /Reservation/SlotsAjax?date=yyyy-MM-dd
        [HttpGet("SlotsAjax")]
        public async Task<IActionResult> SlotsAjax(DateTime date)
        {
            if (date == default)
                return BadRequest("Невалидна дата");

            var slots = await _db.WorkingSchedules
                .Where(ws => ws.Date == date && ws.IsAvailable)
                .OrderBy(ws => ws.Time)
                .ToListAsync();

            return PartialView("_SlotsPartial", slots);
        }

        // POST /Reservation/Confirm
        [HttpPost("Confirm")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int slotId)
        {
            var slot = await _db.WorkingSchedules.FindAsync(slotId);
            if (slot == null || !slot.IsAvailable)
            {
                TempData["Error"] = "Този час вече е зает или не съществува.";
                return RedirectToAction("Calendar");
            }

            // Отметни слота като зает
            slot.IsAvailable = false;

            // Създай резервация
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            _db.Appointments.Add(new Appointment
            {
                Date = slot.Date,
                Time = slot.Time,
                UserId = userId!
            });

            await _db.SaveChangesAsync();
            return RedirectToAction("MyBookings");
        }

        // GET /Reservation/MyBookings
        [HttpGet("MyBookings")]
        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            var uid = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var list = await _db.Appointments
                .Include(a => a.Hairstyle)
                .Where(a => a.UserId == uid)
                .OrderBy(a => a.Date).ThenBy(a => a.Time)
                .ToListAsync();
            return View(list);
        }

        // GET: /Reservation/BookSlotForm
        public IActionResult BookSlotForm(DateTime date, string hour)
        {
            var model = new ReservationFormViewModel
            {
                Date = date,
                Hour = hour
            };
            return PartialView("_BookSlotForm", model);
        }


    }
}
