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

    /// <summary>
    /// Report input, that contains data to be parsed
    /// </summary>
    public struct Report
    {
        private ReportType _type;
        private byte[] _data;
        private string _filename;

        /// <summary>
        /// Type of the Report Data Format 
        /// </summary>
        public ReportType Type
        { get { return _type; } }

        /// <summary>
        /// Data of the Report
        /// </summary>
        public byte[] Data
        { get { return _data; } }

        /// <summary>
        /// Filename of the Report 
        /// </summary>
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

        /// <summary>
        /// Loads the Report from file system by filename
        /// </summary>
        /// <param name="filename">Filename of the report to be loaded</param>
        /// <param name="selector">Service that defines the ReportType</param>
        /// <returns>The Report structure loaded from the file</returns>
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

        /// <summary>
        /// Loads the Report from byte array
        /// </summary>
        /// <param name="data">Data of the report</param>
        /// <param name="selector">Service that defines the ReportType</param>
        /// <returns>The Report structure loaded from the data</returns>
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
