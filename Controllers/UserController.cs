using Shop.Services;
using Shop.Data;
using Shop.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Shop.Controllers
{
    [Route("v1/users")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context
                .Users
                .AsNoTracking()
                .ToListAsync();
            return users;
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Product>> Post(
           [FromServices] DataContext context,
           [FromBody] User model
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Força o usuário a ser sempre "funcionário"
                model.Role = "employee";

                model.Password = TokenService.HashPassword(model.Password);

                context.Users.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(
            [FromServices] DataContext context,
            [FromBody]User model,
            int id)
        {
            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o ID informado é o mesmo do modelo
            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrado" });

            try
            {
                model.Password = TokenService.HashPassword(model.Password);
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new { message = "Usuário atualizado." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar o usuário" });

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromServices] DataContext context,
            [FromBody] User model
        )
        {
            string passwordWithHash = TokenService.HashPassword(model.Password);
            var user = await context.Users
            .AsNoTracking()
            .Where(x => x.Username == model.Username && x.Password == passwordWithHash)
            .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senhas inválido" });

            var token = TokenService.GenerateToken(user);
            return new
            {
                token = token
            };
        }

    }
}