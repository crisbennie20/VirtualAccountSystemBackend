using Serilog.Events;
using Serilog;
using VirtualAccountSystemBackend.Service;
using Microsoft.EntityFrameworkCore;
using VirtualAccountSystemBackend.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var logFilePath = builder.Configuration.GetValue<string>("LogFile");

builder.Logging.ClearProviders().AddSerilog(new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.File(logFilePath)
                    .CreateLogger());

builder.Services.AddScoped<VirtualAccountService>();
builder.Services.AddScoped<VirtualAccountTransactionService>();

builder.Services.AddDbContext<VASContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
// app.UseSwagger();
// app.UseSwaggerUI();
//}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VASContext>();
    db.Database.Migrate();
}
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
