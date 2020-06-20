using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using do_an_web.Models;
using do_an_web.Data;

namespace do_an_web.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class AdminManageController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminManageController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            
            return View(_db.Admins.ToList());  





        }
        public ViewResult Detail(int id)
        {
            AdminWeb adminWeb = _db.Admins.Where(x => x.Id == id).SingleOrDefault();
            return View(adminWeb);
        }
        //Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminWeb adminWeb)
        {
            if (ModelState.IsValid)
            {
                _db.Add(adminWeb);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminWeb);
        }
        //Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            AdminWeb adminWeb = _db.Admins.Where(x => x.Id == id).SingleOrDefault();

            return View(adminWeb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind] AdminWeb adminWeb)
        {
            if (adminWeb.Id != id) return NotFound();
            if (ModelState.IsValid)
            {
                _db.Update(adminWeb);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(adminWeb);
        }
        //Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            AdminWeb adminWeb  = _db.Admins.Find(id);
            if (adminWeb == null)
            {
                return NotFound();
            }
            return View(adminWeb);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, AdminWeb adminWeb)
        {

            if (adminWeb.Id != id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Admins.Remove(adminWeb);
                _db.SaveChanges();

            }
            return RedirectToAction(nameof(Index));
        }
    }
}