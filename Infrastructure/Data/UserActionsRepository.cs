using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserActionsRepository : IUserActionsRepository
    {
        private readonly StoreContext _context;

        public UserActionsRepository(StoreContext context)
        {
            this._context = context;
        }

        public async Task<ProductReview> CreateProductReview(ProductReview review)
        {
            _context.ProductReviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<IReadOnlyList<ProductReview>> GetProductReviews()
        {
            var reviews = await _context.ProductReviews.ToListAsync();
            return reviews;
        }

        public async Task<IReadOnlyList<ProductReview>> GetProductReviewsByProduct(int id)
        {
            var reviews = await _context.ProductReviews.Where(p => p.ProductId == id).ToListAsync();
            return reviews;
        }
    }
}
