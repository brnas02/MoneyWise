using Microsoft.EntityFrameworkCore;

namespace DWebProjetoFinal.Entities
{
    public class AppDbContext : DbContext
    {
        // Construtor que recebe as opções de configuração do DbContext (string de conexão, etc.)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ------------------ DbSets (tabelas no banco de dados) ------------------

        // Tabela de utilizadores (contas de login)
        public DbSet<UserAccount> UserAccounts { get; set; }

        // Tabela de roles
        public DbSet<Role> Roles { get; set; }

        // Tabela de transações financeiras (receitas/despesas)
        public DbSet<Transacao> Transacoes { get; set; }

        // Tabela de metas financeiras
        public DbSet<Meta> Metas { get; set; }

        // Tabela de orçamentos mensais
        public DbSet<Orcamento> Orcamentos { get; set; }

        // Tabela de Transacoes de users
        public DbSet<UserTransacao> UserTransacao { get; set; }

        // Tabela de segurança de utilizadores (passwords)
        public DbSet<UserSeguranca> UserSeguranca { get; set; }

        // ------------------ Configurações adicionais com Fluent API ------------------

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cria as roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Type = "Pessoal" },
                new Role { Id = 2, Type = "Empresarial" },
                new Role { Id = 3, Type = "Admin" }
            );

            modelBuilder.Entity<UserTransacao>()
                .HasKey(ut => new { ut.UserAccountId, ut.TransacaoId });

            modelBuilder.Entity<UserTransacao>()
                .HasOne(ut => ut.UserAccount)
                .WithMany(u => u.UserTransacoes)
                .HasForeignKey(ut => ut.UserAccountId);

            modelBuilder.Entity<UserTransacao>()
                .HasOne(ut => ut.Transacao)
                .WithMany(t => t.UserTransacoes)
                .HasForeignKey(ut => ut.TransacaoId);

            // Configura índice único para garantir que um utilizador não tenha mais de um orçamento
            // para a mesma categoria, mês e ano
            modelBuilder.Entity<Orcamento>()
                .HasIndex(o => new { o.UserId, o.Categoria, o.Mes, o.Ano })
                .IsUnique();

            // Define o valor padrão inicial de ValorAtual como 0 nas metas
            modelBuilder.Entity<Meta>()
                .Property(m => m.ValorAtual)
                .HasDefaultValue(0);

            // Chamada da implementação base
            base.OnModelCreating(modelBuilder);
        }
    }
}