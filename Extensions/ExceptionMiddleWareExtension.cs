using HD_Backend.Data;
using HD_Backend.Data.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace HD_Backend.Extensions
{
    public static class ExceptionMiddleWareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(
                appError =>
                {
                    appError.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            logger.LogError($"Something went wrong: {contextFeature.Error}");
                            await context.Response.WriteAsync(
                                new ResponseModel()
                                {
                                    StatusCode = context.Response.StatusCode,
                                    Message = "Internal Server Error."
                                }.ToString());
                        }
                    });
                });
        }
    }
}
