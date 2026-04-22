using System.Configuration;
using System.Data.SqlClient;

namespace BTL_LapTrinhWeb
{
    public class DBHelper
    {
        public static SqlConnection GetConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
            return new SqlConnection(connStr);
        }
    }
}