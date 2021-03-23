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
                "<table width = '100%' cellpadding = '0' cellspacing = '0' border = '0' style = 'width:100%; max-width:600px;' align = 'center'>" +
                    "<tbody>" +
                        "<tr>"+
                            "<td role='modules-container' style='padding:0px 0px 0px 0px; color:#000000; text-align:left;' bgcolor='#FFFFFF' width='100%' align='left'>"+
                                "<table class='wrapper' role='module' data-type='image' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='cb31e9b8-b045-4c38-a478-ed2a6e2dc166'>"+
                                    "<tbody>"+
                                        "<tr>" +
                                            "<td style='font-size:6px; line-height:10px; padding:0px 0px 0px 0px;' valign='top' align='center'>" +
                                                "<img class='max-width' border='0' style='display:block; color:#000000; text-decoration:none; font-family:Helvetica, arial, sans-serif; font-size:16px;' width='600' alt='' data-proportionally-constrained='true' data-responsive='false' src='http://cdn.mcauto-images-production.sendgrid.net/954c252fedab403f/4ad091f2-00dc-4c89-9ad8-1d7aeaf169c2/600x189.png' height='189'>" +
                                            "</td>"+
                                        "</tr>" +
                                    "</tbody>" +
                                "</table>" +
                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='8fd711e6-aecf-4663-bf53-6607f08b57e9' data-mc-module-version='2019-10-22'>" +
                                    "<tbody>" +
                                        "<tr>" +
                                            "<td style='padding:30px 0px 40px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                "<div>" +
                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                    "<span style='color: #80817f; font-size: 12px'>" +
                                                        "<strong>THANK YOU FOR SHOPPING WITH US TODAY.</strong>" +
                                                    "</span>" +
                                                    "</div>"+
                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                        "<br>"+
                                                    "</div>" +
                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                        "<span style='color: #80817f; font-size: 12px'>" +
                                                            "<strong>Sales Receipt</strong>" +
                                                        "</span>" +
                                                    "</div>" +
                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                        "<span style='color: #80817f; font-size: 12px'>January 20, 2020 5:40 PM " +
                                                        "</span>" +
                                                    "</div>" +
                                                    "<div></div>" +
                                                "</div>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</tbody>" +
                                "</table>" +
                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='8fd711e6-aecf-4663-bf53-6607f08b57e9.1' data-mc-module-version='2019-10-22'>" +
                                    "<tbody>" +
                                        "<tr>" +
                                            "<td style='padding:0px 40px 40px 40px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                "<div>" +
                                                    "<div style='font-family: inherit; text-align: inherit'>" +
                                                        "<span style='color: #80817f; font-size: 12px'>" +
                                                            "<strong>Employee:</strong>" +
                                                        "</span>" +
                                                        "<span style='color: #80817f; font-size: 12px'> Sammie" +
                                                        "</span>" +
                                                    "</div>" +
                                                    "<div style='font-family: inherit; text-align: inherit'>" +
                                                        "<span style='color: #80817f; font-size: 12px'>" +
                                                            "<strong>Customer:</strong>" +
                                                        "</span>" +
                                                        "<span style='color: #80817f; font-size: 12px'> Jane Doe" +
                                                        "</span>" +
                                                    "</div>" +
                                                    "<div style='font-family: inherit; text-align: inherit'>" +
                                                        "<span style='color: #80817f; font-size: 12px'>" +
                                                            "<strong>Email:</strong>" +
                                                        "</span>" +
                                                        "<span style='color: #80817f; font-size: 12px'> janedoe@example.com" +
                                                        "</span>" +
                                                    "</div> " +
                                                    "<div style='font-family: inherit; text-align: inherit'>" +
                                                        "<span style='color: #80817f; font-size: 12px'>" +
                                                            "<strong>Ticket Number: </strong>" +
                                                        "</span>" +
                                                        "<span style='color: #80817f; font-size: 12px'>123456789 " +
                                                        "</span>" +
                                                    "</div>" +
                                                    "<div></div>" +
                                                "</div>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</tbody> " +
                                "</table>" +
                                "<table class='module' role='module' data-type='divider' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='c614d8b1-248a-48ea-a30a-8dd0b2c65e10'>" +
                                    "<tbody>" +
                                        "<tr> " +
                                            "<td style='padding:0px 40px 0px 40px;' role='module-content' height='100%' valign='top' bgcolor=''> " +
                                                "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' height='2px' style='line-height:2px; font-size:2px;'> " +
                                                    "<tbody>" +
                                                        "<tr>" +
                                                            "<td style='padding:0px 0px 2px 0px;' bgcolor='#80817f'>" +
                                                            "</td>" +
                                                        "</tr>" +
                                                    "</tbody>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</tbody>" +
                                "</table>" +
                                "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' role='module' data-type='columns' style='padding:0px 40px 0px 40px;' bgcolor='#FFFFFF'> " +
                                    "<tbody> " +
                                        "<tr role='module-content'> " +
                                            "<td height='100%' valign='top'>" +
                                                "<table class='column' width='173' style='width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='64573b96-209a-4822-93ec-5c5c732af15c' data-mc-module-version='2019-10-22'>" +
                                                                    "<tbody> " +
                                                                        "<tr> " +
                                                                            "<td style='padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                                                "<div>" +
                                                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                                                        "<span style='color: #80817f; font-size: 12px'>" +
                                                                                            "<strong>ITEM</strong>" +
                                                                                        "</span>" +
                                                                                    "</div>" +
                                                                                    "<div></div>" +
                                                                                "</div>" +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                    "</tbody> " +
                                                                "</table>" +
                                                            "</td>" +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table> " +
                                                "<table width='173' style='width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor='' class='column column-1'>" +
                                                    "<tbody>" +
                                                        "<tr> " +
                                                            "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='64573b96-209a-4822-93ec-5c5c732af15c.1' data-mc-module-version='2019-10-22'>" +
                                                                    " <tbody> " +
                                                                        "<tr> " +
                                                                            "<td style='padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                                                "<div>" +
                                                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                                                        "<span style='color: #80817f; font-size: 12px'>" +
                                                                                            "<strong>QTY</strong>" +
                                                                                        "</span>" +
                                                                                    "</div>" +
                                                                                    "<div></div>" +
                                                                                "</div>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                    "</tbody> " +
                                                                "</table>" +
                                                            "</td>" +
                                                        " </tr> " +
                                                    "</tbody> " +
                                                "</table>" +
                                                "<table width='173' style='width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor='' class='column column-2'> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='64573b96-209a-4822-93ec-5c5c732af15c.1.1' data-mc-module-version='2019-10-22'> " +
                                                                    "<tbody> " +
                                                                        "<tr>" +
                                                                            "<td style='padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                                                "<div>" +
                                                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                                                        "<span style='color: #80817f; font-size: 12px'>" +
                                                                                            "<strong>PRICE</strong>" +
                                                                                        "</span>" +
                                                                                    "</div>" +
                                                                                    "<div></div>" +
                                                                                "</div>" +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                    "</tbody> " +
                                                                "</table>" +
                                                            "</td> " +
                                                        "</tr>" +
                                                    "</tbody> " +
                                                "</table>" +
                                            "</td> " +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                                "<table class='module' role='module' data-type='divider' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='c614d8b1-248a-48ea-a30a-8dd0b2c65e10.1'>" +
                                    "<tbody>" +
                                        "<tr>" +
                                            "<td style='padding:0px 40px 0px 40px;' role='module-content' height='100%' valign='top' bgcolor=''>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' height='2px' style='line-height:2px; font-size:2px;'>" +
                                                    "<tbody>" +
                                                        "<tr> " +
                                                            "<td style='padding:0px 0px 2px 0px;' bgcolor='#80817f'>" +
                                                            "</td> " +
                                                        "</tr> " +
                                                    "</tbody>" +
                                                "</table> " +
                                            "</td> " +
                                        "</tr>" +
                                    "</tbody> " +
                                "</table>" +
                                "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' role='module' data-type='columns' style='padding:0px 40px 0px 40px;' bgcolor='#FFFFFF'> " +
                                    "<tbody> " +
                                        "<tr role='module-content'> " +
                                            "<td height='100%' valign='top'> " +
                                                "<table class='column' width='173' style='width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''> " +
                                                    "<tbody>" +
                                                        "<tr>" +
                                                            "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='64573b96-209a-4822-93ec-5c5c732af15c.2' data-mc-module-version='2019-10-22'>" +
                                                                    "<tbody> " +
                                                                        "<tr>" +
                                                                            "<td style='padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                                                "<div>" +
                                                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                                                        "<span style='color: #80817f; font-size: 12px'>Villa Floral (120 mL)</span>" +
                                                                                    "</div>" +
                                                                                    "<div></div>" +
                                                                                "</div>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                    "</tbody>" +
                                                                "</table>" +
                                                            "</td> " +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table> " +
                                                "<table class='column' width='173' style='width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='64573b96-209a-4822-93ec-5c5c732af15c.1.2' data-mc-module-version='2019-10-22'>" +
                                                                    "<tbody> " +
                                                                        "<tr> " +
                                                                            "<td style='padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                                                "<div>" +
                                                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                                                        "<span style='color: #80817f; font-size: 12px'>1</span>" +
                                                                                    "</div>" +
                                                                                    "<div></div>" +
                                                                                "</div>" +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                    "</tbody> " +
                                                                "</table>" +
                                                            "</td> " +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table> " +
                                                "<table width='173' style='width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor='' class='column column-2'> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='64573b96-209a-4822-93ec-5c5c732af15c.1.1.1' data-mc-module-version='2019-10-22'> " +
                                                                    "<tbody>" +
                                                                        "<tr> " +
                                                                            "<td style='padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                                                "<div>" +
                                                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                                                        "<span style='color: #80817f; font-size: 12px'>$175.90</span>" +
                                                                                    "</div>" +
                                                                                    "<div></div>" +
                                                                                "</div>" +
                                                                            "</td>" +
                                                                        "</tr> " +
                                                                    "</tbody> " +
                                                                "</table>" +
                                                            "</td> " +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table>" +
                                            "</td> " +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                                "<table class='module' role='module' data-type='divider' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='c614d8b1-248a-48ea-a30a-8dd0b2c65e10.1.2'> " +
                                    "<tbody> " +
                                        "<tr> " +
                                            "<td style='padding:0px 40px 0px 40px;' role='module-content' height='100%' valign='top' bgcolor=''>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' height='1px' style='line-height:1px; font-size:1px;'> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td style='padding:0px 0px 1px 0px;' bgcolor='#80817f'></td>" +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table> " +
                                            "</td> " +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                                "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' role='module' data-type='columns' style='padding:0px 40px 0px 40px;' bgcolor='#FFFFFF'> " +
                                    "<tbody> " +
                                        "<tr role='module-content'> " +
                                            "<td height='100%' valign='top'> " +
                                                "<table class='column' width='173' style='width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='64573b96-209a-4822-93ec-5c5c732af15c.2.1' data-mc-module-version='2019-10-22'> " +
                                                                    "<tbody> " +
                                                                        "<tr> " +
                                                                            "<td style='padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                                                "<div>" +
                                                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                                                        "<span style='color: #80817f; font-size: 12px'>Port Toulon (40 mL)</span>" +
                                                                                    "</div>" +
                                                                                    "<div></div>" +
                                                                                "</div>" +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                    "</tbody> " +
                                                                "</table>" +
                                                            "</td> " +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table> " +
                                                "<table class='column' width='173' style='width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='64573b96-209a-4822-93ec-5c5c732af15c.1.2.1' data-mc-module-version='2019-10-22'> " +
                                                                    "<tbody> " +
                                                                        "<tr> " +
                                                                            "<td style='padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                                                "<div>" +
                                                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                                                        "<span style='color: #80817f; font-size: 12px'>1</span>" +
                                                                                    "</div>" +
                                                                                    "<div></div>" +
                                                                                "</div>" +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                    "</tbody> " +
                                                                "</table>" +
                                                            "</td> " +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table> " +
                                                "<table width='173' style='width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor='' class='column column-2'>" +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='64573b96-209a-4822-93ec-5c5c732af15c.1.1.1.1' data-mc-module-version='2019-10-22'> " +
                                                                    "<tbody>" +
                                                                        "<tr>" +
                                                                            "<td style='padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                                                "<div>" +
                                                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                                                        "<span style='color: #80817f; font-size: 12px'>$35.90</span>" +
                                                                                    "</div>" +
                                                                                    "<div></div>" +
                                                                                "</div>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                    "</tbody>" +
                                                                "</table>" +
                                                            "</td>" +
                                                        "</tr> " +
                                                    "</tbody>" +
                                                "</table>" +
                                            "</td> " +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                                "<table class='module' role='module' data-type='divider' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='c614d8b1-248a-48ea-a30a-8dd0b2c65e10.1.2.1'> " +
                                    "<tbody> " +
                                        "<tr> " +
                                            "<td style='padding:0px 40px 0px 40px;' role='module-content' height='100%' valign='top' bgcolor=''>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' height='1px' style='line-height:1px; font-size:1px;'> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td style='padding:0px 0px 1px 0px;' bgcolor='#80817f'></td> " +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table> " +
                                            "</td> " +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                                "<table class='module' role='module' data-type='spacer' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='54da3428-feae-4c1a-a740-9c9fdb4e52d7'> " +
                                    "<tbody> " +
                                        "<tr> " +
                                            "<td style='padding:0px 0px 50px 0px;' role='module-content' bgcolor=''> </td> " +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='8fd711e6-aecf-4663-bf53-6607f08b57e9.2' data-mc-module-version='2019-10-22'> " +
                                    "<tbody> " +
                                        "<tr> " +
                                            "<td style='padding:10px 0px 20px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>" +
                                                "<div>" +
                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                        "<span style='color: #80817f; font-size: 12px'>" +
                                                            "<strong>Want to check out all our products? We now offer free shipping! </strong>" +
                                                        "</span>" +
                                                    "</div>" +
                                                    "<div></div>" +
                                                "</div>" +
                                            "</td>" +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                                "<table border='0' cellpadding='0' cellspacing='0' class='module' data-role='module-button' data-type='button' role='module' style='table-layout:fixed;' width='100%' data-muid='0f986857-87df-4c0e-934f-e77105f78192'> " +
                                    "<tbody> " +
                                        "<tr> " +
                                            "<td align='center' bgcolor='' class='outer-td' style='padding:0px 0px 0px 0px;'> " +
                                                "<table border='0' cellpadding='0' cellspacing='0' class='wrapper-mobile' style='text-align:center;'> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td align='center' bgcolor='#ffecea' class='inner-td' style='border-radius:6px; font-size:16px; text-align:center; background-color:inherit;'> " +
                                                                "<a href='' style='background-color:#ffecea; border:1px solid #ffecea; border-color:#ffecea; border-radius:0px; border-width:1px; color:#80817f; display:inline-block; font-size:12px; font-weight:700; letter-spacing:0px; line-height:normal; padding:12px 40px 12px 40px; text-align:center; text-decoration:none; border-style:solid; font-family:inherit;' target='_blank'>Shop Online</a> " +
                                                            "</td> " +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table> " +
                                            "</td> " +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                                "<table class='module' role='module' data-type='spacer' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='9bbc393c-c337-4d1a-b9f9-f20c740a0d44'> " +
                                    "<tbody> " +
                                        "<tr> " +
                                            "<td style='padding:0px 0px 30px 0px;' role='module-content' bgcolor=''> </td> " +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                                "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='20d6cd7f-4fad-4e9c-8fba-f36dba3278fc' data-mc-module-version='2019-10-22'> " +
                                    "<tbody> " +
                                        "<tr> " +
                                            "<td style='padding:40px 30px 40px 30px; line-height:22px; text-align:inherit; background-color:#80817f;' height='100%' valign='top' bgcolor='#80817f' role='module-content'>" +
                                                "<div>" +
                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                        "<span style='color: #ffffff; font-size: 12px'>" +
                                                            "<strong>Thank you for shopping at Beauvais + Lille. If you need to return any items, they need to be returned in its original packaging with proof of purchase. If you do not have a proof of purchase, we can offer you store credit.</strong>" +
                                                        "</span>" +
                                                    "</div> " +
                                                    "<div style='font-family: inherit; text-align: center'><br></div> " +
                                                    "<div style='font-family: inherit; text-align: center'>" +
                                                        "<span style='color: #ffffff; font-size: 12px'>" +
                                                            "<strong>We hope you enjoy our products! </strong>" +
                                                        "</span>" +
                                                    "</div>" +
                                                    "<div></div>" +
                                                "</div>" +
                                            "</td>" +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table>" +
                            "</td>" +
                        "</tr>" +
                    "</tbody>"+
                "</table>"



































                //"<div style='background-color: white'>" +
                //$"<h1 style='color:red'>Hello, {order.ShipToAddress.FirstName}! Thank you for your order!</h1>" +
                //$"<p>Your order #{order.Id} has been received! </p>" +
                //$"<p>Order total {order.Subtotal}.00 $ </p>" +
                //$"<p>You will receive your order at:</p>" +
                //"<div class='d-flex row'>" +
                //$"<p>{order.ShipToAddress.FirstName} {order.ShipToAddress.FirstName} </p>" +
                //$"<p>{order.ShipToAddress.Street}, {order.ShipToAddress.City} </p>" +
                //$"<p>{order.ShipToAddress.State}</p>" +
                //$"<p>{order.ShipToAddress.ZipCode}</p>" +
                //"</div>"
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