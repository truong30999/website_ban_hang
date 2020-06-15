//Sinh viên thực hiện: Tôn Long Hoàng Lãm
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace do_an_web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Product_Name { get; set; }
        public string Detail { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        public int ViewNumber { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }



    }
}
