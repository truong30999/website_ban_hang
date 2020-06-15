using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace do_an_web.Models.ViewModel
{
    public class OrderDetailViewModel
    {
        public Order Order { get; set; }
        public List<Product> Products { get; set; }
    }
}
