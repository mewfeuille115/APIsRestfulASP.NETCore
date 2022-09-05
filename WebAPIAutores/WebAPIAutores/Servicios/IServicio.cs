using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIAutores.Servicios
{
    public interface IServicio
    {
        Guid ObtenerScoped();
        Guid ObtenerSingleton();
        Guid ObtenerTransient();
        void RealizarTarea();
    }

    public class ServicioA : IServicio
    {
        private readonly ILogger<ServicioA> _logger;
        private readonly ServicioTransient _servicioTransient;
        private readonly ServicioScoped _servicioScoped;
        private readonly ServicioSingleton _servicioSingleton;

        public ServicioA(
            ILogger<ServicioA> logger,
            ServicioTransient servicioTransient,
            ServicioScoped servicioScoped,
            ServicioSingleton servicioSingleton)
        {
            _logger = logger;
            _servicioTransient = servicioTransient;
            _servicioScoped = servicioScoped;
            _servicioSingleton = servicioSingleton;
        }

        public Guid ObtenerTransient() { return _servicioTransient.Guid; }
        public Guid ObtenerScoped() { return _servicioScoped.Guid; }
        public Guid ObtenerSingleton() { return _servicioSingleton.Guid; }

        public void RealizarTarea()
        {
        }
    }

    public class ServicioB : IServicio
    {
        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTarea()
        {
        }
    }

    public class ServicioTransient
    {
        public Guid Guid = Guid.NewGuid();
    }

    public class ServicioScoped
    {
        public Guid Guid = Guid.NewGuid();
    }

    public class ServicioSingleton
    {
        public Guid Guid = Guid.NewGuid();
    }
}