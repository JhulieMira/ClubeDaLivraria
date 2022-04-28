using Livraria.Business.Models;

namespace Livraria.Business.Services
{
    public interface IAutorService : IDisposable
    {
        Task Adicionar(Autor autor);
        Task Atualizar(Autor autor);
        Task Remover(Guid id);
    }
}
