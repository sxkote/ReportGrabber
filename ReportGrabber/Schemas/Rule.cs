using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    public struct Rule
    {
        private string _name;
        private Condition _condition;

        public string Name
        { get { return _name; } }

        public Condition Condition
        { get { return _condition; } }

        public Rule(string name, Condition condition)
        {
            _name = name;
            _condition = condition;
        }
    }
}
