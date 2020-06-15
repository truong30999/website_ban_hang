using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace do_an_web.Models.ViewModel
{
    public class ProductsViewModel
    {
        public Product Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
