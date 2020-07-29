using TechTalk.SpecFlow;
using AssureApp.Pages;
using AssureApp.DataEntities;
using FW.Config;
using TechTalk.SpecFlow.Assist;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AssureApp.Tests.Steps
{
    [Binding]
    public class ManageAirEmissionRecordsSteps
    {
        private readonly IWebDriver _webDriver;
        private FeatureContext _featureContext;
        private LoginPage _loginPage;
        private HomePage _homePage = new HomePage();
        private ModulePage _modulePage = new ModulePage();
        private RecordData _recordData;
        private SavedRecordData _savedRecordData;

        public ManageAirEmissionRecordsSteps(FeatureContext featureContext, IWebDriver webDriver)
        {
            _featureContext = featureContext;
            _webDriver = webDriver;
        }

        [Given(@"I navigate to the Assure login page")]
        public void GivenINavigateToTheAssureLoginPage()
        {
            _loginPage = new LoginPage(AppSettings.Url);
        }

        [Given(@"I login with username (.*)")]
        public void GivenILoginWithUsername(string username)
        {
            _loginPage.LoginAsUser(username);
        }

        [When(@"I open the ([\w\s]+) page from the (.*) module")]
        public void WhenIOpenAPageFromAModule(string pageName, string moduleName)
        {
            _featureContext.Add("pageName", pageName);
            _featureContext.Add("moduleName", moduleName);
            _homePage.OpenPageFromModule(pageName, moduleName);
        }

        [Given(@"I open the Air Emissions page")]
        public void GivenIOpenTheAirEmissionsPage()
        {
            var moduleName = _featureContext.Get<string>("moduleName");
            var pageName = _featureContext.Get<string>("pageName").Replace(" ", "");
            var airEmissionsPageUrl = $"{AppSettings.Url}/{moduleName}/{pageName}/Page/1";
            _webDriver.Navigate().GoToUrl(airEmissionsPageUrl);
        }

        [Then(@"the record should no longer be visible on the page")]
        public void ThenTheRecordShouldNoLongerBeVisibleOnThePage()
        {
            var recordDoesNotExist = _modulePage.CheckRecordExists(_savedRecordData.Reference);
            Assert.IsTrue(recordDoesNotExist);
        }

        [When(@"I delete the record")]
        public void WhenIDeleteTheRecord()
        {
            _savedRecordData = _featureContext.Get<SavedRecordData>("savedRecordData");
            _modulePage.DeleteRecord(_savedRecordData.Id);
        }

        [Then(@"the record should be visible on the page")]
        public void ThenTheRecordShouldBeSavedAndVisibleOnThePage()
        {
            Assert.That(_recordData.Date, Is.EqualTo(_savedRecordData.SampleDate));
            Assert.That(_recordData.Description, Does.StartWith(_savedRecordData.Description.Replace("...", "").Trim()));
        }

        [When(@"create a new record:")]
        public void WhenCreateANewRecord(Table table)
        {
            _recordData = table.CreateInstance<RecordData>();
            _savedRecordData = _modulePage.CreateNewRecord(_recordData);
            _featureContext.Add("savedRecordData", _savedRecordData);
        }
    }
}
