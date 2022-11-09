using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealtySale.Api.Extensions;
using RealtySale.Api.Helpers;
using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Api.Repositories.Repository;
using RealtySale.Api.Services.IService;
using RealtySale.Api.Services.Service;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
string? defaultDbConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddCors();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtKey = configuration["JwtOptions:Key"];
    var jwtSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["JwtOptions:Issuer"],
        ValidAudience = configuration["JwtOptions:Audience"],
        IssuerSigningKey = jwtSecurityKey
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<RealtySaleContext>(options => options.UseSqlServer(defaultDbConnectionString));

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IPhotoService, PhotoService>();

WebApplication app = builder.Build();

app.ConfigureExceptionHandler(environment);

app.UseRouting();

app.UseCors(x => x.WithOrigins(configuration["BrowserPaths:MainApplication"])
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
});

app.Run();
