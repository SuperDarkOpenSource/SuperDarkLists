using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Serilog;
using Serilog.Events;
using SuperdarkLists.Transformers;

namespace SuperdarkLists;

internal static class Program
{
    private static void SetupLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
    }

    private static void ApplyConfigurations(WebApplicationBuilder builder, string[] programArgs)
    {
        builder.Host.ConfigureAppConfiguration((context, config) =>
        {
            config.Sources.Clear();

            var env = context.HostingEnvironment;

            // Order of calls matters here. Do not change.
            config.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            

            if (programArgs.Any())
            {
                config.AddCommandLine(programArgs);
            }
        });
    }
    
    private static void RegisterServices(WebApplicationBuilder builder)
    {
        // Tell ASP.NET Core to use AutoFac
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(e =>
            e.RegisterAssemblyModules(Assembly.GetExecutingAssembly()));
        
        // We also want to use Serilog
        builder.Host.UseSerilog();
        
        // Add our controllers to the ContainerRegistry for use later
        // Also we want to hookup NewtonsoftJson
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        }).AddNewtonsoftJson();
        
        
    }

    private static void RegisterMiddleware(WebApplication app)
    {
        // Log requests to Serilog instead of Microsoft's framework
        app.UseSerilogRequestLogging();

        // Map controller endpoints for the router
        app.MapControllers();
        
        // Use Routing Middleware
        app.UseRouting();
    }
    
    public static async Task Main(string[] args)
    {
        SetupLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            ApplyConfigurations(builder, args);
            RegisterServices(builder);
            
            var app = builder.Build();
            RegisterMiddleware(app);

            await app.RunAsync();
        }
        catch (Exception e) when (e.Source != "Microsoft.EntityFrameworkCore.Design") // see https://github.com/dotnet/efcore/issues/29923
        {
            // Suppress the application killing exception and log it
            Log.Fatal(e, "SuperdarkLists has terminated with an exception...");
        }
        finally
        {
            // Make sure the log gets flushed before exiting
            await Log.CloseAndFlushAsync();
        }
    }
}