using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaAPI.Data;
using PizzariaAPI.Models;

namespace PizzariaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 1. Abrir pedido
        [HttpPost("abrir")]
        public async Task<ActionResult> AbrirPedido(int mesaId, int usuarioId)
        {
            var pedido = new Pedido
            {
                MesaId = mesaId,
                UsuarioId = usuarioId,
                Status = "Aberto",
                DataAbertura = DateTime.Now
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        // 🔹 2. Adicionar item ao pedido
        [HttpPost("adicionar-item")]
        public async Task<ActionResult> AdicionarItem(int pedidoId, int produtoId, int quantidade)
        {
            var produto = await _context.Produtos.FindAsync(produtoId);

            if (produto == null)
                return NotFound("Produto não encontrado");

            var item = new PedidoItem
            {
                PedidoId = pedidoId,
                ProdutoId = produtoId,
                Quantidade = quantidade,
                PrecoUnitario = produto.Preco,
                Status = "Pendente"
            };

            _context.PedidoItens.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // 🔹 3. Ver pedido por mesa
        [HttpGet("mesa/{mesaId}")]
        public async Task<ActionResult> GetPorMesa(int mesaId)
        {
            var pedido = await _context.Pedidos
                .Where(p => p.MesaId == mesaId && p.Status != "Pago")
                .Include(p => p.Id)
                .FirstOrDefaultAsync();

            if (pedido == null)
                return NotFound("Nenhum pedido aberto para essa mesa");

            var itens = await _context.PedidoItens
                .Where(i => i.PedidoId == pedido.Id)
                .Include(i => i.ProdutoId)
                .ToListAsync();

            return Ok(new
            {
                Pedido = pedido,
                Itens = itens
            });
        }
    }
}