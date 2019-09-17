using MSSurfing.Data.Entity.Users;
using MSSurfing.Services.Users;
using Newtonsoft.Json;
using System;
using System.Collections;

namespace MSSurfing.ThriftServer.NetCore.Processors
{
    public class UserProcessor
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public UserProcessor(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Utilities
        public string Json(IEnumerable Data, int Total)
        {
            return Json(new { data = Data, total = Total });
        }

        public string Json(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
        #endregion


        public string Add()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = "Ab",
                Email = "zzAb@ab.ab",
                Mobilephone = "18000009999",
                Active = true,
                Deleted = false,
            };
            var result = _userService.InsertUser(user);
            //DebugLogger.Debug($"inserted user:id{user.Id}");
            return Json(result);
        }

        public string Search()
        {
            var list = _userService.Search();
            return Json(list, list.TotalCount);
        }

        public string Overflow()
        {

            for (int i = 0; i < 10; i++)
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

                var list = _userService.Search(pageIndex: 0, pageSize: 50);
            }

            return Json("ok");
        }
    }
}
