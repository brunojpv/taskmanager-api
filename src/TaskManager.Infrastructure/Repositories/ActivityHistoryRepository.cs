using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ActivityHistoryRepository : IActivityHistoryRepository
    {
        private readonly AppDbContext _context;

        public ActivityHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ActivityHistory history)
        {
            _context.ActivityHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActivityHistory>> GetByActivityIdAsync(Guid activityId)
        {
            return await _context.ActivityHistories
                .Where(h => h.ActivityId == activityId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }
    }
}
