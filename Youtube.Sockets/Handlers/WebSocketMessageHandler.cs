using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Youtube.Sockets.SocketsManager;

namespace Youtube.Sockets.Handlers
{
    public class WebSocketMessageHandler : SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connections) : base(connections)
        {

        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var socketId = Connections.GetId(socket);
            //var response = new { systemLoginId = 293461, loginModeId = 293461, ldapLoginId = 293461, userName = "Epic,User" };
            string response = "{'hub.mode':'subscribe','hub.topic':'fdb2f928-5546-4f52-87a0-0648e9ded065','hub.events':'epic/com.epic.userlogin.read,epic/com.epic.userlogout.read,epic/com.epic.userhibernate.read','hub.lease-seconds':7200}";
            await SendMessageToAll($"{response}");
            //Below code to test the denied scenario
            //string response = "{'hub.mode':'denied','hub.topic':'fdb2f928-5546-4f52-87a0-0648e9ded065','hub.events':'epic/com.epic.userlogin.read,epic/com.epic.userlogout.read,epic/com.epic.userhibernate.read','hub.reason':'Subscriber does not have valid authorization for one or more requested events.'}";
            //await SendMessageToAll($"{response}");
            string message = "{ \"timestamp\": \"2022 - 01 - 24T14: 58:58.73\", \"id\": \"JVoD0v9rR0uoOmqw2iEUyQ\", \"event\": { \"hub.topic\": \"TORxeQrHb06e - eu5z0TV6Q\", \"hub.event\": \"com.epic.userlogin\", \"context\": [ { \"systemLoginId\": \"SUNQUESTONE\", \"loginModeId\": \"SUNQUESTONE\", \"ldapLoginId\": \"SUNQUESTONE\", \"userName\": \"SUNQUEST, USER\" } ] } }";
            await SendMessageToAll($"{message}");
            //await SendMessageToAll($"{socketId} just joined the party");
        }
        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = Connections.GetId(socket);
            var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            await SendMessageToAll(message);
        }
    }
}
