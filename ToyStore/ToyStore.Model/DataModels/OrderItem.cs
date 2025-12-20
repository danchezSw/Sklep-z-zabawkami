using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyStore.Model.DataModels
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string ProductName { get; set; }

        public string? Color { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
        public string? ImageUrl { get; set; }
    }
}
