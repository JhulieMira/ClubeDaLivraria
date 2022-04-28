using Microsoft.AspNetCore.Mvc;
using Livraria.App.ViewModels;
using Livraria.Business.Interfaces;
using AutoMapper;
using Livraria.Business.Models;
using Livraria.Business.Services;


namespace Livraria.App.Controllers
{
    public class AutoresController : BaseController
    {
        private readonly IAutorRepository _autorRepository;
        private readonly ILivroRepository _livroRepository;
        private readonly IAutorService _autorService;
        private readonly IMapper _mapper;

        public AutoresController(IAutorRepository autorRepository, IAutorService autorService, IMapper mapper, ILivroRepository livroRepository, INotificador notificador) : base(notificador)
        {
            _livroRepository = livroRepository;
            _autorRepository = autorRepository;
            _autorService = autorService;
            _mapper = mapper;
        }

        [Route("lista-de-autores")]
        public async Task<IActionResult> Index(Guid id)
        {
            
            return View(_mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.ObterTodos())); //vai receber uma lista de fornecedores que sera convertido para fornecedor view model
        }

        [Route("detalhes-autor")]
        public async Task<IActionResult> Details(AutorViewModel autorViewModel)
        {
            if (autorViewModel == null) return NotFound();

            return View(await ObterAutor(autorViewModel.Id));
        }

        [Route("criar-autor")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("criar-autor")]
        [HttpPost]
        public async Task<IActionResult> Create(AutorViewModel autorViewModel)
        {
            if (!ModelState.IsValid) return View(autorViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";

            if (!await UploadArquivo(autorViewModel.ImagemUpload, imgPrefixo))
            {
                return View(autorViewModel);
            }

            autorViewModel.Imagem = imgPrefixo + autorViewModel.ImagemUpload.FileName;

            await _autorService.Adicionar(_mapper.Map<Autor>(autorViewModel));

            if (!OperacaoValida()) return View(autorViewModel);

            return RedirectToAction("Index", "Livros"); 
        }

        [Route("editar-autor")]
        public async Task<IActionResult> Edit(Guid id)
        {       
            var autorViewModel = await ObterAutor(id);

            if (autorViewModel == null) return NotFound();

            return View(autorViewModel);
        }

        [Route("editar-autor")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AutorViewModel autorViewModel)
        {
            if (id != autorViewModel.Id) return NotFound();

            var autorAtualizacao = await ObterAutor(id);

            autorViewModel.Livros = autorAtualizacao.Livros;
            autorViewModel.Imagem = autorAtualizacao.Imagem;

            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "A alteração não pôde ser concluida. Verifique os campos e tente novamente";
                return View(autorViewModel);
            }


            if (autorViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";

                if (!await UploadArquivo(autorViewModel.ImagemUpload, imgPrefixo))
                {
                    TempData["Erro"] = "Erro no upload da imagem";
                    return View(autorViewModel);
                }

                autorAtualizacao.Imagem = imgPrefixo + autorViewModel.ImagemUpload.FileName;
            }

            autorAtualizacao.Nome = autorViewModel.Nome;
            autorAtualizacao.Descricao = autorViewModel.Descricao;

            await _autorRepository.Atualizar(_mapper.Map<Autor>(autorAtualizacao));

            if (!OperacaoValida()) return View(autorViewModel);

            return RedirectToAction("Details", autorViewModel); 
        }

        [Route("excluir-autor")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var autorViewModel = await ObterAutor(id);

            if (autorViewModel == null) return NotFound();

            return View(autorViewModel);
        }

        [Route("excluir-autor")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var autor = await ObterAutor(id);

            if (autor == null) return NotFound();

            await _autorService.Remover(id);

            if (!OperacaoValida()) return View("Delete",autor);

            return RedirectToAction("Index");

        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }


        public async Task<AutorViewModel> ObterAutor(Guid id)
        {
            var autor = _mapper.Map<AutorViewModel>(await _autorRepository.ObterPorId(id));
            autor.Livros = _mapper.Map<IEnumerable<LivroViewModel>>(await _livroRepository.ObterLivrosPorAutor(id));

            return autor;
        }

    }
}
