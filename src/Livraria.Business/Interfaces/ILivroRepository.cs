using Livraria.Business.Models;

namespace Livraria.Business.Interfaces
{
    public interface ILivroRepository : IRepository<Livro>
    {
        Task<IEnumerable<Livro>> ObterLivrosPorFornecedor(Guid fornecedorId); //Retorna uma lista de produtos por fornecedor
        Task<IEnumerable<Livro>> ObterLivrosFornecedores(); //Lista de produtos e fornecedores deste produto
        Task<Livro> ObterLivroFornecedor(Guid id); //Retorna um livro e o fornecedor dele
    }
}
