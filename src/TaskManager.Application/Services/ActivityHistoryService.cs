using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class ActivityHistoryService : IActivityHistoryService
    {
        private readonly IActivityHistoryRepository _historyRepository;

        public ActivityHistoryService(IActivityHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task RecordHistoryAsync(Guid activityId, Guid? userId, string description)
        {
            var history = new ActivityHistory(activityId, description, userId);
            await _historyRepository.AddAsync(history);
        }

        public async Task<IEnumerable<ActivityHistory>> GetHistoryByActivityIdAsync(Guid activityId)
        {
            return await _historyRepository.GetByActivityIdAsync(activityId);
        }
    }
}
