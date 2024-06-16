using NLog;
using NLog.Web;
using testWorkKoshelek.API;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

public class Program
{
    public static void Main(string[] args)
    {
        var logger = LogManager.Setup()
            .SetupExtensions(builder =>
            {
                builder.RegisterAspNetLayoutRenderer("http-request-query", (eventInfo, httpContext, config) => GetQueryStringFromQuery(httpContext));
                builder.RegisterAspNetLayoutRenderer("user-info", (eventInfo, httpContext, config) => GetUserInfoFromQuery(httpContext));
            })
            .LoadConfigurationFromFile("NLog.config")
            .GetCurrentClassLogger();


        try
        {
            logger.Debug("Запуск микросервиса");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Остановка микросервиса из-за исключения");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();

    #region Actions for NLog render layout

    /// <summary>
    /// user-info
    /// </summary>
    public static string GetUserInfoFromQuery(HttpContext context)
    {
        if (context?.User != null)
        {
            var userInfo = context.User.FindFirst("Category")?.Value;
            return userInfo;
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// http-request-query
    /// </summary>
    public static string GetQueryStringFromQuery(HttpContext context)
    {
        if (context?.Request?.QueryString != null)
        {
            var queryString = context.Request.QueryString.Value;
            return queryString;
        }

        return string.Empty;
    }

    #endregion
}