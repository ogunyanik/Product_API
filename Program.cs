using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product_API.Core.Filters;
using Product_API.Core.Interfaces;
using Product_API.Core.Mappings;
using Product_API.Core.Services;
using Product_API.Infrastructure.Data;
using Product_API.Infrastructure.Repositories;
using AspNetCoreRateLimit;
using Polly;
using Product_API.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDatabase")));

builder.Services.AddValidatorsFromAssemblyContaining<ValidateProductDTOAttribute>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimiting"));

builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<IProductService, ProductService>(c =>
    {
    }
).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(30)));

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
var app = builder.Build();
app.UseIpRateLimiting();

// Configure Serilog for logging
var configuration = new ConfigurationBuilder()
    .SetBasePath(Environment.CurrentDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
app.UseMiddleware<ExceptionHandlingMiddleware>(logger);

app.UseSerilogRequestLogging();

using (var scope =  app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
} 

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
 