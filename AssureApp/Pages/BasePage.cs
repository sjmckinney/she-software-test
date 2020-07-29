using System;
using FW.Selenium;
using static NLog.LogManager;

namespace AssureApp.Pages
{
    public class BasePage
    {
        private static readonly NLog.Logger _logger = GetCurrentClassLogger();

        public string GetCurrentTitle()
        {
            var currPageTitle = "";

            try
            {
                currPageTitle = Driver.Current.Title;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }

            _logger.Debug($"Value of current page title is: {currPageTitle}");
            return currPageTitle;
        }

        public string GetCurrentUrl()
        {
            var currPageUrl = "";

            try
            {
                currPageUrl = Driver.Current.Url;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }

            _logger.Debug($"Value of current page URL is: {currPageUrl}");
            return currPageUrl;
        }
    }
}
