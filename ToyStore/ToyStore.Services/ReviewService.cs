using Microsoft.EntityFrameworkCore;
using ToyStore.Data;
using ToyStore.Model.DataModels;

namespace ToyStore.Services
{
    public class ReviewService
    {
        private readonly ApplicationDbContext _db;

        public ReviewService(ApplicationDbContext db)
        {
            _db = db;
        }

        
        public async Task AddAsync(Review review)
        {
            
            review.CreatedAt = DateTime.UtcNow;

            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
        }

        
        public async Task<List<Review>> GetByProductAsync(int productId, bool onlyApproved = true)
        {
            var query = _db.Reviews.Where(r => r.ProductId == productId);

            if (onlyApproved)
                query = query.Where(r => r.IsApproved);

            return await query
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        
        public async Task<double> GetAverageRatingAsync(int productId)
        {
            var ratings = await _db.Reviews
                .Where(r => r.ProductId == productId && r.IsApproved)
                .Select(r => (int?)r.Rating)
                .ToListAsync();

            if (!ratings.Any())
                return 0;

            return ratings.Average() ?? 0;
        }
    }
}