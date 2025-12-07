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
            Id = Guid.NewGuid().ToString();
            Name = name;
            NormalizedName = name.ToUpper();
            RoleValue = roleValue;
        }
    }
}
