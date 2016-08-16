using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;

namespace Itanc.AspNetIdentity.AdoNet
{
    public class SqlServerDatabase : IDisposable
    {
        private SqlConnection _connection;
        private readonly string _connectionStringr;

        public SqlServerDatabase(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connectionStringr = connectionString;
          //  SelectCommand = new SqlCommand();
        }


        #region ADO.NET wrapper methods

        /// <summary>
        ///     Executes a T-SQL query in the database and returns the number of rows affected.
        /// </summary>
        /// <param name="sqlQuery">The T-SQL statement to execute.</param>
        /// <param name="dbSqlParameters">The parameters of the T-SQL query.</param>
        /// <returns>The number of rows affected by the T-SQL query.</returns>
        /// 
        public int ExecuteNonQuery(string sqlQuery, DbSqlParameterCollection dbSqlParameters)
        {

            if (string.IsNullOrEmpty(sqlQuery))
            {
                throw new ArgumentException("The T-SQL command cannot be null or empty.");
            }

            int result;

            var selectCommand = new SqlCommand
            {
                CommandText = sqlQuery,
                Connection = _connection,
                CommandType = CommandType.Text
            };


            // it can reRewrite all prameters in SelectCommand.Parameters
            if (dbSqlParameters != null)
            {
                for (var i = 0; i <= dbSqlParameters.Count - 1; i++)
                {
                    selectCommand.Parameters.AddWithValue(dbSqlParameters[i].Name,dbSqlParameters[i].Value ?? DBNull.Value);
                    selectCommand.Parameters[i].SqlDbType = dbSqlParameters[i].DbType;
                    selectCommand.Parameters[i].Size = dbSqlParameters[i].Size;
                    selectCommand.Parameters[i].IsNullable = dbSqlParameters[i].IsNullable;

                    
                }
            }

            try
            {
                OpenConnection();
                result = selectCommand.ExecuteNonQuery();
            }
            catch (SqlTypeException ex)
            {
                throw new Exception("An error occured in ExecuteNonQuery: " + ex.Message);
            }

            catch (SystemException ex)
            {
                throw new Exception("An error occured in ExecuteNonQuery: " + ex.Message);
            }

            finally
            {
                selectCommand.Dispose();
                ClosedConnection();
            }

            return result;
        }

        /// <summary>
        ///   Executes a T-SQL query and returns the results as a SqlDataReader object.
        /// </summary>
        /// <param name="sqlQuery">The T-SQL statement to execute.</param>
        /// <param name="dbSqlParameters">The parameters of the T-SQL query.</param>
        /// <returns>The number of rows affected by the T-SQL query.</returns>
        /// 
        public object ExecuteScalar(string sqlQuery, DbSqlParameterCollection dbSqlParameters)
        {
            if (string.IsNullOrEmpty(sqlQuery))
            {
                throw new ArgumentException("The T-SQL command cannot be null or empty.");
            }

            object result;

            var selectCommand = new SqlCommand
            {
                CommandText = sqlQuery,
                Connection = _connection,
                CommandType = CommandType.Text
            };


            // it can reRewrite all prameters in SelectCommand.Parameters
            if (dbSqlParameters != null)
            {
                for (var i = 0; i <= dbSqlParameters.Count - 1; i++)
                {
                    selectCommand.Parameters.AddWithValue(dbSqlParameters[i].Name, dbSqlParameters[i].Value ?? DBNull.Value);
                    selectCommand.Parameters[i].SqlDbType = dbSqlParameters[i].DbType;
                    selectCommand.Parameters[i].Size = dbSqlParameters[i].Size;
                    selectCommand.Parameters[i].IsNullable = dbSqlParameters[i].IsNullable;
                }
            }

            try
            {
                OpenConnection();
                result = selectCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured in ExecuteScalar: " + ex.Message);
            }
            finally
            {
                selectCommand.Dispose();
                ClosedConnection();
            }

            return result;
        }

        public object ExecuteReaderSingleResult(string sqlQuery, DbSqlParameterCollection dbSqlParameters)
        {
            object singleResult = null;

            var selectCommand = new SqlCommand
            {
                CommandText = sqlQuery,
                Connection = _connection,
                CommandType = CommandType.Text
            };

            if (dbSqlParameters != null)
            {
                for (var i = 0; i <= dbSqlParameters.Count - 1; i++)
                {
                    selectCommand.Parameters.AddWithValue(dbSqlParameters[i].Name, dbSqlParameters[i].Value ?? DBNull.Value);
                    selectCommand.Parameters[i].SqlDbType = dbSqlParameters[i].DbType;
                    selectCommand.Parameters[i].Size = dbSqlParameters[i].Size;
                    selectCommand.Parameters[i].IsNullable = dbSqlParameters[i].IsNullable;
                }
            }

            try
            {
                OpenConnection();
                var myDataReader = selectCommand.ExecuteReader(CommandBehavior.SingleResult);

                while (myDataReader.Read())
                {
                    singleResult = myDataReader[0];
                }
            }

            catch (SqlException ex)
            {
                throw new Exception("An error occured in ExecuteReaderSingleResult: " + ex.Message);
            }

            finally
            {
                selectCommand.Dispose();
                ClosedConnection();
            }

            return singleResult;
    }


        /// <summary>
        /// Create Custom DataTable by Just Sql Query
        /// </summary>
        /// <returns>Data Table</returns>
        public DataTable GetTable(string sqlQuery, DbSqlParameterCollection dbSqlParameters)
        {

            var dt = new DataTable();
            var da = new SqlDataAdapter(sqlQuery, _connection);

            //Manage Parameter for DbManager
            if (dbSqlParameters != null)
            {
                for (var i = 0; i <= dbSqlParameters.Count - 1; i++)
                {
                    da.SelectCommand.Parameters.AddWithValue(dbSqlParameters[i].Name, dbSqlParameters[i].Value ?? DBNull.Value);
                    da.SelectCommand.Parameters[i].SqlDbType = dbSqlParameters[i].DbType;
                    da.SelectCommand.Parameters[i].Size = dbSqlParameters[i].Size;
                    da.SelectCommand.Parameters[i].IsNullable = dbSqlParameters[i].IsNullable;
                }
            }

            using (da)
            {
                da.Fill(dt);
                da.SelectCommand.Dispose();
                da.Dispose();
            }

            return dt;
        }

        #endregion


        private void OpenConnection()
        {
            var numberOfTries = 3;

            if (_connection.State == ConnectionState.Open)
            {
                return;
            }

            while (numberOfTries >= 0 && _connection.State != ConnectionState.Open)
            {
                _connection.Open();
                numberOfTries--;
                Thread.Sleep(30);
            }
        }

        private void ClosedConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public void Dispose()
        {
            if (_connection == null)
            {
                return;
            }

            _connection.Dispose();
            _connection = null;
        }
    }
}