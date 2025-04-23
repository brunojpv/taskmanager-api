using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Activity>> GetAllAsync(Guid? userId)
        {
            return await _context.Activities
                .Include(t => t.Project)
                .Where(t => t.Project != null && t.Project.UserId == userId)
                .ToListAsync();
        }

        public async Task<Activity?> GetByIdAsync(Guid id)
        {
            return await _context.Activities
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity is not null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
