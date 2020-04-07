using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string sciezka = httpContext.Request.Path; 
                string querystring = httpContext.Request?.QueryString.ToString();
                string metoda = httpContext.Request.Method.ToString();
                string bodyStr = "";

                using (StreamReader reader= new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }

                string allCombined = $"1. Metoda: {metoda} 2. Sciezka: {sciezka}  3. Cialo: {bodyStr}  4. Query: {querystring} \n";
                //logowanie do pliku
                File.AppendAllText("log\\APBDlog.txt", allCombined);
                
            }

            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next(httpContext);
        }

    }
}
