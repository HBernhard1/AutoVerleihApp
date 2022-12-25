using AutoVerleih.Data;
using AutoVerleih.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using Segment.Model;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Diagnostics;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

namespace AutoVerleih.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBProjectContext _context;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(DBProjectContext context, ILogger<HomeController> logger)        {
            _context = context;
            _logger = logger;
        }

        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Get Action
        public async Task<IActionResult> Login(string ReturnUrl)
        {
//            var machineName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            if (Debugger.IsAttached)
            {
                await WriteLoginClaims("bh");
                return RedirectToAction("Index", ReturnUrl);
            }

            if (ReturnUrl != null)
            {
                // expand Controller Name from Url
                if (ReturnUrl.IndexOf("/", 1) > 0) ReturnUrl.Substring(1, ReturnUrl.IndexOf("/", 1)-1);
                TempData["ReturnUrl"] = ReturnUrl;
            }

            return View();
        }

        //Post Action
        [HttpPost]
        public async Task<IActionResult> Login(Accounts u)
        {
            string returnUrl = "";

            if (TempData["ReturnUrl"] != null) returnUrl = TempData["ReturnUrl"].ToString();

            if (!User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var obj = _context.Accounts.Where(a => a.username.Equals(u.username) && a.password.Equals(u.password)).FirstOrDefault();
                    if (obj == null)
                    {
                        ViewBag.Message = "Login nicht erfolgreich!";
                    }
                    else
                    {
                        _logger.LogInformation("User {Email} logged in at {Time}.", u.username, DateTime.UtcNow);
                        await WriteLoginClaims(u.username);

                        return RedirectToAction("Index", returnUrl);
                    }

                }
            }
            else
            {
                return RedirectToAction("Home");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {

            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        private async Task WriteLoginClaims(string username)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, "User"), };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddMinutes(1000), };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}
