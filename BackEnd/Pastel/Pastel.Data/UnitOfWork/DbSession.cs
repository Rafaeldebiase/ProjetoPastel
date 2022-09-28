using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Pastel.Data.UnitOfWork
{
    public sealed class DbSession : IDisposable
    {

        public DbSession(IConfiguration configuration)
        {
            var conn = configuration.GetSection("StringConnection").Value;
            Connection = new NpgsqlConnection(conn);
            Connection.Open();
        }

        public IDbConnection Connection { get; }
        public IDbTransaction? Transaction { get; set; }

        public void Dispose() => Connection?.Dispose();
    }
}
