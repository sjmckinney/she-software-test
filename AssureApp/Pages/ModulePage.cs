using System;

using OpenQA.Selenium;
using FW.Selenium;
using static NLog.LogManager;

using AssureApp.DataEntities;

namespace AssureApp.Pages
{
    public class ModulePage : BasePage
    {
        private static readonly NLog.Logger _logger = GetCurrentClassLogger();

        private RecordPage _recordPage = new RecordPage();

        private IWebElement _newRecordBtn;

        private readonly string _newRecordBtnCssSelector = "a.btn[href$='AirEmissions/Create']";
        private readonly string _spinnerId = "spinner";
        private readonly string _recordSectionCssSelector = "div.item-box ul.list_information";

        public SavedRecordData CreateNewRecord(RecordData data)
        {
            OpenNewRecordPage();
            _recordPage.EnterRecord(data);
            _recordPage.SaveRecordAndClose();
            Driver.Wait.Until(WaitConditions.ElementIsDisplayedByLocator(By.CssSelector(_newRecordBtnCssSelector)));
            return GetNewestRecord();
        }

        private void OpenNewRecordPage()
        {
            try
            {
                _newRecordBtn = Driver.FindElementWhenVisible(By.CssSelector(_newRecordBtnCssSelector));
                _newRecordBtn.Click();
                Driver.Wait.Until(WaitConditions.ElementIsNotVisible(_spinnerId));
            }
            catch (Exception e)
            {
                _logger.Error($"Unable to open new Record page");
                _logger.Error($"Error message: {e.Message}");
                throw;
            }
        }

        private SavedRecordData GetNewestRecord()
        {
            SavedRecordData savedRecordData = new SavedRecordData();

            try
            {
                var records = Driver.FindElementsWhenVisible(By.CssSelector(_recordSectionCssSelector));
                
                var newestRecord = records[records.Count - 1];
                var recordLink = newestRecord.FindElement(By.TagName("a")).GetAttribute("href");
                var embeddedId = recordLink.Split("Edit/")[1].Replace("/0", "");
                savedRecordData.Id = int.Parse(embeddedId);
                var recordText = newestRecord.Text.Split(Environment.NewLine);
                var arrayLength = recordText.Length;

                for (var i = 0; i < arrayLength; i++)
                {
                    if (i + 1 < arrayLength && recordText[i].Trim().EndsWith(":") && !recordText[i + 1].Trim().EndsWith(":"))
                    {
                        var propName = recordText[i]
                                        .Replace(" ", "")
                                        .Replace(":", "")
                                        .Replace("?", "")
                                        .Trim();
                        savedRecordData[propName] = recordText[i + 1].Trim();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Unable to retrive newest record");
                _logger.Error($"Error message: {e.Message}");
                throw;
            }
            return savedRecordData;
        }

        public bool CheckRecordExists(string reference)
        {
            try
            {
                return Driver.Wait.Until(WaitConditions.ElementDoesNotExist($"div.information a[title = '{reference}']"));
            }
            catch (Exception e)
            {
                _logger.Error($"Unable to validate existence of record with reference {reference}");
                _logger.Error($"Error message: {e.Message}");
                throw;
            }
        }

        private void ClickManageRecordBtn(int recordId)
        {
            try
            {
                var manageRecordButton = Driver.FindElementWhenVisible(By.Id($"manageRecord{recordId}"));
                manageRecordButton.Click();
            }
            catch (Exception e)
            {
                _logger.Error($"Unable to click manage Record button with id {recordId}");
                _logger.Error($"Error message: {e.Message}");
                throw;
            }
        }

        public void DeleteRecord(int recordId)
        {
            try
            {
                ClickManageRecordBtn(recordId);
                var deleteRecordLink = Driver.FindElementWhenVisible(By.Id($"cogDelete{recordId}"));
                deleteRecordLink.Click();
                var confirmBtn = Driver.Wait.Until(WaitConditions.ElementIsDisplayedByLocator(By.XPath("//button[text()='Confirm']")));
                confirmBtn.Click();
            }
            catch (Exception e)
            {
                _logger.Error($"Unable to delete record with id {recordId}");
                _logger.Error($"Error message: {e.Message}");
                throw;
            }
        }
    }
}
