using Livraria.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Data.Mappings //mapeando para o banco de dados. Nao é necessario mapear valores, nem bool 
{
    public class LivroMapping : IEntityTypeConfiguration<Livro> // a classe que possui outras filhas é que configura o mapeamento
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.HasKey(p => p.Id); //configurando a chave primaria sendo o id

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(p => p.Imagem)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.ToTable("Livros"); //é possivel adicionar uma virgula e em seguida o nome do schema 
        }
    }

}
