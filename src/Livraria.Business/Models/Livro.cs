namespace Livraria.Business.Models
{
    public class Livro : Entity
    {
        public Guid FornecedorId { get; set; }  
        public Guid AutorId { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public Fornecedor Fornecedor { get; set; }
        public Autor Autor { get; set; }
    }
}
