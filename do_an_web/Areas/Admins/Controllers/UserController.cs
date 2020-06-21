using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using do_an_web.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using do_an_web.Models;

namespace do_an_web.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
       
        public IActionResult Index()
        {
            
            return View( _db.ApplicationCustomer.ToList());
        }
    }
}