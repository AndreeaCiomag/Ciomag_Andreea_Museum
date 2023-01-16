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
    [Authorize]
    public class VisitsController : Controller
    {
        private readonly MuseumContext _context;

        public VisitsController(MuseumContext context)
        {
            _context = context;
        }

        // GET: Visits
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var visits = from v in _context.Visits
                         .Include(v => v.Client)
                         .Include(v => v.Gallery)
                         select v;
            visits = visits.OrderByDescending(v => v.ID);
            int pageSize = 5;
            //var museumContext = _context.Visits.Include(v => v.Client).Include(v => v.Gallery);
            //return View(await museumContext.ToListAsync());
            return View(await PaginatedList<Visit>.CreateAsync(visits.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Visits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Visits == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.Client)
                .Include(v => v.Gallery)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // GET: Visits/Create
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList((from c in _context.Clients
                                                   select new
                                                   {
                                                       ID = c.ID,
                                                       FullName = c.FirstName + " " + c.LastName
                                                   }), "ID", "FullName");
            ViewData["GalleryID"] = new SelectList(_context.Galleries, "ID", "Name");
            return View();
        }

        // POST: Visits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientID,GalleryID,VisitDate")] Visit visit)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                _context.Add(visit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                //}
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "LastName", visit.ClientID);
            ViewData["GalleryID"] = new SelectList(_context.Galleries, "ID", "Name", visit.GalleryID);
            return View(visit);
        }

        // GET: Visits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Visits == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList((from c in _context.Clients
                                                   select new
                                                   {
                                                       ID = c.ID,
                                                       FullName = c.FirstName + " " + c.LastName
                                                   }), "ID", "FullName", visit.ClientID);
            ViewData["GalleryID"] = new SelectList(_context.Galleries, "ID", "Name", visit.GalleryID);
            return View(visit);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClientID,GalleryID,VisitDate")] Visit visit)
        {
            if (id != visit.ID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitExists(visit.ID))
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
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "LastName", visit.ClientID);
            ViewData["GalleryID"] = new SelectList(_context.Galleries, "ID", "Name", visit.GalleryID);
            return View(visit);
        }

        // GET: Visits/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.Client)
                .Include(v => v.Gallery)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (visit == null)
            {
                return NotFound();
            }
            if(saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }
            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Visits == null)
            {
                return Problem("Entity set 'MuseumContext.Visits'  is null.");
            }
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Visits.Remove(visit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool VisitExists(int id)
        {
          return (_context.Visits?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
