using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Business.Models
{
    public class Livro : Entity
    {
        public Guid FornecedorId { get; set; } //Chave estrangeira, significa que esse produto pertence a um fornecedor //Serve para uma relação no db

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        /* EF Relation */
        public Fornecedor Fornecedor { get; set; } //esse produto tem um fornecedor
    }
}
