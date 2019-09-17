using Autofac.Engine;
using Microsoft.EntityFrameworkCore.Engine;
using System;

namespace MSSurfing.ThriftServer.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineContext.Initialize();
            EngineContext.Resolve<IDbContext>().ModelBuild();

            ProgramCommander.LoopCmd();

            Console.ReadLine();
        }
    }
}
