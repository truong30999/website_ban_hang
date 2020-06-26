//Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using do_an_web.Data;
using do_an_web.Models;
using do_an_web.Extensions;
using do_an_web.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace do_an_web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ShoppingCartViewModel shoppingCartVM { get; set; }
        public ShoppingCartController( ApplicationDbContext db)
        {
            _db = db;
            shoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Product>()
            };
        }
      

        public async Task<IActionResult> Index()
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if (lstShoppingCart == null)
                return View(shoppingCartVM);
            if(lstShoppingCart.Count>0)
            {
                foreach(int item in lstShoppingCart)
                {
                    Product prod = _db.Products.Include(p => p.Category).Where(p => p.Id == item).FirstOrDefault();
                    shoppingCartVM.Products.Add(prod);
                }
            }
           
            return View(shoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            Order order = shoppingCartVM.Order;
           
            _db.Orders.Add(order);
            _db.SaveChanges();

            int orderId = order.Id;
            foreach(int productId in lstCartItems)
            {
                OrderDetail orderDetail = new OrderDetail { ProductId = productId, OrderId = orderId, Status=1, Amount=1 };
                _db.OrderDetails.Add(orderDetail);
            }
            _db.SaveChanges();
            lstCartItems = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);
            return RedirectToAction("OrderComfirmation", "ShoppingCart", new { Id = orderId }) ;

        }
        public IActionResult Remove(int Id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstCartItems.Count>0)
            {
                if(lstCartItems.Contains(Id))
                {
                    lstCartItems.Remove(Id);

                }

            }
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult OrderComfirmation(int id)
        {
            shoppingCartVM.Order = _db.Orders.Where(a => a.Id == id).FirstOrDefault();
            List<OrderDetail> orderDetails = _db.OrderDetails.Where(p => p.OrderId == id).ToList();
            foreach(OrderDetail item in orderDetails)
            {
                shoppingCartVM.Products.Add(_db.Products.Include(p => p.Category).Where(p => p.Id == item.ProductId).FirstOrDefault());

            }
            return View(shoppingCartVM);
        }
    }
}