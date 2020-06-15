﻿//Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using do_an_web.Models;
using do_an_web.Data;
using Microsoft.EntityFrameworkCore;
using do_an_web.Extensions;

namespace do_an_web.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
       
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
      
        public async Task<IActionResult> Index()
        {
            var productList = _db.Products.Include(m => m.Category).ToList();
            return View(productList);
        }
        //Details
        public async Task<IActionResult> Details(int id)
        {
            var product = _db.Products.Include(m => m.Category).Where(m=>m.Id==id).FirstOrDefault();
            product.ViewNumber++;
            _db.Update(product);
            _db.SaveChanges();
            return View(product);
        }
        [HttpPost,ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstShoppingCart==null)
            {
                lstShoppingCart = new List<int>();
            }
            lstShoppingCart.Add(id);
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            return RedirectToAction("Index", "Home", new { Areas="Customer" });
        }

        public IActionResult Remove(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstShoppingCart.Count>0)
            {
                if (lstShoppingCart.Contains(id))
                    lstShoppingCart.Remove(id);

            }
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
