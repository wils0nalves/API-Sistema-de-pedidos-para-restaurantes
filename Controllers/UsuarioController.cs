using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaAPI.Data;
using PizzariaAPI.Models;

namespace PizzariaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO dados)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Login == dados.Login &&
                    u.Senha == dados.Senha &&
                    u.Ativo == true);

            if (user == null)
                return Unauthorized("Usuário ou senha inválidos");

            return Ok(user);
        }
    }
}