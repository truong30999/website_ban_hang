//Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using do_an_web.Data;
using Microsoft.AspNetCore.Mvc;
using do_an_web.Models;
using Microsoft.AspNetCore.Authorization;
using do_an_web.Utility;

namespace do_an_web.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admins")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
           _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Categories.ToList());
        }

        public ViewResult Details(int id)
        {
            Category category = _db.Categories.Where(x => x.Id == id).SingleOrDefault();
            return View(category);
        }
        //Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if(ModelState.IsValid)
            {
                _db.Add(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        //Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            Category category = _db.Categories.Where(m => m.Id == id).SingleOrDefault();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind] Category category)
        {
            if (category.Id != id) return NotFound();
            if (ModelState.IsValid)
            { 
                    _db.Update(category);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        //Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category category = _db.Categories.Find(id);
            if(category==null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult>Delete(int id,Category category)
        {
           
            if(category.Id!=id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();

            }
            return RedirectToAction(nameof(Index));
        }
    }
}