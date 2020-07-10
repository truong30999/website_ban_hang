//Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using do_an_web.Data;
using do_an_web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using do_an_web.Models.ViewModel;
using do_an_web.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace do_an_web.Areas.Admins.Controllers
{
    [Authorize(Roles =SD.AdminEndUser +","+ SD.SuperAdminEndUser)]
    [Area("Admins")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private int PageSize = 3;
        public OrdersController(ApplicationDbContext db )
        {
            _db = db;
        }
        public IActionResult Index(int productPage=1, string searchName=null,string searchEmail=null, string searchPhone=null, string searchDate=null )
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderViewModel orderVM = new OrderViewModel()
            {
                Orders = new List<Models.Order>()
            };
            StringBuilder param = new StringBuilder();
            param.Append("/Admins/Orders?productPage=:");
            param.Append("&searchName=");
            if(searchName!=null)
            {
                param.Append(searchName);
            }
            param.Append("&searchEmail=");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }
            param.Append("&searchPhone=");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }
            param.Append("&searchDate=");
            if (searchDate != null)
            {
                param.Append(searchDate);
            }


            orderVM.Orders = _db.Orders.ToList();

            if (searchName != null)
            {
                orderVM.Orders = orderVM.Orders.Where(a=>a.Name.ToLower().Contains(searchName.ToLower())).ToList();                
            }
            if (searchEmail != null)
            {
                orderVM.Orders = orderVM.Orders.Where(a => a.Email.ToLower().Contains(searchEmail.ToLower())).ToList();
            }
            if (searchPhone != null)
            {
                orderVM.Orders = orderVM.Orders.Where(a => a.Phone.Equals(searchPhone)).ToList();
            }
            if (searchDate != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(searchDate);
                    orderVM.Orders = orderVM.Orders.Where(a => a.DateOrder.ToShortDateString().Equals(date.ToShortDateString())).ToList();
                    
                }
                catch (Exception ex)
                {

                }
            }

            var count = orderVM.Orders.Count;
            orderVM.Orders = orderVM.Orders.OrderBy(p => p.DateOrder)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();

            orderVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };

            return View(orderVM);
        }

        //Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Product>)(from p in _db.Products
                                                     join a in _db.OrderDetails
                                                     on p.Id equals a.ProductId
                                                     where a.OrderId == id
                                                     select p).Include("Category");
            OrderDetailViewModel objOrderVM = new OrderDetailViewModel()
            {
                Order = _db.Orders.Where(a => a.Id == id).FirstOrDefault(),
                Products = productList.ToList()
            };
            return View(objOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDetailViewModel objOrderVM)
        {
            if(ModelState.IsValid)
            {
                var orderFromDb = _db.Orders.Where(a => a.Id == objOrderVM.Order.Id).FirstOrDefault();

                orderFromDb.Name = objOrderVM.Order.Name;
                orderFromDb.Email = objOrderVM.Order.Email;
                orderFromDb.Phone = objOrderVM.Order.Phone;
                orderFromDb.Address = objOrderVM.Order.Address;
                orderFromDb.PaymentMethod = objOrderVM.Order.PaymentMethod;
                orderFromDb.DateOrder = objOrderVM.Order.DateOrder;
                orderFromDb.TotalPrice = objOrderVM.Order.TotalPrice;
                orderFromDb.Status = objOrderVM.Order.Status;
                orderFromDb.StatusDelivery = objOrderVM.Order.StatusDelivery;
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(objOrderVM);
        }

        //Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Product>)(from p in _db.Products
                                                     join a in _db.OrderDetails
                                                     on p.Id equals a.ProductId
                                                     where a.OrderId == id
                                                     select p).Include("Category");
            OrderDetailViewModel objOrderVM = new OrderDetailViewModel()
            {
                Order = _db.Orders.Where(a => a.Id == id).FirstOrDefault(),
                Products = productList.ToList()
            };
            return View(objOrderVM);
        }
        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Product>)(from p in _db.Products
                                                     join a in _db.OrderDetails
                                                     on p.Id equals a.ProductId
                                                     where a.OrderId == id
                                                     select p).Include("Category");
            OrderDetailViewModel objOrderVM = new OrderDetailViewModel()
            {
                Order = _db.Orders.Where(a => a.Id == id).FirstOrDefault(),
                Products = productList.ToList()
            };
            return View(objOrderVM);
        }
        //POST Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}


