using Marketplace.API.Mapping;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MarketplaceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MarketplaceDbConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
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
