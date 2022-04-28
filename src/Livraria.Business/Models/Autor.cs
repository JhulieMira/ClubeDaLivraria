namespace Livraria.Business.Models
{
    public class Autor : Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }

        public IEnumerable<Livro> Livros { get; set; }
    }
}
