using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace WebAPIAutores.Middlewares
{
    public static class LoguearRespuestaHttpMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoguearRespuestaHttp(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuestaHttpMiddleware>();
        }
    }

    public class LoguearRespuestaHttpMiddleware
    {
        private readonly RequestDelegate _siguiente;
        private readonly ILogger<LoguearRespuestaHttpMiddleware> _logger;

        public LoguearRespuestaHttpMiddleware(
            RequestDelegate siguiente,
            ILogger<LoguearRespuestaHttpMiddleware> logger)
        {
            _siguiente = siguiente;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;

                await _siguiente(contexto);

                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(cuerpoOriginalRespuesta);
                contexto.Response.Body = cuerpoOriginalRespuesta;

                _logger.LogInformation(respuesta);
            }
        }
    }
}
