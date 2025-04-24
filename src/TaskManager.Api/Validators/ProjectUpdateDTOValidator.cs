using FluentValidation;
using TaskManager.Application.DTOs;

namespace TaskManager.Api.Validators
{
    public class ProjectUpdateDTOValidator : AbstractValidator<ProjectUpdateDTO>
    {
        public ProjectUpdateDTOValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("O ID do projeto é obrigatório.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do projeto não pode ter mais de 100 caracteres.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("A descrição do projeto não pode ter mais de 500 caracteres.");
        }
    }
}
