using ReportGrabber.Cursors;
using ReportGrabber.Schemas;
using System;
using System.Collections.Generic;
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
        ICursor DefineCursor(Report report);
    }

    public class CursorSelector : ICursorSelector
    {
        public ICursor DefineCursor(Report report)
        {
            try
            {
                // if Report Type is presented
                // we could assume the Cursor Type
                switch (report.Type)
                {
                    case ReportType.Excel2003:
                        return new CursorExcel2003(report.Data);
                    case ReportType.Excel2007:
                        return new CursorExcel2007(report.Data);
                }
            }
            catch { }

            // if the Report Type is wrong or not presented
            // than we should loop for all available Cursors
            ICursor result = null;

            if ((result = this.TryExcel2003(report.Data)) != null)
                return result;

            if ((result = this.TryExcel2007(report.Data)) != null)
                return result;

            return null;
        }

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
