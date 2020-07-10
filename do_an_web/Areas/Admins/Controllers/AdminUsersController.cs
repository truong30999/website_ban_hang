////Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using do_an_web.Data;
using do_an_web.Models;
using do_an_web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace do_an_web.Areas.Admins.Controllers
{
    [Authorize(Roles = SD.AdminEndUser + "," + SD.SuperAdminEndUser)]
    [Area("Admins")]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminUsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.ApplicationUser.ToList());
        }
        //Edit
        public IActionResult Edit(string id)
        {
            if(id==null ||id.Trim().Length==0)
            {
                return NotFound();  
            }
            var userFromDb = _db.ApplicationUser.Find(id);
            if (userFromDb == null)
                return NotFound();
            return View(userFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id , ApplicationUser applicationUser)
        {
            if(id!=applicationUser.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                ApplicationUser userFromDb = _db.ApplicationUser.Where(m => m.Id == id).FirstOrDefault();
                userFromDb.Name = applicationUser.Name;
                //userFromDb.Email = applicationUser.Email;
                userFromDb.PhoneNumber = applicationUser.PhoneNumber;

                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);

        }

        //Delete
        public IActionResult Delete(string id)
        {
            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }
            var userFromDb = _db.ApplicationUser.Find(id);
            if (userFromDb == null)
                return NotFound();
            return View(userFromDb);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string id)
        {
           
            ApplicationUser userFromDb = _db.ApplicationUser.Where(m => m.Id == id).FirstOrDefault();
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}