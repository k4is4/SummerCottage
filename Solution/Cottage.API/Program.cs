using Cottage.API.Middleware;
using Cottage.API.Models;
using Cottage.API.Repositories;
using Cottage.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CottageContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Cottage")));
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
builder.Services.AddScoped<ICalendarEventsService, CalendarEventsService>();
builder.Services.AddScoped<ICalendarEventsRepository, CalendarEventsRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		});
});

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
	app.UseSwagger();
	app.UseSwaggerUI();
//}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
