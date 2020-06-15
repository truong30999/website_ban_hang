//Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using do_an_web.Data;
using do_an_web.Models;
using do_an_web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace do_an_web.Areas.Admins
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admins")]
    public class ManageNewController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ManageNewController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View(_db.News.ToList());
        }
        //CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news)
        {
            if (!ModelState.IsValid)
            {
                return View(news);
                
            }
            _db.News.Add(news);
            await _db.SaveChangesAsync();
            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            

            if (files.Count != 0)
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);
                using (var filestream = new FileStream(Path.Combine(uploads,@"news"+ news.Id+ extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                news.Image = @"\" + SD.ImageFolder + @"\news" + news.Id + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\news" + news.Id + ".png");
                news.Image = @"\" + SD.ImageFolder + @"\news" + news.Id + ".png";

            }
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        //EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            News news = _db.News.Where(m => m.Id == id).SingleOrDefault();
            return View(news);
        }
        [HttpPost]
        public IActionResult Edit(int id, [Bind] News news)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var newsFromDb = _db.News.Where(m => m.Id == news.Id).FirstOrDefault();
               
                //var newsFromDb = _db.Products.Where(m => m.Id ==id).FirstOrDefault();
                if (files.Count > 0 && files[0] != null)
                {
                    var upload = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(news.Image);
                    if (System.IO.File.Exists(Path.Combine(upload,@"\news"+news.Id + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(upload, @"\news"+news.Id + extension_old));
                    }
                    using (var filestream = new FileStream(Path.Combine(upload, @"\news"+ news.Id + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    news.Image = @"\" + SD.ImageFolder + @"\news" + news.Id + extension_new;
                }
                if (news.Image != null)
                {
                    newsFromDb.Image =news.Image;
                }
                newsFromDb.Author = news.Author;
                newsFromDb.DateCreate = news.DateCreate;
                newsFromDb.Decreption = news.Decreption;
                newsFromDb.Detail = news.Detail;
                newsFromDb.Tilte = news.Tilte;

                _db.Update(newsFromDb);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }
        //Details
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            News news = _db.News.Where(m => m.Id == id).SingleOrDefault();
            return View(news);
        }
        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            News news = _db.News.Where(m => m.Id == id).SingleOrDefault();
            return View(news);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            News news = _db.News.Find(id);
            if (news == null)
                return NotFound();
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(news.Image);
                if (System.IO.File.Exists(Path.Combine(uploads,@"\news"+news.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, @"\news" + news.Id + extension));
                }
                _db.News.Remove(news);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}