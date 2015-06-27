using OfficeOpenXml;
using ReportGrabber.Schemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportGrabber.Values;

namespace ReportGrabber.Cursors
{
    public class CursorExcel2007 : CursorExcel
    {
        protected ExcelWorksheet _worksheet = null;

        public CursorExcel2007(byte[] data)
        {
            var wb = (new ExcelPackage(new MemoryStream(data))).Workbook;
            if (wb == null || wb.Worksheets == null || wb.Worksheets.Count <= 0)
                throw new ReportFormatException("Data was not recognized as Excel2007");

            _worksheet = wb.Worksheets.First();
        }

        protected override Value GetValue(int row, int col, Value.ValueType type = Value.ValueType.Text)
        {
            if (_worksheet == null)
                throw new CursorException("Worksheet is Null for Excel2007 Cursor");

            if (row <= 0 || col <= 0)
                throw new CursorException("Row and Column Indexes should be positive (>=1)");

            var range = _worksheet.Cells[row, col];
            if (range == null || range.Value == null)
                return "";

            if (type == Value.ValueType.Date || range.Value is DateTime)
                return Convert.ToDateTime(range.Value);

            return range.Value.ToString();
        }
    }
}
