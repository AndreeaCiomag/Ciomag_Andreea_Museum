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
    public class ClientsController : Controller
    {
        private readonly MuseumContext _context;

        public ClientsController(MuseumContext context)
        {
            _context = context;
        }

        // GET: Clients
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LNSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ln_asc" : "";
            ViewData["TypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var clients = from c in _context.Clients
                          select c;
            if(!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c => c.FirstName.ToLower().Contains(searchString.ToLower()) || c.LastName.ToLower().Contains(searchString.ToLower()));
            }
            switch(sortOrder)
            {
                case "ln_asc":
                    clients = clients.OrderBy(c => c.LastName);
                    break;
                case "type_desc":
                    clients = clients.OrderByDescending(c => c.Type);
                    break;
                default:
                    clients = clients.OrderByDescending(c => c.ID);
                    break;
            }
            int pageSize = 5;
            return View(await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Clients/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(s => s.Visits)
                .ThenInclude(e => e.Gallery)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,BirthDate,Type")] Client client)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                //}
            }
            catch(DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,BirthDate,Type")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }
            if(saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ClientExists(int id)
        {
          return (_context.Clients?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
