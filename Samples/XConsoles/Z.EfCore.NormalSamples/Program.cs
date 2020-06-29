using Microsoft.EntityFrameworkCore.Engine;
using Microsoft.EntityFrameworkCore.Engine.Readonly;
using System;
using System.Threading.Tasks;
using Z.EfCore.NormalSamples.Entity.Users;
using Z.EfCore.NormalSamples.Processors;
using Z.EfCore.NormalSamples.Services.Users;

namespace Z.EfCore.NormalSamples
{
    class Program
    {
        static string connectionString = "Database=MDProject;Server=.;User ID=sa;Password=sa;Pooling=true;Max Pool Size=32767;Min Pool Size=0;";
        static string readonlyConnectionString = "Database=MDProject;Server=.;User ID=sa;Password=sa;Pooling=true;Max Pool Size=32767;Min Pool Size=0;";

        static void Main(string[] args)
        {
            Sample010_02();


        }

        static void Sample010_02()
        {

            var context = new EfeObjectContext(connectionString, DbOptions.UseSqlServer);
            context.ModelBuild();

            var readonlyContext = new EfeReadonlyDbContext(readonlyConnectionString, DbOptions.UseSqlServer);
            readonlyContext.ModelBuild();

            IRepository<User> repository = new EfeRepository<User>(context);
            IReadonlyRepository<User> readRepository = new EfeReadonlyRepository<User>(readonlyContext);
            var userService = new UserService(repository, readRepository);
            var userProcess = new UserProcessor(userService);


            bool isContinue = true;
            do
            {
                try
                {
                    Console.WriteLine("please entry cmd: add / search / loop");
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
                        case "loop":
                            loop_01();
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

        static void loop_01()
        {
            Parallel.For(1, 100, i =>
            {
                using (var context = new EfeObjectContext(connectionString, DbOptions.UseSqlServer))
                {
                    context.ModelBuild();
                    using (var readonlyContext = new EfeReadonlyDbContext(readonlyConnectionString, DbOptions.UseSqlServer))
                    {
                        readonlyContext.ModelBuild();

                        IRepository<User> repository = new EfeRepository<User>(context);
                        IReadonlyRepository<User> readRepository = new EfeReadonlyRepository<User>(readonlyContext);
                        var userService = new UserService(repository, readRepository);
                        var userProcess = new UserProcessor(userService);

                        userProcess.Add();
                        userProcess.Search();

                        repository.Dispose();
                        readRepository.Dispose();
                    }
                }
            });

            // 查询 SqlServer 中的链接数
            //SELECT * FROM sys.dm_exec_sessions WHERE host_name IS NOT NULL AND host_name = 'Dell_2039100' ORDER BY host_name
        }
    }
}
