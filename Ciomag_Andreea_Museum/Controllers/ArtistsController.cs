using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciomag_Andreea_Museum.Data;
using Ciomag_Andreea_Museum.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ciomag_Andreea_Museum.Controllers
{
    [Authorize(Policy = "PurchasingEmployee")]
    public class ArtistsController : Controller
    {
        private readonly MuseumContext _context;

        public ArtistsController(MuseumContext context)
        {
            _context = context;
        }

        // GET: Artists
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewData["SpecSortParm"] = String.IsNullOrEmpty(sortOrder) ? "spec_desc" : "";
            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var artists = from a in _context.Artists
                          select a;
            if(!String.IsNullOrEmpty(searchString))
            {
                artists = artists.Where(a => a.Name.ToLower().Contains(searchString.ToLower()));
            }
            switch(sortOrder)
            {
                case "name_asc":
                    artists = artists.OrderBy(a => a.Name);
                    break;
                case "spec_desc":
                    artists = artists.OrderByDescending(a => a.Specialization);
                    break;
                default:
                    artists = artists.OrderByDescending(a => a.ID);
                    break;
            }
            int pageSize = 5;
            return View(await PaginatedList<Artist>.CreateAsync(artists.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Artists/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .Include(s => s.Exhibits)
                .ThenInclude(e => e.Exhibition)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            //var artist = await _context.Artists.FirstOrDefaultAsync(m => m.ID == id);

            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Specialization")] Artist artist)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                //}
            }
            catch(DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes." + "Try again, and if the problem persists ");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Specialization")] Artist artist)
        {
            if (id != artist.ID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (artist == null)
            {
                return NotFound();
            }
            if(saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ArtistExists(int id)
        {
          return (_context.Artists?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
