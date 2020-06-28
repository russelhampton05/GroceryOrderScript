using System;
using System.Collections.Generic;
using System.Threading;

namespace GroceryOrderScript
{
    public class ScriptRunner
    {
        private OpenQA.Selenium.Firefox.FirefoxDriver driver;
        
        public ScriptRunner(string pathToGecko)
        {
            driver = new OpenQA.Selenium.Firefox.FirefoxDriver(pathToGecko);
        }

        public void RunActions(List<Action> actions)
        {
            foreach (var action in actions)
            {
                var element = driver.FindElementById(action.ObjectID);
                switch (action.ActionType)
                {
                    case ActionType.Click:
                        element.Click();
                        break;
                    case ActionType.InputText:
                    case ActionType.InputUsername:
                    case ActionType.InputPassword:
                        element.SendKeys(action.InputData);
                        break;
                    case ActionType.Wait:
                        Thread.Sleep(1000);
                        break;
                    case ActionType.BackNavigate:
                        throw new Exception("Don't know if I need this yet");
                    case ActionType.ForwardNavigate:
                        throw new Exception("Don't know if I need this yet");
                    case ActionType.CaptureData:
                        throw new Exception("Don't know if I need this yet");
                    case ActionType.DownloadFile:
                        throw new Exception("Don't know if I need this yet");
                }
            }
        }
    }
}
