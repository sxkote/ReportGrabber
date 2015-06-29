using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    /// <summary>
    /// Mapping that defines the schema of grabbing the Report
    /// </summary>
    public class Mapping
    {
        protected ReportType _type;
        protected string _title;

        protected Range _range;
        protected Condition _match;
        
        protected IEnumerable<Field> _fields;
        protected IEnumerable<Rule> _rules;

        /// <summary>
        /// Type of the Report that suits this Mapping 
        /// (Report Type that Mapping could be applied to)
        /// </summary>
        public ReportType Type
        { get { return _type; } }

        /// <summary>
        /// Title of the Mapping
        /// </summary>
        public string Title
        { get { return _title; } }

        /// <summary>
        /// Range of the the Mapping schema in witch the grabbing happens
        /// </summary>
        public Range Range
        {
            get { return _range; }
            set { _range = value; }
        }

        /// <summary>
        /// Match Condition, to be verified that Mapping suits the Report
        /// </summary>
        public Condition Match
        {
            get { return _match; }
            set { _match = value; }
        }

        /// <summary>
        /// Collection of Fields to be grabbed from the Report
        /// </summary>
        public IEnumerable<Field> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        /// <summary>
        /// Collection of Rules to be applied while the grabbing
        /// </summary>
        public IEnumerable<Rule> Rules
        {
            get { return _rules; }
            set { _rules = value; }
        }

        public Mapping(ReportType type, string title = "")
        {
            _type = type;
            _title = title;
        }
    }
}
