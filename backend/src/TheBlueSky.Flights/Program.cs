using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TheBlueSky.Flights.Data.Seeders;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;
using TheBlueSky.Flights.Repositories.Interfaces;
using TheBlueSky.Flights.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FlightsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TheBlueSky_FlightDB"));
});

var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];

if (string.IsNullOrWhiteSpace(jwtSecretKey))
{
    throw new InvalidOperationException("Missing Jwt:SecretKey. In dev: 'dotnet user-secrets set \"Jwt:SecretKey\" <value>'. In prod: set env var Jwt__SecretKey.");
}

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

var autoMapperlicenseKey = builder.Configuration["AutoMapper:LicenseKey"];
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = autoMapperlicenseKey, typeof(Program));

builder.Services.AddScoped<DatabaseSeeder>();
builder.Services.AddScoped<IDataSeeder,CountrySeeder>();

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

builder.Services.AddScoped<IFlightSeatStatusRepository, FlightSeatStatusRepository>();
builder.Services.AddScoped<IFlightSeatStatusService, FlightSeatStatusService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var seeder = services.GetRequiredService<DatabaseSeeder>();

        await seeder.SeedAsync(app.Lifetime.ApplicationStopping);
    }
    catch (Exception e)
    {
        logger.LogError("Error during data seeding: " + e);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
