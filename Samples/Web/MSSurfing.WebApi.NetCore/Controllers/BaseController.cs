using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;

namespace MSSurfing.WebApi.NetCore.Controllers
{
    public abstract class BaseController : Controller
    {
        protected virtual string Json(IEnumerable Data, int Total)
        {
            return Json(new { data = Data, total = Total });
        }

        protected virtual string Json(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        protected virtual string ErrorMessage(string Msg = null, object Data = null)
        {
            return Json(new { Msg, Data });
        }

        protected virtual string SuccessMessage(string Msg = null, object Data = null)
        {
            return Json(new { Msg, Data });
        }
    }
}
