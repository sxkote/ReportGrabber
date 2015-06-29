using ReportGrabber.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    /// <summary>
    /// Field of the Report to be grabbed
    /// </summary>
    public struct Field
    {
        private string _name;
        private Address _address;
        private Value.ValueType _type;
        private Vocabulary _vocabulary;

        /// <summary>
        /// Name of the Field
        /// Name of the Data that would be grabbed
        /// </summary>
        public string Name
        { get { return _name; } }

        /// <summary>
        /// Address of the Value to be grabbed
        /// </summary>
        public Address Address
        { get { return _address; } }

        /// <summary>
        /// Type of the Value to be grabbed (text, number, datetime, ...)
        /// </summary>
        public Value.ValueType Type
        { get { return _type; } }

        /// <summary>
        /// Vocabulary of replacements to be applied to grabbed value
        /// </summary>
        public Vocabulary Vocabulary
        {
            get { return _vocabulary; }
        }

        public Field(string name, Address address, Value.ValueType type = Value.ValueType.Text, Vocabulary vocabulary = null)
        {
            _name = name;
            _address = address;
            _type = type;
            _vocabulary = vocabulary;
        }
    }
}
