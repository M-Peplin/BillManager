using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillManager.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public bool IdPaid { get; set; }
        public IEnumerable<Bill> Bills { get; set; }
        public IEnumerable<Information> Informations { get; set; }
    }
}
