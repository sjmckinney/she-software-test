using System;
using System.Collections.ObjectModel;
using System.IO;
using FW.Config;
using NUnit.Framework;
using OpenQA.Selenium;
using static NLog.LogManager;

namespace FW.Selenium
{
    public static class Driver
    {

        private static readonly NLog.Logger _logger = GetCurrentClassLogger();

        [ThreadStatic]
        private static IWebDriver _driver;

        [ThreadStatic]
        public static Wait Wait;

        public static IWebDriver Current => _driver ?? throw new NullReferenceException("value of Driver._driver is null");

        public static void Init(int waitSeconds = 10)
        {
            _driver = DriverFactory.Build(AppSettings.Driver);
            Wait = new Wait(waitSeconds);
        }

        public static IWebElement FindElement(By by)
        {
            IWebElement element = null;
            try
            {
                element = _driver.FindElement(by);
            }
            catch (Exception e)
            {
                _logger.Error($"Driver.FindElement - Error: {e.Message} occurred for process {TestContext.CurrentContext.WorkerId}");
                throw;
            }
            return element;
        }

        public static ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            ReadOnlyCollection<IWebElement> elements = null;
            try
            {
                elements = _driver.FindElements(by);
            }
            catch (Exception e)
            {
                _logger.Error($"Driver.FindElements - Error: {e.Message} occurred for process {TestContext.CurrentContext.WorkerId}");
                throw;
            }
            return elements;
        }

        public static IWebElement FindElementWhenVisible(By by)
        {
            IWebElement element = null;
            try
            {
                element = Wait.Until(WaitConditions.ElementIsDisplayed(_driver.FindElement(by)));
            }
            catch (Exception e)
            {
                _logger.Error($"Driver.FindElementWhenVisible - Error: {e.Message} occurred for process {TestContext.CurrentContext.WorkerId}");
                throw;
            }
            return element;
        }

        public static ReadOnlyCollection<IWebElement> FindElementsWhenVisible(By by)
        {
            ReadOnlyCollection<IWebElement> elements = null;
            try
            {
                elements = Wait.Until(WaitConditions.ElementsAreDisplayed(_driver.FindElements(by)));
            }
            catch (Exception e)
            {
                _logger.Error($"Driver.FindElementsWhenVisible - Error: {e.Message} occurred for process {TestContext.CurrentContext.WorkerId}");
                throw;
            }
            return elements;
        }

        public static void Quit()
        {
            _driver.Quit();
        }

        public static void TakeScreenshot(string imageName)
        {
            var unique = DateTime.Now.TimeOfDay.ToString().Replace(":", "");
            var screenShot = ((ITakesScreenshot)Current).GetScreenshot();
            var screenShotFileName = Path.Combine($"{AppSettings.WORKSPACE_DIR}{Path.DirectorySeparatorChar}Logs", imageName);
            var screenShotName = $"{screenShotFileName}-{unique}.png";
            screenShot.SaveAsFile(screenShotName, ScreenshotImageFormat.Png);
            _logger.Info($"Took screenshot {screenShotName}");
        }
    }
}
