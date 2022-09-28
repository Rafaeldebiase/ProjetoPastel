using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastel.Domain.Dto;

namespace Pastel.Handles.Interfaces
{
    public interface IImageHandle
    {
        Task<ResultDto> ImageIngestionAsync(IFormFile file, Guid userId);

        Task<FileStreamResult> GetPhoto(Guid userId);
    }
}
