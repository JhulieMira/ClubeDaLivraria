﻿// <auto-generated />
using System;
using Livraria.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Livraria.Data.Migrations
{
    [DbContext(typeof(LivrariaDbContext))]
    [Migration("20220420134928_AdicionandoAutor")]
    partial class AdicionandoAutor
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Livraria.Business.Models.Autor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Imagem")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Autores", (string)null);
                });

            modelBuilder.Entity("Livraria.Business.Models.Endereco", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("varchar(8)");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Complemento")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<Guid>("FornecedorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("FornecedorId")
                        .IsUnique();

                    b.ToTable("Enderecos", (string)null);
                });

            modelBuilder.Entity("Livraria.Business.Models.Fornecedor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Documento")
                        .IsRequired()
                        .HasColumnType("varchar(14)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<int>("TipoFornecedor")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Fornecedores", (string)null);
                });

            modelBuilder.Entity("Livraria.Business.Models.Livro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("AutorId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<Guid>("FornecedorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Imagem")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.HasIndex("FornecedorId");

                    b.ToTable("Livros", (string)null);
                });

            modelBuilder.Entity("Livraria.Business.Models.Endereco", b =>
                {
                    b.HasOne("Livraria.Business.Models.Fornecedor", "Fornecedor")
                        .WithOne("Endereco")
                        .HasForeignKey("Livraria.Business.Models.Endereco", "FornecedorId")
                        .IsRequired();

                    b.Navigation("Fornecedor");
                });

            modelBuilder.Entity("Livraria.Business.Models.Livro", b =>
                {
                    b.HasOne("Livraria.Business.Models.Autor", "Autor")
                        .WithMany("Livros")
                        .HasForeignKey("AutorId")
                        .IsRequired();

                    b.HasOne("Livraria.Business.Models.Fornecedor", "Fornecedor")
                        .WithMany("Livros")
                        .HasForeignKey("FornecedorId")
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Fornecedor");
                });

            modelBuilder.Entity("Livraria.Business.Models.Autor", b =>
                {
                    b.Navigation("Livros");
                });

            modelBuilder.Entity("Livraria.Business.Models.Fornecedor", b =>
                {
                    b.Navigation("Endereco")
                        .IsRequired();

                    b.Navigation("Livros");
                });
#pragma warning restore 612, 618
        }
    }
}
