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
        bool MoveNext(Range range);

        /// <summary>
        /// Checks if selected Condition is valid for the current Cursor position
        /// </summary>
        /// <param name="condition">Condition to be verified</param>
        /// <returns>True if Condition is passed, False in Condition not passed</returns>
        bool CheckCondition(Condition condition);

        /// <summary>
        /// Get the Value from the current Cursor position by specific Address
        /// </summary>
        /// <param name="address">Address of the Value to be retrieved from the Cursor</param>
        /// <param name="type">Type of the Value to be retrieved</param>
        /// <returns>The Value that presented on specific Address of current Cursor position</returns>
        Value GetValue(Address address, Value.ValueType type = Value.ValueType.Text);

        ///// <summary>
        ///// Get Data item from current position of Cursor for selected Field
        ///// </summary>
        ///// <param name="field">Field that defines the Data parametres (name, address, type)</param>
        ///// <returns>Data for selected Field from the current position</returns>
        //Data GetData(Field field);
    }

    public abstract class Cursor : ICursor
    {
        public abstract bool MoveNext(Range range);

        public abstract Value GetValue(Address address, Value.ValueType type = Value.ValueType.Text);

        public abstract bool CheckCondition(Condition condition);

        //public virtual Data GetData(Field field)
        //{
        //    return new Data(field.Name, this.GetValue(field.Address, field.Type));
        //}
    }
}
