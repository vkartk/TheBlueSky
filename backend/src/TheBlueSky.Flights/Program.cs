using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;
using TheBlueSky.Flights.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FlightsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TheBlueSky_FlightDB"));
});

var autoMapperlicenseKey = builder.Configuration["AutoMapper:LicenseKey"];
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = autoMapperlicenseKey, typeof(Program));

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();

builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IAirportService, AirportService>();

builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteService, RouteService>();

builder.Services.AddScoped<ISeatClassRepository, SeatClassRepository>();
builder.Services.AddScoped<ISeatClassService, SeatClassService>();

builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();
builder.Services.AddScoped<IAircraftService, AircraftService>();

builder.Services.AddScoped<IAircraftSeatRepository, AircraftSeatRepository>();
builder.Services.AddScoped<IAircraftSeatService, AircraftSeatService>();

builder.Services.AddScoped<IFlightScheduleRepository, FlightScheduleRepository>();
builder.Services.AddScoped<IFlightScheduleService, FlightScheduleService>();

builder.Services.AddScoped<IScheduleDayRepository, ScheduleDayRepository>();
builder.Services.AddScoped<IScheduleDayService, ScheduleDayService>();

builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IFlightService, FlightService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
