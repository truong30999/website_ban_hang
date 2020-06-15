//Sinh viên thực hiện: Nguyễn Nhật Trường
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace do_an_web.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Tilte { get; set; }
        public string Decreption { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        public DateTime DateCreate { get; set; }

    }
}
