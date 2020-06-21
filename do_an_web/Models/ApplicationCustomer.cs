using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace do_an_web.Models
{
    public class ApplicationCustomer : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }    
        public string Address { get; set; }
        public DateTime MemberSince { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
