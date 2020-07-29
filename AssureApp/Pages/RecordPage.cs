using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using FW.Selenium;
using TechTalk.SpecFlow;
using static NLog.LogManager;

using AssureApp.DataEntities;
using System.Reflection;

namespace AssureApp.Pages
{
    class RecordPage : BasePage
    {
        private static readonly NLog.Logger _logger = GetCurrentClassLogger();

        private IWebElement _saveAndCloseBtn;

        private readonly string _descriptionsInputId = "SheAirEmissions_Description";
        private readonly string _dateInputId = "SheAirEmissions_SampleDate";
        private readonly string _locationInputId = "SheAirEmissions_Location";
        private readonly string _saveAndCloseBtnCssSelector = "button[value='Close']";

        public void EnterRecord(RecordData data)
        {
            var properties = GetRecordProperties(data);
            foreach (var prop in properties)
            {
                switch (prop.Name)
                {
                    case ("Description"):
                        EnterData(data.Description, By.Id(_descriptionsInputId));
                        break;
                    case ("Date"):
                        if(data.Date.ToLower() == "current")
                        {
                            data.Date = DateTime.Now.ToString("dd/MM/yyyy");
                        }
                        EnterData(data.Date, By.Id(_dateInputId));
                        break;
                    case ("Location"):
                        EnterData(data.Location, By.Id(_locationInputId));
                        break;
                    case ("RecordNumber"):
                        break;
                    default:
                        throw new System.ArgumentException($"Property {prop} not matched by case statement.");
                }
            }
        }

        private void EnterData(string data, By locator)
        {
            if(!string.IsNullOrEmpty(data))
            {
                IWebElement input = Driver.FindElementWhenVisible(locator);
                input.SendKeys(data);
            }
        }

        private PropertyInfo[] GetRecordProperties(object obj)
        {
            try
            {
                return obj.GetType().GetProperties();
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to get object properties");
                _logger.Error($"Error message: {e.Message}");
                throw;
            }
        }
        public void SaveRecordAndClose()
        {
            try
            {
                _saveAndCloseBtn = Driver.FindElementWhenVisible(By.CssSelector(_saveAndCloseBtnCssSelector));
                _saveAndCloseBtn.Click();
            }
            catch (Exception e)
            {
                _logger.Error($"Unable to save and close record");
                _logger.Error($"Error message: {e.Message}");
                throw;
            }
        }
    }
}
