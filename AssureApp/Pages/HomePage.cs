using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using FW.Selenium;
using static NLog.LogManager;

namespace AssureApp.Pages
{
    public class HomePage : BasePage
    {
        private static readonly NLog.Logger _logger = GetCurrentClassLogger();

        public static string environmentalModuleCssLocator = "a[data-areaname='Environment']";

        private string _modulesMenuItemXPathLocator = "//div[contains(@class,'headercenter')]//a[text()='Modules']";

        private IWebElement _modulesMenuItem;

        public void OpenPageFromModule(string pageName, string moduleName)
        {
            try
            {
                _modulesMenuItem = Driver.FindElementWhenVisible(By.XPath(_modulesMenuItemXPathLocator));
                _modulesMenuItem.Click();
                var moduleMenuItem = Driver.FindElementWhenVisible(By.CssSelector($"li[data-areaname='{moduleName}']"));
                new Actions(Driver.Current).MoveToElement(moduleMenuItem).Build().Perform();
                var pageMenuItem = Driver.FindElementWhenVisible(By.CssSelector($"a[href*='{pageName.Replace(" ", "")}/Page']"));
                pageMenuItem.Click();
                Driver.Wait.Until(WaitConditions.ElementIsDisplayedByLocator(By.XPath($"//nav/strong[text()='{pageName}']")));
            }
            catch (Exception e)
            {
                _logger.Error($"Unable to open module {moduleName} and menu item {pageName}");
                _logger.Error($"Error message: {e.Message}");
                throw;
            }
        }
    }
}
