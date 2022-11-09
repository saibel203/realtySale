using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using RealtySale.Api.Middlewares;

namespace RealtySale.Api.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
    }

    public static void ConfigureBuiltinExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var exception = context.Features.Get<IExceptionHandlerFeature>();

                    if (exception is not null)
                        await context.Response.WriteAsync(exception.Error.Message);
                });
            });
        }
    }
}
