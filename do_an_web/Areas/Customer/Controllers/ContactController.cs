//Sinh viên thực hiện: Tôn Long Hoàng Lãm
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using do_an_web.Data;
using do_an_web.Models;
using Microsoft.AspNetCore.Mvc;

namespace do_an_web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ContactController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        //Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _db.Add(contact);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            return View(contact);
        }

    }
}