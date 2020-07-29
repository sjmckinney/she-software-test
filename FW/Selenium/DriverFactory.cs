using static System.Environment;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using FW.Config;

namespace FW.Selenium
{
    public static class DriverFactory
    {
        readonly static string pathToDriversDir = $"{AppSettings.WORKSPACE_DIR}{System.IO.Path.DirectorySeparatorChar}_drivers";
        public static IWebDriver Build(string browserName)
        {
            switch (browserName.ToLower())
            {
                case "chrome":
                    var chromeService = ChromeDriverService.CreateDefaultService(pathToDriversDir);
                    //service.LogPath = "./chromedriver.log";
                    //service.EnableVerboseLogging = true;
                    return new ChromeDriver(chromeService);
                case "firefox":
                    var firefoxService = FirefoxDriverService.CreateDefaultService(pathToDriversDir);
                    //firefoxService.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
                    firefoxService.FirefoxBinaryPath = GetFolderPath(SpecialFolder.LocalApplicationData) + "\\Mozilla Firefox\\firefox.exe";
                    return new FirefoxDriver(firefoxService);
                case "edge":
                    //Version 4.0.0-alpha05
                    var edgeOptions = new EdgeOptions() { UseChromium = true }; //set to false so do not create legacy version
                    //edgeOptions.UseInPrivateBrowsing = true;
                    // prevents popup regarding ability to load extensions when browser
                    // spawned due to non admin permissions on file system
                    edgeOptions.AddAdditionalChromeOption("useAutomationExtension", false);
                    var edgeService = EdgeDriverService.CreateChromiumService(pathToDriversDir);
                    return new EdgeDriver(edgeService, edgeOptions);
                default:
                    throw new System.ArgumentException($"{browserName} not supported.");
            }
        }
    }
}
