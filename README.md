serilog-sinks-cloudlog
===============
[![Nuget](https://img.shields.io/nuget/v/Serilog.Sinks.CloudLog)](https://www.nuget.org/packages/Serilog.Sinks.CloudLog)
[![Test status](https://github.com/anexia-it/serilog-sinks-cloudlog/actions/workflows/test.yml/badge.svg?branch=main)](https://github.com/anexia-it/dotnet-cloudlog/actions/workflows/test.yml)

Serilog Sink for Anexia CloudLog

### Getting started

To use this sink, first install the [NuGet package](https://nuget.org/packages/Serilog.Sinks.CloudLog):

```powershell
Install-Package Serilog.Sinks.CloudLog
```

Then enable the sink using `WriteTo.CloudLog()`:

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.CloudLog(index: "index-name", caFile: "C:/private/cloudlog-ca.pem", certFile: "C:/private/cloudlog-client.pem", keyFile: "C:/private/cloudlog-client.key")
    .CreateLogger();
    
Log.Information("Hello CloudLog!");
```

# Supported versions

|          | Supported |
|----------|-----------|
| .Net 5.0 | ✓         |
| .Net 6.0 | ✓         |
| .Net 7.0 | ✓         |
