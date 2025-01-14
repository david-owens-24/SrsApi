using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SrsApi.DbContext;
using SrsApi.Interfaces;
using SrsApi.Managers;
using SrsApi.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddScoped<UserManager<IdentityUser>, SrsApiUserManager<IdentityUser>>();
builder.Services.AddScoped<IUserResolutionService, UserResolutionService>();

builder.Services.AddScoped<IBaseService<SrsItemLevel>, BaseService<SrsItemLevel>>();
builder.Services.AddScoped<IBaseServiceWithIncludes<SrsItem>, SrsItemService>();
builder.Services.AddScoped<IBaseServiceWithIncludes<SrsAnswer>, SrsAnswerService>();

builder.Services.AddScoped<IFuzzySearchMethodService, FuzzySearchMethodService>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

var identityApiMap = app.MapIdentityApi<IdentityUser>();

//Disable some auto-generated endpoints as below
//https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-9.0#the-mapidentityapituser-endpoints

//identityApiMap.AddEndpointFilter(async (efiContext, next) =>
//{
//    if (efiContext.HttpContext.Request.Path == "/register")
//    {
//        return Results.Forbid();
//    }
//    return await next(efiContext);
//});

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
