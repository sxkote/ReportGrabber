using ReportGrabber.Values;

namespace ReportGrabber
{
    public class Data
    {
        private string _name;
        private Value _value;

        public string Name { get { return _name; } }

        public Value Value { get { return _value; } }

        public Data(string name, Value value)
        {
            _name = name;
            _value = value;
        }
    }
}
