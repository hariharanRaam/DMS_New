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
    public class ApproveEditedIndexedDoc_Data
    {
        //MySql Connection String.
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        public DataSet Initiatecontrols(int? id,string mode)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Sp_Getapprovevalues", con);
                cmd.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = id;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = mode;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }
        public DataSet Initiatedeletecontrols(int? id)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Sp_Getdeleteapprovevalues", con);
                cmd.Parameters.Add("In_AttribId", MySqlDbType.VarChar).Value = id;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public Int32 approvetoupdatefile(int? id, string Mode)
        {
            try
            {
                int res = 0;
                con.Open();
                if (Mode == "E")
                {
                    DataTable dt = new DataTable();
                    MySqlCommand cmd = new MySqlCommand("Sp_GetneedtoUpdateRecords", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = id;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            MySqlCommand cmd1 = new MySqlCommand("Sp_UpdateIndexedFiles", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Approve";
                            cmd1.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = Convert.ToInt32(dt.Rows[i]["Attrib_Id"].ToString());
                            cmd1.Parameters.Add("In_AttribdtlId", MySqlDbType.Int32).Value = Convert.ToInt32(dt.Rows[i]["Attribdtl_Id"].ToString());
                            cmd1.Parameters.Add("In_AttribDesc", MySqlDbType.VarChar).Value = dt.Rows[i]["EAttribdtl_Description"].ToString();
                            res = cmd1.ExecuteNonQuery();
                        }
                    }
                }
                if (Mode == "D")
                {
                    MySqlCommand cmd2 = new MySqlCommand("SP_ApproveRejectDeletedIndexedDoc", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = id;
                    cmd2.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Approve";
                    res = cmd2.ExecuteNonQuery();
                    if (res == 1)
                    {
                        res = 2;
                    }
                }
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 Reject(int? id, string Mode)
        {
            try
            {
                int res = 0;
                 con.Open();
                 if (Mode == "E")
                 {
                     DataTable dt = new DataTable();
                     MySqlCommand cmd = new MySqlCommand("Sp_GetneedtoUpdateRecords", con);
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = id;
                     MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                     da.Fill(dt);
                     if (dt.Rows.Count > 0)
                     {
                         for (int i = 0; i < dt.Rows.Count; i++)
                         {
                             MySqlCommand cmd1 = new MySqlCommand("Sp_UpdateIndexedFiles", con);
                             cmd1.CommandType = CommandType.StoredProcedure;
                             cmd1.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Reject";
                             cmd1.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = Convert.ToInt32(dt.Rows[i]["Attrib_Id"].ToString());
                             cmd1.Parameters.Add("In_AttribdtlId", MySqlDbType.Int32).Value = Convert.ToInt32(dt.Rows[i]["Attribdtl_Id"].ToString());
                             cmd1.Parameters.Add("In_AttribDesc", MySqlDbType.VarChar).Value = dt.Rows[i]["EAttribdtl_Description"].ToString();
                             res = cmd1.ExecuteNonQuery();
                         }
                     }
                 }
                 if (Mode == "D")
                 {
                     MySqlCommand cmd2 = new MySqlCommand("SP_ApproveRejectDeletedIndexedDoc", con);
                     cmd2.CommandType = CommandType.StoredProcedure;
                     cmd2.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = id;
                     cmd2.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Reject";
                     res = cmd2.ExecuteNonQuery();
                     if (res == 1)
                     {
                         res = 2;
                     }
                 }
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
