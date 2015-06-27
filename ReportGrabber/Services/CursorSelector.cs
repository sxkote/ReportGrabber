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
        ICursor DefineCursor(Report report, IEnumerable<Mapping> mappings);
    }

    public class CursorSelector : ICursorSelector
    {
        public ICursor DefineCursor(Report report, IEnumerable<Mapping> mappings)
        {
            if (mappings == null)
                throw new ArgumentException("Can't select Cursor without Mappings");

            ICursor cursor = this.DefineCursor(report.Type, report.Data, mappings);

            if (cursor == null)
                throw new ReportFormatException();

            if (cursor.Mapping == null)
                throw new MappingNotFoundException();

            return cursor;
        }

        protected ICursor DefineCursor(ReportType type, byte[] data, IEnumerable<Mapping> mappings)
        {
            try
            {
                switch (type)
                {
                    case ReportType.Excel2003:
                        return new CursorExcel2003(data, mappings);
                    case ReportType.Excel2007:
                        return new CursorExcel2007(data, mappings);
                }
            }
            catch { }

            return null;
        }
    }
}
