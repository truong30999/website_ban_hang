//Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using do_an_web.Data;
using Microsoft.AspNetCore.Mvc;
using do_an_web.Models;

namespace do_an_web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NewsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var news = _db.News.ToList();
            return View(news);
        }
        //Detail
        public async Task<IActionResult> Details(int id)
        {
            var news = _db.News.Where(m => m.Id == id).FirstOrDefault();
            
            
            return View(news);
        }
    }
}