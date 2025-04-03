﻿using Microsoft.EntityFrameworkCore;

namespace DWebProjetoFinal.Entities {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserAccount>().Property(u => u.Role).IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}