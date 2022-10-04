using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IImageHandle
    {
        Task<ResultDto> ImageIngestion(IFormFile file, Guid userId);

        Task<IEnumerable<FileStreamResult>> GetPhoto(Guid userId);
        
        Task<ResultDto> DeleteImage(DeleteCommand command);
    }
}
