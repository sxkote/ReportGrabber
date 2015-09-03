using ReportGrabber.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber
{
    public interface IReport
    {
        byte[] Data { get; }
        string Filename { get; }
    }

    /// <summary>
    /// Report input, that contains data to be parsed
    /// </summary>
    public struct Report : IReport
    {
        private byte[] _data;
        private string _filename;

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

        private Report(byte[] data, string filename = "")
        {
            _filename = filename;
            _data = data;

            if (_data == null || _data.Length <= 0)
                throw new ReportInputException();
        }

        /// <summary>
        /// Loads the Report from file system by filename
        /// </summary>
        /// <param name="filename">Filename of the report to be loaded</param>
        /// <returns>The Report structure loaded from the file</returns>
        static public Report Load(string filename)
        {
            var fileinfo = new FileInfo(filename);
            if (!fileinfo.Exists)
                throw new ReportInputException("Report File not found");

            byte[] data = File.ReadAllBytes(filename);
            if (data == null || data.Length <= 0)
                throw new ReportInputException();

            return new Report(data, fileinfo.Name);
        }

        /// <summary>
        /// Loads the Report from byte array
        /// </summary>
        /// <param name="data">Data of the report</param>
        /// <returns>The Report structure loaded from the data</returns>
        static public Report Load(byte[] data)
        {
            if (data == null || data.Length <= 0)
                throw new ReportInputException();

            return new Report(data);
        }
    }
}
