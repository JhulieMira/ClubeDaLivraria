using Livraria.Business.Interfaces;
using Livraria.Business.Models;
using Livraria.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Data.Repository
{
    public class LivroRepository : Repository<Livro>, ILivroRepository
    {
        public LivroRepository(LivrariaDbContext context) : base(context){ }

        public async Task<Livro> ObterLivroAutorEFornecedor(Guid id)
        {
            return await Db.Livros.AsNoTracking()
                .Include(f => f.Fornecedor)
                .Include(a => a.Autor)
                .FirstOrDefaultAsync(predicate: p => p.Id == id);
        }

        public async Task<IEnumerable<Livro>> ObterLivrosAutoresEFornecedores()
        {
            return await Db.Livros.AsNoTracking()
                .Include(f => f.Fornecedor)
                .Include(a => a.Autor)
                .OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Livro>> ObterLivrosPorAutor(Guid autorId)
        {
            return await Buscar(a => a.AutorId == autorId);
        }

        public async Task<IEnumerable<Livro>> ObterLivrosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(l => l.FornecedorId == fornecedorId);
        }
    }
}
