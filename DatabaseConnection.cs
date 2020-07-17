using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using SendGrid;

namespace TeamBuilderPkmnASP
{
    public static class DatabaseConnection
    {
        private static SqlConnection _connection = null;
        private static SendGridClient _mailClient = null;

        public static SqlConnection Connection
        {
            get
            {
                return GetConnection();
            }
        }

        public static SendGridClient MailClient
        {
            get
            {
                return GetMailClient();
            }
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
                builder.MultipleActiveResultSets = true;
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    _connection = connection;
                }
                else
                {
                    throw new Exception("Error, connection to database could not be established");
                }
            }
            return _connection;
        }

        public static SendGridClient GetMailClient()
        {
            if (_mailClient == null)
            {
                string api_key = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                _mailClient = new SendGridClient(api_key);
            }
            return _mailClient;
        }
    }
}
