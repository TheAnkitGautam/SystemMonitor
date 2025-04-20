#if WINDOWS
using Core.Interfaces;
using Core.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Infrastructure.Windows;

public sealed class WindowsSystemMonitor : ISystemMonitor
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    public SystemMetrics GetMetrics()
    {
        // 1. Initialize MEMORYSTATUSEX
        var memStatus = new MEMORYSTATUSEX
        {
            dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX))
        };

        // 2. Call API and check for errors
        if (!GlobalMemoryStatusEx(ref memStatus))
        {
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        }

        // 3. CPU Usage
        var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        cpuCounter.NextValue();
        Thread.Sleep(500);
        float cpu = cpuCounter.NextValue();

        // 4. RAM Calculation (using ulong to avoid overflow)
        ulong totalRam = memStatus.ullTotalPhys;
        ulong availableRam = memStatus.ullAvailPhys;
        ulong usedRam = totalRam - availableRam;

        // 5. Disk Usage
        var diskCounter = new DriveInfo("C");
        long diskUsed = diskCounter.TotalSize - diskCounter.AvailableFreeSpace;
        long diskTotal = diskCounter.TotalSize;

        return new SystemMetrics(
            cpu,
            (long)usedRam, 
            (long)totalRam,
            diskUsed,
            diskTotal
        );
    }
}
#endif