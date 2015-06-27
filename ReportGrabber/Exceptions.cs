using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber
{
    public class ReportGrabberException: ApplicationException
    {
        public ReportGrabberException()
            : base() { }

        public ReportGrabberException(string message)
            : base(message) { }
    }

    public class ReportInputException:ReportGrabberException
    {
        public ReportInputException()
            : base("Report Data is empty or invalid") { }

        public ReportInputException(string message)
            : base(message) { }
    }

    public class ReportFormatException : ReportGrabberException
    {
        public ReportFormatException()
            : base("Report Format not recognized") { }

        public ReportFormatException(string message)
            : base(message) { }
    }

    public class MappingNotFoundException : ReportGrabberException
    {
        public MappingNotFoundException()
            : base("No appropriate Mapping was found") { }

        public MappingNotFoundException(string message)
            : base(message) { }
    }

    public class CursorException : ReportGrabberException
    {
        public CursorException(string message)
            : base(message) { }
    }

    public class CursorNoMappingException : CursorException
    {
        public CursorNoMappingException()
            : base("Cursor has no Mapping defined") { }

        public CursorNoMappingException(string message)
            : base(message) { }
    }

}
