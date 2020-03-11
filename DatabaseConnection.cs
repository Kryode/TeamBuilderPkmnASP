using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace TeamBuilderPkmnASP
{
    public static class DatabaseConnection
    {
        private static SqlConnection _connection = null;

        public static SqlConnection Connection
        {
            get
            {
                return GetConnection();
            }

            set { _connection = value; }
        }

        private static SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = Environment.GetEnvironmentVariable("dbadress"),
                    InitialCatalog = Environment.GetEnvironmentVariable("dbname"),
                    UserID = Environment.GetEnvironmentVariable("dbusername"),
                    Password = Environment.GetEnvironmentVariable("dbpassword")
                };
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    //connection.Close();
                    _connection = connection;
                }
                else
                {
                    throw new Exception("Error, connection to database could not be established");
                }
            }
            return _connection;
        }

        public static object[][] GetDatas(string query)
        {
            try
            {
                List<object[]> datas = new List<object[]>();
                SqlCommand commandQuery = new SqlCommand(query, Connection);
                Connection.Open();
                SqlDataReader reader = commandQuery.ExecuteReader();
                
                while (reader.Read())
                {
                    List<object> row = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetValue(i));
                    }
                    datas.Add(row.ToArray());
                }
                Connection.Close();
                return datas.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
