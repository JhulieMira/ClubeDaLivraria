using Livraria.Business.Models;

namespace Livraria.Business.Interfaces
{
    public interface ILivroRepository : IRepository<Livro>
    {
        Task<IEnumerable<Livro>> ObterLivrosPorFornecedor(Guid fornecedorId); 
        Task<IEnumerable<Livro>> ObterLivrosPorAutor(Guid autorId);
        Task<IEnumerable<Livro>> ObterLivrosAutoresEFornecedores();
        Task<Livro> ObterLivroAutorEFornecedor(Guid id); 
    }
}
