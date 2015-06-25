using ReportGrabber.Schemas;
using ReportGrabber.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Cursors
{
    public abstract class Cursor
    {
        #region Variables
        protected byte[] _data = null;
        protected Mapping _mapping = null;
        #endregion

        #region Virtual
        public bool IsSkip()
        {
            //if (this.Schema == null || this.Schema.Rules == null) return true;

            //foreach (SXSchemaRule r in this.Schema.Rules)
            //    if (r.Name.Trim().ToLower() == "skip")
            //    {
            //        if (this.Schema.Fields[r.Param] != null)
            //        {
            //            if (this.GetFieldString(r.Param).Trim().ToLower().Contains(r.Value.Trim().ToLower()))
            //                return true;
            //        }
            //        else
            //        {
            //            if (this.GetValue(new SXSchemaAddress(r.Param)).Trim().ToLower() == r.Value.Trim().ToLower())
            //                return true;
            //        }
            //    }

            return false;
        }

        protected void Map(ICollection<Mapping> mappings)
        {
            _mapping = mappings == null ? null : mappings.FirstOrDefault(m => m.Conditions.All(c => this.CheckCondition(c)));
        }

        protected bool CheckCondition(Condition condition)
        {
            return this.GetValue(condition.Address).ToString().ToLower() == condition.Value.Trim().ToLower();
        }

        public abstract bool MoveNext();

        public abstract Value GetValue(Address address);

        public Data GetData(Field field)
        {
            return new Data(field.Name, this.GetValue(field.Address));
        }
        #endregion

        #region Statics
        static public Cursor DefineCursor(byte[] data, ICollection<Mapping> mappings)
        {
            using (var ms = new MemoryStream(data))
            {
                try //пытаемся установить курсор в формате Excel2003
                {
                    ExcelLibrary.SpreadSheet.Workbook wb = ExcelLibrary.SpreadSheet.Workbook.Load(ms);
                    if (wb.Worksheets != null && wb.Worksheets.Count > 0)
                        return new CursorExcel2003(data, mappings);
                }
                catch { ms.Position = 0; }

                try //пытаемся установить курсор в формате Excel2007
                {
                        OfficeOpenXml.ExcelWorkbook wb = (new OfficeOpenXml.ExcelPackage(ms)).Workbook;
                        if (wb != null && wb.Worksheets != null && wb.Worksheets.Count > 0)
                        return new CursorExcel2007(data, mappings);
                }
                catch { ms.Position = 0; }

                //try //пытаемся установить курсор в формате XML
                //{
                //    SXCursorXML xml_cursor = new SXCursorXML(ms);
                //    if (xml_cursor != null && xml_cursor.Document != null)
                //        cursor = xml_cursor;
                //}
                //catch { ms.Position = 0; }
            }

            return null;
        }

        //static public Cursor DefineCursor(byte[] data, ICollection<Mapping> mappings)
        //{
        //    SXCursor cursor = null;

        //    #region Excel2003
        //    if (cursor == null)
        //    {
        //        try //пытаемся установить курсор в формате Excel2003
        //        {
        //            using (MemoryStream ms = new MemoryStream(data))
        //            {
        //                ExcelLibrary.SpreadSheet.Workbook wb = ExcelLibrary.SpreadSheet.Workbook.Load(ms);
        //                if (wb.Worksheets != null && wb.Worksheets.Count > 0)
        //                    cursor = new SXCursorExcel2003(wb.Worksheets[0]);
        //            }
        //        }
        //        catch { cursor = null; }
        //    }
        //    #endregion

        //    #region Excel2007
        //    if (cursor == null)
        //    {
        //        try //пытаемся установить курсор в формате Excel2007
        //        {
        //            using (MemoryStream ms = new MemoryStream(data))
        //            {
        //                OfficeOpenXml.ExcelWorkbook wb = (new OfficeOpenXml.ExcelPackage(ms)).Workbook;
        //                if (wb != null && wb.Worksheets != null && wb.Worksheets.Count > 0)
        //                    cursor = new SXCursorExcel2007(wb.Worksheets.First());
        //            }
        //        }
        //        catch { cursor = null; }
        //    }
        //    #endregion

        //    #region XML
        //    if (cursor == null)
        //    {
        //        try //пытаемся установить курсор в формате XML
        //        {
        //            using (MemoryStream ms = new MemoryStream(data))
        //            {
        //                SXCursorXML xml_cursor = new SXCursorXML(ms);
        //                if (xml_cursor != null && xml_cursor.Document != null)
        //                    cursor = xml_cursor;
        //            }
        //        }
        //        catch { cursor = null; }
        //    }
        //    #endregion

        //    if (cursor == null || cursor.MappingType == SXSchema.SXMappingType.None)
        //        return null;

        //    #region Define Mapping from list for current Cursor and CursorType
        //    foreach (SXSchema schema in schema_list)
        //    {
        //        if (schema == null || schema.MappingType != cursor.MappingType)
        //            continue;

        //        cursor.Schema = schema;

        //        if (cursor.CheckCondition())
        //            return cursor;
        //        else
        //            cursor.Schema = null;
        //    }
        //    #endregion

        //    return null;
        //}
        #endregion
    }
}
