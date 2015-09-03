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

            _mappings.Add(new Mapping("Ursa")
            {
                Range = new Range("2", "6"),
                Match = "rc(1;1) == \"Кредитор\" && rc(1;2) == \"Текст заголовка документа\" && rc(1;3) == \"Вид документа\" && rc(1;6) == \"Сумма в валюте документа\" && rc(1;8) == \"Текст\"",
                Rules = new List<Rule>() { new Rule("skip", "cell(8).tostring() == \"\"") },
                Fields = new List<Field>()
                { 
                    new Field("type", "3", Value.ValueType.Text, new Vocabulary(new VocabularyItem("RE", "Waybill"))), 
                    new Field("date", "cell(8).substring(cell(8).indexof(\" от \") + 4)"), 
                    new Field("summ", "-getnumber(6)", Value.ValueType.Number), 
                    new Field("number", "cell(8).substring(cell(8).indexof(\"счф \") + 4; cell(8).indexof(\" от \") - cell(8).indexof(\"счф\") - 4)")
                }
            });

            _mappings.Add(new Mapping("X5")
            {
                Range = new Range("7", "10"),
                Match = "rowcol(6;1) == \"ИНН клиента\" && rowcol(6;2) == \"Наименование поставщика\" && rc(6;3) == \"ИНН дебитора\"&& rc(6;5) == \"Сумма\"  && rc(6;6) == \"Дата накладной\" && rc(6;7) == \"Номер накладной\"",
                Rules = new List<Rule>() { new Rule("skip", "cell(9) != \"829\"") },
                Fields = new List<Field>()
                { 
                    new Field("type", "'Waybill'", Value.ValueType.Text, new Vocabulary(new VocabularyItem("829", "Waybill"))), 
                    new Field("date", "cell(6, \"date\")", Value.ValueType.Date), 
                    new Field("summ", "cell(5)", Value.ValueType.Number), 
                    new Field("number", "cell(10).tostring()")
                }
            });

<<<<<<< HEAD
            _mappings.Add(new Mapping(ReportType.Excel2003, "Castorama")
            {
                Range = new Range("2", "1"),
                Match = "rc(1;2) == \"Счет\" && rc(1;6).contains(\"Дата\") && rc(1;7) == \"Номер\" && rc(1;8).contains(\"Сумма\")",
                Fields = new List<Field>() 
                {
                    new Field("contract", "getnumber(2)", Value.ValueType.Text),
                    new Field("date", "getdate(6)", Value.ValueType.Date),
                    new Field("number", "gettext(7)", Value.ValueType.Text),
                    new Field("summ", "getnumber(8)", Value.ValueType.Number), 
                    new Field("type", "'Waybill'", Value.ValueType.Text)
                }
            });

            _mappings.Add(new Mapping(ReportType.Excel2003, "Castorama2")
            {
                Range = new Range("2", "1"),
                Match = "rc(1;1) == \"Счет\" && rc(1;2).contains(\"Дата\") && rc(1;3) == \"Ссылка\" && rc(1;4).contains(\"Сумма\")",
                Fields = new List<Field>() 
                {
                    new Field("contract", "gettext(1)", Value.ValueType.Text),
                    new Field("date", "getdate(2)", Value.ValueType.Date),
                    new Field("number", "gettext(3)", Value.ValueType.Text),
                    new Field("summ", "getnumber(4)", Value.ValueType.Number), 
                    new Field("type", "'Waybill'", Value.ValueType.Text)
                }
            });

            _mappings.Add(new Mapping(ReportType.Excel2003, "Leroy")
            {
                Range = new Range("7", "7"),
                Match = "rc(6;2) == \"Наименование поставщика / Name of Supplier\"",
                Fields = new List<Field>() 
                {
                    new Field("contract", "gettext(2)", Value.ValueType.Text),
                    new Field("date", "getdate(6)"),
                    new Field("number", "gettext(5)", Value.ValueType.Text),
                    new Field("summ", "getnumber(7)", Value.ValueType.Number), 
                    new Field("type", "'Waybill'", Value.ValueType.Text)
                }
            });
=======


>>>>>>> origin/master
        }

        public List<Duty> Grab(string filename)
        {
            var grabbed = Grabber.Grab(filename, _mappings.ToArray()).ToList();

            return grabbed.Select(dataCollection => new Duty()
            {
                Type = dataCollection["type"],
                Contract = dataCollection["contract"],
                Number = dataCollection["number"],
                Date = dataCollection["date"],
                Summ = dataCollection["summ"]
            }).ToList();
        }

        [TestMethod]
        [DeploymentItem("Reports\\ursa.xls")]
        public void test_ursa()
        {
            var duties = this.Grab("ursa.xls");

            Assert.AreEqual(4, duties.Count());
            Assert.AreEqual(7237600, duties.Sum(d => d.Summ), _delta);

            Assert.IsTrue(duties.Any(d => d.Number == "226" && d.Date.Date == new DateTime(2015, 5, 26) && d.Type == "Waybill"));
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

        [TestMethod]
<<<<<<< HEAD
        [DeploymentItem("Reports\\castorama_limit.xls")]
        public void test_castorama()
        {
            var duties = this.Grab("castorama_limit.xls");

            var limits = duties.GroupBy(x => x.Contract)
                .Select(l => new { Contract = l.Key, Value = (decimal)l.Sum(s => s.Summ) * -1 })
                .Select(limitCollection => new Limit()
                {
                    Contract = limitCollection.Contract,
                    Comment = "",
                    Date = DateTime.Now.Date,
                    LimitValue = limitCollection.Value                   
                })
                .ToList();

            //var limits = duties.GroupBy(x => x.Contract)
            //    .Select(l => new 
            //    { 
            //        Contract = l.Key,
            //        Value = l.Sum(s => s.Summ) 
            //    });


            Assert.AreEqual(944, duties.Count());
            Assert.AreEqual(-144423153.01, duties.Sum(d => d.Summ));
        }

        [TestMethod]
        [DeploymentItem("Reports\\castorama_limit_2.xls")]
        public void test_castorama2()
        {
            var duties = this.Grab("castorama_limit_2.xls");

            Assert.AreEqual(944, duties.Count());
        }

        [TestMethod]
        [DeploymentItem("Reports\\leroy_limit.xls")]
        public void test_leroy()
        {
            var duties = this.Grab("leroy_limit.xls");

            var limits = duties
                .GroupBy(x => x.Contract).Select(l => new { Contract = l.Key, Value = l.Sum(s => s.Summ) });

=======
        [DeploymentItem("Reports\\x5-rap.xls")]
        public void test___()
        {
            var duties = this.Grab("x5-rap.xls");
>>>>>>> origin/master
        }
    }
}
