using Microsoft.EntityFrameworkCore;
using PizzariaAPI.Models;

namespace PizzariaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        // 👇 TEM QUE FICAR AQUI DENTRO
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>().ToTable("Pedido");
            modelBuilder.Entity<PedidoItem>().ToTable("PedidoItem");
            modelBuilder.Entity<Mesa>().ToTable("Mesa");
            modelBuilder.Entity<Produto>().ToTable("Produto");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Pagamento>().ToTable("Pagamento");
        }
    }
}