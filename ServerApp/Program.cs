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
using ServerApp.Handler;
using System.Collections.Generic;

int CREATE_COUNT = 100_000;

MPI.Environment.Run(ref args, comm =>
{

    if(args.Length != 0)
    {

        for(int i = 0; i < args.Length; i++)
        {

            switch (args[i])
            {

                case "--h":
                case "-help":

                    if (comm.Rank == 0)
                        Console.WriteLine("\n" +
                                        "\t-help(--h), \n" +
                                        "\t-maxCount(--mc) <count>, \n" +
                                        "\n" +
                                        "\n" +
                                        "");

                    return;

                case "--mc":
                case "-maxCount":

                    i++;
                    if (i >= args.Length)
                    {
                        
                        if (comm.Rank == 0)
                            Logger.LogError("The parameter -MaxCount(--MC) <count> must contain <count>");
                    
                    }
                    else if (!int.TryParse(args[i], out CREATE_COUNT))
                    {
                    
                        if (comm.Rank == 0) 
                            Logger.LogError("Parameter -MaxCount(--MC) <count> must be an integer");
                    
                    }
                    else
                    {
                        if (comm.Rank == 0)
                            Logger.LogDebug($"Parameter -MaxCount(--MC) set to {CREATE_COUNT}");

                    }

                    break;

                default:

                    if (comm.Rank == 0)
                        Logger.LogWarning($"Parameter '{args[i]}' not founded. Use -help(--h) for help");

                    break;

            }

        }

    }

    List<MpiObj> tasks = new List<MpiObj>();

    //start tcp
    if (comm.Rank == 0)
        Task.Run(async () =>
        {

            var listenAddress = new Uri("tcp://0.0.0.0:5555");
            TcpListener _tcpListener;
            TcpBase tcpBase = new TcpServer();

            Logger.LogInformation($"There are {comm.Size} pods");
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

                    Logger.LogInformation($"New connection on {tcpClient.Client.RemoteEndPoint}");
                    var res = await tcpBase.ProcessClientAsync(tcpClient);
                    tasks.Add(res!);

                }

            }
            catch (Exception e)
            {

                Logger.LogError(e);

            }
            finally
            {

                Logger.LogError("Server stopped");
                _tcpListener.Stop();
                MPI.Environment.Abort(1);

            }

        });

    comm.Barrier();

    try
    {

        if (comm.Rank == 0)
        {

            while (true)
            {

                if (tasks.Count == 0) continue;
                Logger.LogInformation($"Main pod ready for send");

                var item = tasks![0];
                tasks.Remove(item);

                for (int i = 1; i < comm.Size; i++)
                    comm.Send(JsonConvert.SerializeObject(item), i, (int)MpiTags.HandleTask);

                var ms = HandleTask(item, comm.Rank, comm.Size);

                Logger.LogInformation($"{ms} ms");

                var resArr = comm.Gather(ms, 0);

                var res = $"min: {resArr.Min()} average: {resArr.Average()} max: {resArr.Max()}";
                Logger.LogDebug(res);

                RabbitMQHandler.Send(item.uid, res);

            }

        }
        else
        {

            while (true)
            {

                Logger.LogInformation($"Ready for recive {comm.Rank}");
                var task = JsonConvert.DeserializeObject<MpiObj>(comm.Receive<string>(0, (int)MpiTags.HandleTask));

                var ms = HandleTask(task, comm.Rank, comm.Size);

                Logger.LogInformation($"{ms} ms");
                comm.Gather(ms, 0);

            }

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

void GetWorkCount(int arrSize, int current, int size, out int workStart, out int workCount)
{

    workCount = arrSize / size + (arrSize % size >= 1 ? 1 : 0);
    workStart = workCount * current;

}

long HandleTask(MpiObj task, int current, int size)
{

    Stopwatch sw = Stopwatch.StartNew();

    sw.Start();

    switch (task.code)
    {
        case CommandCode.CreateData:
            {

                GetWorkCount(CREATE_COUNT, current, size, out int workStart, out int workCount);
                DatabaseController.CreateData(workCount);

            }
            break;

        case CommandCode.FindArithmeticMeanValues:
            {

                var count = DatabaseController.GetStudentCount;
                GetWorkCount(count, current, size, out int workStart, out int workCount);
                DatabaseController.FindArithmeticMeanValues(workStart, workCount);

            }
            break;

        case CommandCode.CountAllGroups:
            {

                var count = DatabaseController.GetGroupCount;
                GetWorkCount(count, current, size, out int workStart, out int workCount);
                DatabaseController.CountAllGroups(workStart, workCount);

            }
            break;

        case CommandCode.ChangeCourseToAll:
            {

                var count = DatabaseController.GetStudentCount;
                GetWorkCount(count, current, size, out int workStart, out int workCount);
                DatabaseController.ChangeCourseToAll(workStart, workCount);

            }
            break;

        case CommandCode.FindOldestStudent:
            {

                var count = DatabaseController.GetStudentCount;
                GetWorkCount(count, current, size, out int workStart, out int workCount);
                DatabaseController.FindOldestStudent(workStart, workCount);

            }
            break;

        case CommandCode.FindYoungerInstructor:
            {

                var count = DatabaseController.GetInstructorCount;
                GetWorkCount(count, current, size, out int workStart, out int workCount);
                DatabaseController.FindYoungerInstructor(workStart, workCount);

            }
            break;

        default:

            Logger.LogError($"Could not find {task.code}");

            break;

    }

    sw.Stop();

    return sw.ElapsedMilliseconds;

}
