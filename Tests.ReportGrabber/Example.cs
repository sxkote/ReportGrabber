using ReportGrabber;
using ReportGrabber.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ReportGrabber
{
    #region Reports Deliverers
    public struct ReportDeliveryInfo
    {
        public enum SourceType { MetroLink, Auchan, Email, X5, Other1, Other2 };

        private SourceType _type;

        public SourceType Type
        { get { return _type; } }

        public ReportDeliveryInfo(SourceType type)
        {
            _type = type;
        }
    }

    public interface IReportsDeliverer
    {
        ICollection<IReport> Deliver();
    }

    public class MetroLinkReportsDeliverer : IReportsDeliverer
    {
        public ICollection<IReport> Deliver()
        {
            throw new NotImplementedException();
        }
    }

    public class AuchanReportsDeliverer : IReportsDeliverer
    {
        public ICollection<IReport> Deliver()
        {
            throw new NotImplementedException();
        }
    }

    public class ReportDelivererFabric
    {
        static public IReportsDeliverer Create(ReportDeliveryInfo deliveryInfo)
        {
            if (deliveryInfo.Type == ReportDeliveryInfo.SourceType.Auchan)
                return new AuchanReportsDeliverer();
            if (deliveryInfo.Type == ReportDeliveryInfo.SourceType.MetroLink)
                return new MetroLinkReportsDeliverer();

            throw new NotSupportedException();
        }
    }
    #endregion

    #region Grabbers
    public struct ReportGrabberInfo
    {
        public enum GrabberType { Standart, MyCustomAuchan, MyCustomOBI, MyOther1, MyOther2 };

        private GrabberType _type;

        public GrabberType Type 
        { get { return _type; } }

        public ReportGrabberInfo(GrabberType type)
        {
            _type = type;
        }
    }

    public class MyCustomAuchanReportGrabber : IGrabber
    {
        public IEnumerable<DataCollection> Grab(IReport report)
        {
            throw new NotImplementedException();
        }
    }

    public class ReportGrabberFabric
    {
        static public IGrabber Create(ReportGrabberInfo grabberInfo)
        {
            if (grabberInfo.Type == ReportGrabberInfo.GrabberType.Standart)
            {
                var mappings = new List<Mapping>();
                //mappings = GetMappingsFromDataBase(....);
                return new Grabber(mappings);
            }

            if (grabberInfo.Type == ReportGrabberInfo.GrabberType.MyCustomAuchan)
                return new MyCustomAuchanReportGrabber();

            throw new NotImplementedException();
        }
    }
    #endregion

    #region Executors
    public struct ReportExecutionInfo
    {
        public enum DuplicatesOperationType { Sum, First, Exception };

        private bool _insertDuty;
        private bool _updateLimits;
        private bool _paymentsReestr;
        private DuplicatesOperationType _duplicatesOperation;

        public bool InsertDuty 
        { get { return _insertDuty; } }
        
        public bool UpdateLimits 
        { get { return _updateLimits; } }
        
        public bool PaymentsReestr 
        { get { return _paymentsReestr; } }
        
        public DuplicatesOperationType DuplicatesOperation 
        { get { return _duplicatesOperation; } }

        public ReportExecutionInfo(bool insertDuty, bool updateLimits, bool paymentsReestr, DuplicatesOperationType duplicatesOperation = DuplicatesOperationType.Sum)
        {
            _insertDuty = insertDuty;
            _updateLimits = updateLimits;
            _paymentsReestr = paymentsReestr;
            _duplicatesOperation = duplicatesOperation;
        }
    }

    public interface IReportExecutor
    {
        void Execute(IEnumerable<DataCollection> data);
    }

    // пока не буду нагружать этим... - но нужно разделить ответственность
    //public interface IInsertReportDutyService
    //{
    //    public void AddDuty(Duty duty);
    //    public void SaveChanges();
    //}

    public class InsertDutyReportExecutor : IReportExecutor
    {
        //public IInsertReportDutyService InsertService {get; private set;}
        //public InsertDutyReportExecutor(IInsertReportDutyService insertService)
        //{
        //    InsertService = insertService;
        //}

        public void Execute(IEnumerable<DataCollection> data)
        {
            // здесь мы вставляем задолженность из данных в БД:
            // var duties = data.Select(d=>new Duty(d["Contract"], d["Date"], d["Summ"], d["Number"]));
            // dbContext = new DutiesDbContext();
            // dbContext.Duties.Add(duties);
            // dbContext.SaveChanges();

            throw new NotImplementedException();
        }
    }

    public class UpdateLimitsReportExecutor : IReportExecutor
    {
        public void Execute(IEnumerable<DataCollection> data)
        {
            // здесь мы считаем лимитные условия из данных и вставляем их в БД
            throw new NotImplementedException();
        }
    }

    public class AddDebtorPaymentsReportExecutor:IReportExecutor
    {
        public void Execute(IEnumerable<DataCollection> data)
        {
            // здесь мы определяем реестры платежей и сохраняем их в БД
            throw new NotImplementedException();
        }
    }

    public class ReportExecutorFabric
    {
        static public IEnumerable<IReportExecutor> Create(ReportExecutionInfo executionInfo)
        {
            var executors = new List<IReportExecutor>();

            if (executionInfo.InsertDuty)
                executors.Add(new InsertDutyReportExecutor());

            if (executionInfo.UpdateLimits)
                executors.Add(new UpdateLimitsReportExecutor());

            if (executionInfo.PaymentsReestr)
                executors.Add(new AddDebtorPaymentsReportExecutor());

            return executors;
        }
    }
    #endregion

    #region Example
    public struct ReportTaskInfo
    {
        private ReportDeliveryInfo _delivery;
        private ReportGrabberInfo _grabber;
        private ReportExecutionInfo _execution;

        public ReportDeliveryInfo Delivery
        { get { return _delivery; } }

        public ReportGrabberInfo Grabber
        { get {return _grabber; } }

        public ReportExecutionInfo Execution
        { get { return _execution; } }

        public ReportTaskInfo(ReportDeliveryInfo delivery, ReportGrabberInfo grabber, ReportExecutionInfo execution)
        {
            _delivery = delivery;
            _grabber = grabber;
            _execution = execution;
        }
    }

    public class Example
    {
        public void ExecuteReportTask(ReportTaskInfo info)
        {
            // создаем правильного Доставителя отчетов
            var deliverer = ReportDelivererFabric.Create(info.Delivery);

            // берем все Отчеты для обработки из этого Доставителя
            var reports = deliverer.Deliver();

            // каждый Отчет нам нужно обработать
            foreach (var report in reports)
            {
                // создаем правильный Разбиратель Отчета
                var grabber = ReportGrabberFabric.Create(info.Grabber);

                // получаем данные из Отчета
                var data = grabber.Grab(report);

                // создаем необходимые Исполнители Очтета
                var executors = ReportExecutorFabric.Create(info.Execution);

                // каждый Исполнетель должен выполнить свою задачу при обработке данных Отчета
                foreach (var executor in executors)
                    executor.Execute(data);
            }
        }
    }
    #endregion
}
