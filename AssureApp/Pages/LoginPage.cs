using System;
using System.Linq;

using OpenQA.Selenium;
using FW.Selenium;
using static NLog.LogManager;

using FW.Config;

namespace AssureApp.Pages
{
    public class LoginPage : BasePage
    {
        private static readonly NLog.Logger _logger = GetCurrentClassLogger();

        //private IWebDriver _driver;

        private IWebElement _userNameInput;
        private IWebElement _passwordInput;
        private IWebElement _loginBtn;

        private readonly string _usernameInputId = "username";
        private readonly string _passwordInputId = "password";
        private readonly string _loginBtnId = "login";

        public LoginPage(string testUrl)
        {
            _logger.Info($"Navigating to {testUrl}");
            try
            {
                Driver.Current.Navigate().GoToUrl(testUrl);
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to navigate to {testUrl} with error message {e.Message}");
                throw;
            }
        }

        public void LoginAsUser(string username)
        {    
            try
            {
                var password = GetPasswordForUser(username);
                _userNameInput = Driver.FindElementWhenVisible(By.Id(_usernameInputId));
                _passwordInput = Driver.FindElementWhenVisible(By.Id(_passwordInputId));
                _loginBtn = Driver.FindElementWhenVisible(By.Id(_loginBtnId));
                _userNameInput.Clear();
                _passwordInput.Clear();
                _userNameInput.SendKeys(username);
                _passwordInput.SendKeys(password);
                _loginBtn.Click();

                // Ordinarily would identify a state that implied that the home page load was complete
                // and wait for that state to exist i.e. the existance of an element that was late/last
                // to load or that a number of elements had been loaded.
                // In this instance will wait for element interested in to exist

                Driver.Wait.Until(WaitConditions
                    .ElementIsDisplayedByLocator(By.CssSelector(HomePage.environmentalModuleCssLocator)));

            }
            catch(Exception e)
            {
                _logger.Error($"Unable to login as user {username}.");
                _logger.Error($"Error message {e.Message}.");
            }
        }

        private string GetPasswordForUser(string username)
        {
            var users = AppSettings.Users;
            string password = null;

            try
            {
                password = users.FirstOrDefault(x => x.Username == username).Password;
            }
            catch (ArgumentNullException e)
            {
                _logger.Error($"User {username} does not exist");
                _logger.Error($"Error Msg {e.Message}");
                throw;
            }

            return password;
        }
    }
}
