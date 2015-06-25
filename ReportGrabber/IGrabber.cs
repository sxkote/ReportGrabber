using ReportGrabber.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber
{
    public interface IGrabber
    {
        IEnumerable<DataCollection> Grab(byte[] data);
    }
}
