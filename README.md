# IdentityServer3 Integration Library for ASP.NET 5

Data protection must be enabled for your ASP.NET 5 application to use this library. Example:

```
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDataProtection();
    }
}
```
