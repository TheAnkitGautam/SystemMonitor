using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var services = new ServiceCollection();

        // Dependency Injection
        services.AddSingleton<ISystemMonitor, Infrastructure.Windows.WindowsSystemMonitor>();
        services.AddTransient<IMonitorPlugin, Plugins.FileLoggerPlugin>();
        services.AddTransient<IMonitorPlugin, Plugins.ApiPublisherPlugin>();
        services.AddHttpClient();

        // Configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        services.AddSingleton<IConfiguration>(configuration);

        var provider = services.BuildServiceProvider();
        var monitor = provider.GetRequiredService<ISystemMonitor>();
        var plugins = provider.GetServices<IMonitorPlugin>();
        int intervalSeconds = configuration.GetValue<int>("IntervalSeconds") * 1000;

        Console.Clear();
        Console.WriteLine("\x1b[1m=== SYSTEM MONITOR (Live) ===\x1b[0m");
        Console.WriteLine("Press Ctrl+C to exit\n");

        while (true)
        {
            try
            {
                var metrics = monitor.GetMetrics();
                PrintMetric("CPU", metrics.CpuUsage, "%", ConsoleColor.Cyan);
                PrintMemoryAndDisk(metrics);
                foreach (var plugin in plugins)
                    await plugin.ExecuteAsync(metrics);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\x1b[91mERROR: {ex.Message}\x1b[0m");
            }
            await Task.Delay(intervalSeconds);
        }

        static void PrintMetric(string label, float value, string unit, ConsoleColor color)
        {
            Console.Write($"{DateTime.Now:dd-MM-yy HH:mm:ss} | ");
            Console.ForegroundColor = color;
            Console.Write($"{label}: {value:N1}{unit}".PadRight(15));
            Console.ResetColor();
        }

        static void PrintMemoryAndDisk(SystemMetrics metrics)
        {
            // RAM
            var ramUsedGB = metrics.MemoryUsed / 1024.0 / 1024 / 1024;
            var ramTotalGB = metrics.MemoryTotal / 1024.0 / 1024 / 1024;
            Console.Write($"RAM: \x1b[93m{ramUsedGB:N1}GB\x1b[0m / ");
            Console.Write($"\x1b[92m{ramTotalGB:N1}GB\x1b[0m");
            Console.Write(" [");
            PrintProgressBar((double)metrics.MemoryUsed / metrics.MemoryTotal, ConsoleColor.Blue);
            Console.Write("] ");

            // Disk
            var diskUsedGB = metrics.DiskUsed / 1024.0 / 1024 / 1024;
            var diskTotalGB = metrics.DiskTotal / 1024.0 / 1024 / 1024;
            Console.Write($"Disk: \x1b[93m{diskUsedGB:N1}GB\x1b[0m / ");
            Console.Write($"\x1b[92m{diskTotalGB:N1}GB\x1b[0m");
            Console.Write(" [");
            PrintProgressBar((double)metrics.DiskUsed / metrics.DiskTotal, ConsoleColor.Magenta);
            Console.Write("]");
            Console.WriteLine("\n"); // Add a blank line after each update
        }

        static void PrintProgressBar(double percentage, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            const int barLength = 10;
            var progress = (int)(percentage * barLength);
            Console.Write(new string('█', progress));
            Console.Write(new string('░', barLength - progress));
            Console.ResetColor();
        }
    }
}