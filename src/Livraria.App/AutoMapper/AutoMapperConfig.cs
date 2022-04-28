using AutoMapper;
using Livraria.App.ViewModels;
using Livraria.Business.Models;

namespace Livraria.App.AutoMapper
{
    public class AutoMapperConfig : Profile 
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Livro, LivroViewModel>().ReverseMap(); ;
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Autor, AutorViewModel>().ReverseMap(); ;
        }
    }
}
