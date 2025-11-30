using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyStore.Model.DataModels
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string ?Name { get; set; }

        [MaxLength(200)]
        public string ?Description { get; set; }
        public virtual ICollection<Product> ?Products { get; set; }
    }
}
