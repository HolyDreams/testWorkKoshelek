using Base.Infrastructure.WebSockets;
using Base.Interfaces;
using Core.Domain.Domain;
using Core.Domain.Models.Settings;
using Core.Interfaces.WebSocketConnection;
using Infrastructure.WebSockets.Models;
using Microsoft.Extensions.Options;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Infrastructure.WebSockets.EventsArgs;

namespace Infrastructure.WebSockets
{
    public class WebSocketStreamProvider : BaseWebSocketStreamProvider, IWebSocketStreamProvider
    {
        #region Private

        private readonly IAutoMapperAdapter _mapper;

        private Task listener;
        private Task sender;

        #endregion

        public WebSocketStreamProvider(IOptions<WebSocketOptions> options, IAutoMapperAdapter mapper) : base(options.Value.Uri, options.Value.ChunkSize)
        {
            _mapper = mapper;
        }

        #region Events

        public event EventHandler OnConnected;
        public event EventHandler OnReceive;
        public event EventHandler OnClosed;

        #endregion

        #region Methods

        public override void Connect()
        {
            Task.Run(ConnectAsync);
        }

        public async Task ConnectAsync()
        {
            await clientWebSocket.ConnectAsync(uri, cancellationToken);
            IsConnected.Set();
            OnConnected?.Invoke(null, EventArgs.Empty);
            sender = Task.Run(StartSend);
            listener = Task.Run(StartListen);
        }

        public void SendMessage(Message message)
        {
            var test = clientWebSocket.State;

            var messageDto = _mapper.Map<MessageDTO>(message);
            var sendMessage = JsonSerializer.Serialize(messageDto);
            var data = Encoding.UTF8.GetBytes(sendMessage);

            Send(new ArraySegment<byte>(data));
        }

        #endregion

        #region Private Methods

        private async Task StartSend()
        {
            while (await reader.WaitToReadAsync())
            {
                if (reader.TryRead(out ArraySegment<byte> message))
                {
                    if (clientWebSocket.State != WebSocketState.Open)
                        throw new WebSocketException(WebSocketError.InvalidState, "Ошибка при отправки сообщения. Состояние веб сокета: " + clientWebSocket.State);

                    int messageCount = message.Count / chunkSize;
                    messageCount += message.Count % chunkSize > 0 ? 1 : 0;

                    for (int i = 0; i < messageCount; i++)
                    {
                        int offset = chunkSize * i;
                        bool endOfMessage = (i == messageCount - 1);
                        int count = endOfMessage ? message.Count - offset : chunkSize;


                        await clientWebSocket.SendAsync(new ArraySegment<byte>(message.Array, offset, count), WebSocketMessageType.Text, endOfMessage, cancellationToken).ConfigureAwait(false);
                    }
                }
            }

            clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).Wait();
            IsConnected.Reset();
            OnClosed?.Invoke(null, EventArgs.Empty);
        }

        private async Task StartListen()
        {
            byte[] buffer = new byte[chunkSize];

            while (clientWebSocket.State == WebSocketState.Open)
            {
                MemoryStream memoryStream = new MemoryStream();
                WebSocketReceiveResult result;
                do
                {
                    result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Close)
                        return;

                    memoryStream.Write(buffer, 0, result.Count);

                } while (!result.EndOfMessage);

                OnReceive?.Invoke(this, new MessageEventArgs(memoryStream.ToArray()));
            }
        }

        #endregion
    }
}
