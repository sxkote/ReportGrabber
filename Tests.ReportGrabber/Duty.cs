using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ReportGrabber
{
    public class Duty
    {
        public string Type { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public double Summ { get; set; }

        public Duty() { }

        public override string ToString()
        {
            return String.Format("{0} : {1} от {2} = {3}", Type, Number, Date.ToShortDateString(), Summ);
        }
    }
}
