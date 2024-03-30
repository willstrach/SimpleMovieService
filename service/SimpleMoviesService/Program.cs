using Microsoft.EntityFrameworkCore;
using SimpleMoviesService.Endpoints;
using SimpleMoviesService.Persistance;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDatabase(configuration);
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseExceptionHandler();

app.MapApiEndpoints();
app.UseStatusCodePages();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
