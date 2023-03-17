
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Model;

namespace DMS.Data
{
    public class LOVMaster_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        public Int32 SaveLMaster(string lovname, int UserID)
        {
            try
            {
                //to store a textbox value in lovhdr
                int ret = 0;
                MySqlCommand cmd = new MySqlCommand("SP_LOVHDR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Lov_Name", MySqlDbType.VarChar).Value = lovname;
                cmd.Parameters.Add("In_userid", MySqlDbType.Int32).Value = UserID;
                con.Open();
                ret = cmd.ExecuteNonQuery();
                con.Close();
                return ret;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public Int32 SaveLodtl(List<LOVMaster_Model> lstmodel, int UserID)
        {
            int ret = 0;
            DataTable dt2 = new DataTable();
            try
            {
                //to store the excel file data inlovdtl
                for (int i = 0; i < lstmodel.Count; i++)
                {
                    MySqlCommand cmd = new MySqlCommand("SP_Lovdtl", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("In_excelId", MySqlDbType.Int32).Value = lstmodel[i].excelId;
                    cmd.Parameters.Add("In_excelName", MySqlDbType.VarChar).Value = lstmodel[i].excelName;
                    cmd.Parameters.Add("In_LovName", MySqlDbType.VarChar).Value = lstmodel[i].LovName;
                    cmd.Parameters.Add("Actionval", MySqlDbType.VarChar).Value = "Savedata";
                    cmd.Parameters.Add("In_userid", MySqlDbType.Int32).Value = UserID;
                    con.Open();
                    ret = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ret;
        }

        public DataSet GetFile(string Filename)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_Lovdtl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_LovName", MySqlDbType.VarChar).Value = Filename;
                cmd.Parameters.Add("In_excelId", MySqlDbType.Int32).Value = 0;
                cmd.Parameters.Add("In_excelName", MySqlDbType.VarChar).Value = "NULL";
                cmd.Parameters.Add("Actionval", MySqlDbType.VarChar).Value = "GetData";
                cmd.Parameters.Add("In_userid", MySqlDbType.Int32).Value = 0;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return ds;
        }

    }
}

