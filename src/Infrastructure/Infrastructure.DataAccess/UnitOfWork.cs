using Base.Infrastructure.DataAccess;
using Core.Domain.Models.Settings;
using Core.Interfaces.DataAccess;
using Core.Interfaces.DataAccess.Repositories;
using Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.DataAccess
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        #region Private

        private readonly ILoggerFactory _loggerFactory;
        private readonly DbOptions _options;

        #endregion

        public UnitOfWork(ILoggerFactory loggerFactory, IOptions<DbOptions> options) : base(options.Value.ConnectionString)
        {
            _loggerFactory = loggerFactory;
            _options = options.Value;
        }

        #region Protected

        protected override void ConnectToDb(string connectionString)
        {
            try
            {
                _connection = new NpgsqlConnection(connectionString);
                _connection.Open();
            }
            catch (Exception ex)
            {
                _loggerFactory.CreateLogger<UnitOfWork>().LogCritical(ex,
                    "Не удалось установить соединение с базой данных");
                throw;
            }
        }

        #endregion

        #region Repositories

        public IMessageRepository MessageRepository =>
            new MessageRepository(_connection, _transaction, _options, _loggerFactory.CreateLogger<MessageRepository>());

        #endregion
    }
}
