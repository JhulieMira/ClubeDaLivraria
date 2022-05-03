using Livraria.Business.Interfaces;
using Livraria.Business.Models;
using Livraria.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Data.Repository
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(LivrariaDbContext context) : base(context)
        {

        }

        public async Task<Autor> ObterPorId(Guid id)
        {
            return await Db.Autores.Include(a => a.Livros).FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
