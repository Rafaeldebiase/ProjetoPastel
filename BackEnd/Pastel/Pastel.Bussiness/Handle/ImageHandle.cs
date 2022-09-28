using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using Pastel.Handles.Interfaces;

namespace Pastel.Handles.Handle
{
    public class ImageHandle : IImageHandle
    {
        private readonly ILogger<ImageHandle> _logger;
        private readonly IImageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ImageHandle(ILogger<ImageHandle> logger,
            IImageRepository repository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FileStreamResult> GetPhoto(Guid userId)
        {
            var files = await _repository.GetPhoto(userId);
            var file = files.FirstOrDefault();
            MemoryStream memoryStream = new MemoryStream(file.Data);
            return new FileStreamResult(memoryStream, file.ContentType);
        }

        public async Task<ResultDto> ImageIngestionAsync(IFormFile file, Guid userId)
        {
            var result = new ResultDto();
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var fileModel = new Photo(file.FileName, memoryStream.ToArray(), file.ContentType, userId);

                _unitOfWork.BeginTransaction();
                await _repository.ImageIngestionAsync(fileModel);
                _unitOfWork.Commit();

                return result;
            }
            catch (Exception error)
            {
                _unitOfWork.Rollback();
                var message = $"{error.InnerException}\n " +
                    $"{error.Message} \n " +
                    $"{error.StackTrace}";

                _logger.LogError(message);

                result.AddError(message);

                return result;
            }

        }
    }
}
