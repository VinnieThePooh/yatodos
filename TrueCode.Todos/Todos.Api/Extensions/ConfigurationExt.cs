using Microsoft.Extensions.Options;
using TrueCode.Todos.Auth;

namespace TrueCode.Todos.Controllers;

public static class ConfigurationExt
{
    public static IServiceCollection ConfigureSingletonSettings<T>(WebApplicationBuilder applicationBuilder) where T : class, ISettings
    {
        applicationBuilder.Services.Configure<T>(applicationBuilder.Configuration.GetSection(T.SectionKey));
        applicationBuilder.Services.AddSingleton<T>(x => x.GetRequiredService<IOptions<T>>().Value);
        return applicationBuilder.Services;
    }
}