using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserActionsRepository
    {
        Task<IReadOnlyList<ProductReview>> GetProductReviews();
        Task<IReadOnlyList<ProductReview>> GetProductReviewsByProduct(int id);
        Task<ProductReview> CreateProductReview(ProductReview review);

    }
}
