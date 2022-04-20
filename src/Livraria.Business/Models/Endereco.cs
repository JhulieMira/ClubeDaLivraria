namespace Livraria.Business.Models
{
    public class Endereco : Entity
    {
        public Guid FornecedorId { get; set; }

        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        /* EF Relation */
        public Fornecedor Fornecedor { get; set; } 
    }
}
//colocando o nome da entidadeID e fazendo uma relaçção de "tem-um" o EF ja entende que o ID é a ForeingKey e faz esse relacionamento automaticamente