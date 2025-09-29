using Microsoft.EntityFrameworkCore;
using StockApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// CORS: allow any localhost origin in Development (supports varying VS ports)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:5178") 
            .AllowAnyHeader()
            .AllowAnyMethod());
});

//DOCKER SETUP
//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(int.Parse(port));
//});

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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnectionNpgSql"), npgsql =>
    {
        npgsql.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null);
    }));
//End Npgsql setup

//End DB setup////


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Disable HTTPS redirection when front-end calls API over HTTP from Docker
// app.UseHttpsRedirection();
//app.UseCors("AllowFrontend");

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

// Serve SPA static files from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
// SPA fallback: serve index.html for non-API routes
app.MapFallbackToFile("index.html");
// Apply pending EF migrations at startup (safe for dev/test)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration failed: {ex}");
    }
}
app.Run();
