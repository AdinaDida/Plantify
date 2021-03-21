using Core.Interfaces;
using Core.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AdminOrderRepository : IAdminOrderRepository
    {

        private readonly StoreContext _context;

        public AdminOrderRepository(StoreContext context)
        {
            this._context = context;
        }
        public async Task<Order> ChangeOrderStatus(int id, OrderStatus status)
        {
            var order = _context.Orders.Where(o => o.Id == id).FirstOrDefault();
            order.Status = status;
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
