//Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace do_an_web.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }//
        public int Phone { get; set; } //
        public string Email { get; set; }//

        public string Address { get; set; }//
        public DateTime DateOrder { get; set; }//
        public float TotalPrice { get; set; }//
        public string PaymentMethod { get; set; }//
        public int Status { get; set; }//

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
