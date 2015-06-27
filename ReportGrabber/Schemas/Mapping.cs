using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    public class Mapping
    {
        protected ReportType _type;
        protected Range _range;
        protected IEnumerable<Condition> _matches;
        protected IEnumerable<Field> _fields;
        protected IEnumerable<Rule> _rules;

        public ReportType Type
        { get { return _type; } }

        public Range Range
        {
            get { return _range; }
            set { _range = value; }
        }

        public IEnumerable<Condition> Matches
        {
            get { return _matches; }
            set { _matches = value; }
        }

        public IEnumerable<Field> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        public IEnumerable<Rule> Rules
        {
            get { return _rules; }
            set { _rules = value; }
        }

        public Mapping(ReportType type)
        {
            _type = type;
        }

    }
}
