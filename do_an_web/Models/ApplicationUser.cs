using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace do_an_web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name="Admin")]
        public string Name { get; set; }
        [NotMapped]
        public bool IsSuperAdmin { get; set; }

    }
}
