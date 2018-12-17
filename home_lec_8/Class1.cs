using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace home_lec_8
{
    [TestFixture]
    public class Home_task_8
    {
        IWebDriver driver;
        IJavaScriptExecutor jexec;

        string downloadDirectoryPath;
        [OneTimeSetUp]
        public void InitDriver()
        {
            //Activate the full-screen mode
            var options = new ChromeOptions();
            options.AddArguments
                (
                    "--start-fullscreen"
                );
            
            downloadDirectoryPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "download", DateTime.Now.ToString("yy-MM-dd HH-mm-ss")); 
            Directory.CreateDirectory(downloadDirectoryPath);

            //Change the default download directory to any other of your choice
            options.AddUserProfilePreference("download.default_directory", downloadDirectoryPath);

            driver = new ChromeDriver(options);
            jexec = (IJavaScriptExecutor)driver;
        }

        [Test]
        public void DownloadLastImg()
        {
            //Go to https://unsplash.com/search/photos/test
            driver.Navigate().GoToUrl("https://unsplash.com/search/photos/test");

            var nameLastImg = By.XPath("//img[@alt='cactus succulent plant on white vase']");
            var downloadButton = By.XPath("//img[@alt='cactus succulent plant on white vase']/../../..//a[@class='_1QwHQ _1l4Hh _1CBrG _1zIyn xLon9 _1Tfeo _2L6Ut _2Xklx']");
            IWebElement lastImg;

            //Scroll to the last picture on the page
            while (true)
            {
                if (driver.FindElements(nameLastImg).Count != 0)
                {
                    lastImg = driver.FindElement(nameLastImg);
                    break;
                }
                jexec.ExecuteScript("window.scrollBy(0,100)");
            }
            jexec.ExecuteScript("arguments[0].scrollIntoView()", lastImg);

            //Click on Download button using JS.
            jexec.ExecuteScript("arguments[0].click()", driver.FindElement(downloadButton));
            Thread.Sleep(5000);

            //Verify that file was downloaded.          
            Assert.That(Directory.GetFiles(downloadDirectoryPath, "the-roaming-platypus-529026-unsplash.jpg"), Is.Not.Empty);
        }

        [OneTimeTearDown]
        public void ClearDriver()
        {
            driver.Quit();
        }
    }
}
