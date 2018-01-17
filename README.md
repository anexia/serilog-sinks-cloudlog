# serilog-sinks-cloudlog
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
