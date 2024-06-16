using Base.Models.Results;
using Core.Domain.Domain;
using Core.Domain.Models.Settings;
using Core.Interfaces.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace Infrastructure.DataAccess.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        #region Private

        private readonly NpgsqlConnection _connection;
        private readonly NpgsqlTransaction _transaction;
        private readonly DbOptions _options;
        private readonly ILogger<MessageRepository> _logger;

        private const string Format = "yyyy.MM.dd HH:mm:ss";

        #endregion

        public MessageRepository(IDbConnection connection,
            IDbTransaction transaction,
            DbOptions options,
            ILogger<MessageRepository> logger) 
        {
            _connection = (NpgsqlConnection)connection;
            _transaction = (NpgsqlTransaction)transaction;
            _options = options;
            _logger = logger;
        }

        #region Methods

        public async Task<Result> AddMessageAsync(Message message)
        {
            _logger.LogDebug("{Method} Начало добавления нового сообщения", nameof(AddMessageAsync));

            var query = $@"INSERT INTO {_options.Scheme}.messages (id, msg_text, msg_date)
                           VALUES ('{message.Id}',
                                   '{message.Text}',
                                   '{message.CreatedDate.ToString(Format)}')";

            var command = _connection.CreateCommand();
            command.CommandText = query;
            command.CommandTimeout = _options.CommandTimeOut;
            command.Transaction = _transaction;

            try
            {
                await command.ExecuteNonQueryAsync();

                _logger.LogDebug("{Method} Успешное добавление нового сообщения", nameof(AddMessageAsync));

                return Result.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} Исключение при добавлении нового сообщения", nameof(AddMessageAsync));

                return Result.CreateErrorResult(ex.Message);
            }
        }

        public async Task<ListResult<Message>> GetMessagesByPeriodAsync(DateTime start, DateTime end)
        {
            _logger.LogDebug("{Method} Начало получения сообщений в диапазоне дат {start} - {end}",
                nameof(GetMessagesByPeriodAsync), start, end);

            var query = $@"SELECT id,
                                  msg_text,
                                  msg_date
                           FROM {_options.Scheme}.messages
                           WHERE msg_date BETWEEN '{start.ToString(Format)}' AND '{end.ToString(Format)}'";

            var command = _connection.CreateCommand();
            command.CommandText = query;
            command.CommandTimeout = _options.CommandTimeOut;

            var result = new List<Message>();

            try
            {

                using (var rdr = await command.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    { 
                        result.Add(
                            new Message(
                                id: rdr.GetGuid(0),
                                text: rdr.GetString(1),
                                createdDate: rdr.GetDateTime(2)
                            ));
                    }
                }

                _logger.LogDebug("{Method} Успешное получение сообщений в диапазоне дат {start} - {end}",
                    nameof(GetMessagesByPeriodAsync), start, end);

                return ListResult<Message>.CreateSuccessListResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} Исключение при получении сообщений в диапазоне дат {start} - {end}",
                    nameof(GetMessagesByPeriodAsync), start, end);

                return ListResult<Message>.CreateErrorListResult(message: ex.Message);
            }
        }

        #endregion
    }
}
