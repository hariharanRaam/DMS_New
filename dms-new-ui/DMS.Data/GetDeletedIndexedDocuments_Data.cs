using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DMS.Data
{
   public class GetDeletedIndexedDocuments_Data
    {
       //connection string.
       MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
       public DataSet Getdeletedindexeddocs()
       {
           DataSet ds = new DataSet();
           try
           {
               con.Open();
               MySqlCommand cmd = new MySqlCommand("SP_GetDeletedIndexedDocToApproveReject", con);
               cmd.CommandType = CommandType.StoredProcedure;
               MySqlDataAdapter da = new MySqlDataAdapter(cmd);
               da.Fill(ds);
               con.Close();
               return ds;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

    }
}
