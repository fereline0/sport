using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace frontend.Services
{
    public static class HttpClientFactory
    {
        public static HttpClient CreateClient()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var apiUrl = configuration["API:URL"];

            if (string.IsNullOrEmpty(apiUrl))
            {
                throw new InvalidOperationException("API base URL is not configured.");
            }

            var client = new HttpClient { BaseAddress = new Uri(apiUrl) };

            return client;
        }
    }
}
