using ReportGrabber.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    public struct Field
    {
        private string _name;
        private Address _address;
        private Value.ValueType _type;

        public string Name
        { get { return _name; } }

        public Address Address
        { get { return _address; } }

        public Value.ValueType Type
        { get { return _type; } }

        public Field(string name, Address address, Value.ValueType type = Value.ValueType.Text)
        {
            _name = name;
            _address = address;
            _type = type;
        }
    }
}
