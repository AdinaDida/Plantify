using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Models.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    //[Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly IAdminOrderRepository _rep;
        private readonly IMessagingService _messagingService;


        public OrdersController(IMailService mailService, IOrderService orderService, IMapper mapper, IAdminOrderRepository rep, IMessagingService messagingService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _rep = rep;
            _messagingService = messagingService;
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);
            var products = GetOrderProductsAsHtml(order);
            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));
            _messagingService.SendMessage($"Thank you for your order #{order.Id} the order status is {order.Status}");
            await _mailService.SendEmail(email, "Order Received",
                $"<h1 style='color:red'>Hello, {order.ShipToAddress.FirstName}! Thank you for your order!</h1>" +
                $"<p>Your order #{order.Id} has been received! </p>" +
                $"<p>Order total {order.Subtotal}.00 $ </p>" +
                $"<p>You will receive your order at:</p>" +
                "<div class='d-flex row'>" +
                $"<p>{order.ShipToAddress.FirstName} {order.ShipToAddress.FirstName} </p>" +
                $"<p>{order.ShipToAddress.Street}, {order.ShipToAddress.City} </p>" +
                $"<p>{order.ShipToAddress.State}</p>" +
                $"<p>{order.ShipToAddress.ZipCode}</p>"
                //$"<p>You ordered: </p> " + products + "</div>"
            );
            return Ok(order);
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var email = User.RetrieveEmailFromPrincipal();

            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<OrderToReturnDto>(order);
        }

        [Route("/orders/{id}/{status}")]
        [HttpPatch("{id}/{status}")]
        public async Task<ActionResult<Order>> ConfirmOrderById(int id, int status)
        {
            var email = User.RetrieveEmailFromPrincipal();
            OrderStatus orderStatus = (OrderStatus)status;
            _messagingService.SendMessage($"The status for your order #{id} is now {orderStatus}.");
            await _mailService.SendEmail(email, $"Order {orderStatus}", $"<h1>Thank you for your order!</h1><p>Your order #{id} now has the status ${orderStatus}" + DateTime.Now + "</p>");
            return await _rep.ChangeOrderStatus(id, orderStatus);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }

        private string GetOrderProductsAsHtml(Order order)
        {
            var products = order.OrderItems;
            var message = "";
            foreach (var prod in products)
            {
                message +=
                    $"<div class=" + "p-2 d-flex" + ">" +
                    "<div class='ml-3 d-inline-block align-middle'>" +
                    $"<h5 class='mb-0'><a class='text-dark'>{ prod.ItemOrdered.ProductName}</h5>" +
                    $"<strong>{prod.Price}.00 $</strong></td>" +
                    $"<div class='d-flex justify-content-start align-items-center'>" +
                    $"<span class='font-weight-bold' style='font-size: 1.5em;'> {prod.Quantity}</span></div><strong>{prod.Price * prod.Quantity}.00 $</strong>";
            }
            return message;
        }
    }
}