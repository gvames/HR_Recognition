using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]  
                // Daca pastram doar [Authorize] call-ul catre http://localhost:8888/api/orders va fi redirectionat catre 
                 // Login() din AccountController ceea ce nu este de dorit atunci cand se face autentificarea in api. Se asteapta 
                 // erarea 401 sau 403 care spune ca trebuie sa fi autentificat sau autorizat

    public class OrdersController : Controller
    {
        private readonly IDutchRepository repo;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly UserManager<StoreUser> userManager;

        public OrdersController(IDutchRepository repo, ILogger<Order> logger, IMapper mapper, UserManager<StoreUser> userManager)
        {
            this.repo = repo;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;
                var result = repo.getAllOrdersByUser(username,includeItems);
                return Ok(mapper.Map<IEnumerable<Order>,IEnumerable<OrderViewModel>>(result));
            }
            catch (Exception ex)
            {

                logger.LogError($"Failed to get orders: {ex}");
                return BadRequest($"Failed to get orders ");
            }
        }
        [HttpGet("{id:int}")]
        public IActionResult GetOrder(int id)
        {
            try
            {
                var username = User.Identity.Name;

                var order = repo.GetOrderById(username,id);
                //  return order != null ? Ok(order) : (IActionResult)NotFound(); // fara mapper
                return order != null ? Ok(mapper.Map<Order,OrderViewModel>(order)) : (IActionResult)NotFound(); // cu mapper

            }
            catch (Exception ex)
            {

                logger.LogError($"Failed to get order having Id: {id} :{ex}");
                return BadRequest($"Failed to get order having Id: { id}");
            }          
            

        }
      

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]OrderViewModel model)
        // by default, modelul primit in controller este setat in query string-ul din url
        // ex: -http://localhost:8888/api/orders?orderdate=2018-05-05&ordernumber=255
        // pentru a citi parametrii trimisi in request din body-ul mesajului, trebuie 
        // adaugat un atributul [FromBody] in fata tipului modelului pe care il asteapta controller-ul.
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var newOrder = mapper.Map<OrderViewModel, Order>(model);                  

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    // insinte de a adauga entitatea in baza de date 
                    //
                    var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                    newOrder.User = currentUser;

                    // add it to the db
                    repo.AddEntity(newOrder);
                    if (repo.SaveAll())
                    {
                        var vm = mapper.Map<Order, OrderViewModel>(newOrder);
                        return Created($"/api/orders/{vm.OrderID}", vm);
                    }
                                                         
                    // In HTTP, cand se trimite un request de tip POST, cerinta este ca daca s-a creeat 
                    // un nou obiect, acest obiect trebuie returnat
                    // In caz de POST nu se mai returneaza OK(200) ci se returneaza Created (201)
                }
                else
                {
                    return BadRequest(ModelState);
                    // se trimite ca si raspuns motivul pentru care modelul nu este valid
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"The post operation failed: {ex}");              
            }

            return BadRequest($"The post operation failed");

        }
    }
}
