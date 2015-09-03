using System;
using System.Linq;
using System.Collections.Generic;
using ReportGrabber.Values;
using ReportGrabber.Schemas;
using ReportGrabber.Services;
using ReportGrabber.Cursors;

namespace ReportGrabber
{
    public interface IGrabber
    {
        /// <summary>
        /// Grab Data from Report
        /// </summary>
        /// <param name="report">Report that contains Data to be grabbed</param>
        /// <returns>Collection of Data Values grouped to DataCollections</returns>
        IEnumerable<DataCollection> Grab(IReport report);
    }

    public class Grabber : IGrabber
    {
        protected ICursorSelector _cursorSelector;
        protected IEnumerable<Mapping> _mappings;

        public Grabber(IEnumerable<Mapping> mappings, ICursorSelector cursorSelector = null)
        {
            _mappings = mappings;
            _cursorSelector = cursorSelector == null ? new CursorSelector() : cursorSelector;
        }

        public IEnumerable<DataCollection> Grab(IReport report)
        {
            // define appropriate Cursor for current Report
            var cursor = _cursorSelector.DefineCursor(report);
            if (cursor == null)
                throw new ReportFormatException();

            // define appropriate Mapping for current Report from _mappings list
            var mapping = _mappings.FirstOrDefault(m =>
            {
                try { return cursor.CheckCondition(m.Match); }
                catch { return false; }
            });

            if (mapping == null)
                throw new MappingNotFoundException();

            var result = new List<DataCollection>();
            while (cursor.MoveNext(mapping.Range))
            {
                // define if current position should be skipped
                if (mapping.Rules != null)
                    if (mapping.Rules.Any(r => r.Name.ToLower() == "skip" && cursor.CheckCondition(r.Condition)))
                        continue;

                // get the DataCollection for all the Fields in the Mapping
                var collection = new DataCollection(mapping.Fields.Select(f => this.GetDataFromField(cursor, f)));
                if (collection.Values != null && collection.Values.Count > 0)
                    result.Add(collection);
            }

            return result;
        }

        protected Data GetDataFromField(ICursor cursor, Field field)
        {
            if (cursor == null)
                throw new CursorException("Can't get Data from null Cursor");

            var value = cursor.GetValue(field.Address, field.Type);

            if (field.Vocabulary != null)
            {
                var replacement = field.Vocabulary.Items.FirstOrDefault(i => i.Text.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase));
                if (replacement != null)
                    value = replacement.Value;
            }

            return new Data(field.Name, value);
        }

        /// <summary>
        /// Grab the Report by Mappings with standart CursorSelector
        /// </summary>
        /// <param name="report">Report to grab data from</param>
        /// <param name="mappings">Mappings that could be applied to the Report</param>
        /// <returns>Collection of Data Values from Report, depends on Mapping</returns>
        static public IEnumerable<DataCollection> Grab(IReport report, params Mapping[] mappings)
        {
            return new Grabber(mappings).Grab(report);
        }

        /// <summary>
        /// Grab the Report (builded from data) by Mappings with standart CursorSelector
        /// </summary>
        /// <param name="data">Data of the Report to be grabbed</param>
        /// <param name="mappings">Mappings that could be applied to the Report</param>
        /// <returns>Collection of Data Values from Report, depends on Mapping</returns>
        static public IEnumerable<DataCollection> Grab(byte[] data, params Mapping[] mappings)
        {
            return Grabber.Grab(Report.Load(data), mappings);
        }

        /// <summary>
        /// Grab the Report (builded from file) by Mappings with standart CursorSelector
        /// </summary>
        /// <param name="filename">Filename the Report to be loaded and grabbed</param>
        /// <param name="mappings">Mappings that could be applied to the Report</param>
        /// <returns>Collection of Data Values from Report, depends on Mapping</returns>
        static public IEnumerable<DataCollection> Grab(string filename, params Mapping[] mappings)
        {
            return Grabber.Grab(Report.Load(filename), mappings);
        }
    }
}
