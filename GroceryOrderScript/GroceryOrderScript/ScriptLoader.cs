using System.Collections.Generic;

namespace GroceryOrderScript
{
    public class ScriptLoader
    {
        public List<Action> GetActions(string pathToActions)
        {
            var contents = System.IO.File.ReadAllText(pathToActions);
            var actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Action>>(contents);
            return actions;
        }

        public List<Action> LoadGetItemAction(GroceryItem item, string pathToCheckoutScript )
        {
            var contents = System.IO.File.ReadAllText(pathToCheckoutScript);
            var actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Action>>(contents);
            foreach (var action in actions)
            {
                if (action.ActionType == ActionType.InputText)
                {
                    action.InputData = item.UID;
                }
            }

            return actions;
        }

        public List<Action> LoadLoginAction(string pathToActions, string userName, string password, string signInUrl)
        {
            var contents = System.IO.File.ReadAllText(pathToActions);
            var actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Action>>(contents);
            foreach (var action in actions)
            {
                if(action.ActionType == ActionType.Navigate)
                {
                    action.InputData = signInUrl;
                }
                if (action.ActionType == ActionType.InputUsername)
                {
                    action.InputData = userName;
                }
                else if (action.ActionType == ActionType.InputPassword)
                {
                    action.InputData = password;
                }
            }

            return actions;
        }
    }
}
