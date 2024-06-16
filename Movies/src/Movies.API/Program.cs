using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Models;
using Movies.API.Helpers;
using Movies.API.Helpers.Formatters;
using Movies.API.Middleware;
using Movies.Application;
using Movies.Persistence;
using Movies.Storage;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddControllers(options =>
		// Add support for JSON Patch using NewtonSoft, while leaving the other input and output formatters unchanged
		options.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter())
	)
	.AddJsonOptions(x =>
		// Ignores the cyclical reference of the Entities.
		x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
	);

// Add the services for the HypermediaLinkService.
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(x =>
{
	var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
	var factory = x.GetRequiredService<IUrlHelperFactory>();
	return factory.GetUrlHelper(actionContext!);
});
builder.Services.AddScoped<HypermediaLinkService>();

// Add the services for the application and persistence layers.
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddStorageServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Movies.API",
		Version = "v1",
		Description = "This is a web api for working with movies and reviews.",
		Contact = new OpenApiContact
		{
			Name = builder.Configuration["Author:Name"],
			Email = builder.Configuration["Author:Email"],
			Url = new Uri(builder.Configuration["Author:Website"]!),
		},
		License = new OpenApiLicense
		{
			Name = "MIT",
		},
	});

	var archivoXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var rutaXML = Path.Combine(AppContext.BaseDirectory, archivoXML);
	c.IncludeXmlComments(rutaXML);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies.API v1");
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseCustomExceptionHandler();

app.MapControllers();

await app.RunAsync();
