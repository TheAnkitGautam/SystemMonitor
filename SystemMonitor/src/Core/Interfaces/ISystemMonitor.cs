using Core.Models;

namespace Core.Interfaces;
public interface ISystemMonitor
{
    SystemMetrics GetMetrics();
}