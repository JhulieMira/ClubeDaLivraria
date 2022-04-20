using Livraria.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livraria.Data.Mappings //mapeando para o banco de dados. Nao é necessario mapear valores, nem bool 
{
    internal class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasKey(p => p.Id); //configurando a chave primaria sendo o id

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Documento)
                .IsRequired()
                .HasColumnType("varchar(14)");

            //1 : 1 => Fornecedor : Endereço

            builder.HasOne(f => f.Endereco) //um fornecedor tem um endereço, 
                .WithOne(e => e.Fornecedor); //um endereço tem um fornecedor

            // 1 : N  => Fornecedor : Produtos

            builder.HasMany(f => f.Livros)
                .WithOne(p => p.Fornecedor)
                .HasForeignKey(p => p.FornecedorId);

            builder.ToTable("Fornecedores"); //é possivel adicionar uma virgula e em seguida o nome do schema 
        }
    }

}
