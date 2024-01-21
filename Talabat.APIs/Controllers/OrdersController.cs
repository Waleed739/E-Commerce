﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggreation;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderSevices orderSevices;
        private readonly IMapper mapper;

        public OrdersController(IOrderSevices orderSevices,IMapper mapper)
        {
            this.orderSevices = orderSevices;
            this.mapper = mapper;
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponses), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var address = mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

            var order = await orderSevices.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

            if (order is null)
                return BadRequest(new ApiErrorResponses(400));
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await orderSevices.GetOrdersForUserAsync(buyerEmail);

            return Ok(orders);
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponses), StatusCodes.Status404NotFound)]
        [HttpGet("id")]
        public async Task<ActionResult<Order>> GetOrderByIdForUser(int id)
        {
            var buyerEmail =  User.FindFirstValue(ClaimTypes.Email);

            var order = await orderSevices.GetOrderByIdForUserAsync(buyerEmail, id);

            if (order is null)
                return NotFound(new ApiErrorResponses(404));

            return Ok(order); 
        }

        [HttpGet("deliverymethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethodS()
        {
            var deliverymethods = await orderSevices.GetAllDeliveryMethodsAsync();
            return Ok(deliverymethods);
        }


    }
}
