﻿using Microsoft.EntityFrameworkCore;

namespace DWebProjetoFinal.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>().Property(u => u.Type).IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}
