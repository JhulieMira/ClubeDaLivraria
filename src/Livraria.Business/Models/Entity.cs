using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Business.Models
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        protected Entity() //Somente a classe filha (que tambem é uma entidade) irá acessar esse construtor
        {
            Id = Guid.NewGuid();
        }
    }
}
