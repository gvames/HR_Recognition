using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/Orders/{orderid}/Items")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository repo;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public OrderItemsController(IDutchRepository repo,
            ILogger<OrderItemsController> logger,
            IMapper mapper)
        {
            this.repo = repo;
            this.logger = logger;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetOrderItems(int orderid)
        {
            try
            {
                var order = repo.GetOrderById(User.Identity.Name,orderid);
                if (order != null)
                {
                    return Ok(mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Calling GetOrderItems has failed: {ex}");
                return BadRequest($"Calling GetOrderItems({orderid}) has failed:");
            }

            return BadRequest("Calling GetOrderItems has failed");
        }

        [HttpGet("{ItemId}")]
        public IActionResult GetOrderItems(int orderid, int itemid)
        {
            try
            {
                var order = repo.GetOrderById(User.Identity.Name,orderid);
                if (order != null)
                {
                    var item = order.Items.Where(i => i.Id == itemid).FirstOrDefault();
                    if (item != null)
                    {
                        return Ok(mapper.Map<OrderItem, OrderItemViewModel>(item));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Calling GetOrderItems has failed: {ex}");
                return BadRequest($"Calling GetOrderItems({orderid}) has failed:");
            }

            return BadRequest("Calling GetOrderItems has failed");
        }
    }


}

