using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Core.Data;
using ECommerce.Core.Entities;
using ECommerce.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.WebAPI.Controllers
{
    /// <summary>Product catelog APIs, should provide Authorization Bearer token in the header
    /// 
    /// </summary>
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : AuthController
    {
        private readonly IECommerceRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;
        private readonly ITrackOrderService _trackOrderService;

        public OrdersController(IECommerceRepository repository, ILogger<OrdersController> logger, 
            IMapper mapper, UserManager<StoreUser> userManager, ITrackOrderService trackOrderService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _trackOrderService = trackOrderService;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;

                var results = _repository.GetAllOrdersByUser(username, includeItems);

                var data = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, id);

                if (order != null)
                {
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderCreateModel, Order>(model);

                    newOrder.UserId = CurrentUserId;

                    _repository.AddOrder(newOrder);
                    if (await _repository.SaveAllAsync())
                    {
                       
                        newOrder = _repository.GetOrderByUserId(CurrentUserId, newOrder.Id);

                        await _trackOrderService.ProcessNewOder(newOrder);

                        var orderVm = _mapper.Map<Order, OrderViewModel>(newOrder);
                        return Created($"/api/orders/{orderVm.OrderId}", orderVm);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new order: {ex}");
            }

            return BadRequest("Failed to save new order");
        }
    }
}
