using Livraria.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Data.Context
{
    public class LivrariaDbContext : DbContext
    {
        public LivrariaDbContext(DbContextOptions<LivrariaDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //aplicando os mappings 
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes() //criando uma garantia caso esqueça de mapear alguma entidaded
                .SelectMany(e => e.GetProperties()   //fazendo uma query nas entidades, selecionando pelo tipo
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LivrariaDbContext).Assembly); //Acima ele vai pegar o db context, buscar todas as entidades que estao mapeadas nele, e buscar classes que herdem de EntityTypeConfiguration para as entidades que estao relacionadas no dbcontext(nesse caso livro, endereco e fornecedor) e registra-las

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;  //desabilitando o delete cascade

            base.OnModelCreating(modelBuilder);
        }
    }
}
