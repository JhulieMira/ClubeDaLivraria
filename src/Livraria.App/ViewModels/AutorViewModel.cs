using Livraria.Business.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Livraria.App.ViewModels
{
    public class AutorViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [DisplayName("Sobre o autor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(2000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 5)]
        public string Descricao { get; set; }

        [DisplayName("Imagem do autor")]
        public IFormFile ImagemUpload { get; set; }

        public string Imagem { get; set; }

        [NotMapped]
        public IEnumerable<LivroViewModel> Livros { get; set; }
    }
}

