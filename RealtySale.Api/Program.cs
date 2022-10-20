using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealtySale.Api.Data.IRepositories;
using RealtySale.Api.Data.Repositories;
using RealtySale.Api.Extensions;
using RealtySale.Api.Helpers;
using RealtySale.Api.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
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
    var configuration = builder.Configuration;
    var jwtKey = configuration["JwtOptions:Key"];
    var jwtSecurityKeyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["JwtOptions:Issuer"],
        ValidAudience = configuration["JwtOptions:Audience"],
        IssuerSigningKey = jwtSecurityKeyKey
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<RealtySaleContext>(options => options.UseSqlServer(defaultDbConnectionString));

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

WebApplication app = builder.Build();

app.ConfigureExceptionHandler(app.Environment);

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
});

app.Run();
