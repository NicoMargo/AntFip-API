using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IT_Arg_API.Services;

var builder = WebApplication.CreateBuilder(args);

DBHelper.ConnectionString = builder.Configuration["ConnectionString"];
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();

//JWT config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

//Cors
//WithOrigins("http://localhost:3000", "https://localhost:3000", "api.ayukelen.com.ar","frontadmin.ayukelen.com.ar")
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_corsPolicy",
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:3000", "frontadmin.ayukelen.com.ar", "https://frontadmin.it-arg.com", "http://ayukelen.com.ar", "https://ayukelen.com.ar", "https://localhost:7191", "null", "https://it-arg.com")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                          });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//app.UseStaticFiles();
app.UseCors("_corsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();