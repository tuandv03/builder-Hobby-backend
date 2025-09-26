using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MediatR;
using Application;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ------------------ Services ------------------

// PostgreSQL DbContext (connection string trong appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR v12 (scan Application layer)
builder.Services.AddMediatR(cfg =>
{
	cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
});

// Repository DI
builder.Services.AddScoped<ICardRepository, CardRepository>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Builder Hobby API",
		Version = "v1",
		Description = "Clean Architecture API for Yugioh Cards"
	});
});

var app = builder.Build();

// ------------------ Middleware ------------------

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Builder Hobby API v1");
		c.RoutePrefix = string.Empty;
	});
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
