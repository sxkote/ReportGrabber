using ReportGrabber.Schemas;
using ReportGrabber.Values;
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
        public const string ExcelAddressPattern = @"(?i)^r(?<rownum>(\+|\-)?\d+)?c(?<colnum>(\+|\-)?\d+)?$";

        #region Variables
        protected int _row = -1;
        #endregion

        #region Properties
        protected int InitRow
        {
            get
            {
                if (_mapping == null)
                    return 0;

                var init = this.GetValue(_mapping.InitPosition) as ValueNumber;
                if (init == null)
                    return 0;

                return (int)init.Value;
            }
        }
        #endregion

        #region Functions
        protected virtual void SetRow(int row)
        {
            _row = row;
        }

        public override bool MoveNext()
        {
            if (_mapping == null)
                return false;

            this.SetRow(_row < 0 ? this.InitRow : _row + 1);

            return !this.CheckCondition(_mapping.FinishCondition);
        }

        public override Value GetValue(Address address)
        {
            if (String.IsNullOrEmpty(address.Uri))
                return "";

            var adr = address.Uri;

            #region Expression
            //if (adr.StartsWith("=") && adr.Length > 1)
            //    return this.GetCalc(new SXExpression(adr.Substring(1)));
            #endregion

            int row = _row, col = 0;
            if (Int32.TryParse(adr, out col) || CursorExcel.ParseExcelAddress(adr, ref row, ref col))
                return this.GetValue(row, col, address.Type);

            if (adr.Contains('+'))
                return String.Join("", adr.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries).Select(part => this.GetValue(part).ToString()));

            throw new FormatException(String.Format("Excel Address not recognized: {0}", adr));
        }

        protected abstract Value GetValue(int row, int col, Value.ValueType type = Value.ValueType.Text);
        #endregion

        #region Statics
        static public bool ParseExcelAddress(string address, ref int row, ref int col)
        {
            var match = Regex.Match(address, ExcelAddressPattern);
            if (match == null || !match.Success)
                return false;

            var rowText = match.Groups["rownum"].Value;
            var colText = match.Groups["colnum"].Value;

            int rowValue = 0;
            if (!String.IsNullOrEmpty(rowText) && !Int32.TryParse(rowText, out rowValue))
                return false;

            int colValue = 0;
            if (!String.IsNullOrEmpty(colText) && !Int32.TryParse(colText, out colValue))
                return false;

            if (!String.IsNullOrEmpty(rowText))
                row = rowText.StartsWith("+") || rowText.StartsWith("-") ? row + rowValue : rowValue;

            if (!String.IsNullOrEmpty(colText))
                col = colText.StartsWith("+") || colText.StartsWith("-") ? col + colValue : colValue;

            return true;
        }
        #endregion
    }
}
