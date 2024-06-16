using Base.Interfaces.DataAccess;
using System.Net.WebSockets;
using System.Threading.Channels;

namespace Base.Infrastructure.WebSockets
{
    public abstract class BaseWebSocketStreamProvider : IBaseWebSocketStreamProvider
    {
        #region Protected

        protected readonly ClientWebSocket clientWebSocket;
        protected readonly Uri uri;
        protected readonly int chunkSize;
        protected readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        protected readonly CancellationToken cancellationToken;

        protected ManualResetEvent IsConnected = new ManualResetEvent(false);
        protected AutoResetEvent IsReadyToSend = new AutoResetEvent(true);

        protected ChannelReader<ArraySegment<byte>> reader;
        protected ChannelWriter<ArraySegment<byte>> writer;

        #endregion

        public BaseWebSocketStreamProvider(string uri, int chunkSize)
        {
            if (!uri.StartsWith("ws://") && !uri.StartsWith("wss://"))
            {
                throw new Exception("Неправильный адрес подключения для WebSocket");
            }

            this.uri = new Uri(uri);
            this.chunkSize = chunkSize;

            Channel<ArraySegment<byte>> channel = Channel.CreateUnbounded<ArraySegment<byte>>(new UnboundedChannelOptions()
            {
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = false
            });

            writer = channel.Writer;
            reader = channel.Reader;

            clientWebSocket = new ClientWebSocket();
            cancellationToken = cancellationTokenSource.Token;
        }

        #region Methods

        public abstract void Connect();

        public virtual void Close()
        {
            if (IsAlive())
            {
                writer.TryComplete();
            }
        }

        public virtual bool IsAlive()
        {
            return clientWebSocket.State == WebSocketState.Open;
        }

        public virtual void Send(ArraySegment<byte> message)
        {
            if (!writer.TryWrite(message))
            {
                throw new Exception();
            }
        }

        public virtual void Dispose()
        {
            Close();
            clientWebSocket.Dispose();
        }

        #endregion
    }
}
