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
        #region Variables
        protected Worksheet _worksheet = null;
        #endregion

        #region Constructor
        public CursorExcel2003(byte[] data, ICollection<Mapping> mappings)
        {
            _data = data;

            Workbook wb = Workbook.Load(new MemoryStream(data));
            if (wb == null || wb.Worksheets.Count <= 0)
                throw new NotSupportedException("Data was not recognized as Excel2003");

            _worksheet = wb.Worksheets[0];

            this.Map(mappings.Where(m => m.Type == ReportType.Excel2003).ToList());
        }
        #endregion

        #region Functions
        protected override Value GetValue(int row, int col, Value.ValueType type = Value.ValueType.Text)
        {
            if (_worksheet == null)
                throw new ArgumentNullException("Worksheet");

            if (row < 0 || col < 0)
                throw new IndexOutOfRangeException("Row and Column Indexes can't be negative");

            var cell = _worksheet.Cells[row, col];
            if (cell == null || cell.Value == null || cell.IsEmpty)
                return "";

            var cellType = cell.Format.FormatType;

            if (type == Value.ValueType.Date || cellType == CellFormatType.DateTime || cellType == CellFormatType.Date)
                return cell.DateTimeValue;

            if (type == Value.ValueType.Number || cellType == CellFormatType.Number || cellType == CellFormatType.Currency || cellType == CellFormatType.Percentage)
                return Convert.ToDouble(cell.Value);

            return cell.StringValue;
        }
        #endregion
    }
}
