using Livraria.Business.Interfaces;
using Livraria.Business.Models;
using Livraria.Business.Models.Validations;

namespace Livraria.Business.Services
{
    public class LivroService : BaseService, ILivroService
    {
        private readonly ILivroRepository _livroRepository;

        public LivroService(ILivroRepository livroRepository, INotificador notificador) : base(notificador)
        {
            _livroRepository = livroRepository;
        }
        public async Task Adicionar(Livro livro)
        {
            if (!ExecutarValidacao(new LivroValidation(), livro)) return;

            if(_livroRepository.Buscar(l => l.Nome == livro.Nome).Result.Any() 
                && _livroRepository.Buscar(l => l.FornecedorId == livro.FornecedorId).Result.Any())
            {
                Notificar("Já existe um livro com o mesmo título para este fornecedor.");
                return;
            }

            await _livroRepository.Adicionar(livro);
        }

        public async Task Atualizar(Livro livro)
        {
            if (!ExecutarValidacao(new LivroValidation(), livro)) return;

            if (_livroRepository.Buscar(l => l.Nome == livro.Nome).Result.Any()
                && _livroRepository.Buscar(l => l.FornecedorId == livro.FornecedorId).Result.Any())
            {
                Notificar("Já existe um livro com o mesmo título para este fornecedor.");
                return;
            }

            await _livroRepository.Atualizar(livro);
        }

        public async Task Remover(Guid id)
        {
            await _livroRepository.Remover(id);
        }

        public void Dispose()
        {
            _livroRepository?.Dispose();
        }
    }
}
