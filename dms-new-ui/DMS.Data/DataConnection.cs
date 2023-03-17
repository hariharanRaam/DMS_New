using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;

namespace DMS.Data
{
   public class DataConnection
    {
        public string ConnectionString;
        public string MyConnectionString;
        DataSet DSdataset;
        MySqlConnection MYSQLCON;
        MySqlDataAdapter daDataAdapter;
        MySqlCommand MySqlCmd;
        public string MySqlDataAccess()
        {
            string CString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            string ConStr = ConfigurationManager.ConnectionStrings[CString].ToString();
            ConnectionString = ConStr;
            MYSQLCON = new MySqlConnection(ConnectionString);
            return ConStr;
        }

    }
}
