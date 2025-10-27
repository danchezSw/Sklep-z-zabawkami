using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyStore.Model.DataModels
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
