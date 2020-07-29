using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using static FW.Config.ConfigurationManager;

namespace FW.Config
{
    public static class AppSettings
    {
        public static string Driver => AppSetting["driver:browser"];
        public static string Url => AppSetting["url"];
        public static string CurrentEnv => AppSetting["current_env"];
        public static List<User> Users => AppSetting.GetSection("users").Get<List<User>>();
        public static string WORKSPACE_DIR => Path.Combine("..", "..", "..", "..");
	}
}
