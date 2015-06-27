using ReportGrabber.Schemas;
using ReportGrabber.Values;
using SXCore.Lexems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Cursors
{
    public interface ICursor
    {
        Mapping Mapping { get; }
        bool MoveNext();
        DataCollection GetData();
    }

    public abstract class Cursor : ICursor
    {
        #region Variables
        protected Mapping _mapping = null;
        #endregion

        #region Properties
        public Mapping Mapping
        { get { return _mapping; } }
        #endregion

        #region Constructor
        protected Cursor()
        {

        }
        #endregion

        #region Virtual
        public abstract bool MoveNext();

        protected abstract Value GetValue(Address address, Value.ValueType type = Value.ValueType.Text);

        protected abstract bool CheckCondition(Condition condition);
        #endregion

        #region Functions
        /// <summary>
        /// Defines the appropriate Mapping for current Report from the Mappings list
        /// </summary>
        /// <param name="mappings">List of the available Mappings to select from</param>
        protected void Map(IEnumerable<Mapping> mappings)
        {
            _mapping = mappings == null ? null : mappings.FirstOrDefault(m => m.Matches.All(c => this.CheckCondition(c)));
        }

        /// <summary>
        /// Defines if the current position should be skipped from getting Data (row in excel, node in xml, etc)
        /// </summary>
        /// <returns>True if the current position should be skipped</returns>
        protected bool IsSkip()
        {
            if (_mapping == null)
                throw new CursorNoMappingException();

            // if there are no rules then no Skip
            if (_mapping.Rules == null || _mapping.Rules.Count() <= 0)
                return false;

            // find all Skip rules 
            var skip = _mapping.Rules.Where(r => r.Name.Equals("skip", StringComparison.InvariantCultureIgnoreCase));
            if (skip == null || skip.Count() <= 0)
                return false;

            // check if any Skip rule passed
            return skip.Any(r => this.CheckCondition(r.Condition));
        }

        /// <summary>
        /// Get DataCollection from the Report on the current position (row, node, etc.)
        /// </summary>
        /// <returns>DataCollection defined by Fields in the Mapping</returns>
        public DataCollection GetData()
        {
            if (_mapping == null)
                throw new CursorNoMappingException();

            if (this.IsSkip())
                return null;

            return new DataCollection(_mapping.Fields.Select(f => new Data(f.Name, this.GetValue(f.Address, f.Type))));
        }
        #endregion
    }
}
