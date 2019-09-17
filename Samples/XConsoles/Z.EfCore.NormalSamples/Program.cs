using Microsoft.EntityFrameworkCore.Engine;
using System;
using Z.EfCore.NormalSamples.Entity.Users;
using Z.EfCore.NormalSamples.Processors;
using Z.EfCore.NormalSamples.Services.Users;

namespace Z.EfCore.NormalSamples
{
    class Program
    {
        static string connectionString = "Data Source=47.98.165.82;Initial Catalog=Surfing10;Persist Security Info=True;User ID=sa;Password=z;Connect Timeout=360";
        static void Main(string[] args)
        {
            Sample010_02();


        }

        static void Sample010_02()
        {

            var context = new EfeObjectContext(connectionString, DbOptions.UseSqlServer);
            context.ModelBuild();

            IRepository<User> repository = new EfeRepository<User>(context);
            var userService = new UserService(repository);
            var userProcess = new UserProcessor(userService);


            bool isContinue = true;
            do
            {
                try
                {
                    Console.WriteLine("please entry cmd: add / search ");
                    var cmd = Console.ReadLine();
                    switch (cmd)
                    {
                        case "add":
                            var result = userProcess.Add();
                            Console.WriteLine(result);
                            break;
                        case "search":
                            var result2 = userProcess.Search();
                            Console.WriteLine(result2);
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
    }
}
