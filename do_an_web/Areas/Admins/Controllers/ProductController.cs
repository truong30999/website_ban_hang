////Sinh viên thực hiện: Tôn Long Hoàng Lãm
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using do_an_web.Data;
using do_an_web.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using do_an_web.Utility;
using do_an_web.Models;
using Microsoft.AspNetCore.Authorization;

namespace do_an_web.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admins")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        [BindProperty]
        public ProductsViewModel ProductsVM { get; set; }
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment ;                   
            ProductsVM = new ProductsViewModel()
            {
                Categories = _db.Categories.ToList(),
                Products = new Models.Product()
            };
        }
        public async Task<IActionResult> Index()
        {
            var product = _db.Products.Include(m => m.Category);
            return View(await product.ToListAsync());
        }
        //Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(ProductsVM);
        }
        [HttpPost,ActionName("Create")]
        public async Task<IActionResult> CreatePost()
        {
            if(!ModelState.IsValid)
            {
                return View(ProductsVM);
            }
            _db.Products.Add(ProductsVM.Products);
            _db.SaveChanges();
            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var productFromDb = _db.Products.Find(ProductsVM.Products.Id);

            if(files.Count!=0)
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);
                using(var filestream=new FileStream(Path.Combine(uploads,ProductsVM.Products.Id + extension),FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                productFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension; 
            }
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".png");
                productFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".png";

            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        //Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            ProductsVM.Products = _db.Products.Include(m => m.Category).SingleOrDefault(m => m.Id == id);
            if (ProductsVM.Products == null)
                return NotFound();
            return View(ProductsVM);
        }
        [HttpPost]
        public IActionResult Edit(int id)
        {
            if(ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var productFromDb = _db.Products.Where(m => m.Id == ProductsVM.Products.Id).FirstOrDefault();
                if(files.Count>0 && files[0]!=null)
                {
                    var upload = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(productFromDb.Image);
                    if(System.IO.File.Exists(Path.Combine(upload, ProductsVM.Products.Id+extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(upload, ProductsVM.Products.Id + extension_old));
                    }
                    using (var filestream = new FileStream(Path.Combine(upload, ProductsVM.Products.Id + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    ProductsVM.Products.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension_new;
                }
                if(ProductsVM.Products.Image!=null)
                {
                    productFromDb.Image = ProductsVM.Products.Image;
                }
                productFromDb.Product_Name = ProductsVM.Products.Product_Name;
                productFromDb.Price = ProductsVM.Products.Price;
                productFromDb.Detail = ProductsVM.Products.Detail;
                productFromDb.ViewNumber = ProductsVM.Products.ViewNumber;
                productFromDb.CategoryId = ProductsVM.Products.CategoryId;
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(ProductsVM);
        }
        //Detail
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            ProductsVM.Products = _db.Products.Include(m => m.Category).SingleOrDefault(m => m.Id == id);
            if (ProductsVM.Products == null)
                return NotFound();
            return View(ProductsVM);
        }
        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            ProductsVM.Products = _db.Products.Include(m => m.Category).SingleOrDefault(m => m.Id == id);
            if (ProductsVM.Products == null)
                return NotFound();
            return View(ProductsVM);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            Product product = _db.Products.Find(id);
            if (product == null)
                return NotFound();
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(product.Image);
                if(System.IO.File.Exists(Path.Combine(uploads, product.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, product.Id + extension));
                }
                _db.Products.Remove(product);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}