﻿using Autofac.Engine;
using MSSurfing.ThriftServer.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSurfing.ThriftServer.Net45
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
                    Console.WriteLine("please entry cmd: add / search / loadallplugins");
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

        static void LoadAllPlugins()
        {
            //var result = EngineContext.Resolve<PluginProcessor>().LoadAllPlugins();
            //Console.WriteLine(result);
        }
        #endregion
    }
}
