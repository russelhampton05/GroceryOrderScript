namespace GroceryOrderScript
{
    public class Action
    {
        public string ObjectID { get; set; }
        public string InputData { get; set; }
        public ActionType ActionType { get; set; }
        public string Label { get; set; }
        public bool Optional { get; set; }
    }
}
