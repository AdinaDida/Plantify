using Core.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAdminOrderRepository
    {
        Task<Order> ChangeOrderStatus(int id, OrderStatus status);
        
    }
}
