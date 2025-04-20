using Core.Interfaces;
using Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Plugins;

public class FileLoggerPlugin : IMonitorPlugin
{
    public async Task ExecuteAsync(SystemMetrics metrics)
    {
        // Get the user's Documents folder path (cross-platform)
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string logPath = Path.Combine(documentsPath, "system_monitor_logs.txt");

        // Format the log entry
        string log = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | " +
                    $"CPU: {metrics.CpuUsage:N1}%, " +
                    $"RAM: {metrics.MemoryUsed / 1024 / 1024}MB, " +
                    $"Disk: {metrics.DiskUsed / 1024 / 1024 /1024}GB\n";

        // Write to file
        await File.AppendAllTextAsync(logPath, log);
    }
}