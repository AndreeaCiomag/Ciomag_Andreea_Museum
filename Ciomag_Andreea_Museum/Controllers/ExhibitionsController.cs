using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciomag_Andreea_Museum.Data;
using Ciomag_Andreea_Museum.Models;
using Ciomag_Andreea_Museum.Models.MuseumViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Ciomag_Andreea_Museum.Controllers
{
    [Authorize(Policy = "MarketingManager")]
    public class ExhibitionsController : Controller
    {
        private readonly MuseumContext _context;

        public ExhibitionsController(MuseumContext context)
        {
            _context = context;
        }

        // GET: Exhibitions
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentFilter"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewData["ThemeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "theme_desc" : "";
            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var exhibitions = from e in _context.Exhibitions
                              .Include(e => e.Galleries)
                              select e;
            if(!String.IsNullOrEmpty(searchString))
            {
                exhibitions = exhibitions.Where(e => e.Name.ToLower().Contains(searchString.ToLower()));
            }
            switch (sortOrder)
            {
                case "name_asc":
                    exhibitions = exhibitions.OrderBy(e => e.Name);
                    break;
                case "theme_desc":
                    exhibitions = exhibitions.OrderByDescending(e => e.Theme);
                    break;
                default:
                    exhibitions = exhibitions.OrderByDescending(e => e.ID);
                    break;
            }
            int pageSize = 5;
            return View(await PaginatedList<Exhibition>.CreateAsync(exhibitions.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Exhibitions/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Exhibitions == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions
                .Include(e => e.Galleries)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (exhibition == null)
            {
                return NotFound();
            }

            return View(exhibition);
        }

        // GET: Exhibitions/Create
        public IActionResult Create()
        {
            ViewData["GalleryID"] = new SelectList(_context.Galleries, "ID", "Name");
            return View();
        }

        // POST: Exhibitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Theme,GalleryID,StartDate,FinishDate")] Exhibition exhibition)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                _context.Add(exhibition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                //}
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }
            ViewData["GalleryID"] = new SelectList(_context.Galleries, "ID", "Name", exhibition.GalleryID);
            return View(exhibition);
        }

        // GET: Exhibitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Exhibitions == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition == null)
            {
                return NotFound();
            }
            ViewData["GalleryID"] = new SelectList(_context.Galleries, "ID", "Name", exhibition.GalleryID);
            return View(exhibition);
        }

        // POST: Exhibitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Theme,GalleryID,StartDate,FinishDate")] Exhibition exhibition)
        {
            if (id != exhibition.ID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exhibition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExhibitionExists(exhibition.ID))
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
            ViewData["GalleryID"] = new SelectList(_context.Galleries, "ID", "Name", exhibition.GalleryID);
            return View(exhibition);
        }

        // GET: Exhibitions/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions
                .Include(e => e.Galleries)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (exhibition == null)
            {
                return NotFound();
            }
            if(saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }
            return View(exhibition);
        }

        // POST: Exhibitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Exhibitions == null)
            {
                return Problem("Entity set 'MuseumContext.Exhibitions'  is null.");
            }
            var exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Exhibitions.Remove(exhibition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException /*ex*/)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ExhibitionExists(int id)
        {
          return (_context.Exhibitions?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
