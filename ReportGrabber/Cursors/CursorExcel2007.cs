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
        #region Variables
        protected ExcelWorksheet _worksheet = null;
        #endregion

        #region Constructor
        public CursorExcel2007(byte[] data, ICollection<Mapping> mappings)
        {
            _data = data;

            var wb = (new ExcelPackage(new MemoryStream(data))).Workbook;
            if (wb == null || wb.Worksheets == null || wb.Worksheets.Count <= 0)
                throw new NotSupportedException("Data was not recognized as Excel2007");

            _worksheet = wb.Worksheets.First();

            this.Map(mappings.Where(m => m.Type == ReportType.Excel2007).ToList());
        }
        #endregion

        #region Functions
        protected override Value GetValue(int row, int col, Value.ValueType type = Value.ValueType.Text)
        {
            if (_worksheet == null)
                throw new ArgumentNullException("Worksheet");

            if (row < 0 || col < 0)
                throw new IndexOutOfRangeException("Row and Column Indexes can't be negative");

            var range = _worksheet.Cells[row + 1, col + 1];
            if (range == null || range.Value == null)
                return "";

            if (type == Value.ValueType.Date || range.Value is DateTime)
                return Convert.ToDateTime(range.Value);

            return range.Value.ToString();
        }
        #endregion
    }
}
