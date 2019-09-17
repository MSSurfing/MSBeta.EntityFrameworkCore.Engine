using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Z.EfCore.NormalSamples.Entity.Users;
using Z.EfCore.NormalSamples.Services.Users;

namespace Z.EfCore.NormalSamples.Processors
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
    }
}
