using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web3_1.MapperExamples;

namespace Web3_1.CustomMiddleware
{
    public class BlockEventCodeMiddleware
    {
        private readonly RequestDelegate _next;

        public BlockEventCodeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            EventViewModel model;
            if (HttpMethods.IsPost(httpContext.Request.Method))
            {
                var stream = httpContext.Request.Body;
                var content = await new StreamReader(stream).ReadToEndAsync();

                model = Newtonsoft.Json.JsonConvert.DeserializeObject<EventViewModel>(content);

                if (model.Id % 2 != 0)
                {
                    model.Title += " (edited)";
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var contentModified = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                    stream = await contentModified.ReadAsStreamAsync();
                }
                else
                {
                    var requestData = Encoding.UTF8.GetBytes(content);
                    stream = new MemoryStream(requestData);
                }

                httpContext.Request.Body = stream;

                await _next(httpContext);

            }
        }
    }


}
