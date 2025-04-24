using FluentValidation;
using TaskManager.Application.DTOs;

namespace TaskManager.Api.Validators
{
    public class TaskUpdateDTOValidator : AbstractValidator<TaskUpdateDTO>
    {
        public TaskUpdateDTOValidator()
        {
            RuleFor(t => t.Id)
                .NotEmpty().WithMessage("O ID da tarefa é obrigatório.");

            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("O título da tarefa é obrigatório.")
                .MaximumLength(100).WithMessage("O título da tarefa não pode ter mais de 100 caracteres.");

            RuleFor(t => t.Description)
                .MaximumLength(500).WithMessage("A descrição da tarefa não pode ter mais de 500 caracteres.");

            RuleFor(t => t.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today).When(t => t.DueDate.HasValue)
                .WithMessage("A data de vencimento deve ser atual ou futura.");

            RuleFor(t => t.Status)
                .IsInEnum().WithMessage("Status inválido.");

            RuleFor(t => t.UserId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório.");
        }
    }
}
