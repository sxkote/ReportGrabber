using ReportGrabber.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber
{
    public class DataCollection
    {
        protected List<Data> _values;

        public IList<Data> Values
        { get { return _values.AsReadOnly(); } }

        public DataCollection()
        {
            _values = new List<Data>();
        }

        public DataCollection(IEnumerable<Data> collection)
        {
            _values = collection == null ? new List<Data>() : collection.ToList();
        }

        public Data Get(string name)
        {
            return _values.FirstOrDefault(v => v.Name == name);
        }

        public void Add(string name, Value value)
        {
            if (value == null)
                throw new ArgumentException("ReportData can't have null Value");

            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("ReportData can't have empty Name");

            var item = this.Get(name);
            if (item != null)
                throw new ArgumentException(String.Format("ReportData with same name {0} already exists in current Collection"), name);

            _values.Add(new Data(name, value));
        }
    }
}
