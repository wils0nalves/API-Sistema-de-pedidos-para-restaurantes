using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaAPI.Data;
using PizzariaAPI.Models;

namespace PizzariaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 1. Listar todos ativos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.Produtos
                .Where(p => p.Ativo)
                .ToListAsync();

            return Ok(produtos);
        }

        // 🔹 2. Buscar por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetById(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        // 🔹 3. Listar por categoria
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetPorCategoria(int categoriaId)
        {
            var produtos = await _context.Produtos
                .Where(p => p.CategoriaId == categoriaId && p.Ativo)
                .ToListAsync();

            return Ok(produtos);
        }

        // 🔹 4. Criar produto
        [HttpPost]
        public async Task<ActionResult> Post(Produto produto)
        {
            if (string.IsNullOrEmpty(produto.Nome))
                return BadRequest("Nome é obrigatório");

            if (produto.Preco <= 0)
                return BadRequest("Preço inválido");

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
        }

        // 🔹 5. Atualizar produto
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Produto produto)
        {
            var existente = await _context.Produtos.FindAsync(id);

            if (existente == null)
                return NotFound();

            existente.Nome = produto.Nome;
            existente.Preco = produto.Preco;
            existente.CategoriaId = produto.CategoriaId;
            existente.Ativo = produto.Ativo;

            await _context.SaveChangesAsync();

            return Ok(existente);
        }

        // 🔹 6. "Deletar" (soft delete)
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound();

            produto.Ativo = false;

            await _context.SaveChangesAsync();

            return Ok("Produto desativado");
        }
    }
}