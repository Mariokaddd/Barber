using Barber.Data;
using Barber.Data.models;
using Barber.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Взимаме само наличните слотове
            var workingSlots = await _context.WorkingSchedules
                .Where(ws => ws.IsAvailable)
                .OrderBy(ws => ws.Date).ThenBy(ws => ws.Time)
                .ToListAsync();

            // Преобразуваме към ScheduleFormViewModel
            var viewModelList = workingSlots
                .Select(ws => new ScheduleFormViewModel
                {
                    Date = ws.Date,
                    Hour = ws.Time.Hours, // вземаме само часа от TimeSpan
                    Description = null,          // тук можеш да вкараш доп. поле, ако имаш
                    IsAvailable = ws.IsAvailable
                })
                .ToList();

            return View(viewModelList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ScheduleFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // конвертиране на int час в TimeSpan
            var ts = TimeSpan.FromHours(vm.Hour.Value);

            // проверка за дублиране
            bool exists = _context.WorkingSchedules
                .Any(ws => ws.Date == vm.Date && ws.Time == ts);

            if (exists)
            {
                ModelState.AddModelError("", "Този час вече е въведен като свободен.");
                return View(vm);
            }

            var slot = new WorkingSchedule
            {
                Date = vm.Date,
                Time = ts,
                IsAvailable = true
            };

            _context.WorkingSchedules.Add(slot);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
