using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    public struct Range
    {
        private Address _initPosition;
        private Condition _finishCondition;

        public Address InitPosition
        { get { return _initPosition; } }

        public Condition FinishCondition
        { get { return _finishCondition; } }

        public Range(Address initPosition, Condition finishCondition)
        {
            _initPosition = initPosition;
            _finishCondition = finishCondition;
        }
    }
}
