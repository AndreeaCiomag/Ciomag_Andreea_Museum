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
    public class ExhibitsController : Controller
    {
        private readonly MuseumContext _context;

        public ExhibitsController(MuseumContext context)
        {
            _context = context;
        }

        // GET: Exhibits
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewData["MovementSortParm"] = String.IsNullOrEmpty(sortOrder) ? "mov_desc" : "";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var exhibits = from e in _context.Exhibits.Include(i => i.Exhibition).Include(i => i.Artist)
                           select e;
            if(!String.IsNullOrEmpty(searchString))
            {
                exhibits = exhibits.Where(e => e.Name.ToLower().Contains(searchString.ToLower()));
            }
            switch(sortOrder)
            {
                case "type_desc":
                    exhibits = exhibits.OrderByDescending(e => e.Type);
                    break;
                case "mov_desc":
                    exhibits = exhibits.OrderByDescending(e => e.Movement);
                    break;
                default:
                    exhibits = exhibits.OrderByDescending(e => e.ID);
                    break;
            }
            int pageSize = 5;
            return View(await PaginatedList<Exhibit>.CreateAsync(exhibits.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Exhibits/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Exhibits == null)
            {
                return NotFound();
            }

            var exhibit = await _context.Exhibits
                .Include(e => e.Artist)
                .Include(e => e.Exhibition)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (exhibit == null)
            {
                return NotFound();
            }

            return View(exhibit);
        }

        // GET: Exhibits/Create
        public IActionResult Create()
        {
            ViewData["ArtistID"] = new SelectList((from a in _context.Artists
                                                   select new
                                                   {
                                                       ID = a.ID,
                                                       Name = a.Name + " - " + a.Specialization
                                                   }), "ID", "Name");
            ViewData["ExhibitionID"] = new SelectList(_context.Exhibitions, "ID", "Name");
            return View();
        }

        // POST: Exhibits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Type,Movement,ExhibitionID,ArtistID")] Exhibit exhibit)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                _context.Add(exhibit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                //}
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ID", "Name", exhibit.ArtistID);
            ViewData["ExhibitionID"] = new SelectList(_context.Exhibitions, "ID", "Name", exhibit.ExhibitionID);
            return View(exhibit);
        }

        // GET: Exhibits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Exhibits == null)
            {
                return NotFound();
            }

            var exhibit = await _context.Exhibits.FindAsync(id);
            if (exhibit == null)
            {
                return NotFound();
            }
            ViewData["ArtistID"] = new SelectList((from a in _context.Artists
                                                   select new
                                                   {
                                                       ID = a.ID,
                                                       Name = a.Name + " - " + a.Specialization
                                                   }), "ID", "Name", exhibit.ArtistID);
            ViewData["ExhibitionID"] = new SelectList(_context.Exhibitions, "ID", "Name", exhibit.ExhibitionID);
            return View(exhibit);
        }

        // POST: Exhibits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Type,Movement,ExhibitionID,ArtistID")] Exhibit exhibit)
        {
            if (id != exhibit.ID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exhibit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExhibitExists(exhibit.ID))
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
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ID", "Name", exhibit.ArtistID);
            ViewData["ExhibitionID"] = new SelectList(_context.Exhibitions, "ID", "Name", exhibit.ExhibitionID);
            return View(exhibit);
        }


        // GET: Exhibits/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Exhibits == null)
            {
                return NotFound();
            }

            var exhibit = await _context.Exhibits
                .Include(e => e.Artist)
                .Include(e => e.Exhibition)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (exhibit == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }
            return View(exhibit);
        }

        // POST: Exhibits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Exhibits == null)
            {
                return Problem("Entity set 'MuseumContext.Exhibits'  is null.");
            }
            var exhibit = await _context.Exhibits.FindAsync(id);
            if (exhibit == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Exhibits.Remove(exhibit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ExhibitExists(int id)
        {
          return (_context.Exhibits?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
