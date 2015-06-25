using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    public struct Condition
    {
        private Address _address;
        private string _value;

        public Address Address { get { return _address; } }

        public string Value { get { return _value; } }

        public Condition(Address address, string value)
        {
            _address = address;
            _value = value;
        }
    }
}
