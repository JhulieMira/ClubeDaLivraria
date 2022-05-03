using Livraria.Business.Models;
using System.Linq.Expressions;

namespace Livraria.Business.Interfaces
{
    public interface IAutorRepository : IRepository<Autor>
    {
        public Task<Autor> ObterPorId(Guid id);
    }
}
