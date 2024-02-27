using DishesAPI.DbContexts;
using DishesAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(builder.Configuration["ConnectionStrings:DishesDBConnectionString"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Important that is the first app item configured as it will catch all 
// exceptions from the components that follow it. 

app.RegisterApiExceptionHander();

app.UseHttpsRedirection();
app.RegisterDishesEndpoints();
app.Run();
