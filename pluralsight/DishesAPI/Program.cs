using DishesAPI.DbContexts;
using DishesAPI.EndpointFilters;
using DishesAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(builder.Configuration["ConnectionStrings:DishesDBConnectionString"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminFromKilmarnock", policy => {
        policy
            .RequireRole("admin")
            .RequireClaim("town", "Kilmarnock");
    });
// Wonder what one we would use for Keycloak
builder.Services.AddAuthentication().AddJwtBearer();

// Configure swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("TokenAuthNZ", new()
    {
        Name = "Authorization",
        Description = "Token-based authentication and authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header 
    });
    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "TokenAuthNZ" }
            }, new List<string>() 
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// Important that is the first app item configured as it will catch all 
// exceptions from the components that follow it. 

app.RegisterApiExceptionHander();

app.UseHttpsRedirection();

// For this to work you also need to install the Microsoft.AspNetCore.OpenAPI package 
// otherwise none of the summaries or descriptions will go through. 
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.RegisterDishesEndpoints();
app.Run();
