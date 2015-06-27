using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Linq;
using ReportGrabber;
using ReportGrabber.Schemas;
using System.Collections.Generic;
using ReportGrabber.Values;

namespace Tests.ReportGrabber
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string pattern = @"(?i)^r(?<rownum>(\+|\-)?\d+)?c(?<colnum>(\+|\-)?\d+)?$";
            var match = Regex.Match("R+2c-1", pattern);
            Assert.IsTrue(match != null && match.Success);
            Assert.IsTrue(match.Groups["rownum"].Value == "+2");
            int r = Int32.Parse(match.Groups["rownum"].Value);
            int c = Int32.Parse(match.Groups["colnum"].Value);
        }

        [TestMethod]
        [DeploymentItem("Reports\\ursa.xls")]
        public void test_ursa()
        {
            byte[] data = System.IO.File.ReadAllBytes("ursa.xls");
           
            var mapping = new Mapping(ReportType.Excel2003)
            {
                Range = new Range("2", "6"),
                Matches = new List<Condition>() { "rc(1;1) == \"Кредитор\"" },
                Rules = new List<Rule>() { new Rule("skip", "cell(4).tostring() == \"\"") },
                Fields = new List<Field>()
                { 
                    new Field("type", "3"), 
                    new Field("date", "4"), 
                    new Field("summ", "6", Value.ValueType.Number), 
                    new Field("number", "1+2+'~~'+3+4")
                }
            };

            var values = Grabber.Grab(data, mapping).ToList();

            var duties = values.Select(d => new Duty()
            {
                Type = d["type"],
                Number = d["number"],
                Date = d["date"],
                Summ = d["summ"]
            }).ToList();

            Assert.IsNotNull(values);
            Assert.AreEqual(4, values.Count());
        }
    }
}
