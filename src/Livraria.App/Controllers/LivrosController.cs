using Microsoft.AspNetCore.Mvc;
using Livraria.App.ViewModels;
using Livraria.Business.Interfaces;
using AutoMapper; //estou usando o auto mapper para transformar de model em view model
using Livraria.Business.Models;
using Livraria.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Livraria.App.Extensions;

namespace Livraria.App.Controllers
{
    [Authorize]
    public class LivrosController : BaseController
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly ILivroService _livroService;
        private readonly IMapper _mapper;

        public LivrosController(ILivroRepository livroRepository, IMapper mapper, IFornecedorRepository fornecedorRepository, ILivroService livroService, INotificador notificador) : base(notificador)
        {
            _livroRepository = livroRepository;
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
            _livroService = livroService;
        }

        [AllowAnonymous]
        [Route("lista-de-livros")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<LivroViewModel>>(await _livroRepository.ObterLivrosFornecedores()));
        }

        [AllowAnonymous]
        [Route("dados-do-livro/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var livroViewModel = await ObterLivro(id);

            if (livroViewModel == null)
            {
                return NotFound();
            }

            return View(livroViewModel);
        }

        [ClaimsAuthorize("Livro","Adicionar")]
        [Route("novo-livro")]
        public async Task<IActionResult> Create()
        {
            var livroViewModel = await PopularFornecedores(new LivroViewModel());
            return View(livroViewModel);
        }

        [ClaimsAuthorize("Livro", "Adicionar")]
        [Route("novo-livro")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LivroViewModel livroViewModel)
        {
            livroViewModel = await PopularFornecedores(livroViewModel);

            //ModelState.Remove("Complemento");

            if (!ModelState.IsValid) return View(livroViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";
            
            if(!await UploadArquivo(livroViewModel.ImagemUpload, imgPrefixo))
            {
                return View(livroViewModel);
            }

            livroViewModel.Imagem = imgPrefixo + livroViewModel.ImagemUpload.FileName; //passando o arquivo que foi criado no disco para o campo imagem

            await _livroService.Adicionar(_mapper.Map<Livro>(livroViewModel));

            if (!OperacaoValida()) return View(livroViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Livro", "Editar")]
        [Route("editar-livro/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var livroViewModel = await ObterLivro(id);

            if (livroViewModel == null) return NotFound();

            return View(livroViewModel);
        }

        [ClaimsAuthorize("Livro", "Editar")]
        [Route("editar-livro/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, LivroViewModel livroViewModel)
        {
            if (id != livroViewModel.Id) return NotFound();

            //livroViewModel = await ObterLivro(id); //serviria para a model voltar da mesma forma que foi populada no momento da edição

            var livroAtualizacao = await ObterLivro(id); //para trabalhar em dados em cima de uma instância que não é a que veio, porque preciso ter os dados separados do que veio "via formulário" e do que realmente esta no bd
            livroViewModel.Fornecedor = livroAtualizacao.Fornecedor;
            livroViewModel.Imagem = livroAtualizacao.Imagem; //repassando os dados que estavam no banco

            if (!ModelState.IsValid) return View(livroViewModel);

            if(livroViewModel.ImagemUpload != null) //se for true, eu tenho uma imagem pra upar
            {
                var imgPrefixo = Guid.NewGuid() + "_";

                if (!await UploadArquivo(livroViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(livroViewModel);
                }

                livroAtualizacao.Imagem = imgPrefixo + livroViewModel.ImagemUpload.FileName; //passando o arquivo que foi criado no disco para o campo imagem
            }

            livroAtualizacao.Nome = livroViewModel.Nome;
            livroAtualizacao.Descricao = livroViewModel.Descricao; //atualizando os dados do banco com os dados que chegaram de entrada com exceção do fornecedor, que, segundo a regra de negócio, não pode ser editado.
            livroAtualizacao.Valor = livroViewModel.Valor;
            livroAtualizacao.Ativo = livroViewModel.Ativo;

            await _livroService.Atualizar(_mapper.Map<Livro>(livroAtualizacao));
            RemoverArquivo(livroViewModel.Imagem);

            if (!OperacaoValida()) return View(livroViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Livro", "Excluir")]
        [Route("excluir-livro/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var livro = await ObterLivro(id);

            if(livro == null) return NotFound();

            return View(livro);
        }

        [ClaimsAuthorize("Livro", "Excluir")]
        [Route("excluir-livro/{id:guid}")]
        [HttpPost, ActionName("Delete")] //action name usada porque nao podemos ter dois metodos com o mesmo nome e mesma assinatura. para que o metodo deleteconfirmed siga respondendo ao nome "delete" especificado
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var livro = await ObterLivro(id);

            if (livro == null)
            {
                return NotFound();
            }

            await _livroService.Remover(id);

            if (!OperacaoValida()) return View(livro);

            TempData["Sucesso"] = "Livro excluido com sucesso!";

            return RedirectToAction(nameof(Index));
        }
        private async Task<LivroViewModel> ObterLivro(Guid id)
        {
            var produto = _mapper.Map<LivroViewModel>(await _livroRepository.ObterLivroFornecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<LivroViewModel> PopularFornecedores(LivroViewModel livro)
        {
            livro.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return livro;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream); //copiando o arqquivo recebido para o disco
            }

            return true;
        }

        private static bool RemoverArquivo(string nomeArquivo)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", nomeArquivo);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);

                return true;
            }

            return false;
        }
    }
}

