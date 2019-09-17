using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSurfing.WebApi.NetCore.HttpFramework.Builders
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigurePipeline(this IApplicationBuilder app)
        {
            // 捕获日志
            app.UseExceptionHandler();

            // 异常请求记录 
            //applicationBuilder.UseBadRequestResult();
        }

        #region Utilities 类-公用方法
        private static string BodyToString(this HttpContext httpContext)
        {
            // ToPerfect 走到这的时候Body流基本上被读过了
            if (httpContext.Request.Body.CanSeek && httpContext.Request.Body.Position > 0)
                httpContext.Request.Body.Position = 0;

            //// Test ,new method 
            ///     !!!使用StreamReader读过之后，Body会关闭，如果后面还有 Middleware 中直接读就会报错
            using (var sr = new StreamReader(httpContext.Request.Body, Encoding.ASCII))
            {
                return sr.ReadToEnd();
            }

            // 方式 二
            //byte[] body;
            //using (var stream = new MemoryStream())
            //{
            //    httpContext.Request.Body.CopyTo(stream);
            //    body = stream.ToArray();
            //}
            //return Encoding.ASCII.GetString(body);
        }

        private static string PrepareRequestContent(this HttpContext httpContext)
        {
            var message = "";

            if (httpContext.Request.QueryString.HasValue)
                message = $"QueryString:{httpContext.Request.QueryString.Value}";

            if (httpContext.Request.Body.Length > 0)
                message += $",Body:{httpContext.BodyToString()}";

            return message;
        }
        #endregion

        #region Extensions Methods 扩展方法
        private static void UseExceptionHandler(this IApplicationBuilder app)
        {
            // 捕获日志
            app.UseExceptionHandler(e =>
            {
                e.Run(httpContext =>
                {
                    var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception != null)
                    {
                        var action = httpContext.Request.Path;
                        var message = httpContext.PrepareRequestContent();

                        //Todo log error message    记录日志
                        //SurfLogger.Error(message: message, exception: exception);

                        Debug.WriteLine("get an exception!");
                    }
                    return Task.CompletedTask;
                });
            });
        }

        private static void UseBadRequestResult(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseStatusCodePages(httpContext =>
            {
                // ToImprove? 异常请求记录 
                if (httpContext.HttpContext.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    //Todo;
                }

                // ToImprove? 异常请求路径记录
                if (httpContext.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    //Todo;
                }

                return Task.CompletedTask;
            });
        }
        #endregion
    }
}
