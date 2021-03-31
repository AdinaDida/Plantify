using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UserActionsController : BaseApiController
    {
        private readonly IUserActionsRepository _repo;
        public UserActionsController(IUserActionsRepository repo)
        {
            _repo = repo;
        }

        //[Route("/api/get-reviews")]
        [HttpGet("reviews")]
        public async Task<IReadOnlyList<ProductReview>> GetProductReviews()
        {
            return await _repo.GetProductReviews();
        }

        //[Route("/api/reviews/add")]
        [HttpPost("reviews/add")]
        public async Task<ProductReview> CreateProductReview(ProductReview review)
        {
            await _repo.CreateProductReview(review);
            return review;
        }

        //[Route("/api/reviews/{id}")]
        [HttpGet("reviews/{id}")]
        public async Task<IReadOnlyList<ProductReview>> GetProductReviews(int id)
        {
            return await _repo.GetProductReviewsByProduct(id);
        }
    }
}
