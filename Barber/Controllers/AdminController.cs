using Barber.Data;
using Barber.Data.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CreateSchedule() => View();

        [HttpPost]
        public async Task<IActionResult> CreateSchedule(AdminSchedule model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.AdminSchedules.Add(model);
            await _context.SaveChangesAsync();

            // Генериране на WorkingSchedule
            var slots = new List<WorkingSchedule>();
            for (var time = model.StartTime; time < model.EndTime; time += model.SlotDuration)
            {
                // Избягваме дублирани
                bool exists = _context.WorkingSchedules.Any(w =>
                    w.Date == model.Date && w.Time == time);
                if (!exists)
                {
                    slots.Add(new WorkingSchedule
                    {
                        Date = model.Date,
                        Time = time,
                        IsAvailable = true
                    });
                }
            }

            _context.WorkingSchedules.AddRange(slots);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Графикът е създаден!";
            return RedirectToAction("ListWorkingSlots");
        }

        public async Task<IActionResult> ListWorkingSlots()
        {
            var slots = await _context.WorkingSchedules
                .OrderBy(w => w.Date).ThenBy(w => w.Time).ToListAsync();

            return View(slots);
        }

        [HttpGet]
        public async Task<IActionResult> AllAppointments()
        {
            var list = await _context.Appointments
                .Include(a => a.User)
                .OrderBy(a => a.Date)
                .ThenBy(a => a.Time)
                .ToListAsync();

            return View(list);
        }

    }
}
