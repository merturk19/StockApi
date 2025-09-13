using Microsoft.EntityFrameworkCore;
using StockApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DB setup////
//var folder = Environment.SpecialFolder.LocalApplicationData;
//var path = Environment.GetFolderPath(folder);

//SQLite setup
//var dbPath = System.IO.Path.Join(path, "stockitems.db");

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlite($"Data Source={dbPath}"));
//End SQLite setup

//Npgsql setup
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnectionNpgSql")));
//End Npgsql setup

//End DB setup////


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
