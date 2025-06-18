using Microsoft.EntityFrameworkCore;

namespace DWebProjetoFinal.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Entidades existentes
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        // Novas entidades para Planeamento
        public DbSet<Meta> Metas { get; set; }
        public DbSet<Orcamento> Orcamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações para Orcamento
            modelBuilder.Entity<Orcamento>()
                .HasIndex(o => new { o.UserId, o.Categoria, o.Mes, o.Ano })
                .IsUnique(); // Evita duplicatas por usuário/categoria/mês

            // Configuração opcional para Meta
            modelBuilder.Entity<Meta>()
                .Property(m => m.ValorAtual)
                .HasDefaultValue(0); // Valor inicial padrão

            base.OnModelCreating(modelBuilder);
        }
    }
}