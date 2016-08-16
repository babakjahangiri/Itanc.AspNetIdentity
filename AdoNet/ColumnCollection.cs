using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itanc.AspNetIdentity.AdoNet
{
    public class ColumnCollection : System.Collections.ObjectModel.Collection<ColumnItem>
    {

    }

    public class ColumnItem
    {
        public string ColumnName { get; }
        public object ColumnValue { get; }

        public ColumnItem(string columnName, object columnValue)
        {
            this.ColumnName = columnName;
            this.ColumnValue = columnValue;
        }
    }
}
