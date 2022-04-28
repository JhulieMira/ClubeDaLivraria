using Livraria.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livraria.Data.Mappings
{
    public class LivroMapping : IEntityTypeConfiguration<Livro> 
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.HasKey(p => p.Id); 

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(p => p.Imagem)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.ToTable("Livros");
        }
    }

}
