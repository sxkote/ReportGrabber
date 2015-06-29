using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    /// <summary>
    /// Rule of the grabbing
    /// </summary>
    public struct Rule
    {
        private string _name;
        private Condition _condition;

        /// <summary>
        /// Name of the Rule
        /// </summary>
        public string Name
        { get { return _name; } }

        /// <summary>
        /// Condition to be verified if the Rule should be applied
        /// </summary>
        public Condition Condition
        { get { return _condition; } }

        public Rule(string name, Condition condition)
        {
            _name = name;
            _condition = condition;
        }
    }
}
