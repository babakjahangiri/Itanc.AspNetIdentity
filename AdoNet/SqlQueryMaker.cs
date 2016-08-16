using System;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itanc.AspNetIdentity.AdoNet
{
    public class SqlQueryMaker
    {
        public ColumnCollection ValueParameters { get; }
        public ColumnCollection WhereParameters { get; }
        public string TableName { get; set; }

        public SqlQueryMaker() 
            : base()
        {
            WhereParameters = new ColumnCollection();
            ValueParameters = new ColumnCollection();
        }

        private static string ReliefValue(object value)
        {

            var returnValue = "";
            if (value == null) return "NULL";  //Null String For SQL Query
            if (value.GetType().IsValueType == true)
            {
                switch (Information.IsDate(value))
                {
                    case true:  //Value is Date
                        returnValue = "'" + value.ToString() + "'";
                        break;

                    default:
                        if ((Information.IsNumeric(value) == true) &
                            (value.GetType().ToString() != typeof(bool).ToString()))
                            returnValue = value.ToString(); //Value is Number
                        else if (value.GetType().ToString() == typeof(bool).ToString())
                            returnValue = (Convert.ToByte(value)).ToString(); //value is boolean
                        else
                            returnValue = "'" + value.ToString() + "'";
                        //Value is String Or Char
                        break;
                }
            }
            else
            {
                if (value.ToString().StartsWith("@"))
                    returnValue = value.ToString();
                //Value is SQL parameter
                else
                    returnValue = "'" + value.ToString() + "'";
                //Value is String
            }


            return returnValue;
        }

        /// <summary>
        /// Generat Insert Sql Query But Before using it you must Used ValueParameters 
        /// </summary>
        /// <returns></returns>
        /// 
        public string Insert()
        {
            var insQuery = "INSERT INTO " + TableName + " (";

            for (var i = 0; i <= ValueParameters.Count - 1; i++)
                insQuery += ValueParameters[i].ColumnName.ToString() + ",";

            var leng = insQuery.Length;
            insQuery = insQuery.Remove(leng - 1, 1);
            insQuery = insQuery + ") VALUES (";

            for (var i = 0; i <= ValueParameters.Count - 1; i++)
                insQuery += ReliefValue(ValueParameters[i].ColumnValue).ToString() + ",";

            leng = insQuery.Length;
            insQuery = insQuery.Remove(leng - 1, 1);
            insQuery = insQuery + ")";

            return insQuery;
        }

        /// <summary>
        /// Generat Delete Sql Query But Before it you must Used  WhereParameters  
        /// </summary>
        /// <returns></returns>
        public string Delete()
        {
            var delQuery = "DELETE FROM " + TableName + " WHERE ";

            for (var i = 0; i <= WhereParameters.Count - 1; i++)
                delQuery += WhereParameters[i].ColumnName.ToString() + " = " +
                            ReliefValue(WhereParameters[i].ColumnValue).ToString() + " AND ";

            var leng = delQuery.Length;
            delQuery = delQuery.Remove(leng - 5, 5);

            return delQuery;
        }

        /// <summary>
        ///  Generat Update Sql Query But Before it you must Used  WhereParameters and  ValueParameters
        /// </summary>
        /// <returns></returns>
        public string Update()
        {
            var updQuery = "UPDATE " + TableName + " SET ";

            for (var i = 0; i <= ValueParameters.Count - 1; i++)
                updQuery += ValueParameters[i].ColumnName.ToString() + " = " +
                            ReliefValue(ValueParameters[i].ColumnValue).ToString() + " ,";

            var leng = updQuery.Length;
            updQuery = updQuery.Remove(leng - 1, 1);
            updQuery += " WHERE ";

            for (var i = 0; i <= WhereParameters.Count - 1; i++)
                updQuery += WhereParameters[i].ColumnName.ToString() + " = " +
                            ReliefValue(WhereParameters[i].ColumnValue).ToString() + " AND ";

            var intlen = updQuery.Length;
            return WhereParameters.Count == 0 ? updQuery.Remove(intlen - 7, 7) : updQuery.Remove(intlen - 4, 4);

        }

    }
}
