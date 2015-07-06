using ExcelLibrary.SpreadSheet;
using ReportGrabber.Schemas;
using ReportGrabber.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Cursors
{
    public class CursorExcel2003 : CursorExcel
    {
        protected Worksheet _worksheet = null;

        public CursorExcel2003(byte[] data)
        {
            var wb = Workbook.Load(new MemoryStream(data));
            if (wb == null || wb.Worksheets.Count <= 0)
                throw new ReportFormatException("Data was not recognized as Excel2003");

            _worksheet = wb.Worksheets[0];
        }

        protected override Value GetValue(int row, int col, Value.ValueType type = Value.ValueType.Text)
        {
            if (_worksheet == null)
                throw new CursorException("Worksheet is Null for Excel2003 Cursor");

            if (row <= 0 || col <= 0)
                throw new CursorException("Row and Column Indexes should be positive (>=1)");

            var cell = _worksheet.Cells[row-1, col-1];
            if (cell == null || cell.Value == null || cell.IsEmpty)
                return "";

            //var cellType = cell.Format.FormatType;

            if (type == Value.ValueType.Date)
            {
                return cell.DateTimeValue;
            }

            if (type == Value.ValueType.Number)
            {
                return Convert.ToDouble(cell.Value);
            }

            return cell.StringValue;
        }
    }
}
