using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoVerleih.Models;
using X.PagedList;
using AutoVerleih.Data;
using AutoVerleih.Filter;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace AutoVerleih.Controllers
{
    [Authorize]
    public class KundenController : Controller
    {
        private readonly DBProjectContext _context;
        
        public KundenController(DBProjectContext context)
        {
            _context = context;
        }
         
        // GET: Kunden
        public async Task<IActionResult> Index(string button, string currentFilter, string searchString, string currentScreen, int? page, int anzLine)
        {
            if (anzLine == 0) anzLine = DefaultFilter.AnzLine;

            if (!HttpContext.Request.Cookies.ContainsKey("first_request"))
            {

                HttpContext.Response.Cookies.Append("first_request", DateTime.Now.ToString());
//                return Content("Welcome, new visitor!");
            }
            else
            {
                DateTime firstRequest = DateTime.Parse(HttpContext.Request.Cookies["first_request"]);
            }

            if (!String.IsNullOrEmpty(currentFilter) && String.IsNullOrEmpty(searchString)) searchString = currentFilter;
            IEnumerable<Kunden> kunden = Enumerable.Empty<Kunden>();

            if (!String.IsNullOrEmpty(searchString))
            {
                kunden = await _context.Kunde.Where(s => s.Name.Contains(searchString)).ToListAsync();       
            }
            else
            {
                searchString = currentFilter;
                kunden = await _context.Kunde.OrderByDescending(s => s.KundenId).Take(10).ToListAsync();
            }

            //            var xx = kunden.FirstOrDefault(a => a.Verleihs.Count > 0);

            ViewBag.CurrentFilter = searchString;
            ViewBag.AnzLine = anzLine;

            int pageSize = anzLine;
            int pageNumber = (page ?? 1);
            return View(kunden.ToPagedList(pageNumber, pageSize));
        }

        // GET: Kunden/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kunde = await _context.Kunde
                .FirstOrDefaultAsync(m => m.KundenId == id);
            if (kunde == null)
            {
                return NotFound();
            }

            return View(kunde);
        }

        // GET: Kunden/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kunden/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KundenId,Name,Plz,Ort,Strasse")] Kunden kunde)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kunde);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kunde);
        }

        // GET: Kunden/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kunde = await _context.Kunde.FindAsync(id);
            if (kunde == null)
            {
                return NotFound();
            }
            return View(kunde);
        }

        // POST: Kunden/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KundenId,Name,Plz,Ort,Strasse")] Kunden kunde)
        {
            var xx = DefaultFilter.DT_From;
            
            if (id != kunde.KundenId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kunde);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KundeExists(kunde.KundenId))
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
            return View(kunde);
        }

        // GET: Kunden/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var kunde = await _context.Kunde.FirstOrDefaultAsync(m => m.KundenId == id);
            if (kunde == null)
            {
                return NotFound();
            }

            var verleih = await _context.Verleih.FirstOrDefaultAsync(r => r.KundenId == id);
            if (verleih != null)
            {
                ViewBag.ErrorMessage = "Löschen nicht möglich, für diesen Kunden sind im Verleih Datnsätze vorhanden!";
            }

            return View(kunde);
        }

        // POST: Kunden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kunde = await _context.Kunde.FindAsync(id);
            _context.Kunde.Remove(kunde);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KundeExists(int id)
        {
            return _context.Kunde.Any(e => e.KundenId == id);
        }
    }
}
