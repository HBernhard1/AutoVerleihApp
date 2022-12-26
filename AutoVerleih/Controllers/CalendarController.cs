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
using Microsoft.Extensions.Logging;

namespace AutoVerleih.Controllers
{
    public class CalendarController : Controller
    {

        List<Autos> au = new List<Autos>();
        List<Kunden> ku = new List<Kunden>();
        List<Verleih> vl = new List<Verleih>();

        private readonly DBProjectContext _context;
        private readonly ILogger<HomeController> _logger;

        public CalendarController(DBProjectContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

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
    }
}
