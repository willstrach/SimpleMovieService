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

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();
app.UseExceptionHandler();

app.MapEndpoints();
app.UseStatusCodePages();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
