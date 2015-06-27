using System;
using System.Linq;
using System.Collections.Generic;
using ReportGrabber.Values;
using ReportGrabber.Schemas;
using ReportGrabber.Services;

namespace ReportGrabber
{
    public interface IGrabber
    {
        IEnumerable<DataCollection> Grab(Report report);
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

        public IEnumerable<DataCollection> Grab(Report report)
        {
            var cursor = _cursorSelector.DefineCursor(report, _mappings);

            var result = new List<DataCollection>();
            while (cursor.MoveNext())
            {
                var collection = cursor.GetData();
                if (collection != null)
                    result.Add(collection);
            }

            return result;
        }

        static public IEnumerable<DataCollection> Grab(Report report, params Mapping[] mappings)
        {
            return new Grabber(mappings).Grab(report);
        }

        static public IEnumerable<DataCollection> Grab(byte[] data, params Mapping[] mappings)
        {
            return new Grabber(mappings).Grab(Report.Load(data));
        }
    }
}
