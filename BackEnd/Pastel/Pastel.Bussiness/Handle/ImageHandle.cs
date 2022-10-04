using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Command;
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

        public async Task<IEnumerable<FileStreamResult>> GetPhoto(Guid userId)
        {
            try
            {
                var result = new List<FileStreamResult>();
                var files = await _repository.GetPhoto(userId);

                if (files.Count() <= 0)
                {
                    return result;
                }

                var file = files.FirstOrDefault();
                MemoryStream memoryStream = new MemoryStream(file.Data);
                result.Add(new FileStreamResult(memoryStream, file.ContentType));
                return result;
            }
            catch (Exception error)
            {
                var message = $"{error.InnerException}\n " +
                     $"{error.Message} \n " +
                     $"{error.StackTrace}";

                _logger.LogError(message);

                return new List<FileStreamResult>();
            }
            
        }

        public async Task<ResultDto> ImageIngestion(IFormFile file, Guid userId)
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

        public async Task<ResultDto> DeleteImage(DeleteCommand command)
        {
            var result = new ResultDto();
            try
            {
                Guid.TryParse(command.Id, out var userId);
                _unitOfWork.BeginTransaction();
                await _repository.Delete(userId);
                _unitOfWork.Commit();

                result.AddObject(command);

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
