//Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace do_an_web.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public int  Status { get; set; }
        public virtual ICollection<Product> Products { get; set; }
       

    }
}
