using Livraria.Business.Interfaces;
using Livraria.Business.Models;
using Livraria.Data.Context;

namespace Livraria.Data.Repository
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(LivrariaDbContext context) : base(context)
        {

        }

        public virtual async Task<Autor> ObterPorId(Guid id)
        {
            return await Db.Autores.FindAsync(id);
        }
    }
}
