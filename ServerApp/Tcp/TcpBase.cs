using CourseWorkLibrary;
using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerApp.Tcp
{
    public abstract class TcpBase
    {

        public async Task<MpiObj?> ProcessClientAsync(TcpClient tcpClient)
        {

            MpiObj? res = null;

            var stream = tcpClient.GetStream();
            // буфер для входящих данных
            var request = new List<byte>();
            int bytesRead = 255;
            while (true)
            {
                // считываем данные до конечного символа
                while ((bytesRead = stream.ReadByte()) != Const.ETX)
                {
                    // добавляем в буфер
                    request.Add((byte)bytesRead);
                }

                request.Add(Const.ETX);

                var requestArray = request.ToArray();
                string uid = Guid.NewGuid().ToString();

                if (requestArray[0] == Const.EOT)
                {
                    await SendOkResponseAsync(0xff, stream, uid);
                    break;
                }

                if (!requestArray.TryDeserializeCommand(out var command))
                {
                    await SendErrorResponseAsync(stream, "wrong message");
                    break;
                }

                await SendOkResponseAsync((byte)command!.Code, stream, uid);
                request.Clear();

                res = ProcessCommand(command!, uid);
                break;

            }
            request.Clear();
            tcpClient.Close();

            return res;

        }

        protected abstract MpiObj ProcessCommand(Command command, string uid);

        protected static async Task SendCommandResponseAsync(Command command, NetworkStream stream)
        {
            var bytesToResponse = command.SerializeCommand();
            await stream.WriteAsync(bytesToResponse);
        }

        protected static async Task SendOkResponseAsync(byte code, NetworkStream stream, string uid)
        {
            Logger.LogInformation($"Send Ok message with code #{code}");

            var okResponse = new Command()
            {
                Code = code,
                Arguments = new Dictionary<string, object?>()
                {
                    ["Data"] = uid,
                    ["Ok"] = true
                }
            };

            await SendCommandResponseAsync(okResponse, stream);
        }

        protected static async Task SendErrorResponseAsync(NetworkStream stream, string errorMessage)
        {
            Logger.LogError(errorMessage);

            var errorCommand = new Command()
            {
                Code = 0,
                Arguments = new Dictionary<string, object?>()
                {
                    ["Error"] = errorMessage
                }
            };
            await SendCommandResponseAsync(errorCommand, stream);
        }

    }
}
