using FluentValidation;
using TaskManager.Application.DTOs;

namespace TaskManager.Api.Validators
{
    public class ProjectCreateDTOValidator : AbstractValidator<ProjectCreateDTO>
    {
        public ProjectCreateDTOValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do projeto não pode ter mais de 100 caracteres.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("A descrição do projeto não pode ter mais de 500 caracteres.");

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório.");
        }
    }
}
