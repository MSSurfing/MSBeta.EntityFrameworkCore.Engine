using Autofac.Engine;
using Microsoft.EntityFrameworkCore.Engine;
using MSSurfing.Data.Entity.Users;
using MSSurfing.Services.Users;
using MSSurfing.ThriftServer.NetCore.Processors;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSSurfing.ThriftServer.NetCore
{
    public static class ProgramCommander
    {
        #region Utilities
        public static void LoopCmd()
        {
            bool isContinue = true;
            do
            {
                try
                {
                    Console.WriteLine("please entry cmd: add / search / Overflow / loadallplugins");
                    var cmd = Console.ReadLine();
                    switch (cmd)
                    {
                        case "add":
                            AddUser();
                            break;
                        case "search":
                            SearchUser();
                            break;
                        case "loadallplugins":
                            LoadAllPlugins();
                            break;
                        case "Overflow":
                            Overflow();
                            break;


                        case "exit":
                            isContinue = false;
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (isContinue);
        }
        #endregion

        #region Methods
        static void SearchUser()
        {
            var result = EngineContext.Resolve<UserProcessor>().Search();
            Console.WriteLine(result);
        }

        static void AddUser()
        {
            var result = EngineContext.Resolve<UserProcessor>().Add();
            Console.WriteLine(result);
        }

        static void Overflow()
        {
            for (int i = 0; i < 10000; i++)
            {
                var result = EngineContext.Resolve<UserProcessor>().Overflow();
                Console.WriteLine(result);
            }
        }

        static void LoadAllPlugins()
        {
            //var result = EngineContext.Resolve<PluginProcessor>().LoadAllPlugins();
            //Console.WriteLine(result);
        }
        #endregion
    }
}
