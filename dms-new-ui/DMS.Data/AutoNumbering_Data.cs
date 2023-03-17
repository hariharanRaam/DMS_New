using DMS.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Data
{
    public class AutoNumbering_Data
    {
            MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        //read the data
            public DataSet getAllautonumbering()
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                try
                {
                    Con.Open();
                    MySqlCommand cmd = new MySqlCommand("SP_GetAllAutonumberingData", Con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                    Con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Con.Close();
                }
                return ds;
            }


            public AutoNumbering_Model AutoNumberIud(AutoNumbering_Model autonumbermodel)
            {
                try
                {
                    DataTable dt = new DataTable();
                    MySqlCommand cmd = new MySqlCommand("SP_AutonumberingSaveUpdateDelete", Con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = autonumbermodel.mode;
                    cmd.Parameters.Add("In_Autonumber_rowid", MySqlDbType.Int32).Value = autonumbermodel.autonumber_rowid;
                    cmd.Parameters.Add("In_Autonumber_prefix", MySqlDbType.VarChar).Value = autonumbermodel.autonumber_prefix;
                    cmd.Parameters.Add("In_Autonumber_year", MySqlDbType.VarChar).Value = autonumbermodel.autonumber_year;
                    cmd.Parameters.Add("In_Autonumber_month", MySqlDbType.VarChar).Value = autonumbermodel.autonumber_month;
                    cmd.Parameters.Add("In_Autonumber_date", MySqlDbType.VarChar).Value = autonumbermodel.autonumber_date;
                    cmd.Parameters.Add("In_Autonumber_sequence", MySqlDbType.VarChar).Value = autonumbermodel.autonumber_sequence;
                    cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = autonumbermodel.createdby;
                    Con.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        autonumbermodel.resultcode = dr["result"].ToString();
                        autonumbermodel.resultstatus = dr["resultstat"].ToString();
                    }
                }
                catch (Exception er)
                {
                    autonumbermodel.resultstatus = er.ToString();
                }
                finally {
                    Con.Close();
                }

                return autonumbermodel;
            }


    }
}
