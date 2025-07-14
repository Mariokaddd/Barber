using Barber.Data;
using Barber.Data.models;
using Barber.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Route("Reservation")]
public class ReservationController : Controller
{
    private readonly ApplicationDbContext _db;

    public ReservationController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /Reservation/Calendar?year=2025&month=7
    [HttpGet("Calendar")]
    public async Task<IActionResult> Calendar(int? year, int? month)
    {
        var today = DateTime.Today;
        int y = year ?? today.Year;
        int m = month ?? today.Month;

        // Не позволявай да се зареждат минали месеци
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

    // GET /Reservation/SlotsAjax?date=2025-07-15
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

    // GET /Reservation/Confirm?slotId=123
    [HttpGet("Confirm")]
    [Authorize]
    public async Task<IActionResult> Confirm(int slotId)
    {
        var slot = await _db.WorkingSchedules.FindAsync(slotId);
        if (slot == null || !slot.IsAvailable)
        {
            TempData["Error"] = "Този час вече е зает или не съществува.";
            return RedirectToAction("Calendar");
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Forbid();

        var user = await _db.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.FirstName, u.LastName, u.PhoneNumber })
            .FirstOrDefaultAsync();
        if (user == null)
            return Forbid();

        var vm = new ReservationFormViewModel
        {
            Date = slot.Date,
            Hour = slot.Time.ToString(@"hh\:mm"),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.PhoneNumber
        };

        ViewBag.SlotId = slotId;
        return View(vm);
    }

    [HttpPost("Confirm")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(int SlotId, ReservationFormViewModel model)
    {
        var slot = await _db.WorkingSchedules.FindAsync(SlotId);
        if (slot == null || !slot.IsAvailable)
        {
            TempData["Error"] = "Този час вече е зает или не съществува.";
            return RedirectToAction("Calendar");
        }

        slot.IsAvailable = false;

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Forbid();

        _db.Appointments.Add(new Appointment
        {
            Date = slot.Date,
            Time = slot.Time,
            UserId = userId,
            // Ако няма прически, няма HairstyleId тук
        });

        slot.IsAvailable = false;

        await _db.SaveChangesAsync();
        return RedirectToAction("MyBookings");
    }

    // GET /Reservation/MyBookings
    [HttpGet("MyBookings")]
    [Authorize]
    public async Task<IActionResult> MyBookings()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Forbid();

        var list = await _db.Appointments
         .Where(a => a.UserId == userId)
         .Select(a => new BookingViewModel
         {
             Date = a.Date,
             Time = a.Time,
             FirstName = a.User != null ? a.User.FirstName ?? "" : "",
             LastName = a.User != null ? a.User.LastName ?? "" : "",
             Phone = a.User != null ? a.User.PhoneNumber ?? "" : "",
             IsBooked = a.IsBooked
         })
         .OrderBy(a => a.Date)
         .ThenBy(a => a.Time)
         .ToListAsync();

        return View(list);



        return View(list);
    }

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
