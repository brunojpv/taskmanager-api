﻿using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<Activity>> GetAllAsync(Guid userId);
        Task<Activity?> GetByIdAsync(Guid id);
        Task AddAsync(Activity task);
        Task UpdateAsync(Activity task);
        Task DeleteAsync(Guid id);
    }
}
