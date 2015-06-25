using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Values
{
    public abstract class Value
    {
        public enum ValueType { Text, Number, Date };

        public abstract ValueType Type { get; }

        static public implicit operator string (Value value)
        {
            if (value == null)
                return "";

            var number = value as ValueText;
            if (number != null)
                return number.Value;

            return value.ToString();
        }

        static public implicit operator Value(string value)
        { return new ValueText(value); }

        static public implicit operator double (Value value)
        {
            var number = value as ValueNumber;
            if (number != null)
                return number.Value;

            throw new NotSupportedException("Can't convert Value to Double");
        }

        static public implicit operator Value(decimal value)
        { return new ValueNumber((double)value); }

        static public implicit operator Value(double value)
        { return new ValueNumber(value); }

        static public implicit operator Value(int value)
        { return new ValueNumber(value); }

        static public implicit operator Value(DateTime value)
        { return new ValueDate(value); }

        static public implicit operator DateTime(Value value)
        {
            var number = value as ValueDate;
            if (number != null)
                return number.Value;

            throw new NotSupportedException("Can't convert Value to DateTime");
        }
    }
}