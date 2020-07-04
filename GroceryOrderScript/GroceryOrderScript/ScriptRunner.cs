using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GroceryOrderScript
{
    public class ScriptRunner : IDisposable
    {
        private OpenQA.Selenium.Chrome.ChromeDriver driver;

        public ScriptRunner(string pathToGecko)
        {


            OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-blink-features");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddArgument("disable-gpu");
            // options.AddArgument("--headless");

            driver = new OpenQA.Selenium.Chrome.ChromeDriver(pathToGecko, options);
            var parameters = new Dictionary<string, object>
            {
                ["source"] = "Object.defineProperty(navigator, 'webdriver', { get: () => undefined })"
            };

            driver.ExecuteChromeCommand("Page.addScriptToEvaluateOnNewDocument", parameters);


        }

        public void RunActions(List<Action> actions)
        {
            var missedActions = new List<Action>();

            foreach (var action in actions)
            {
                try
                {
                    if (action.ActionType == ActionType.Wait)
                    {
                        Thread.Sleep(TimeSpan.Parse($"{action.InputData}"));
                        continue;
                    }

                    if (action.ActionType != ActionType.Navigate)
                    {

                        IWebElement element = null;

                        if (!string.IsNullOrEmpty(action.ObjectID))
                        {
                            element = driver.FindElementById(action.ObjectID);
                        }
                        else if (!string.IsNullOrEmpty(action.Label))
                        {
                            element = driver.FindElementByClassName(action.Label);
                        }

                        Console.WriteLine($"Running {action.ActionType} {action.InputData} {action.Label} {action.ObjectID}");

                        switch (action.ActionType)
                        {
                            case ActionType.ClearText:
                                int n = 18;
                                while (n > 0)
                                {
                                    element.SendKeys(Keys.Backspace);
                                    n--;
                                }
                                break;
                            case ActionType.Click:
                                //element.Click();
                                Actions webAction = new Actions(driver);
                                webAction.MoveToElement(element).Click().Perform();
                                break;
                            case ActionType.InputText:
                            case ActionType.InputUsername:
                            case ActionType.InputPassword:
                                element.SendKeys(action.InputData);
                                break;
                            case ActionType.EnterPush:
                                element.SendKeys(Keys.Enter);
                                break;
                        }
                    }
                    else if (action.ActionType == ActionType.Navigate)
                    {
                        driver.Url = action.InputData;
                        driver.Navigate();
                    }
                }
                catch(Exception ex)
                {
                    if (action.Optional)
                    {
                        Console.WriteLine($"ERROR : {action.InputData} {action.ActionType} {action.Label} {action.ObjectID} action missed");
                        missedActions.Add(action);
                        continue;
                    }

                    throw ex;
                }
            }

            if (missedActions.Any())
            {
                string errorMessage = "";
                foreach(var action in missedActions)
                {
                    errorMessage += $"Exception on {action.InputData}";
                }

                throw new Exception(errorMessage);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    driver.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ScriptRunner()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
