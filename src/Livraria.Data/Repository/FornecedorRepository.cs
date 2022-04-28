using Livraria.Business.Interfaces;
using Livraria.Business.Models;
using Livraria.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(LivrariaDbContext context) : base(context)
        {

        }

        public async Task<Fornecedor> ObterEnderecoFornecedor(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking().Include(c => c.Endereco).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Fornecedor> ObterProdutosEEnderecoFornecedor(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(c => c.Livros)
                .Include(c => c.Endereco)
                .OrderBy(c => c.Nome)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
