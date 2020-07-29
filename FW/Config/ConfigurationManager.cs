using System.IO;
using static NLog.LogManager;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FW.Config
{
    public static class ConfigurationManager
    {
        private static readonly NLog.Logger _logger = GetCurrentClassLogger();

        public static IConfiguration AppSetting { get; }

        static ConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
         
            SetLogDirectory();
        }

        public static void SetLogDirectory()
        {
            var logDir = $"{AppSettings.WORKSPACE_DIR}{Path.DirectorySeparatorChar}Logs";

            _logger.Info($"Value of logDir: {logDir}");

            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
                _logger.Info($"Created log directory");
            }
        }
    }
}
