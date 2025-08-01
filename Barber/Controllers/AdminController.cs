using Barber.Data;
using Barber.Data.models;
using Barber.Models;
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
        public async Task<IActionResult> AllAppointments(string? name, string? phoneNumber, DateTime? date)
        {
            var query = _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Hairstyle) // ако искаш да имаш данните за прическата
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(a => a.User.FirstName.Contains(name) || a.User.LastName.Contains(name));

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                query = query.Where(a => a.User.PhoneNumber.Contains(phoneNumber));

            if (date.HasValue)
                query = query.Where(a => a.Date == date.Value.Date);

            var result = await query
                .OrderBy(a => a.Date).ThenBy(a => a.Time)
                .ToListAsync();

            var model = new AppointmentFilterViewModel
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Date = date,
                Results = result
            };

            return View(model);
        }

    }
}
