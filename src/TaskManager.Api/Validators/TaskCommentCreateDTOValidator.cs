using FluentValidation;
using TaskManager.Application.DTOs;

namespace TaskManager.Api.Validators
{
    public class TaskCommentCreateDTOValidator : AbstractValidator<TaskCommentCreateDTO>
    {
        public TaskCommentCreateDTOValidator()
        {
            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("O conteúdo do comentário é obrigatório.")
                .MaximumLength(500).WithMessage("O comentário não pode ter mais de 500 caracteres.");

            RuleFor(c => c.TaskId)
                .NotEmpty().WithMessage("O ID da tarefa é obrigatório.");

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório.");
        }
    }
}
