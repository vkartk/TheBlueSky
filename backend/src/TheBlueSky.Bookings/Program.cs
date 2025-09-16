using Microsoft.EntityFrameworkCore;
using TheBlueSky.Bookings.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BookingsDbContext>( options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TheBlueSky_BookingsDB"));
});

var autoMapperlicenseKey = builder.Configuration["AutoMapper:LicenseKey"];
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = autoMapperlicenseKey, typeof(Program));


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
