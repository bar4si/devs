using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TaskMaster.Core.Interfaces;
using TaskMaster.Infrastructure.Data;
using TaskMaster.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- SERVIÇOS DO SISTEMA ---

builder.Services.AddOpenApi();
builder.Services.AddControllers();

// --- CONFIGURAÇÃO DE PERSISTÊNCIA ---

// 1. Configuração Global do Entity Framework (Usada pelo TaskEfRepository)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- CONFIGURAÇÃO DE REPOSITÓRIOS (Injeção de Dependência) ---

// OPÇÃO A: Registro padrão (Injeção simples de ITaskRepository)
builder.Services.AddScoped<ITaskRepository, TaskEfRepository>();

// OPÇÃO B: Registro Híbrido (Keyed Services - .NET 8+)
// Permite coexistência: EF Core para comandos complexos e Dapper para consultas rápidas (CQRS).

// Registro EF: Resolve automaticamente o AppDbContext configurado acima
//builder.Services.AddKeyedScoped<ITaskRepository, TaskEfRepository>("ef");

// Registro Dapper: Injeta uma Connection String específica (pode ser a mesma ou uma réplica de leitura)
//builder.Services.AddKeyedScoped<ITaskRepository, TaskSqlRepository>("sql", (sp, key) => 
//{
//    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
//                           ?? throw new InvalidOperationException("DefaultConnection not found");
//    return new TaskSqlRepository(connectionString);
//});

var app = builder.Build();

// --- PIPELINE ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
