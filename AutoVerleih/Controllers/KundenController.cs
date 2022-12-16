﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoVerleih.Models;
using System.Collections;
using X.PagedList;
using AutoVerleih.Data;
using AutoVerleih.Filter;
using Newtonsoft.Json.Linq;

namespace AutoVerleih.Controllers
{
    public class KundenController : Controller
    {
        private readonly DBProjectContext _context;
        
        public KundenController(DBProjectContext context)
        {
            _context = context;
        }

        // GET: Kunden
        //         public async Task<IActionResult> Index(string currentFilter, DefaultFilter filter, int? page)
        public async Task<IActionResult> Index(string button, string currentFilter, string searchString, int? page)
        {
//            Request.Form.TryGetValue("DT_X", out var xyz);

            IEnumerable <Kunden> kunden = Enumerable.Empty<Kunden>();
            if (!String.IsNullOrEmpty(searchString))
            {
                page = 1;
                kunden = await _context.Kunde.Where(s => s.Name.Contains(searchString)).ToListAsync();       
            }
            else
            {
                page = 1;
                searchString = currentFilter;
                kunden = await _context.Kunde.OrderByDescending(s => s.KundenId).Take(10).ToListAsync();
            }
            ViewBag.CurrentFilter = searchString;

            // IList test3 = _context.Kunde.Local.ToList();
            // return View(test3);
            int pageSize = 20;
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
        public async Task<IActionResult> Create([Bind("KundenId,Name,Plz,Ort,Adresse")] Kunden kunde)
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
        public async Task<IActionResult> Edit(int id, [Bind("KundenId,Name,Plz,Ort,Adresse")] Kunden kunde)
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

/* ToDO           
            var rechnung = await _context.Rechnung.FirstOrDefaultAsync(r => r.KundenId == id);
            if (rechnung != null)
            {
              ViewBag.ErrorMessage = "Für diesen Kunden sind noch Rechnungen vorhanden!";
            }
*/
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