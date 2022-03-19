using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dabi_Arguinena_back.Context;
using Dabi_Arguinena_back.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dabi_Arguinena_back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly BlogContext _blogContext;
        private readonly ILogger _logger;


        public CategoryController(ILogger<ProductController> logger, BlogContext blogContext)
        {
            _blogContext = blogContext;
            _logger = logger;
        }


        // GET: Category
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Products>> Get()
        {
            _logger.LogInformation("GET all categories");

            return Ok(_blogContext.Categories);
        }

        // GET: Category/5
        [HttpGet("{id}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Products> Get(int id)
        {
            _logger.LogInformation($"GET category {id}");
            return Ok(_blogContext.Categories);

        }


        // POST: Category
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Post([FromBody] Category value)
        {
            _logger.LogInformation($"POST category {JsonConvert.SerializeObject(value)}");

            if (_blogContext.Products.Any())
                value.Id = _blogContext.Categories.Max(p => p.Id) + 1;
            else
                value.Id = 1;
            _blogContext.Categories.Add(value);
            _blogContext.SaveChanges();

            return Ok(value.Id);
        }

        // PUT: Category/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Put(int id, [FromBody] Category value)
        {
            _logger.LogInformation($"PUT post {JsonConvert.SerializeObject(value)}");

            var postToUpdate = _blogContext.Categories.FirstOrDefault(p => p.Id == id);

            if (postToUpdate == null)
                return ValidationProblem($"Category with id {id} not found");

            if (!_blogContext.Categories.Any(c => c.Id == id))
                return ValidationProblem($"Category with id {id} not found");

            Category newcategory = new Category { Id = id, CategoryName = postToUpdate.CategoryName, Description = postToUpdate.Description };


            value.Id = id;
            _blogContext.Categories.Remove(postToUpdate);
            _blogContext.Categories.Add(value);
            _blogContext.SaveChanges();
            return Ok();
        }

        // DELETE: Category/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE category {id}");

            var categoryToDelete = _blogContext.Categories.FirstOrDefault(p => p.Id == id);
            if (categoryToDelete == null)
                return ValidationProblem($"category with id {id} not found");

            _blogContext.Categories.Remove(categoryToDelete);
            _blogContext.SaveChanges();
            return Ok();
        }
    }
}
