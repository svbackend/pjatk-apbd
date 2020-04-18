using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace APBD3.Middlewares
{
    public class FileLoggingMiddleware : ILoggingMiddleware
    {
        public const string LogFilePath = @"access.log";
        
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            var method = httpContext.Request.Method;
            var path = httpContext.Request.Path.ToString();
            var query = httpContext.Request.QueryString.ToString();

            bool isBodyUsed = false;
            var body = "";

            string[] methodsWithBody = {"POST", "PUT", "PATCH"};
            if (methodsWithBody.Contains(method))
            {
                isBodyUsed = true;
                using (StreamReader streamReader =
                    new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, false))
                {
                    body = await streamReader.ReadToEndAsync();
                    streamReader.Close();
                }
            }

            await next(httpContext);

            using (TextWriter textWriter = new StreamWriter(LogFilePath, true))
            {
                await textWriter.WriteLineAsync($"{method} {path}{query}");
                if (isBodyUsed)
                {
                    await textWriter.WriteLineAsync($"{body}");
                }

                textWriter.Close();
            }
        }
    }

    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FileLoggingMiddleware>();
        }
    }
}