using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

builder.Services.AddScoped<IBookingRepository,BookingRepository>();
builder.Services.AddScoped<IPassengerRepository,PassengerRepository>();
builder.Services.AddScoped<IBookingPassengerRepository,BookingPassengerRepository>();
builder.Services.AddScoped<IMealPreferenceRepository,MealPreferenceRepository>();
builder.Services.AddScoped<IBookingCancellationRepository,BookingCancellationRepository>();
builder.Services.AddScoped<IPaymentRepository,PaymentRepository>();

builder.Services.AddScoped<IBookingService,BookingService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<IBookingPassengerService,BookingPassengerService>();
builder.Services.AddScoped<IMealPreferenceService,MealPreferenceService>();
builder.Services.AddScoped<IBookingCancellationService,BookingCancellationService>();
builder.Services.AddScoped<IPaymentService,PaymentService>();


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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
