using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace APBD3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public const string LogFilePath = @"access.log";

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
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
                using (StreamReader streamReader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    body = await streamReader.ReadToEndAsync();
                }
            }

            TextWriter textWriter = new StreamWriter(LogFilePath, true);
            await textWriter.WriteAsync($"${method} ${path} ${query}");
            if (isBodyUsed)
            {
                await textWriter.WriteLineAsync($" ${body}");
            }
            
            await textWriter.WriteLineAsync();
        }
    }
}