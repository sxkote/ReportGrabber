using ReportGrabber.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber
{
    public enum ReportType { Unknown, Excel2003, Excel2007 };

    public struct Report
    {
        private ReportType _type;
        private byte[] _data;
        private string _filename;

        public ReportType Type
        { get { return _type; } }

        public byte[] Data
        { get { return _data; } }

        public string Filename
        { get { return _filename; } }

        private Report(ReportType type, byte[] data, string filename = "")
        {
            _type = type;
            _filename = filename;
            _data = data;

            if (_data == null || _data.Length <= 0)
                throw new ReportInputException();
        }

        static public Report Load(string filename, IReportTypeSelector selector = null)
        {
            var fileinfo = new FileInfo(filename);
            if (!fileinfo.Exists)
                throw new ReportInputException("Report File not found");

            byte[] data = File.ReadAllBytes(filename);
            if (data == null || data.Length <= 0)
                throw new ReportInputException();

            var service = selector == null ? new ReportTypeSelector() : selector;

            var type = service.DefineReportType(data, fileinfo.Name);

            return new Report(type, data, fileinfo.Name);
        }

        static public Report Load(byte[] data, IReportTypeSelector selector = null)
        {
            if (data == null || data.Length <= 0)
                throw new ReportInputException();

            var service = selector == null ? new ReportTypeSelector() : selector;

            var type = service.DefineReportType(data, "");

            return new Report(type, data);
        }
    }
}
