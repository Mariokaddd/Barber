using Barber.Data;
using Barber.Data.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ScheduleController(ApplicationDbContext db) => _db = db;

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(DateTime date, TimeSpan time)
        {
            if (!ModelState.IsValid) return View();
            _db.WorkingSchedules.Add(new WorkingSchedule { Date = date, Time = time });
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            var list = await _db.WorkingSchedules
                                .OrderBy(ws => ws.Date).ThenBy(ws => ws.Time)
                                .ToListAsync();
            return View(list);
        }
    }

}
