serilog-sinks-cloudlog
======================
[![Nuget](https://img.shields.io/nuget/v/Serilog.Sinks.CloudLog)](https://www.nuget.org/packages/Serilog.Sinks.CloudLog)
[![Test status](https://github.com/anexia/serilog-sinks-cloudlog/actions/workflows/test.yml/badge.svg?branch=main)](https://github.com/anexia/serilog-sinks-cloudlog/actions/workflows/test.yml)

`serilog-sinks-cloudlog` is a Serilog sink that delivers log events to Anexia CloudLog.

**Note:** Usually it is considered best-practice to write rotating log-files to the filesystem, and send those logs to
CloudLog via `Filebeat`.

# Install

With a correctly set up .NET SDK, run in `PowerShell`:

```powershell
Install-Package Serilog.Sinks.CloudLog
```

# Getting started

To enable the sink, call `WriteTo.CloudLog()`.

## Example 1

To create logger without passing a `HttpClient` instance, use the code as follows:

```cs
using Serilog;

…
Log.Logger = new LoggerConfiguration()
    .WriteTo.CloudLog(index: "index-name", token: "token")
    .CreateLogger();
    
Log.Information("Hello CloudLog!");
```

## Example 2

To create logger with a custom `HttpClient` instance, use the code as follows:

```cs
using Serilog;

…
Log.Logger = new LoggerConfiguration()
    .WriteTo.CloudLog(
        index: "index-name",
        token: "token",
        (nameOfClass)=>{
            clientFactory.CreateClient(nameOfClass);
        })
    )
    .CreateLogger();
    
Log.Information("Hello CloudLog!");
```

**Note:** `HttpClient` has to be enabled: [Make HTTP requests using IHttpClientFactory](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0)


## Example 3

To create logger with a custom `HttpClient` instance using a `IHttpClientFactory`, use the code as follows:

```cs
using Serilog;

…
Log.Logger = new LoggerConfiguration()
    .WriteTo.CloudLog(
        index: "index-name",
        token: "token",
        clientFactory
    )
    .CreateLogger();
    
Log.Information("Hello CloudLog!");
```

**Note:** `HttpClient` has to be enabled: [Make HTTP requests using IHttpClientFactory](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0)

# Supported versions

|          | Supported |
|----------|-----------|
| .Net 5.0 | ✓         |
| .Net 6.0 | ✓         |
| .Net 7.0 | ✓         |