using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Core.Models.OrderAggregate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {

        private readonly IOrderService _orderService;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly IAdminOrderRepository _rep;
        private readonly IMessagingService _messagingService;


        public AdminController(IMailService mailService, IOrderService orderService, IMapper mapper, IAdminOrderRepository rep, IMessagingService messagingService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _rep = rep;
            _messagingService = messagingService;
            _mailService = mailService;
        }

        [HttpPatch("order/{id}/{status}")]
        public async Task<ActionResult<Order>> ConfirmOrderById(int id, int status)
        {
            //var email = User.RetrieveEmailFromPrincipal();
            OrderStatus orderStatus = (OrderStatus)status;
            //if(!orderStatus.Equals("Finished"))
            //{
            //    _messagingService.SendMessage($"The status for your order #{id} is now {orderStatus}.");
                //await _mailService.SendEmailAsync(email, $"Order {orderStatus}", $"<h1>Thank you for your order!</h1><p>Your order #{id} now has the status ${orderStatus}" + DateTime.Now + "</p>");
            //}
            return await _rep.ChangeOrderStatus(id, orderStatus);
        }
        //[Route("/api/admin/product/{id}")]
        [HttpGet("product/{id}")]
        public async Task<Product> GetProductById(int id)
        {
            return await _rep.GetProductById(id);
        }


        [HttpGet("order/{id}")]
        public async Task<Order> GetOrderById(int id)
        {
            return await _rep.GetOrderById(id);
        }

        [HttpGet("orders")]
        public async Task<IReadOnlyList<OrderToReturnDto>> GetOrders()
        {
            var orders = await _rep.GetOrders();
            foreach (var order in orders)
            {
                order.GetTotal();
            }
            return _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
        }

        [HttpGet("orders/finished")]
        public async Task<IReadOnlyList<OrderToReturnDto>> GetFinishedOrders()
        {
            var orders = await _rep.GetFinishedOrders();
            foreach (var order in orders)
            {
                order.GetTotal();
            }
            return _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
        }

        //[Route("/api/admin/product")]
        [HttpPost("product")]
        public async Task<Product> AddProduct([FromBody] Product product)
        {
            return await _rep.AddProduct(product);
        }

        [HttpGet("products")]
        public async Task<List<Product>> GetProducts()
        {
            return await _rep.GetProducts();
        }
        //[Route("/api/admin/delete-product/{id}")]
        [HttpDelete("delete-product/{id}")]
        public async Task<Product> DeleteProduct(int id)
        {
            return await _rep.DeleteProductById(id);
        }
    }
}
