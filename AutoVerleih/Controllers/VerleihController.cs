using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoVerleih.Data;
using AutoVerleih.Models;
using System.Collections;
using AutoVerleih.ViewModels;
using AutoVerleih.Filter;
using Microsoft.AspNetCore.Authorization;

namespace AutoVerleih.Controllers
{
    [Authorize]
    public class VerleihController : Controller
    {
        List<Autos> au = new List<Autos>();
        List<Kunden> ku = new List<Kunden>();
        List<Verleih> vl = new List<Verleih>();

        private readonly DBProjectContext _context;

        public VerleihController(DBProjectContext context)
        {
            _context = context;
        }

        // GET: Verleih
        public async Task<IActionResult> Index(string DT_From, bool IsOnlyShowRentCars)
        {
            if (Request.QueryString.HasValue)
            {
                DefaultFilter.DT_From = Convert.ToDateTime(DT_From);
                DefaultFilter.IsOnlyShowRentCars = IsOnlyShowRentCars;
            }
            if (DefaultFilter.IsOnlyShowRentCars)
            {
                vl = await _context.Verleih.Where(a => a.DT_Von >= DefaultFilter.DT_From && a.DT_Rueckgabe == null).ToListAsync();
            }
            else
            {
                vl = await _context.Verleih.Where(a => a.DT_Von >= DefaultFilter.DT_From).ToListAsync();
            }
            au = await _context.Autos.ToListAsync();
            ku = await _context.Kunde.ToListAsync();
            ViewBag.AutosVerliehen = vl.Where(a => a.DT_Rueckgabe == null).Count();

            IEnumerable verleihView = from v in vl
                                      join kui in ku on v.KundenId equals kui.KundenId
                                      join aui in au on v.AutoId equals aui.AutoId   
                                      select new VerleihVW { Verleih = v, Kunden = kui, Autos = aui };
            return View(verleihView);
        }

        // GET: Verleih/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verleih = await _context.Verleih.FirstOrDefaultAsync(m => m.ID == id);
            if (verleih == null)
            {
                return NotFound();
            }

            return View(verleih);
        }

        // GET: Verleih/Create
        public async Task<IActionResult> Create(int KundenNr)
        {
            var verleih = new Verleih();
            verleih.KundenId = KundenNr;
            verleih.DT_Von = DateTime.Today;
            
            ViewBag.KundenNr = verleih.KundenId;
            verleih.SelectAutos = await CreateSelList();

            return View(verleih);
        }

        // POST: Verleih/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,KundenId,AutoId,DT_Von,DT_Bis,DT_Rueckgabe,KM_gefahren")] Verleih verleih)
        {
            if (verleih.AutoId == 0 )
            {
                ViewBag.ErrorMessage = "Es wurde keine Auto ausgewählt, bitte auswählen!";
            }
            else if (ModelState.IsValid)
            {
                _context.Add(verleih);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KundenNr = verleih.KundenId;
            verleih.SelectAutos = await CreateSelList();

            return View(verleih);
        }

        // GET: Verleih/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verleih = await _context.Verleih.FindAsync(id);
            if (verleih == null)
            {
                return NotFound();
            }
            return View(verleih);
        }

        // POST: Verleih/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,KundenId,AutoId,DT_Von,DT_Bis,DT_Rueckgabe,KM_gefahren")] Verleih verleih)
        {
            if (id != verleih.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(verleih);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VerleihExists(verleih.ID))
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
            return View(verleih);
        }

        // GET: Verleih/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verleih = await _context.Verleih
                .FirstOrDefaultAsync(m => m.ID == id);
            if (verleih == null)
            {
                return NotFound();
            }

            return View(verleih);
        }

        // POST: Verleih/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var verleih = await _context.Verleih.FindAsync(id);
            _context.Verleih.Remove(verleih);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VerleihExists(int id)
        {
            return _context.Verleih.Any(e => e.ID == id);
        }

        private async Task<List<SelectListItem>> CreateSelList()
        {
            au = await _context.Autos.ToListAsync();
            List<SelectListItem> autoList = new List<SelectListItem>();
            autoList.AddRange(au.Select(a => new SelectListItem() { Value = a.AutoId.ToString(), Text = a.AuswahlListe }).ToList());

            return autoList;
        }

        public async Task<IActionResult> Calendar(int? id)
        {
            /*
                        if (id == null)
                        {
                            return NotFound();
                        }

                        var verleih = await _context.Verleih
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (verleih == null)
                        {
                            return NotFound();
                        }
            */

            var verleih = await _context.Verleih.ToListAsync();
            return View(verleih);
        }


    }
}
