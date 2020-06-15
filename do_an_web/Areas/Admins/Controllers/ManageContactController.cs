//Sinh viên thực hiện: Tôn Long Hoàng Lãm
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

    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admins")]
    public class ManageContactController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ManageContactController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Contacts.ToList());
        }
        public async Task<IActionResult> Details(int id)
        {
            Contact contact = _db.Contacts.Where(m => m.Id == id).FirstOrDefault();
            contact.Status=1;
            _db.Update(contact);
            _db.SaveChanges();
            return View(contact);
        }
        public IActionResult Delete(int id)
        {
            Contact contact= _db.Contacts.Where(m=>m.Id==id).FirstOrDefault();
            if (contact == null)
            {
                return NotFound();
            }
            _db.Contacts.Remove(contact);
            _db.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }

    }
}