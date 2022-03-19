using Dabi_Arguinena_back.Context;
using Dabi_Arguinena_back.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dabi_Arguinena_back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly BlogContext _blogContext;
        private readonly ILogger _logger;


        public ProductController(ILogger<ProductController> logger, BlogContext blogContext)
        {
            _blogContext = blogContext;
            _logger = logger;
        }

 
        // GET: Products
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Products>> Get()
        {
            _logger.LogInformation("GET all products");

            return Ok(_blogContext.Products);
        }

        // GET: Product/5
        [HttpGet("{id}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Products> Get(int id)
        {
            _logger.LogInformation($"GET product {id}");
            return Ok(_blogContext.Products);

        }

        // GET: Product/Category/5
        [HttpGet("Category/{id}", Name = "GetProductByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Products> GetByCategoryId(int categoryId)
        {
            _logger.LogInformation($"GET product by Category{categoryId}");
            return Ok(_blogContext.Products.FirstOrDefault(p => p.CategoryId == categoryId));

        }

        // POST: Product
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Post([FromBody] Products value)
        {
            _logger.LogInformation($"POST product {JsonConvert.SerializeObject(value)}");

            if (_blogContext.Products.Any())
                value.Id = _blogContext.Products.Max(p => p.Id) + 1;
            else
                value.Id = 1;
            _blogContext.Products.Add(value);
            _blogContext.SaveChanges();

            return Ok(value.Id);
        }

        // PUT: Product/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Put(int id, [FromBody] Products value)
        {
            _logger.LogInformation($"PUT post {JsonConvert.SerializeObject(value)}");

            var postToUpdate = _blogContext.Products.FirstOrDefault(p => p.Id == id);

            if (postToUpdate == null)
                return ValidationProblem($"Product with id {id} not found");

            if (!_blogContext.Products.Any(c => c.Id == id))
                return ValidationProblem($"Products with id {id} not found");

            Products newproduct = new Products{Id = id, Name = postToUpdate.Name, Price = postToUpdate.Price, CategoryId = postToUpdate.CategoryId, Description = postToUpdate.Description};
           
            
            value.Id = id;
            _blogContext.Products.Remove(postToUpdate);
            _blogContext.Products.Add(value);
            _blogContext.SaveChanges();
            return Ok();
        }

        // DELETE: Product/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE Product {id}");

            var productDelete = _blogContext.Products.FirstOrDefault(p => p.Id == id);
            if (productDelete == null)
                return ValidationProblem($"Product with id {id} not found");

            _blogContext.Products.Remove(productDelete);
            _blogContext.SaveChanges();
            return Ok();
        }
    }
}
