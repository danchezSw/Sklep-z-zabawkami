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

        [Required, MaxLength(100)]
        public string ?Name { get; set; }

        [MaxLength(500)]
        public string ?Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string ?ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public virtual Category ?Category { get; set; }

        public virtual ICollection<OrderItem> ?OrderItems { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>(); // Opinie
    }
}
