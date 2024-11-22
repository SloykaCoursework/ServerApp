using CourseWorkLibrary;
using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Tcp
{
    internal class TcpServer : TcpBase
    {

        protected override MpiObj ProcessCommand(Command command, string uid)
        {

            Logger.LogInformation($"Handle command with code #{command.Code}");

            return new MpiObj()
            {
                uid = uid,
                code = (CommandCode)command.Code
            };

        }

        

    }
}
