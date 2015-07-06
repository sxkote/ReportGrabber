using SXCore.Lexems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Values
{
    /// <summary>
    /// Represents the Value item from the Report (text value, number value, datetime value, ...)
    /// </summary>
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
            var number = Value.Convert(value, ValueType.Number) as ValueNumber;
            if (number != null)
                return number.Value;

            throw new ArgumentException("Can't convert Value to Double");
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
            var number = Value.Convert(value, ValueType.Date) as ValueDate;
            if (number != null)
                return number.Value;

            throw new ArgumentException("Can't convert Value to DateTime");
        }

        static public implicit operator SXLexemVariable(Value value)
        {
            if (value == null)
                return "";

            switch (value.Type)
            {
                case Value.ValueType.Date:
                    return (value as ValueDate).Value;
                case Value.ValueType.Number:
                    return (value as ValueNumber).Value;
                case Value.ValueType.Text:
                    return (value as ValueText).Value;
                default:
                    return value.ToString();
            }
        }

        static public implicit operator Value(SXLexemVariable variable)
        {
            if (variable == null || variable.Value == null)
                return "";

            switch (variable.Value.Type)
            {
                case SXLexemValue.ValueType.Date:
                    return (variable.Value as SXLexemDate).Value;
                case SXLexemValue.ValueType.Number:
                    return (variable.Value as SXLexemNumber).Value;
                case SXLexemValue.ValueType.Text:
                    return (variable.Value as SXLexemText).Value;
                default:
                    return variable.Value.ToString();
            }
        }

        static public ValueType ParseValueType(string input)
        {
            switch (input.Trim().ToLower())
            {
                case "date":
                case "datetime":
                    return Value.ValueType.Date;

                case "number":
                case "int":
                case "double":
                    return Value.ValueType.Number;

                default: return Value.ValueType.Text;
            }
        }

        static public Value Convert(Value value, Value.ValueType type)
        {
            if (value == null)
                return null;

            if (value.Type == type)
                return value;

            switch (type)
            {
                case Value.ValueType.Number:
                    return SXLexemNumber.ParseDouble(value.ToString(), true);
                case Value.ValueType.Date:
                    return SXLexemDate.ParseDatetime(value.ToString());
                default:
                    return value.ToString();
            }
        }

        static public Value Convert(object obj)
        {
            if (obj == null)
                return new ValueText("");

            if (obj is decimal || obj is double || obj is int)
                return new ValueNumber(System.Convert.ToDouble(obj));

            if (obj is DateTime)
                return new ValueDate((DateTime)obj);

            return new ValueText(obj.ToString());
        }

        static public Value Convert(SXLexemValue value, Value.ValueType type)
        {
            if (value == null)
                throw new ArgumentException("Can't convert null LexemValue to Value");

            switch (type)
            {
                case Value.ValueType.Date:
                    {
                        if (value.Type == SXLexemValue.ValueType.Date)
                            return (value as SXLexemDate).Value;
                        if (value.Type == SXLexemValue.ValueType.Text)
                            return SXLexemDate.ParseDatetime((value as SXLexemText).Value);
                        break;
                    }
                case Value.ValueType.Number:
                    {
                        if (value.Type == SXLexemValue.ValueType.Number)
                            return (value as SXLexemNumber).Value;
                        if (value.Type == SXLexemValue.ValueType.Text)
                            return SXLexemNumber.ParseDouble((value as SXLexemText).Value, true);
                        break;
                    }
                default:
                    {
                        if (value.Type == SXLexemValue.ValueType.Text)
                            return (value as SXLexemText).Value;
                        if (value.Type == SXLexemValue.ValueType.Number)
                            return (value as SXLexemNumber).Value.ToString();
                        if (value.Type == SXLexemValue.ValueType.Date)
                            return (value as SXLexemDate).Value.ToString();
                        break;
                    }
            }

            throw new ReportGrabberException(String.Format("LexemValue {0} not recognized as Value", value.ToString()));
        }
    }
}