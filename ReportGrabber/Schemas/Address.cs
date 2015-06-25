using ReportGrabber.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    public struct Address
    {
        private string _uri;
        private Value.ValueType _type;

        public string Uri
        { get { return _uri; } }

        public Value.ValueType Type
        { get { return _type; } }

        public Address(string uri, Value.ValueType type = Value.ValueType.Text)
        {
            _uri = String.IsNullOrEmpty(uri) ? "" : uri.Trim();
            _type = type;
        }

        static public implicit operator string (Address address)
        { return address.Uri; }

        static public implicit operator Address(string address)
        { return new Address(address); }
    }
}
