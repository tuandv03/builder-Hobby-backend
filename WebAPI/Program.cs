using Application;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

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
builder.Services.AddScoped<ICardInventoryRepository, CardInventoryRepository>();

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

// Add CORS configuration BEFORE app.Build()
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		policy => policy
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
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

app.UseCors("AllowAll"); // Add this line to use the CORS policy
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
// bật serve file tĩnh
app.UseStaticFiles();

// nếu muốn custom cache/đường dẫn
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(builder.Environment.ContentRootPath, "WebAPI", "wwwroot", "Images")),
	RequestPath = "/images"
});
app.Run();
