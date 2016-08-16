using System.Data;
 
namespace Itanc.AspNetIdentity.AdoNet
{
    public class DbSqlParameterCollection : System.Collections.ObjectModel.Collection<DbSqlParameter>
    {
    }

    public class DbSqlParameter
    {
        public string Name { get; }
        public object Value { get; }
        public SqlDbType DbType { get; }
        public int Size { get; }
        public bool IsNullable { get; }


        public DbSqlParameter(string name, object value, SqlDbType dbType)
        {
            this.Name = name;
            this.Value = value;
            this.DbType = dbType;
        }

        public DbSqlParameter(string name, object value, SqlDbType dbType, int size)
        {
            this.Name = name;
            this.Value = value;
            this.DbType = dbType;
            this.Size = size;
        }

        public DbSqlParameter(string name, object value, SqlDbType dbType, bool isNullable)
        {
            this.Name = name;
            this.Value = value;
            this.DbType = dbType;
            this.IsNullable = isNullable;
        }

        public DbSqlParameter(string name, object value, SqlDbType dbType, int size, bool isNullable)
        {
            this.Name = name;
            this.Value = value;
            this.DbType = dbType;
            this.Size = size;
            this.IsNullable = isNullable;
        }

    }
}
