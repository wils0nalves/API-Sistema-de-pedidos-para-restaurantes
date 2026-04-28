using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<PedidoHub> _hub;

        public PedidoController(AppDbContext context, IHubContext<PedidoHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        // 🔹 1. Abrir pedido
        [HttpPost("abrir")]
        public async Task<ActionResult> AbrirPedido(int mesaId, int usuarioId)
        {
            var mesaOcupada = await _context.Pedidos
                .AnyAsync(p => p.MesaId == mesaId && p.Status == "Aberto");

            if (mesaOcupada)
                return BadRequest("Mesa já está ocupada");

            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return BadRequest("Usuário inválido");

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

        // 🔹 2. Adicionar item
        [HttpPost("adicionar-item")]
        public async Task<ActionResult> AdicionarItem(int pedidoId, int produtoId, int quantidade)
        {
            if (quantidade <= 0)
                return BadRequest("Quantidade inválida");

            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
                return NotFound("Pedido não encontrado");

            if (pedido.Status != "Aberto")
                return BadRequest("Pedido não está aberto");

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

            await _hub.Clients.All.SendAsync("novoPedido");

            return Ok(item);
        }

        // 🔹 3. Buscar pedido por mesa
        [HttpGet("mesa/{mesaId}")]
        public async Task<ActionResult> GetPorMesa(int mesaId)
        {
            var pedido = await _context.Pedidos
                .Where(p => p.MesaId == mesaId && p.Status == "Aberto")
                .FirstOrDefaultAsync();

            if (pedido == null)
                return NotFound("Nenhum pedido aberto para essa mesa");

            var itens = await _context.PedidoItens
                .Where(i => i.PedidoId == pedido.Id)
                .Include(i => i.Produto)
                .ToListAsync();

            return Ok(new
            {
                Pedido = pedido,
                Itens = itens
            });
        }

        // 🔹 4. Listar itens
        [HttpGet("{pedidoId}/itens")]
        public async Task<ActionResult> ListarItens(int pedidoId)
        {
            var itens = await _context.PedidoItens
                .Where(i => i.PedidoId == pedidoId)
                .Include(i => i.Produto)
                .ToListAsync();

            return Ok(itens);
        }

        // 🔹 5. Total
        [HttpGet("{pedidoId}/total")]
        public async Task<ActionResult> CalcularTotal(int pedidoId)
        {
            var itens = await _context.PedidoItens
                .Where(i => i.PedidoId == pedidoId)
                .ToListAsync();

            if (!itens.Any())
                return BadRequest("Pedido sem itens");

            var total = itens.Sum(i => i.Quantidade * i.PrecoUnitario);

            return Ok(total);
        }

        // 🔹 6. Fechar pedido
        [HttpPost("fechar")]
        public async Task<ActionResult> FecharPedido(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);

            if (pedido == null)
                return NotFound();

            if (pedido.Status != "Aberto")
                return BadRequest("Pedido não está aberto");

            pedido.Status = "Fechado";

            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        // 🔹 7. Cozinha
        [HttpGet("cozinha")]
        public async Task<ActionResult> PedidosCozinha()
        {
            var itens = await _context.PedidoItens
                .Where(i => i.Status == "Pendente" || i.Status == "Em preparo")
                .Include(i => i.Produto)
                .ToListAsync();

            return Ok(itens);
        }

        // 🔹 8. Atualizar status item
        [HttpPost("item/status")]
        public async Task<ActionResult> AtualizarStatusItem(int itemId, string status)
        {
            var item = await _context.PedidoItens.FindAsync(itemId);

            if (item == null)
                return NotFound();

            item.Status = status;

            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("novoPedido");

            return Ok(item);
        }

        // 🔹 9. Pagar pedido
        [HttpPost("pagar")]
        public async Task<ActionResult> PagarPedido(int pedidoId, string tipo)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);

            if (pedido == null)
                return NotFound();

            if (pedido.Status != "Fechado")
                return BadRequest("Feche o pedido antes de pagar");

            var itens = await _context.PedidoItens
                .Where(i => i.PedidoId == pedidoId)
                .ToListAsync();

            if (!itens.Any())
                return BadRequest("Pedido vazio");

            var total = itens.Sum(i => i.Quantidade * i.PrecoUnitario);

            var pagamento = new Pagamento
            {
                PedidoId = pedidoId,
                Valor = total,
                Tipo = tipo,
                DataPagamento = DateTime.Now
            };

            pedido.Status = "Pago";

            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            return Ok(pagamento);
        }

        // 🔹 10. Relatório
        [HttpGet("relatorio/diario")]
        public async Task<ActionResult> FaturamentoDiario()
        {
            var hoje = DateTime.Today;

            var total = await _context.Pagamentos
                .Where(p => p.DataPagamento >= hoje)
                .SumAsync(p => p.Valor);

            return Ok(total);
        }

        // 🔹 11. Remover item
        [HttpDelete("item/{itemId}")]
        public async Task<ActionResult> RemoverItem(int itemId)
        {
            var item = await _context.PedidoItens.FindAsync(itemId);

            if (item == null)
                return NotFound("Item não encontrado");

            _context.PedidoItens.Remove(item);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("novoPedido");

            return Ok();
        }

        // 🔹 12. Fechar mesa (NOVO)
        [HttpPost("fechar-mesa")]
        public async Task<IActionResult> FecharMesa(int mesaId)
        {
            var pedidos = await _context.Pedidos
                .Where(p => p.MesaId == mesaId && p.Status == "Aberto")
                .ToListAsync();

            if (!pedidos.Any())
                return BadRequest("Nenhum pedido aberto para essa mesa");

            foreach (var p in pedidos)
            {
                p.Status = "Fechado";
            }

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Mesa fechada com sucesso" });
        }
    }
}