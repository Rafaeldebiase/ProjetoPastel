using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Pastel.Data.Interfaces;
using Pastel.Data.UnitOfWork;
using Pastel.Domain.Dto;
using Pastel.Domain.Entities;
using System.Drawing;

namespace Pastel.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly DbSession _dbSession;
        private readonly IConfiguration _configuration;

        public ImageRepository(DbSession dbSession, IConfiguration configuration)
        {
            _dbSession = dbSession;
            _configuration = configuration;
        }

        public async Task<IEnumerable<PhotoDto>> GetPhoto(Guid userId)
        {
            var query = $@"
                            select 
                            id {nameof(Photo.Id)},
                            image_name {nameof(Photo.Name)},
                            image_data {nameof(Photo.Data)},
                            content_type {nameof(Photo.ContentType)},
                            user_id {nameof(Photo.UserId)}
                            from pastel.tb_image
                            where user_id = '{userId}'
                        ";

            return await _dbSession.Connection.QueryAsync<PhotoDto>(query);
        }

        public async Task<bool> ImageIngestionAsync(Photo file)
        {
            var query = $@"
                            insert into pastel.tb_image
                            (
                                id,
                                image_name,
                                image_data,
                                content_type,
                                user_id 
                            )values(
                                '{file.Id}',
                                '{file.Name}',
                                @image_data,
                                '{file.ContentType}',
                                '{file.UserId}'
                            );
                        ";

            using (var conn = new NpgsqlConnection(_configuration.GetSection("StringConnection").Value))
            {
                using (var command = new NpgsqlCommand(query, conn))
                {
                    NpgsqlParameter param = command.CreateParameter();
                    param.ParameterName = "@image_data";
                    param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                    param.Value = file.Data;
                    command.Parameters.Add(param);

                    conn.Open();
                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            var query = $@"
                            delete from pastel.tb_image
                            where user_id = '{id}'
                        ";

            var result = await _dbSession.Connection.ExecuteAsync(query, null, _dbSession.Transaction);
            return result > 0;
        }
    }
}
