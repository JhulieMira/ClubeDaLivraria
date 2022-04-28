using Microsoft.AspNetCore.Mvc;
using Livraria.App.ViewModels;
using Livraria.Business.Interfaces;
using AutoMapper; 
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
        private readonly IAutorRepository _autorRepository;
        private readonly ILivroService _livroService;
        private readonly IMapper _mapper;

        public LivrosController(ILivroRepository livroRepository, IMapper mapper, IFornecedorRepository fornecedorRepository, IAutorRepository autorRepository, ILivroService livroService, INotificador notificador) : base(notificador)
        {
            _livroRepository = livroRepository;
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
            _autorRepository = autorRepository;
            _livroService = livroService;
        }

        [AllowAnonymous]
        [Route("lista-de-livros")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<LivroViewModel>>(await _livroRepository.ObterLivrosAutoresEFornecedores()));
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
            var livroViewModel = await PopularFornecedoresEAutores(new LivroViewModel());
           
            return View(livroViewModel);
        }

        [ClaimsAuthorize("Livro", "Adicionar")]
        [Route("novo-livro")]
        [HttpPost]
        public async Task<IActionResult> Create(LivroViewModel livroViewModel)
        {
            livroViewModel = await PopularFornecedoresEAutores(livroViewModel);


            if (!ModelState.IsValid) return View(livroViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";
            
            if(!await UploadArquivo(livroViewModel.ImagemUpload, imgPrefixo))
            {
                return View(livroViewModel);
            }

            livroViewModel.Imagem = imgPrefixo + livroViewModel.ImagemUpload.FileName;

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
        public async Task<IActionResult> Edit(Guid id, LivroViewModel livroViewModel)
        {
            if (id != livroViewModel.Id) return NotFound();

            var livroAtualizacao = await ObterLivro(id);
            livroViewModel.Fornecedor = livroAtualizacao.Fornecedor;
            livroViewModel.Autor = livroAtualizacao.Autor;
            livroViewModel.Imagem = livroAtualizacao.Imagem; 

            if (!ModelState.IsValid) return View(livroViewModel);

            if(livroViewModel.ImagemUpload != null) 
            {
                var imgPrefixo = Guid.NewGuid() + "_";

                if (!await UploadArquivo(livroViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(livroViewModel);
                }

                livroAtualizacao.Imagem = imgPrefixo + livroViewModel.ImagemUpload.FileName;
            }

            livroAtualizacao.Nome = livroViewModel.Nome;
            livroAtualizacao.Descricao = livroViewModel.Descricao; 
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
        [HttpPost, ActionName("Delete")]
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
            var livro = _mapper.Map<LivroViewModel>(await _livroRepository.ObterLivroAutorEFornecedor(id));
            livro.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            livro.Autores = _mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.ObterTodos());
            return livro;
        }

        private async Task<LivroViewModel> PopularFornecedoresEAutores(LivroViewModel livro)
        {
            livro.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            livro.Autores = _mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.ObterTodos());
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
                await arquivo.CopyToAsync(stream); 
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

