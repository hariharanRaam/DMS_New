using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace DMS.Data
{
    public class GetAllDocuments_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        public DataSet getdocuments(Int64 empid, string activeflag, string docname, string docgroup, string submission, string status)
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("Sp_GetDocArchival", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("in_userid", MySqlDbType.Int64).Value = empid;
                cmd.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = activeflag;
                cmd.Parameters.Add("in_dname_id", MySqlDbType.Int64).Value = Convert.ToInt64(docname);
                cmd.Parameters.Add("in_dgroup_id", MySqlDbType.Int64).Value = Convert.ToInt64(docgroup);
                cmd.Parameters.Add("In_submission_date", MySqlDbType.VarChar).Value = submission;
                cmd.Parameters.Add("In_status_flag", MySqlDbType.VarChar).Value = status;
                con.Open();
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
