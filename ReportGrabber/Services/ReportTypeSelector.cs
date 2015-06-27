using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Services
{
    public interface IReportTypeSelector
    {
        ReportType DefineReportType(byte[] data, string filename = "");
    }

    public class ReportTypeSelector : IReportTypeSelector
    {
        protected readonly bool _openFile;

        public ReportTypeSelector(bool openFile = true)
        {
            _openFile = openFile;
        }

        public ReportType DefineReportType(byte[] data, string filename = "")
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentException("Can't define ReportType without Data");

            if (filename.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase))
            {   
                // try to define ReportType by file extension (.xlsx => Excel 2007)
                if (!_openFile || this.TryOpenExcel2007(data))
                    return ReportType.Excel2007;
            }
            else if (filename.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase))
            {
                // try to define ReportType by file extension (.xls => Excel 2003)
                if (!_openFile || this.TryOpenExcel2003(data))
                    return ReportType.Excel2003;
            }

            // try to define ReportType by opening the Data
            if (_openFile)
            {
                if (this.TryOpenExcel2003(data))
                    return ReportType.Excel2003;

                if (this.TryOpenExcel2007(data))
                    return ReportType.Excel2007;
            }

            return ReportType.Unknown;
        }

        protected bool TryOpenExcel2003(byte[] data)
        {
            try //пытаемся установить курсор в формате Excel2003
            {
                using (var ms = new MemoryStream(data))
                {
                    var workbook = ExcelLibrary.SpreadSheet.Workbook.Load(ms);
                    if (workbook.Worksheets != null && workbook.Worksheets.Count > 0)
                        return true;
                }
            }
            catch { }

            return false;
        }

        protected bool TryOpenExcel2007(byte[] data)
        {
            try //пытаемся установить курсор в формате Excel2003
            {
                using (var ms = new MemoryStream(data))
                {
                    var workbook = (new OfficeOpenXml.ExcelPackage(ms)).Workbook;
                    if (workbook != null && workbook.Worksheets != null && workbook.Worksheets.Count > 0)
                        return true;
                }
            }
            catch { }

            return false;
        }
    }
}
