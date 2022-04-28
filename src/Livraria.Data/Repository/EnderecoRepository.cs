using Livraria.Business.Interfaces;
using Livraria.Business.Models;
using Livraria.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(LivrariaDbContext context) : base(context)
        {
        }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            return await Db.Enderecos.AsNoTracking().FirstOrDefaultAsync(x => x.FornecedorId == fornecedorId);
        }
    }
}
