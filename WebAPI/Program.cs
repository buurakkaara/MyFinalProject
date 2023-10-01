using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

IConfigurationSection tokenOptionsSection = builder.Configuration.GetSection(nameof(TokenOptions));
builder.Services.Configure<TokenOptions>(tokenOptionsSection);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new AutofacBusinessModule());
            });


var tokenOptions = tokenOptionsSection.Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });

builder.Services.AddDependencyResolvers(new ICoreModule[]{
    new CoreModule()
});

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddSingleton<ICarService, CarManager>();
//builder.Services.AddSingleton<ICarRepository, EfCarRepository>();

//builder.Services.AddSingleton<IBrandService, BrandManager>();
//builder.Services.AddSingleton<IBrandRepository, EfBrandRepository>();

//builder.Services.AddSingleton<IColorService, ColorManager>();
//builder.Services.AddSingleton<IColorRepository, EfColorRepository>();

//builder.Services.AddSingleton<IUserService, UserManager>();
//builder.Services.AddSingleton<IUserRepository, EfUserRepository>();

//builder.Services.AddSingleton<ICustomerService, CustomerManager>();
//builder.Services.AddSingleton<ICustomerRepository, EfCustomerRepository>();

//builder.Services.AddSingleton<IRentalService, RentalManager>();
//builder.Services.AddSingleton<IRentalRepository, EfRentalRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
