using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace do_an_web.Models
{
    public class AdminWeb
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName  { get; set; }
        [Required]
        public string Password { get; set; }
        public int Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }
}
