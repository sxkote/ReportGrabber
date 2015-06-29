using ReportGrabber.Values;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ReportGrabber
{
    /// <summary>
    /// Represents Named Value from the Report to be returned by Grabber from one Field
    /// <example>Name = DutyType, Value = "Waybill"</example>
    /// <example>Name = Summ, Value = 1098.54</example>
    /// </summary>
    public class Data
    {
        private string _name;
        private Value _value;

        /// <summary>
        /// Name of the Data
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Value of the Data
        /// </summary>
        public Value Value { get { return _value; } }

        public Data(string name, Value value)
        {
            _name = name;
            _value = value;
        }
    }

    /// <summary>
    /// Represents the collection of the Named Data to be returned by Grabber from one row (one position)
    /// </summary>
    public class DataCollection
    {
        protected List<Data> _values;

        /// <summary>
        /// Data Items in current Collection
        /// </summary>
        public IList<Data> Values
        { get { return _values.AsReadOnly(); } }

        /// <summary>
        /// Gets the Value by Name
        /// </summary>
        /// <param name="name">Name to be searched in collection</param>
        /// <returns>Value of the Data with appropriate name</returns>
        public Value this[string name]
        {
            get
            {
                var data = this.Get(name);
                return data == null ? null : data.Value;
            }
        }

        public DataCollection()
        {
            _values = new List<Data>();
        }

        public DataCollection(IEnumerable<Data> collection)
        {
            _values = collection == null ? new List<Data>() : collection.ToList();
        }

        /// <summary>
        /// Get the appropriate Data item by Name
        /// </summary>
        /// <param name="name">Name to search in the collection</param>
        /// <returns>The Data with selected Name from current collection</returns>
        public Data Get(string name)
        {
            return _values.FirstOrDefault(v => v.Name == name);
        }

        /// <summary>
        /// Adds the Data to current collection
        /// </summary>
        /// <param name="name">Name of the new Data</param>
        /// <param name="value">Value of the new Data</param>
        /// <exception cref="ArgumentException">thrown when Name is empty or Data with such Name already exists in the collection</exception>
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
