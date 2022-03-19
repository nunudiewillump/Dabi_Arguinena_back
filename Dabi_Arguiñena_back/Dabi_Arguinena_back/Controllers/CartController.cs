
using Dabi_Arguinena_back.Context;
using Dabi_Arguinena_back.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dabi_Arguiena_back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly BlogContext _blogContext;
        private readonly ILogger _logger;

        public CartController(ILogger<CartController> logger, BlogContext blogContext)
        {
            _blogContext = blogContext;
            _logger = logger;
        }

        // GET: Carts
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Cart>> Get()
        {
            _logger.LogInformation("GET all posts");

            return Ok(_blogContext.Carts);
        }

        // GET: Cart/5
        [HttpGet("{id}", Name = "GetCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Cart> Get(int id)
        {
            _logger.LogInformation($"GET cart {id}");
            var cart = _blogContext.Carts.FirstOrDefault(cart => cart.Id == id);
            return Ok(cart);

        }

        // POST: Cart
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Post([FromBody] Cart value)
        {
            _logger.LogInformation($"POST cart {JsonConvert.SerializeObject(value)}");

            if (_blogContext.Carts.Any())
                value.Id = _blogContext.Carts.Max(p => p.Id) + 1;
            else
               value.Id = 1;

            if (value.ProductId.HasValue && !_blogContext.Products.Any(c => c.Id == value.ProductId))
                return ValidationProblem($"Product with id {value.ProductId} not found");

            _blogContext.Carts.Add(value);
            _blogContext.SaveChanges();

            return Ok(value.Id);
        }

        // PUT: Carts/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Put(int id, [FromBody] Cart value)
        {
            _logger.LogInformation($"PUT post {JsonConvert.SerializeObject(value)}");

            var postToUpdate = _blogContext.Carts.FirstOrDefault(p => p.Id == id);

            if (postToUpdate == null)
                return ValidationProblem($"Cart with id {id} not found");

            if (value.ProductId.HasValue && !_blogContext.Products.Any(c => c.Id == value.Id))
                return ValidationProblem($"Products with id {value.Id} not found");

            value.Id = id;
            _blogContext.Carts.Update(value);
            _blogContext.SaveChanges();
            return Ok();
        }

        // DELETE: Carts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE cart {id}");

            var postToDelete = _blogContext.Carts.FirstOrDefault(p => p.Id == id);
            if (postToDelete == null)
                return ValidationProblem($"Cart with id {id} not found");

            _blogContext.Carts.Remove(postToDelete);
            _blogContext.SaveChanges();
            return Ok();
        }
    }
}
