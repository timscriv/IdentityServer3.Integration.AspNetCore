using System;
using IdentityServer3.Core.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Owin;

namespace Microsoft.AspNetCore.Builder
{
    using IdentityServer3.Integration.AspNetCore;
    using DataProtectionProviderDelegate = Func<string[], Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>>;
    using DataProtectionTuple = Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>;

    public static class IdentityServerApplicationBuilderExtensions
    {
        public static void UseIdentityServer(this IApplicationBuilder app, IdentityServerOptions options)
        {
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var builder = new Microsoft.Owin.Builder.AppBuilder();
                    var provider = app.ApplicationServices.GetRequiredService<DataProtection.IDataProtectionProvider>();

                    builder.Properties["security.DataProtectionProvider"] = new DataProtectionProviderDelegate(purposes =>
                    {
                        var dataProtection = provider.CreateProtector(String.Join(",", purposes));
                        return new DataProtectionTuple(dataProtection.Protect, dataProtection.Unprotect);
                    });

                    builder.Use<CopyInputStream>();

                    builder.UseIdentityServer(options);

                    var appFunc = builder.Build(typeof(Func<IDictionary<string, object>, Task>)) as Func<IDictionary<string, object>, Task>;
                    return appFunc;
                });
            });
        }
    }
}