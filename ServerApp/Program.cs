using ServerApp;
using System.Net.Sockets;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ServerApp.Tcp;
using CourseWorkLibrary;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using ServerApp.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Linq;

var listenAddress = new Uri("tcp://0.0.0.0:5555");
TcpListener _tcpListener;
TcpBase tcpBase = new TcpServer();

MPI.Environment.Run(ref args, async comm =>
{

    if (comm.Size < 2) 
    {

        Logger.LogError("The program needs more than 2 working processes");
        return;

    }

    //start tcp connect
    if (comm.Rank == 0)
    {

        Logger.LogInformation($"There {(comm.Size != 1 ? "are" : "is")} {comm.Size} pod{(comm.Size != 1 ? "s" : "")}");
        Logger.LogInformation("Main pod starts TCP server");

        var address = IPAddress.Parse(listenAddress.Host);
        _tcpListener = new TcpListener(address, listenAddress.Port);

        try
        {
            _tcpListener.Start();
            Logger.LogInformation($"Server started on {listenAddress.AbsoluteUri}");

            while (true)
            {

                var tcpClient = _tcpListener.AcceptTcpClient();

                Logger.LogInformation($"New connection");
                var res = await tcpBase.ProcessClientAsync(tcpClient);

                for (int i = 1; i < comm.Size; i++)
                    comm.Send<string>(JsonConvert.SerializeObject(res!), i, (int)MpiTags.HandleTask);

                var resArr = comm.Gather<long>(-1, 0).Skip(1);

                Logger.LogDebug($"min: {resArr.Min()} average: {resArr.Average()} max: {resArr.Max()}");

            }

        }
        catch (Exception e)
        {

            Logger.LogError(e.Message);
            Logger.LogErrorTrace(e.StackTrace);

        }
        finally
        {

            Logger.LogError("Server stopped");
            _tcpListener.Stop();
            MPI.Environment.Abort(1);

        }

    }

    //handle requests

    try
    {

        while (true)
        {

            Logger.LogInformation($"Ready for recive {comm.Rank}");
            var task = JsonConvert.DeserializeObject<MpiObj>(comm.Receive<string>(0, (int)MpiTags.HandleTask));

            Stopwatch sw = Stopwatch.StartNew();

            sw.Start();

            switch (task.code)
            {
                case CommandCode.CreateData:

                    DatabaseController.CreateData();

                    break;

                case CommandCode.FindArithmeticMeanValues:

                    DatabaseController.FindArithmeticMeanValues();

                    break;

                case CommandCode.CountAllGroups:

                    DatabaseController.CountAllGroups();

                    break;

                case CommandCode.ChangeCourseToAll:

                    DatabaseController.ChangeCourseToAll();

                    break;

                case CommandCode.FindOldestStudent:

                    DatabaseController.FindOldestStudent();

                    break;

                case CommandCode.FindYoungerInstructor:

                    DatabaseController.FindYoungerInstructor();

                    break;

                default:

                    Logger.LogError($"Could not find {task.code}");

                    break;

            }

            sw.Stop();

            Logger.LogInformation($"{sw.ElapsedMilliseconds} ms");
            comm.Gather(sw.ElapsedMilliseconds, 0);

        }

    }
    catch (Exception e)
    {

        Logger.LogError(e.Message);
        Logger.LogErrorTrace(e.StackTrace);

    }
    finally
    {

        Logger.LogError("Worker stopped");
        MPI.Environment.Abort(1);

    }

});
