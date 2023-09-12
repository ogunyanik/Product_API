using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product_API.Core.Filters;
using Product_API.Core.Interfaces;
using Product_API.Core.Mappings;
using Product_API.Core.Services;
using Product_API.Infrastructure.Data;
using Product_API.Infrastructure.Repositories;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Product_API.Core;
using Product_API.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Elasticsearch.Net;
using Nest;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Product API",
        Version = "v1"
    });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            },
            Scheme = "Oauth2",
            Name = JwtBearerDefaults.AuthenticationScheme,
            In = ParameterLocation.Header
        },
        new List<string>()
    }});
    });
 
var connectionPool = new SingleNodeConnectionPool(new Uri(builder.Configuration["Elasticsearch:Uri"]));
var settings = new ConnectionSettings(connectionPool)
    .DefaultIndex(builder.Configuration["Elasticsearch:DefaultIndex"]);
var client = new ElasticClient(settings);

// Register the Elasticsearch client
builder.Services.AddSingleton<IElasticClient>(client);

// Register the ProductSearchService
builder.Services.AddSingleton<IProductSearchService,ProductSearchService>();





builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDatabase")));

builder.Services.AddValidatorsFromAssemblyContaining<ValidateProductDTOAttribute>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
} );

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimiting"));

builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StaticJwtTokenPolicy", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var authFilterContext = (HttpContext?)context.Resource;
            var authHeader = string.Empty;
            if (authFilterContext != null)
            {
               authHeader  = authFilterContext.Request.Headers["Authorization"];
            }

            // Get the configured JWT secret key from appsettings
            // var jwtSettings = context.Resource as IConfiguration;
            var jwtSecretKey = builder.Configuration["Jwt:Key"];

            // Get the Authorization header from the request
            
            var authorizationHeader = context.User.FindFirst("access_token")?.Value;
 
            // Check if the token matches the configured static token 
            return authHeader == $"Bearer {jwtSecretKey}";
        });
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // ValidIssuer = builder.Configuration["Jwt:Issuer"],
        // ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

// builder.Services.AddHttpClient<IProductService, ProductService>(c =>
//     {
//     }
// ).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(30)));

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddResponseCaching();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200") ){
        DetectElasticsearchVersion = false,
        AutoRegisterTemplate = true,
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
    }).CreateLogger();

Log.Information("Starting up..");
builder.Host.UseSerilog();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(System.Net.IPAddress.Any, 8080, listenOptions =>
    {
         
    });
});


var app = builder.Build();
app.UseIpRateLimiting();

var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseResponseCaching();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

using (var scope =  app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
} 

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());

            options.RoutePrefix = "api/documents";
        }
    } );


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
 