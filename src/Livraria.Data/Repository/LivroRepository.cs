using Livraria.Business.Interfaces;
using Livraria.Business.Models;
using Livraria.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Data.Repository
{
    public class LivroRepository : Repository<Livro>, ILivroRepository
    {
        public LivroRepository(LivrariaDbContext context) : base(context){ }

        public async Task<Livro> ObterLivroFornecedor(Guid id)
        {
            return await Db.Livros.AsNoTracking()
                .Include(f => f.Fornecedor) //estou indo em produtos, fazendo um inner join com o fornecedor, onde o produto tem esse id
                .FirstOrDefaultAsync(predicate: p => p.Id == id);
        }

        public async Task<IEnumerable<Livro>> ObterLivrosFornecedores()
        {
            return await Db.Livros.AsNoTracking().Include(f => f.Fornecedor) //estou indo em produtos, fazendo um inner join com o fornecedor, onde o produto tem esse id
                .OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Livro>> ObterLivrosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(l => l.FornecedorId == fornecedorId);
        }
    }
}
