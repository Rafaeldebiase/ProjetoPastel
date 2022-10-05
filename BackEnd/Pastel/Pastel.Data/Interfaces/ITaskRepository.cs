using Pastel.Domain.Dto;
using Pastel.Domain.Entities;

namespace Pastel.Data.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskDto>> GetTaskById(Guid id);
        Task<IEnumerable<TaskDto>> GetTaskByUserId(Guid userId);
        Task<bool> Close(TaskModel task);
        Task<bool> Create(TaskModel task);
        Task<bool> Edit(TaskModel task);
        Task<bool> Delete(Guid id);
    }
}
