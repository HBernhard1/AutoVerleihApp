using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoVerleih.Data;
using AutoVerleih.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace AutoVerleih.Controllers
{
    [Authorize]
    public class AutosController : Controller
    {
        private readonly DBProjectContext _context;

        public AutosController(DBProjectContext context)
        {
            _context = context;
        }
      
        // GET: Autos
        public async Task<IActionResult> Index(string currentFilter, string searchString)
        {

            var autos = new List<Autos>();
            if (!String.IsNullOrEmpty(searchString))
            {
                autos = await _context.Autos.Where(s => s.Type.Contains(searchString)).ToListAsync();
            }
            else
            {
                autos = await _context.Autos.ToListAsync();
            }
            foreach(var auto in autos)
            {
                var xx = _context.Verleih.Where(a => a.AutoId == auto.AutoId).Select(b => b.KM_gefahren).Sum();
                auto.KM_gesamt = auto.KM_gesamt + xx;
            }
            return View(autos.ToList());
        }

        // GET: Autos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autos = await _context.Autos
                .FirstOrDefaultAsync(m => m.AutoId == id);
            if (autos == null)
            {
                return NotFound();
            }

            return View(autos);
        }

        // GET: Autos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutoId,Marke,Type,Farbe,MietPreis_HH,MietPreis_TG,MietPreis_WE,vermietet,Bild")] Autos autos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autos);
        }

        // GET: Autos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autos = await _context.Autos.FindAsync(id);
            if (autos == null)
            {
                return NotFound();
            }
            return View(autos);
        }

        // POST: Autos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutoId,Marke,Type,Farbe,MietPreis_HH,MietPreis_TG,MietPreis_WE,vermietet,Bild")] Autos autos)
        {
            if (id != autos.AutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutosExists(autos.AutoId))
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
            return View(autos);
        }

        // GET: Autos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autos = await _context.Autos.FirstOrDefaultAsync(m => m.AutoId == id);
            if (autos == null)
            {
                return NotFound();
            }

            var verleih = await _context.Verleih.FirstOrDefaultAsync(r => r.AutoId == id);
            if (verleih != null)
            {
                ViewBag.ErrorMessage = "Löschen nicht mgölich, für dieses Auto sind im Verleih Datnsätze vorhanden!";
            }

            return View(autos);
        }

        // POST: Autos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autos = await _context.Autos.FindAsync(id);
            _context.Autos.Remove(autos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutosExists(int id)
        {
            return _context.Autos.Any(e => e.AutoId == id);
        }
    }
}
