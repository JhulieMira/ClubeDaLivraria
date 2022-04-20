using Livraria.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Business.Interfaces //interface para facilitar o acesso a dados  na camada de negocios, pois a camada de acesso a dados nao conhece a camada business
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity //Especificando que a TEntity so pode ser utilizada se for uma classe filha de Entity
    {
        Task Adicionar(TEntity entity);
        Task<TEntity> ObterPorId(Guid id);
        Task<List<TEntity>> ObterTodos();
        Task Atualizar (TEntity entity);
        Task Remover(Guid id);
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate); //Possibilita que passe uma expressão lambda dentro desse método para buscar qualquer entidade por qualquer parâmetro
        Task<int> SaveChanges();
    }
}
