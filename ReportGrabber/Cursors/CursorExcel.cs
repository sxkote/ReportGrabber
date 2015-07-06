using ReportGrabber.Schemas;
using ReportGrabber.Values;
using SXCore.Lexems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReportGrabber.Cursors
{
    public abstract class CursorExcel : Cursor
    {
        public const string ExcelAddressPattern = @"(?i)^r(?<row>\d+)?c(?<col>\d+)?$";

        #region Variables
        protected SXEnvironment _environment;
        protected int _row = -1;
        #endregion

        #region Constructors
        public CursorExcel()
        {
            _environment = new SXEnvironment();
            _environment.FunctionExecuting += this.OnFunctionExecuting;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Gets the index of first row to be grabbed
        /// </summary>
        /// <param name="range">The Range of the schema that defines the first row index</param>
        /// <returns>Inde of the first row to be grabbed</returns>
        protected int FirstRowIndex(Range range)
        {
            if (String.IsNullOrEmpty(range.InitPosition))
                return 0;

            int row = 0;
            if (Int32.TryParse(range.InitPosition, out row))
                return row;

            return 0;
        }

        public override bool MoveNext(Range range)
        {
            _row = _row < 0 ? this.FirstRowIndex(range) : _row + 1;

            int column = 0;
            if (Int32.TryParse(range.FinishCondition, out column))
            {
                // if in Condition field Column number to check is entered
                return !String.IsNullOrEmpty(this.GetValue(_row, column));
            }

            return !this.CheckCondition(range.FinishCondition);
        }

        public override bool CheckCondition(Condition condition)
        {
            var expression = condition.Value.StartsWith("=") ? condition.Value.Substring(1) : condition.Value;

            return (SXExpression.Calculate(expression, _environment).Value as SXLexemBool).Value == SXLexemBool.BoolType.True;
        }

        protected SXLexemVariable OnFunctionExecuting(SXLexemFunction function)
        {
            Func<int, int> getindex = i => (int)(function.Arguments[i].Calculate(_environment).Value as SXLexemNumber).Value;
            Func<int, Value.ValueType> gettype = i => Value.ParseValueType((function.Arguments[i].Calculate(_environment).Value as SXLexemText).Value);

            if (function.Name.Equals("rowcol", StringComparison.InvariantCultureIgnoreCase) || function.Name.Equals("rc", StringComparison.InvariantCultureIgnoreCase))
            {
                #region Get the Value on the exact Row and Column
                if (function.Arguments.Count == 2)
                    return this.GetValue(getindex(0), getindex(1));
                else if (function.Arguments.Count == 3)
                    return this.GetValue(getindex(0), getindex(1), gettype(2));
                #endregion
            }

            if (function.Name.Equals("cell", StringComparison.InvariantCultureIgnoreCase) || function.Name.Equals("c", StringComparison.InvariantCultureIgnoreCase))
            {
                #region Get the Value on the relative [Row] (row is optional) and Column
                if (function.Arguments.Count == 1)
                    return this.GetValue(_row, getindex(0));
                else if (function.Arguments.Count == 2)
                {
                    var second = function.Arguments[1].Calculate(_environment).Value;
                    if (second.Type == SXLexemValue.ValueType.Number)
                        return this.GetValue(_row + getindex(0), (int)(second as SXLexemNumber).Value);
                    else
                        return this.GetValue(_row, getindex(0), Value.ParseValueType((second as SXLexemText).Value));
                }
                else if (function.Arguments.Count == 3)
                    return this.GetValue(_row + getindex(0), getindex(1), gettype(2));
                #endregion
            }

            if (function.Name.Equals("getdate", StringComparison.InvariantCultureIgnoreCase))
            {
                if (function.Arguments.Count == 1)
                    return this.GetValue(_row, getindex(0), Value.ValueType.Date);
                else if (function.Arguments.Count == 2)
                    return this.GetValue(_row + getindex(0), getindex(1), Value.ValueType.Date);
            }

            if (function.Name.Equals("getnumber", StringComparison.InvariantCultureIgnoreCase))
            {
                if (function.Arguments.Count == 1)
                    return this.GetValue(_row, getindex(0), Value.ValueType.Number);
                else if (function.Arguments.Count == 2)
                    return this.GetValue(_row + getindex(0), getindex(1), Value.ValueType.Number);
            }

            if (function.Name.Equals("gettext", StringComparison.InvariantCultureIgnoreCase))
            {
                if (function.Arguments.Count == 1)
                    return this.GetValue(_row, getindex(0), Value.ValueType.Text);
                else if (function.Arguments.Count == 2)
                    return this.GetValue(_row + getindex(0), getindex(1), Value.ValueType.Text);
            }

            throw new CursorException(String.Format("Expression Function not recognized: {0}", function.Name));
        }

        public override Value GetValue(Address address, Value.ValueType type = Value.ValueType.Text)
        {
            if (address == null || String.IsNullOrEmpty(address.Uri))
                return "";

            var adr = address.Uri;

            //calculate column index or RC value
            int row = _row, col = 0;
            if (Int32.TryParse(adr, out col) || CursorExcel.ParseExcelAddress(adr, ref row, ref col))
                return this.GetValue(row, col, type);

            //calculate expression
            var expression = adr.StartsWith("=") ? adr.Substring(1) : adr;
            var calc = SXExpression.Calculate(expression, _environment);
            if (calc == null || calc.Value == null)
                throw new CursorException("Can't evaluate expression");

            return Value.Convert(calc.Value, type);
            

            //calculate expression
            //if (adr.StartsWith("=") && adr.Length > 1)
            //    return SXExpression.Calculate(adr.Substring(1), _environment);

            ////get '...' value
            //if (adr.Length >= 2 && adr.IndexOf('\'') == 0 && adr.IndexOf('\'', 1) == adr.Length - 1)
            //    return Value.Convert(adr.Substring(1, adr.Length - 2), type);

            ////get concatination ... + ... + ...
            //if (adr.Contains('+'))
            //{
            //    var parts = adr.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
            //    return Value.Convert(String.Join("", parts.Select(part => this.GetValue(part).ToString())), type);                
            //}

            //throw new CursorException(String.Format("Excel Address not recognized: {0}", adr));
        }

        protected abstract Value GetValue(int row, int col, Value.ValueType type = Value.ValueType.Text);
        #endregion

        #region Statics
        static public bool ParseExcelAddress(string address, ref int row, ref int col)
        {
            var match = Regex.Match(address, ExcelAddressPattern);
            if (match == null || !match.Success)
                return false;

            var rowText = match.Groups["row"].Value;
            var colText = match.Groups["col"].Value;

            int rowValue = 0;
            if (!String.IsNullOrEmpty(rowText) && !Int32.TryParse(rowText, out rowValue))
                return false;

            int colValue = 0;
            if (!String.IsNullOrEmpty(colText) && !Int32.TryParse(colText, out colValue))
                return false;

            if (!String.IsNullOrEmpty(rowText))
                row = rowValue;

            if (!String.IsNullOrEmpty(colText))
                col = colValue;

            return true;
        }
        #endregion
    }
}
