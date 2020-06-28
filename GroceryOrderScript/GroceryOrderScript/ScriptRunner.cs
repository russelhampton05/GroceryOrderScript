using System;
using System.Collections.Generic;
using System.Threading;

namespace GroceryOrderScript
{
    public class ScriptRunner
    {
        private OpenQA.Selenium.Chrome.ChromeDriver driver;

        public ScriptRunner(string pathToGecko)
        {
            driver = new OpenQA.Selenium.Chrome.ChromeDriver(pathToGecko);
        }

        public void RunActions(List<Action> actions)
        {
            foreach (var action in actions)
            {
                if (action.ActionType != ActionType.Navigate)
                {

                    var element = driver.FindElementById(action.ObjectID);

                    if (element == null)
                    {
                        element = driver.FindElementByLinkText(action.Label);
                    }


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
                    }
                }
                else if (action.ActionType == ActionType.Navigate)
                {
                    driver.Url = action.InputData;
                    driver.Navigate();
                }
            }
        }
    }
}
