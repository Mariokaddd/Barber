using Barber.Data;
using Microsoft.EntityFrameworkCore;

namespace Barber.Service
{
    public class ScheduleService
    {
        private readonly ApplicationDbContext _db;
        public ScheduleService(ApplicationDbContext db) => _db = db;

        public async Task<List<TimeSpan>> GetAvailableSlotsAsync(DateTime date)
        {
            var all = await _db.WorkingSchedules
                               .Where(ws => ws.Date == date.Date)
                               .Select(ws => ws.Time)
                               .ToListAsync();

            var taken = await _db.Appointments
                                 .Where(a => a.Date == date.Date)
                                 .Select(a => a.Time)
                                 .ToListAsync();

            return all.Except(taken).OrderBy(t => t).ToList();
        }
    }
}
