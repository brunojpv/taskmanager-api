﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Activity>> GetAllAsync(Guid userId)
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

        public async Task AddAsync(Activity task)
        {
            _context.Activities.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activity task)
        {
            _context.Activities.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Activities.FindAsync(id);
            if (task is not null)
            {
                _context.Activities.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
