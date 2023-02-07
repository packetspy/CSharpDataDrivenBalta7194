using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        HttpGet]
        [Route("{publicId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("categories/id:int")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .Where(x => x.Category.Id == id)
            .ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("qlqcoisa/publicId:int")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .Where(x => x.Category.Id == id)
            .ToListAsync();

            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Product>> Post([FromBody] Product model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o produto." });
            }
        }
    }
}