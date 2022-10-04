using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IUserTaskHandle
    {
        Task<IEnumerable<ResultUserTaskDto>> GetUsers(Guid managerId);
    }
}
