using Microsoft.EntityFrameworkCore;
using PizzariaAPI.Data;
using PizzariaAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarTudo",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Controllers + Swagger + SignalR
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// Middleware
app.UseRouting();

app.UseCors("LiberarTudo");

app.UseStaticFiles();

// Swagger ativo
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints
app.MapControllers();
app.MapHub<PedidoHub>("/pedidoHub");

app.Run();