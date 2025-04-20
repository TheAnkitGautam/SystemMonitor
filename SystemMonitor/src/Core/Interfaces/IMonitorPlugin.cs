using Core.Models;

namespace Core.Interfaces;
public interface IMonitorPlugin
{
    Task ExecuteAsync(SystemMetrics metrics);
}