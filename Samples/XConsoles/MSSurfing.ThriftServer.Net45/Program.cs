using Autofac.Engine;
using EntityFramework.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSurfing.ThriftServer.Net45
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineContext.Initialize();
            EngineContext.Resolve<IDbContext>().ModelBuild();

            // loop 
            ProgramCommander.LoopCmd();

            Console.ReadLine();
        }
    }
}
