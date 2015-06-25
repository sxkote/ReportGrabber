using System;
using System.Collections.Generic;
using ReportGrabber.Values;
using ReportGrabber.Schemas;

namespace ReportGrabber
{
    public class Grabber : IGrabber
    {
        protected ICollection<Mapping> _mappings;

        public Grabber(ICollection<Mapping> mappings)
        {
            _mappings = mappings;
        }

        public IEnumerable<DataCollection> Grab(byte[] data)
        {
            //if (data == null || data.Length <= 0)
            //    throw new ArgumentNullException("Empty Data can't be grabbed!");

            //List<DataCollection> result = new List<DataCollection>();

            //    SXCursor cursor = null;

            //    try
            //    {
            //        using (MemoryStream ms = new MemoryStream(data))
            //            cursor = SXCursor.DefineCursor(ms, schemas);
            //    }
            //    catch { cursor = null; }

            //    if (cursor == null || cursor.Schema == null)
            //    {
            //        this.Error("Error", "ImportProcess.Schema.NotFound", "");
            //        return;
            //    }

            //    #region Import Duty
            //    while (cursor.MoveNext())
            //        if (!cursor.IsSkip)
            //        {
            //            SXImportDuty duty = cursor.CreateDuty();

            //            if (this.CheckDuty(duty))
            //                this.AddDuty(duty);
            //        }
            //    #endregion

            //    this.AgregateDuty();
        }
    }
}
