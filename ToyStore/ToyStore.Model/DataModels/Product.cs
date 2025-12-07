using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyStore.Model.DataModels
{
    public class Product
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public string? DefaultImageUrl { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<ProductColorVariant> ColorVariants { get; set; } = new List<ProductColorVariant>();
    }
}
