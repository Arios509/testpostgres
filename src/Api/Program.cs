using Api.Infrastructure;
using Api.Infrastructure.AutofacModules;
using Api.Infrastructure.Repositories;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Dapper;
using Domain.Aggregate.User;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Identity.Helpers;
using Infrastructure.Repositories;
using Infrastructure.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
builder.Services.AddOptions()
    .Configure<ConnectionStringOptions>(configuration.GetSection("ConnectionString"))
    .Configure<JwtOptions>(configuration.GetSection("JWT"));


// postgres connection
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(configuration.GetSection("ConnectionString")["DefaultConnection"],
        b => b.MigrationsAssembly("Api"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDapper, DapperUtils>();
builder.Services.AddScoped<IBaseRepository, BaseRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new MediatorModule("Api"));
});
builder.Services.AddMvc()
 .AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.PropertyNamingPolicy = null;
     options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
 });

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

SqlMapper.AddTypeHandler(typeof(Detail), new JsonTypeHandler());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(MyAllowSpecificOrigins);
}

var swaggerOptions = new SwaggerOptions();

app.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

app.UseSwagger(option =>
{
    option.RouteTemplate = swaggerOptions.JsonRoute;
});

app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
