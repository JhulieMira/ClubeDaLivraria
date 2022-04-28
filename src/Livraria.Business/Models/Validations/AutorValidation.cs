using FluentValidation;

namespace Livraria.Business.Models.Validations
{
    public class AutorValidation : AbstractValidator<Autor>
    {
        public AutorValidation()
        {
            RuleFor(a => a.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {Minlength} e {MaxLength} caracteres.");

            RuleFor(a => a.Descricao)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatorio.")
                .Length(5, 2000).WithMessage("O campo {PropertyName} precisa ter entre {Minlength} e {MaxLength} caracteres.");

            RuleFor(a => a.Imagem)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.");
        }
            
    }
}
