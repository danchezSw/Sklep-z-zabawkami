using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToyStore.Model.DataModels
{
    public class Role : IdentityRole<string>
    {
        public RoleValue RoleValue { get; set; }
        public Role() { }
        public Role(string name, RoleValue roleValue)
        {
            Name = name;
            RoleValue = roleValue;
        }
    }
}
