using Livraria.Business.Models;

namespace Livraria.Business.Services
{
    public interface ILivroService : IDisposable
    {
        Task Adicionar(Livro livro);

        Task Atualizar(Livro livro);

        Task Remover(Guid id);
    }
}
