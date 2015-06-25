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
        protected Address _initPosition;
        protected Condition _finishCondition;
        protected IEnumerable<Condition> _conditions;

        public ReportType Type
        { get { return _type; } }

        public Address InitPosition
        { get { return _initPosition; } }

        public Condition FinishCondition
        { get { return _finishCondition; } }

        public IEnumerable<Condition> Conditions
        { get { return _conditions; } }
    }
}
