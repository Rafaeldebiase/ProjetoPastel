using Pastel.Domain.Dto;
using Pastel.Domain.Entities;

namespace Pastel.Data.Interfaces
{
    public interface IImageRepository
    {
        Task<bool> ImageIngestionAsync(Photo file);
        Task<IEnumerable<PhotoDto>> GetPhoto(Guid userId);
    }
}
