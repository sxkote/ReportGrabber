using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    /// <summary>
    /// Ranges of the Report grabbing
    /// </summary>
    public struct Range
    {
        private Address _initPosition;
        private Condition _finishCondition;

        /// <summary>
        /// Initial Position starts grabbing with
        /// </summary>
        public Address InitPosition
        { get { return _initPosition; } }

        /// <summary>
        /// Condition to be checked to verify the end of the Report
        /// </summary>
        public Condition FinishCondition
        { get { return _finishCondition; } }

        public Range(Address initPosition, Condition finishCondition)
        {
            _initPosition = initPosition;
            _finishCondition = finishCondition;
        }
    }
}
