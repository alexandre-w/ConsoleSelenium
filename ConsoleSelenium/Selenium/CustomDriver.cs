using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using ConsoleSelenium.Helpers;
using OpenQA.Selenium.Firefox;

namespace ConsoleSelenium.Selenium
{
    public class CustomDriver
    {

        public IWebDriver driver { get; set; }

        public WebDriverWait driverWait { get; set; }

        public CustomDriver()
        {
        }

        public void InitDriverChrome()
        {
            //On enlève le plugins PDF viewer
            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference(Const.pluginsDisabled, new[] { Const.chromePdfViewer});

            driver = new ChromeDriver(Const.thirdPartyDirectory, options);

            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        }

        public void InitDriverFirefox()
        {
            driver = new FirefoxDriver();
            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        public IWebElement GetOne(string cssSelector)
        {
            return driver.FindElement(By.CssSelector(cssSelector));
        }

        public ReadOnlyCollection<IWebElement> GetAll(string cssSelector)
        {
            return driver.FindElements(By.CssSelector(cssSelector));
        }
        public string Go(string url)
        {
            return driver.Url = url;
        }

    }
}
