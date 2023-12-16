using Countries;
using Countries.Data;
using Countries.Services;
using Countries.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var sqlConnection = configuration.GetConnectionString("SQLConnection");

builder.Services.AddDbContext<CountryDbContext>(config =>
{
    config.UseSqlServer(sqlConnection);
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRestCountriesService,RestCountriesService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IHotelService, HotelService>();

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
