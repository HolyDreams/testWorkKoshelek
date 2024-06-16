using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Net;
using System.Text;

namespace testWorkKoshelek.WebSocketServer
{
    class Program
    {
        private static ConcurrentBag<WebSocket> _sockets = new ConcurrentBag<WebSocket>();

        static async Task Main(string[] args)
        {
            var address = "";
            if (System.Diagnostics.Debugger.IsAttached)
            {
                address = "localhost:5000";
            }
            else
            {
                address = "+:80";
            }
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://{address}/");
            listener.Start();
            Console.WriteLine($"WebSocket server started at http://+:80/");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                    WebSocket webSocket = wsContext.WebSocket;
                    _sockets.Add(webSocket);

                    _ = Task.Run(async () =>
                    {
                        byte[] buffer = new byte[1024];
                        while (webSocket.State == WebSocketState.Open)
                        {
                            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                            if (message.Length == 0)
                                continue;

                            Console.WriteLine("Received: " + message);
                            

                            if (result.MessageType == WebSocketMessageType.Close)
                            {
                                _sockets.TryTake(out webSocket);
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                            }
                            else
                            {
                                await BroadcastMessage("Message received: " + message);
                            }
                        }
                    });
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private static async Task BroadcastMessage(string message)
        {
            byte[] response = Encoding.UTF8.GetBytes(message);
            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}