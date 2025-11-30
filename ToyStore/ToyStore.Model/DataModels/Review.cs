using System;
using System.ComponentModel.DataAnnotations;

namespace ToyStore.Model.DataModels
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required, MaxLength(100)]
        public string? AuthorName { get; set; }

        [Required, MaxLength(1000)]
        public string? Content { get; set; }

        [Range(1,5)]
        public int Rating { get; set; } = 5;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsApproved { get; set; } = true; 
    }
}