using Livraria.Business.Interfaces;
using Livraria.Business.Models;
using Livraria.Business.Models.Validations;

namespace Livraria.Business.Services
{
    public class AutorService : BaseService, IAutorService
    {
        private readonly IAutorRepository _autorRepository;
        private readonly ILivroRepository _livroRepository;

        public AutorService(IAutorRepository autorRepository, ILivroRepository livroRepository, INotificador notificador) : base(notificador)
        {
            _autorRepository = autorRepository;
            _livroRepository = livroRepository;
        }

        public async Task Adicionar(Autor autor)
        {
            if (!ExecutarValidacao(new AutorValidation(), autor)) return;

            if (_autorRepository.Buscar(a => a.Nome == autor.Nome).Result.Any())
            {
                Notificar("Já existe um autor com o mesmo nome");
            }

            await _autorRepository.Adicionar(autor);
        }

        public async Task Atualizar(Autor autor)
        {
            if (!ExecutarValidacao(new AutorValidation(), autor)) return;

            if (_autorRepository.Buscar(a => a.Nome == autor.Nome && a.Id != autor.Id).Result.Any())
            {
                Notificar("Já existe um autor com o mesmo nome");
            }

            await _autorRepository.Adicionar(autor);
        }

        public async Task Remover(Guid id)
        {
            if(_livroRepository.ObterLivrosPorAutor(id).Result.Any())
            {
                Notificar("Esse autor possui livros cadastrados");
                return;
            }

            await _autorRepository.Remover(id);
        }
        
        public void Dispose()
        {
            _autorRepository?.Dispose();
        }
    }
}
