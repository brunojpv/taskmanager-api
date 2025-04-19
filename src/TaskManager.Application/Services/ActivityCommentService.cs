using TaskManager.Application.DTOs.ActivityComment;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Services
{
    public class ActivityCommentService : IActivityCommentService
    {
        private readonly IActivityCommentRepository _commentRepository;
        private readonly IActivityService _activityService;
        private readonly IActivityHistoryService _historyService;

        public ActivityCommentService(IActivityCommentRepository commentRepository, IActivityService activityService, IActivityHistoryService historyService)
        {
            _commentRepository = commentRepository;
            _activityService = activityService;
            _historyService = historyService;
        }

        public async Task AddCommentAsync(Guid userId, AddCommentDto dto)
        {
            var activity = await _activityService.GetByIdAsync(dto.ActivityId) ?? throw new NotFoundException("Tarefa não encontrada.");

            var comment = new ActivityComment(dto.ActivityId, userId, dto.Content);
            await _commentRepository.AddAsync(comment);

            var description = $"Comentário adicionado: \"{dto.Content}\"";
            await _historyService.RecordHistoryAsync(dto.ActivityId, userId, description);
        }

        public async Task<List<ActivityComment>> GetCommentsByActivityIdAsync(Guid activityId)
        {
            return await _commentRepository.GetByActivityIdAsync(activityId);
        }
    }
}
