using ReportGrabber.Cursors;
using ReportGrabber.Schemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Services
{
    public interface ICursorSelector
    {
        /// <summary>
        /// Define the appropriate Cursor (that could be applied) for selected Report
        /// </summary>
        /// <param name="report">Report with data</param>
        /// <returns>Cursor, that could be applied to the Report data</returns>
        ICursor DefineCursor(IReport report);
    }

    public class CursorSelector : ICursorSelector
    {
        public ICursor DefineCursor(IReport report)
        {
            ICursor result = null;

            if (!String.IsNullOrEmpty(report.Filename))
            {
                // first, try to define the Cursor by file extension

                if (report.Filename.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase))
                {
                    // try to define Cursor by file extension (.xlsx => Excel 2007)
                    if ((result = this.TryExcel2007(report.Data)) != null)
                        return result;
                }
                else if (report.Filename.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase))
                {
                    // try to define Cursor by file extension (.xls => Excel 2003)
                    if ((result = this.TryExcel2003(report.Data)) != null)
                        return result;
                }
            }

            // if the Cursor is not defined yet than we should loop for all available Cursors

            if ((result = this.TryExcel2003(report.Data)) != null)
                return result;

            if ((result = this.TryExcel2007(report.Data)) != null)
                return result;

            return null;
        }

        //protected bool TryOpenExcel2003(byte[] data)
        //{
        //    try //пытаемся установить курсор в формате Excel2003
        //    {
        //        using (var ms = new MemoryStream(data))
        //        {
        //            var workbook = ExcelLibrary.SpreadSheet.Workbook.Load(ms);
        //            if (workbook.Worksheets != null && workbook.Worksheets.Count > 0)
        //                return true;
        //        }
        //    }
        //    catch { }

        //    return false;
        //}

        //protected bool TryOpenExcel2007(byte[] data)
        //{
        //    try //пытаемся установить курсор в формате Excel2003
        //    {
        //        using (var ms = new MemoryStream(data))
        //        {
        //            var workbook = (new OfficeOpenXml.ExcelPackage(ms)).Workbook;
        //            if (workbook != null && workbook.Worksheets != null && workbook.Worksheets.Count > 0)
        //                return true;
        //        }
        //    }
        //    catch { }

        //    return false;
        //}

        protected CursorExcel2003 TryExcel2003(byte[] data)
        {
            try
            {
                return new CursorExcel2003(data);
            }
            catch { return null; }
        }

        protected CursorExcel2007 TryExcel2007(byte[] data)
        {
            try
            {
                return new CursorExcel2007(data);
            }
            catch { return null; }
        }

    }
}
