using Barber.Data;
using Barber.Data.enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Barber.Controllers
{
    public class HairstylesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HairstylesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. ChooseGender
        public IActionResult ChooseGender()
        {
            return View();
        }

        // 2. ByGender
        public IActionResult ByGender(string gender)
        {
            if (!Enum.TryParse<Gender>(gender, true, out var parsedGender))
            {
                return BadRequest("Invalid gender.");
            }

            var hairstyles = _context.Hairstyles
                .Where(h => h.Gender == parsedGender)
                .ToList();

            return View(hairstyles);
        }


        // 3. Details
        public IActionResult Details(int id)
        {
            var hairstyle = _context.Hairstyles.FirstOrDefault(h => h.Id == id);
            if (hairstyle == null) return NotFound();

            return View(hairstyle);
        }
    }

}
