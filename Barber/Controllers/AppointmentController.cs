using Barber.Data;
using Barber.Data.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AvailableSlots()
        {
            var slots = await _context.WorkingSchedules
                .Where(w => w.Date >= DateTime.Today && w.IsAvailable)
                .OrderBy(w => w.Date).ThenBy(w => w.Time)
                .ToListAsync();

            return View(slots);
        }

        [HttpPost]
        public async Task<IActionResult> BookSlot(int id, int hairstyleId)
        {
            var slot = await _context.WorkingSchedules.FindAsync(id);
            if (slot == null || !slot.IsAvailable)
            {
                TempData["Error"] = "Слотът вече е зает.";
                return RedirectToAction(nameof(AvailableSlots));
            }

            var user = await _userManager.GetUserAsync(User);

            var appointment = new Appointment
            {
                UserId = user.Id,
                Date = slot.Date,
                Time = slot.Time,
                HairstyleId = hairstyleId,
                IsBooked = true
            };

            slot.IsAvailable = false;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Успешно запазихте час!";
            return RedirectToAction("MyAppointments");
        }
    }

}
