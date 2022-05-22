using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebCommon.Service;

namespace WebUI_WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration.GetSection("WebApi").GetValue<string>("Url"))
            };

            builder.Services.AddScoped(sp => httpClient);

            var subscriptionKey = builder.Configuration.GetSection("WebApi").GetValue<string>("SubscriptionKey");
            if (subscriptionKey != null)
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");
            }

            builder.Services.AddSingleton((container) =>
            {
                var logger = container.GetRequiredService<ILogger<ApiClient>>();
                return new ApiClient(httpClient, logger);
            });

            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            await builder.Build().RunAsync();
        }
    }
}
