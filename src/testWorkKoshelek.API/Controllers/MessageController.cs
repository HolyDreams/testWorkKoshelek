using Base.Interfaces;
using Base.Models.Results;
using Core.Domain.Domain;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using testWorkKoshelek.API.Methods;
using testWorkKoshelek.API.Models;

namespace testWorkKoshelek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        #region Private

        private readonly ISenderMessage _senderMessage;
        private readonly IGetterMessage _getterMessage;
        private readonly IAutoMapperAdapter _mapper;
        private readonly ILogger<MessageController> _logger;

        #endregion

        public MessageController(ISenderMessage senderMessage, 
            IGetterMessage getterMessage, 
            IAutoMapperAdapter mapper, 
            ILogger<MessageController> logger)
        {
            _senderMessage = senderMessage;
            _getterMessage = getterMessage;
            _mapper = mapper;
            _logger = logger;
        }

        #region Methods

        /// <summary>
        /// Отправка нового сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        [HttpPost("send")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> SendMessage([FromBody] MessageDTO message)
        {
            if (!message.IsValidMessage())
            {
                return BadRequest("Длина сообщения больше 128 символов");
            }

            var msg = new Message(message.Text);

            Result sendResult;

            using (_logger.BeginScope(new[]
            {
                new KeyValuePair<string, object>("SessionId", Guid.NewGuid()),
                new KeyValuePair<string, object>("MessageId", msg.Id)
            }))
            {
                sendResult = await _senderMessage.SendMessage(msg);
            }

            if (sendResult.IsError)
            {
                return BadRequest(sendResult.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Получение сообщений за период
        /// </summary>
        /// <param name="period">Период</param>
        /// <returns></returns>
        [HttpGet("getByPeriod")]
        [ProducesResponseType(typeof(List<MessageRecieveDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MessageRecieveDTO>> GetMessageByPeriod(DateTime start, DateTime? end = null)
        {
            var End = end ?? DateTime.Now;
            if (!Validator.IsValidPeriod(start, End))
            {
                return BadRequest("Дата начала больше даты окончания");
            }

            ListResult<Message> getResult;

            using (_logger.BeginScope(new[]
            { 
                new KeyValuePair<string, object>("SessionId", Guid.NewGuid())
            }))
            {
                getResult = await _getterMessage.GetByPeriod(start, End);
            }

            if (getResult.IsError)
            {
                return BadRequest(getResult.Message);
            }

            var receiveMessages = _mapper.Map<List<Message>, List<MessageRecieveDTO>>(getResult.Value.ToList());

            return Ok(receiveMessages);
        }

        #endregion
    }
}
