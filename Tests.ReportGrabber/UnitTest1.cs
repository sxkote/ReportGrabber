using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

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
    }
}
