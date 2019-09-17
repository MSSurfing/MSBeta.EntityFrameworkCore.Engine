using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Engine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Engine;
using MSSurfing.Data.Entity.Users;
using MSSurfing.Services.Users;

namespace MSSurfing.WebApi.NetCore.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : BaseController
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        [HttpGet, Route("search")]
        public string Search()
        {
            var list = _userService.Search();
            return SuccessMessage(Data: new { list, list.TotalCount });
        }

        [HttpGet, Route("adduser")]
        public ActionResult Add()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = "Ab",
                Email = "Ab@ab.ab",
                Mobilephone = "18000009999",
                Active = true,
                Deleted = false,
            };
            _userService.InsertUser(user);
            //DebugLogger.Debug($"inserted user:id{user.Id}");
            return Ok();
        }

        [HttpGet, Route("proc")]
        public ActionResult Proc()
        {
            var UseMySql = false;
            if (UseMySql)
            {

                var context = EngineContext.Resolve<IDbContext>();

                // Method 1
                var sql = "CALL PUsers()";
                var list = context.Set<User>().FromSql(sql).ToList();

                // Method 2
                var list2 = new List<User>();
                var connection = context.GetDbConnection();
                using (var command = connection.CreateCommand())
                {
                    context.Database.OpenConnection();

                    command.CommandText = "PUsers"; //填 存储过程名称
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //command.Parameters.Add
                    //command.Direction = ParameterDirection.Output;        //这个测试未通过，返回Out值获取不到

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new User();
                            Type type = item.GetType();
                            foreach (var property in type.GetProperties())
                            {
                                // 属性赋值 及 类型转换

                                //var value = reader.GetValue(reader.GetOrdinal(property.Name));
                                //if (property.PropertyType == typeof(bool))
                                //    property.SetValue(item, value.ToString() == "1");
                                //else
                                //    property.SetValue(item, value.ToString());
                            }
                            list2.Add(item);
                        }
                    }
                }
            }
            return Ok();
        }

        [HttpGet, Route("overflow")]
        public ActionResult Overflow()
        {

            for (int i = 0; i < 100000; i++)
            {
                //var user = new User()
                //{
                //    Id = Guid.NewGuid(),
                //    Username = "of2" + i,
                //    Email = "of2@of.of" + i,
                //    Mobilephone = "1822001999" + i,
                //    Active = true,
                //    Deleted = false,
                //};
                //_userService.InsertUser(user);

                var list = _userService.Search();
            }

            return Ok();
        }
        /*
         create stripts

        CREATE PROCEDURE `PUsers`()
        BEGIN
            SELECT * FROM `User`;
        END


         */
    }
}