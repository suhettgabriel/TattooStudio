using Microsoft.EntityFrameworkCore;
using System;
using TattooStudio.Application.Interfaces;
using TattooStudio.Infrastructure.Data;
using TattooStudio.Infrastructure.Repositories;
using TattooStudio.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IStudioRepository, StudioRepository>();
builder.Services.AddScoped<IFormFieldRepository, FormFieldRepository>();
builder.Services.AddScoped<ITattooRequestRepository, TattooRequestRepository>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();