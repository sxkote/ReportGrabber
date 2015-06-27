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

        public string Uri
        { get { return _uri; } }

        public Address(string uri)
        {
            _uri = String.IsNullOrEmpty(uri) ? "" : uri.Trim();
        }

        static public implicit operator string (Address address)
        { return address.Uri; }

        static public implicit operator Address(string address)
        { return new Address(address); }
    }
}
