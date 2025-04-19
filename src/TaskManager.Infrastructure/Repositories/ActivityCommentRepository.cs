using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ActivityCommentRepository : IActivityCommentRepository
    {
        private readonly AppDbContext _context;

        public ActivityCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ActivityComment comment)
        {
            _context.ActivityComments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ActivityComment>> GetByActivityIdAsync(Guid activityId)
        {
            return await _context.ActivityComments
                .Where(c => c.ActivityId == activityId)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
