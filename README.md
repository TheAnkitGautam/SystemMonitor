# System Resource Monitor & API Server

![.NET Version](https://img.shields.io/badge/.NET-8.0-%23512bd4)  
![Node.js Version](https://img.shields.io/badge/Node.js-18+-%23339933)  

A real-time system monitoring tool with plugin architecture and API integration, paired with a Node.js test server.

---

## 🚀 Quick Start

### 1. Clone Repository

```bash
[git clone https://github.com/TheAnkitGautam/system-monitor.git](https://github.com/TheAnkitGautam/SystemMonitor.git)
```


# 2. Install Dependencies
## For C# App:

```bash
cd SystemMonitor
dotnet restore
```

## For Node.js Server:
```bash
cd MetricsApi
npm install
```

# 3. Configure & Run

Edit the API endpoint (if needed) in appsettings.json

```json
{
  "IntervalSeconds": 2,
  "Api": {
    "Url": "http://localhost:5000/api/metrics"
  }
}
```

# 4. Start Node.js API Server:

```bash
cd MetricsApi
node index.js
```

# 5. Run Monitoring App:

```bash
cd SystemMonitor
dotnet run
```

# 🌟 Features
Real-Time Monitoring: CPU/RAM/Disk usage updates every 5 seconds

Plugin System: Extensible via IMonitorPlugin interface

API Integration: Built-in HTTP client for metrics delivery

Cross-Platform: Windows-first design with Linux/macOS readiness

Logging: Persistent storage in Documents/system_monitor_logs.txt

## 🛠️ Architecture

### 🔹 C# Monitoring App

| Component       | Description                         | Key Files                          |
|----------------|-------------------------------------|------------------------------------|
| Core           | Interfaces and models               | `ISystemMonitor`, `SystemMetrics` |
| Infrastructure | Platform-specific implementations   | `WindowsSystemMonitor.cs`         |
| Plugins        | Extensions (File/API logging)       | `FileLoggerPlugin.cs`             |
| ConsoleApp     | Entry point and DI configuration    | `Program.cs`, `appsettings.json`  |

### 🔹 Node.js API Server

| Component   | Description                    | Key Files          |
|------------|--------------------------------|--------------------|
| `index.js`| Express.js endpoint for metrics | `POST /api/metrics`|
| Middleware | JSON parsing and CORS handling  | `body-parser`      |

## 🔍 Testing Guide

### 1. Verify Console Output

You should see output like:

```bash
[14:22:35] CPU: 24.7%       RAM: 6.4GB / 16.0GB [████░░░░░░] Disk: 128.1GB / 256.0GB [█████░░░░░]
```

### 2. Validate Log Files
Check `Documents/system_monitor_logs.txt`:

```txt
2025-04-20 14:22:35 | CPU: 24.7%, RAM: 6723MB, Disk: 134217MB
```
### 3. Test API Integration
Using Node.js Server:

```json
POST http://localhost:5000/api/metrics

{
  "cpu": 24.7,
  "ram_used": "6723 MB",
  "ram_total": "7096 MB",
  "disk_used": "134217 MB",
  "disk_total": "200000 MB",
}
```
