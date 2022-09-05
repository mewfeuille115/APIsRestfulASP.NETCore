using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebAPIAutores.Filtros;
using WebAPIAutores.Middlewares;
using WebAPIAutores.Servicios;

namespace WebAPIAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(opciones =>
                {
                    opciones.Filters.Add(typeof(FiltroDeExcepcion));
                })
                .AddJsonOptions(x =>
                    // Ignora la referencia ciclica de las Entidades.   
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
                );

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddTransient<IServicio, ServicioA>();

            services.AddTransient<ServicioTransient>();
            services.AddScoped<ServicioScoped>();
            services.AddSingleton<ServicioSingleton>();
            services.AddTransient<MiFiltroDeAccion>();
            services.AddHostedService<EscribirEnArchivo>();

            services.AddResponseCaching();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIAutores", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //app.UseMiddleware<LoguearRespuestaHttpMiddleware>();
            app.UseLoguearRespuestaHttp();

            app.Map("/ruta1", app =>
            {
                app.Run(async contexto =>
                {
                    await contexto.Response.WriteAsync("Estoy interceptando la tuberia");
                });
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIAutores v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
