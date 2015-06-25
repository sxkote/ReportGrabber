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

        public string Name
        { get { return _name; } }

        public Address Address
        { get { return _address; } }

        public Field(string name, string address)
        {
            _name = name;
            _address = address;
        }

        public Field(string name, Address address)
        {
            _name = name;
            _address = address;
        }
    }
}
