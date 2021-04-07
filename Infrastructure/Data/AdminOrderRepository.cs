using Core.Interfaces;
using Core.Models;
using Core.Models.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Infrastructure.Data
{
    public class AdminOrderRepository : IAdminOrderRepository
    {

        private readonly StoreContext _context;


        public AdminOrderRepository(StoreContext context)
        {
            _context = context;

        }

        public async Task<Product> AddProduct(Product product)
        {
            var products = await _context.Products.ToListAsync();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Order> ChangeOrderStatus(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            order.Status = status;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Product> DeleteProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Order>> GetOrders()
        {
            var orders = await _context.Orders.Include(o=>o.OrderItems).Include(o=>o.DeliveryMethod).ToListAsync();
            return orders;
        }

        public async Task<List<Product>> GetProducts()
        {
            var products = await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            return products;
        }

        public async Task<Product> GetProductById(int id)
        {
            var products = await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).Include(p=>p.ProductReviews).ToListAsync();
            var product = products.Where(p => p.Id == id).FirstOrDefault();
            return product;
        }
    }
}
