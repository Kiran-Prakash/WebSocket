using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Youtube.Sockets.SocketsManager
{
    public class SocketMiddleWare
    {
        private readonly RequestDelegate _next;

        public SocketMiddleWare(RequestDelegate next, SocketHandler handler)
        {
            _next = next;
            Handler = handler;
        }
        private SocketHandler Handler { get; set; }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await Handler.OnConnected(socket);
            await Receive(socket, async (result, buffer) =>
            {
                if(result.MessageType == WebSocketMessageType.Text)
                {
                    await Handler.Receive(socket, result, buffer);
                } else if(result.MessageType == WebSocketMessageType.Close)
                {
                    await Handler.OnDisconnected(socket);
                }
            });
        }

        private async Task Receive(WebSocket websocket,Action<WebSocketReceiveResult,byte[]> messageHandler)
        {
            var buffer = new byte[1024 * 4];
            while(websocket.State == WebSocketState.Open)
            {
                var result = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageHandler(result, buffer);
            }
        }
    }
}