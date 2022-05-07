using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using RestaurantApp;
using RestaurantApp.Authorization;
using RestaurantApp.Entities;
using RestaurantApp.Middleware;
using RestaurantApp.Models;
using RestaurantApp.Models.Validators;
using RestaurantApp.Services;

var builder = WebApplication.CreateBuilder(args);

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new RestaurantMappingProfile());
});

var services = builder.Services;
services.AddCors();
services.AddControllers();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes("PRIVATE_KEY_DONT_SHARE")),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ResourceOperationRequirementHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AtLeast 20", policy =>
        policy.Requirements.Add(new MinimumAgeRequirement(20)));
});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddAutoMapper(typeof(RestaurantMappingProfile));

builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddDbContext<RestaurantDbContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

builder.Services.AddScoped<IRestaurantService, RestaurantService>();

builder.Services.AddScoped<IDishService, DishService>();

builder.Services.AddScoped<AuthenticationSettings>();

builder.Services.AddSingleton<IValidator<RestaurantQuery>, RestaurantQueryValidator>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

builder.Host.UseNLog();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", builder =>
                builder.AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost:5000"));
});

var app = builder.Build();

{
    // configure HTTP request pipeline
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

}

    app.Use(async (context, next) =>
{
    // Do work that doesn't write to the Response.
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseResponseCaching();

app.UseStaticFiles();

app.UseCors("FronetEndClient");

app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.MapRazorPages();

app.UseAuthorization();   

app.MapControllers();

app.MapControllerRoute
(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.UseEndpoints(x =>
{
    x.MapGet("/ping", () => "pong");
});

app.Run(async context =>
{
    await context.Response.WriteAsync("Hello from 2nd delegate.");
});

app.UseMiddleware<RequestMiddleware>();

app.Run();






