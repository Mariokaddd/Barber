using Barber.Data;
using Barber.Data.enums;
using Barber.Data.models;
using Barber.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var favorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Hairstyle)
                .ToListAsync();

            return View(favorites);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int hairstyleId, string gender)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                // Either redirect to login
                return Challenge();
                // Or show bad request
                // return BadRequest("You must be logged in to favorite.");
            }

            bool alreadyExists = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.HairstyleId == hairstyleId);

            if (!alreadyExists)
            {
                _context.Favorites.Add(new Favorite
                {
                    UserId = userId,
                    HairstyleId = hairstyleId
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ByGender", "Hairstyles", new { gender });
        }


        [HttpPost]
        public async Task<IActionResult> Remove(int hairstyleId)
        {
            var userId = _userManager.GetUserId(User);

            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.HairstyleId == hairstyleId && f.UserId == userId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
