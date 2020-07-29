using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using static NLog.LogManager;

namespace FW.Selenium
{
    public class WaitConditions
    {
        private static readonly NLog.Logger _logger = GetCurrentClassLogger();

        /// <summary>
        /// Returns an existing web element when it becomes visible
        /// </summary>
        /// <param name="element">IWebElement</param>
        /// <returns>IWebElement</returns>
        public static Func<IWebDriver, IWebElement> ElementIsDisplayed(IWebElement element)
        {
            IWebElement condition(IWebDriver driver)
            {
                try
                {
                    _logger.Debug($"Element is displayed: {element.Displayed}");
                    return element.Displayed ? element : null;
                }
                catch (NoSuchElementException NSEE)
                {
                    _logger.Error($"NoSuchElementException message: { NSEE.Message}");
                    return null;
                }
                catch (ElementNotVisibleException ENVE)
                {
                    _logger.Error($"ElementNotVisibleException message: {ENVE.Message}");
                    return null;
                }
            }
            return condition;
        }

        /// <summary>
        /// Returns an existing web element when it exists on the page
        /// </summary>
        /// <param name="by">By</param>
        /// <returns>IWebElement</returns>
        public static Func<IWebDriver, IWebElement> ElementIsDisplayedByLocator(By by)
        {
            IWebElement condition(IWebDriver driver)
            {
                try
                {
                    return Driver.FindElement(by);
                }
                catch (NoSuchElementException NSEE)
                {
                    _logger.Error($"NoSuchElementException message: { NSEE.Message}");
                    return null;
                }
                catch (ElementNotVisibleException ENVE)
                {
                    _logger.Error($"ElementNotVisibleException message: {ENVE.Message}");
                    return null;
                }
            }
            return condition;
        }

        /// <summary>
        /// Returns collection of existing web elementx when they become visible
        /// </summary>
        /// <param name="elements">ReadOnlyCollection<IWebElement></param>
        /// <returns>ReadOnlyCollection<IWebElement></returns>
        public static Func<IWebDriver, ReadOnlyCollection<IWebElement>> ElementsAreDisplayed(ReadOnlyCollection<IWebElement> elements)
        {
            ReadOnlyCollection<IWebElement> condition(IWebDriver driver)
            {
                try
                {
                    _logger.Debug($"Number of elements present: {elements.Count}");
                    return (elements.Count > 0) ? elements : null;
                }
                catch (NoSuchElementException NSEE)
                {
                    _logger.Error($"NoSuchElementException message: { NSEE.Message}");
                    return null;
                }
                catch (ElementNotVisibleException ENVE)
                {
                    _logger.Error($"ElementNotVisibleException message: {ENVE.Message}");
                    return null;
                }
            }
            return condition;
        }

        /// <summary>
        /// Returns true when the page title values changes to expected value
        /// i.e. the new page has loaded and the test can continue
        /// </summary>
        /// <param name="title">string</param>
        /// <returns>bool</returns>
        public static Func<IWebDriver, bool> PageTitleMatches(string title)
        {
            _logger.Debug($"Value of parameter title is : {title}");

            bool condition(IWebDriver driver)
            {
                try
                {
                    _logger.Debug($"Value of driver.Title is : {driver.Title}");
                    return driver.Title.Equals(title);
                }
                catch (Exception e)
                {
                    _logger.Error($"WaitConditions.PageTitleMatches: {e.Message}");
                    return false;
                }
            }

            return condition;
        }

        /// <summary>
        /// Returns true when the page title contains an expected value
        /// </summary>
        /// <param name="keyword">string</param>
        /// <returns>bool</returns>
        public static Func<IWebDriver, bool> PageTitleContains(string keyword)
        {
            bool condition(IWebDriver driver)
            {
                try
                {
                    return driver.Title.Contains(keyword);
                }
                catch (Exception e)
                {
                    _logger.Error($"WaitConditions.PageTitleContains: {e.Message}");
                    return false;
                }
            }

            return condition;
        }

        /// <summary>
        /// Returns true when the page url contains an expected value
        /// </summary>
        /// <param name="keyword">string</param>
        /// <returns>bool</returns>
        public static Func<IWebDriver, bool> CurrentUrlContains(string keyword)
        {
            bool condition(IWebDriver driver)
            {
                try
                {
                    return driver.Url.Contains(keyword);
                }
                catch (Exception e)
                {
                    _logger.Error($"WaitConditions.CurrentUrlContains: {e.Message}");
                    return false;
                }
            }

            return condition;
        }

        /// <summary>
        /// Returns true when the page is loaded defined by
        /// the 'document.readyState' == 'complete'
        /// </summary>
        /// <param name="keyword">string</param>
        /// <returns>bool</returns>
        public static Func<IWebDriver, bool> PageLoadComplete()
        {
            bool condition(IWebDriver driver)
            {
                try
                {
                    return ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");
                }
                catch (Exception e)
                {
                    _logger.Error($"WaitConditions.PageLoadComplete: {e.Message}");
                    return false;
                }
            }
            return condition;
        }

        /// <summary>
        /// Returns true when a number of options exist in drop-down
        /// select box
        /// </summary>
        /// <param name="selectElementId">element id</param>
        /// <returns>bool</returns>
        public static Func<IWebDriver, bool> SelectOptionsPresent(string selectElementId)
        {
            bool condition(IWebDriver driver)
            {
                try
                {
                    return ((IJavaScriptExecutor)driver).ExecuteScript($"return document.querySelectorAll('#{selectElementId} option').length > 0").Equals(true);
                }
                catch (Exception e)
                {
                    _logger.Error($"WaitConditions.SelectOptionsPresent: {e.Message}");
                    return false;
                }
            }
            return condition;
        }

        /// <summary>
        /// Returns true when an element.style.display == none
        /// </summary>
        /// <param name="selectElementId">element id</param>
        /// <returns>bool</returns>
        public static Func<IWebDriver, bool> ElementIsNotVisible(string elementId)
        {
            bool condition(IWebDriver driver)
            {
                try
                {
                    return ((IJavaScriptExecutor)driver).ExecuteScript($"return document.querySelector('#{elementId}').style.display == 'none'").Equals(true);
                }
                catch (Exception e)
                {
                    _logger.Error($"WaitConditions.ElementIsNotVisible: {e.Message}");
                    return false;
                }
            }
            return condition;
        }

        /// <summary>
        /// Returns true when an element does not exist
        /// </summary>
        /// <param name="selectElementId">selector</param>
        /// <returns>bool</returns>
        public static Func<IWebDriver, bool> ElementDoesNotExist(string selector)
        {
            bool condition(IWebDriver driver)
            {
                try
                {
                    return ((IJavaScriptExecutor)driver).ExecuteScript($"return document.querySelector(\"{selector}\") == null").Equals(true);
                }
                catch (Exception e)
                {
                    _logger.Error($"WaitConditions.ElementIsNotVisible: {e.Message}");
                    return false;
                }
            }
            return condition;
        }
    }
}
