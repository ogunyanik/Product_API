using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Product_API.Core;

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
    
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        this._apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }
    
    public void Configure(SwaggerGenOptions options)
    {
        Configure(options);
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        foreach (var item in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(item.GroupName,CreateVersionInfo(item));
        }
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Your Versioned API",
            Version = description.ApiVersion.ToString()
        };
        return info;
    }
}