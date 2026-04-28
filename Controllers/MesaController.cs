using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaAPI.Data;
using PizzariaAPI.Models;

namespace PizzariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MesaController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 1. Listar todas as mesas (debug / admin)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mesa>>> GetMesas()
        {
            return await _context.Mesas.ToListAsync();
        }

        // 🔹 2. Listar somente mesas livres (REGRA CORRETA)
        [HttpGet("livres")]
        public async Task<ActionResult> GetMesasLivres()
        {
            var mesasLivres = await _context.Mesas
                .Where(m => !_context.Pedidos
                    .Any(p => p.MesaId == m.Id && p.Status == "Aberto"))
                .OrderBy(m => m.Numero)
                .ToListAsync();

            return Ok(mesasLivres);
        }

        // 🔹 3. Listar mesas ocupadas (EXTRA - útil pra painel)
        [HttpGet("ocupadas")]
        public async Task<ActionResult> GetMesasOcupadas()
        {
            var mesasOcupadas = await _context.Mesas
                .Where(m => _context.Pedidos
                    .Any(p => p.MesaId == m.Id && p.Status == "Aberto"))
                .OrderBy(m => m.Numero)
                .ToListAsync();

            return Ok(mesasOcupadas);
        }
        [HttpGet("status")]
        public async Task<IActionResult> GetMesasComStatus()
        {
            var mesas = await _context.Mesas.ToListAsync();

            var resultado = mesas.Select(m => new
            {
                m.Id,
                m.Numero,
                Status = _context.Pedidos
                    .Any(p => p.MesaId == m.Id && p.Status == "Aberto")
                    ? "Ocupada"
                    : "Livre"
            });

            return Ok(resultado);
        }
    }
}