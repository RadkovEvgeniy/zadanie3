using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace zadanie3
{
    internal class DB_Connection
    {
        SqlConnection SqlConnection = new SqlConnection(@"Data Source = ZHEKA-PC\SQLEXPRESS; Initial Catalog = AZS_New; Integrated Security = True");

        public void OpenConnection()
        {
            if(SqlConnection.State == System.Data.ConnectionState.Closed)
            {
                SqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if(SqlConnection.State == System.Data.ConnectionState.Open)
            {
                SqlConnection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return SqlConnection;
        }
    }
}
