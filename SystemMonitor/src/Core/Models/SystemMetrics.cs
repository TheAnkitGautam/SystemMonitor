namespace Core.Models;
public record SystemMetrics(
    float CpuUsage,
    long MemoryUsed,
    long MemoryTotal,
    long DiskUsed,
    long DiskTotal
);