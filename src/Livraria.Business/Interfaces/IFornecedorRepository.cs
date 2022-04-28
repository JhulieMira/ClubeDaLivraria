using Livraria.Business.Models;

namespace Livraria.Business.Interfaces
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        Task<Fornecedor> ObterEnderecoFornecedor(Guid id);
        Task<Fornecedor> ObterProdutosEEnderecoFornecedor(Guid id);
    }
}
