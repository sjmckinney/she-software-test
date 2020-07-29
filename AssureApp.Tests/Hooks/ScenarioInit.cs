using BoDi;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using FW.Selenium;

namespace AssureApp.Tests.Hooks
{
    [Binding]
    public sealed class ScenarioInit
    {
        private readonly IObjectContainer objectContainer;

        public ScenarioInit(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
            Driver.Init();
            objectContainer.RegisterInstanceAs<IWebDriver>(Driver.Current);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Driver.Current.Quit();
        }
    }
}
