using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer3.Integration.AspNetCore
{
    internal class CopyInputStream
    {
        readonly Func<IDictionary<string, object>, Task> _next;

        public CopyInputStream(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);

            Stream oldStream = context.Request.Body;
            var body = new StreamReader(context.Request.Body).ReadToEnd();
            var requestData = Encoding.UTF8.GetBytes(body);
            context.Request.Body = new MemoryStream(requestData);

            await _next(env);

            context.Request.Body = oldStream;
        }
    }
}
