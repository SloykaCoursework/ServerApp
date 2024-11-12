using ServerApp;
using System.Net.Sockets;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ServerApp.Tcp;

var listenAddress = new Uri("tcp://0.0.0.0:5555");
TcpListener _tcpListener;
TcpBase tcpBase;

MPI.Environment.Run(ref args, comm =>
{

    //start tcp connect
    if (comm.Rank == 0)
    {

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
                var clientTask = Task.Run(async () => await tcpBase.ProcessClientAsync(tcpClient, comm));

                Logger.LogInformation($"New connection");
            
            }

        }catch(Exception e)
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

            var command = (MpiTags)comm.Receive<int>(0, (int)MpiTags.HandleTask);



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
