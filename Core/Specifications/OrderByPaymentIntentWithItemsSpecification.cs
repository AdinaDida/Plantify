using System;
using System.Linq.Expressions;
using Core.Models.OrderAggregate;

namespace Core.Specifications
{
    public class OrderByPaymentIntentWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentWithItemsSpecification(string paymentIntentId)
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}