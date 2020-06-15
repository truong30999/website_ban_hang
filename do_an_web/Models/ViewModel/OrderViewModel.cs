using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace do_an_web.Models.ViewModel
{
    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
