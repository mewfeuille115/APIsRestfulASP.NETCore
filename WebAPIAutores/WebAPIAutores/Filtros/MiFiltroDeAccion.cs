using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAutores.Filtros
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> _logger;

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Antes de ejecutar la acción");
        }
     
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Después de ejecutar la acción");
        }
    }
}
