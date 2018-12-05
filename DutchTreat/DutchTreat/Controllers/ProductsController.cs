using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController] //Asta va documeta controller-ul
    [Produces("applicationb/json")]
    public class ProductsController : Controller
    {
        private readonly IDutchRepository repo;
        private readonly ILogger<Product> logger;

        public ProductsController(IDutchRepository repo, ILogger<Product> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)] //in scopul de a documenta aceasta metoda 
        [ProducesResponseType(400)]
        public IActionResult Get()
        {
            try
            {
                return Ok(repo.GetAllProducts());
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get products: {ex}");
                return BadRequest("Failed to get products");
            }
          
        }
            

    }
}
