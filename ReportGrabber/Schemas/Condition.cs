using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    public struct Condition
    {
        private string _value;

        public string Value 
        { get { return _value; } }

        public Condition(string value)
        {
            _value = value;
        }

        static public implicit operator string(Condition condition)
        { return condition.Value; }

        static public implicit operator Condition(string condition)
        { return new Condition(condition); }
    }
}
