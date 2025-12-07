using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyStore.Model.DataModels
{
    public class ProductColorVariant
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
