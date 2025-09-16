using Microsoft.EntityFrameworkCore;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services;
using TheBlueSky.Bookings.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BookingsDbContext>( options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TheBlueSky_BookingsDB"));
});

var autoMapperlicenseKey = builder.Configuration["AutoMapper:LicenseKey"];
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = autoMapperlicenseKey, typeof(Program));

builder.Services.AddScoped<IBookingRepository,BookingRepository>();
builder.Services.AddScoped<IPassengerRepository,PassengerRepository>();
builder.Services.AddScoped<IBookingPassengerRepository,BookingPassengerRepository>();
builder.Services.AddScoped<IMealPreferenceRepository,MealPreferenceRepository>();
builder.Services.AddScoped<IBookingCancellationRepository,BookingCancellationRepository>();

builder.Services.AddScoped<IBookingService,BookingService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<IBookingPassengerService,BookingPassengerService>();
builder.Services.AddScoped<IMealPreferenceService,MealPreferenceService>();
builder.Services.AddScoped<IBookingCancellationService,BookingCancellationService>();


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
