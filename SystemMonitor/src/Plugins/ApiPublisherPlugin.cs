using Core.Interfaces;
using Core.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Plugins;

public class ApiPublisherPlugin : IMonitorPlugin
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public ApiPublisherPlugin(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiUrl = config["Api:Url"]!;
    }

    public async Task ExecuteAsync(SystemMetrics metrics)
    {
        var payload = new
        {
            cpu = metrics.CpuUsage + " %",
            ram_used = metrics.MemoryUsed / 1024 / 1024 + " MB",
            ram_total = metrics.MemoryTotal / 1024 / 1024 + " MB",
            disk_used = metrics.DiskUsed / 1024 / 1024 + " MB",
            disk_total = metrics.DiskTotal / 1024 / 1024 + " MB"
        };
        await _httpClient.PostAsJsonAsync(_apiUrl, payload);
    }
}