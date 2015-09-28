using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin
{
    public class GremlinServerClient
    {
        public GremlinServerClient(string Host = "localhost", int Port = 8182, bool UseBinary = true, int ReadBufferSize = 1024)
        {
            this.Host = Host;
            this.Port = Port;
            this.UseBinary = UseBinary;
            this.ReadBufferSize = ReadBufferSize;
            this.Uri = new Uri("ws://" + Host + ":" + Port);
        }

        public List<ResultDataType> Send<ResultDataType>(string Script, Dictionary<string, object> Bindings = null, Guid? Session = null)
        {
            return RunSync(delegate ()
            {
                return SendAsync<ResultDataType>(Script, Bindings, Session);
            });
        }
        public List<ResultDataType> Send<ResultDataType>(ClientWebSocket Socket, string Script, Dictionary<string, object> Bindings = null, Guid? Session = null)
        {
            return RunSync(delegate ()
            {
                return SendAsync<ResultDataType>(Socket, Script, Bindings, Session);
            });
        }

        public async Task<List<ResultDataType>> SendAsync<ResultDataType>(string Script, Dictionary<string, object> Bindings = null, Guid? Session = null)
        {
            var Message = new Messages.ScriptRequestMessage
            {
                Arguments = new Messages.ScriptArguments(Script, Bindings, Session)
            };
            return await SendAsync<ResultDataType>(Message);
        }
        public async Task<List<ResultDataType>> SendAsync<ResultDataType>(ClientWebSocket Socket, string Script, Dictionary<string, object> Bindings = null, Guid? Session = null)
        {
            var Message = new Messages.ScriptRequestMessage
            {
                Arguments = new Messages.ScriptArguments(Script, Bindings, Session)
            };
            return await SendAsync<ResultDataType>(Socket, Message);
        }

        private async Task<List<ResultDataType>> SendAsync<ResultDataType>(Messages.RequestMessage Message)
        {
            var Socket = await OpenSocketAsync();
            var ToReturn = await SendAsync<ResultDataType>(Socket, Message);
            await CloseSocketAsync(Socket);
            return ToReturn;
        }
        private async Task<List<ResultDataType>> SendAsync<ResultDataType>(ClientWebSocket Socket, Messages.RequestMessage Message)
        {
            var Json = JsonConvert.SerializeObject(Message);
            byte[] MessageBytes;
            if (UseBinary)
            {
                var SB = new StringBuilder();
                SB.Append((char)("application/json".Length));
                SB.Append("application/json");
                SB.Append(Json);
                MessageBytes = System.Text.Encoding.UTF8.GetBytes(SB.ToString());
            }
            else
                MessageBytes = System.Text.Encoding.UTF8.GetBytes(Json);

            
            await Socket.SendAsync(new ArraySegment<byte>(MessageBytes), UseBinary ? WebSocketMessageType.Binary : WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);

            List<ResultDataType> ToReturn = null;

            while (true)
            {
                var MS = new System.IO.MemoryStream();
                var Buffer = new ArraySegment<byte>(new byte[ReadBufferSize]);
                while (true)
                {
                    var Data = await Socket.ReceiveAsync(Buffer, System.Threading.CancellationToken.None);
                    MS.Write(Buffer.Array, 0, Data.Count);
                    if (Data.EndOfMessage)
                        break;
                }
                var ResponseString = System.Text.Encoding.UTF8.GetString(MS.ToArray());
                var Response = JsonConvert.DeserializeObject<Messages.ScriptResponse<ResultDataType>>(ResponseString);

                if (Response.Result.Data != null)
                {
                    if (ToReturn == null)
                        ToReturn = Response.Result.Data;
                    else
                        ToReturn.AddRange(Response.Result.Data);
                }

                switch (Response.Status.Code)
                {
                    case 200: // SUCCESS
                    case 204: // NO CONTENT
                        if (ToReturn == null)
                            ToReturn = new List<ResultDataType>();
                        return ToReturn;
                    case 206: // PARTIAL CONTENT
                        continue;
                    case 498:
                        throw new Exceptions.MalformedRequestException(Response.Status.Message);
                    case 499:
                        throw new Exceptions.InvalidRequestArgumentsException(Response.Status.Message);
                    case 500:
                        throw new Exceptions.ServerErrorException(Response.Status.Message);
                    case 597:
                        throw new Exceptions.ScriptEvaluationErrorException(Response.Status.Message);
                    case 598:
                        throw new Exceptions.ServerTimeoutException(Response.Status.Message);
                    case 599:
                        throw new Exceptions.ServerSerializationError(Response.Status.Message);
                    default:
                        throw new Exception("Unsupported StatusCode (" + Response.Status.Code + "): " + Response.Status.Message);
                }
            }
        }

        public async Task<ClientWebSocket> OpenSocketAsync()
        {
            var Socket = new ClientWebSocket();
            await Socket.ConnectAsync(Uri, System.Threading.CancellationToken.None);
            return Socket;
        }
        public async Task CloseSocketAsync(ClientWebSocket Socket)
        {
            await Socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, System.Threading.CancellationToken.None);
        }

        private static readonly TaskFactory TaskFactory = new TaskFactory(System.Threading.CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
        private static TResult RunSync<TResult>(Func<Task<TResult>> Function)
        {
            return TaskFactory.StartNew<Task<TResult>>(Function).Unwrap<TResult>().GetAwaiter().GetResult();
        }

        public string Host { get; private set; }
        public int Port { get; private set; }
        public bool UseBinary { get; private set; }
        public Uri Uri { get; private set; }
        public int ReadBufferSize { get; private set; }
    }
}
