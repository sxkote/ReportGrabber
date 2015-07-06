using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ReportGrabber
{
    public class Limit
    {
        public Limit() { }

        public string Contract { get; set; }
        public decimal LimitValue { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : {1} от {2}", Contract, LimitValue, Date.ToShortDateString());
        }
    }
}
