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
    public class TestReports
    {
        private double _delta = 0.000001;
        private ICollection<Mapping> _mappings;

        [TestInitialize]
        public void Initialize()
        {
            _mappings = new List<Mapping>();

            _mappings.Add(new Mapping(ReportType.Excel2003, "Ursa")
            {
                Range = new Range("2", "6"),
                Match = "rc(1;1) == \"Кредитор\" && rc(1;2) == \"Текст заголовка документа\" && rc(1;3) == \"Вид документа\" && rc(1;6) == \"Сумма в валюте документа\" && rc(1;8) == \"Текст\"",
                Rules = new List<Rule>() { new Rule("skip", "cell(8).tostring() == \"\"") },
                Fields = new List<Field>()
                { 
                    new Field("type", "3", Value.ValueType.Text, new Vocabulary(new VocabularyItem("RE", "Waybill"))), 
                    new Field("date", "= cell(8).substring(cell(8).indexof(\" от \") + 4).trim().todatetime()"), 
                    new Field("summ", " = -cell(6, \"number\")", Value.ValueType.Number), 
                    new Field("number", "= cell(8).substring(cell(8).indexof(\"счф \") + 4; cell(8).indexof(\" от \") - cell(8).indexof(\"счф\") - 4)")
                }
            });

            _mappings.Add(new Mapping(ReportType.Excel2003, "X5")
            {
                Range = new Range("7", "10"),
                Match = "rc(6;1) == \"ИНН клиента\" && rc(6;2) == \"Наименование поставщика\" && rc(6;3) == \"ИНН дебитора\"&& rc(6;5) == \"Сумма\"  && rc(6;6) == \"Дата накладной\" && rc(6;7) == \"Номер накладной\"",
                Rules = new List<Rule>() { new Rule("skip", "cell(9).tostring() != \"829\"") },
                Fields = new List<Field>()
                { 
                    new Field("type", "9", Value.ValueType.Text, new Vocabulary(new VocabularyItem("829", "Waybill"))), 
                    new Field("date", "6", Value.ValueType.Date), 
                    new Field("summ", "5", Value.ValueType.Number), 
                    new Field("number", "10")
                }
            });

        }


        public List<Duty> Grab(string filename)
        {
            var grabbed = Grabber.Grab(filename, _mappings.ToArray());

            return grabbed.Select(d => new Duty()
            {
                Type = d["type"],
                Number = d["number"],
                Date = d["date"],
                Summ = d["summ"]
            }).ToList();
        }

        [TestMethod]
        [DeploymentItem("Reports\\ursa.xls")]
        public void test_ursa()
        {
            var duties = this.Grab("ursa.xls");

            Assert.AreEqual(4, duties.Count());
            Assert.AreEqual(7237600, duties.Sum(d => d.Summ), _delta);
            Assert.IsTrue(duties.Any(d => d.Number == "226" && d.Date.Date == new DateTime(2015,5,26) && d.Type == "Waybill"));
            Assert.IsTrue(duties.Any(d => d.Number == "221" && d.Date.Date == new DateTime(2015, 5, 25) && d.Type == "Waybill"));
            Assert.IsTrue(duties.Any(d => d.Number == "219" && d.Date.Date == new DateTime(2015, 5, 23) && d.Type == "Waybill"));
            Assert.IsTrue(duties.Any(d => d.Number == "220" && d.Date.Date == new DateTime(2015, 5, 23) && d.Type == "Waybill"));
        }

        [TestMethod]
        [DeploymentItem("Reports\\x5-rap.xls")]
        public void test_x5_rap()
        {
            var duties = this.Grab("x5-rap.xls");

            Assert.AreEqual(7615, duties.Count());
            Assert.AreEqual(62666532.03, duties.Sum(d => d.Summ), _delta);
        }
    }
}
