using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using DMS.Model;

namespace DMS.Data
{
    public class ViewAttributeLOV_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataSet GetListView()
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_ViewLovattributes", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "grid";
                cmd.Parameters.Add("In_lovid", MySqlDbType.Int32).Value = 0;
                cmd.Parameters.Add("In_Lovexlid", MySqlDbType.Int32).Value = 0;
                cmd.Parameters.Add("In_Lovexlname", MySqlDbType.VarChar).Value = 0;
                cmd.Parameters.Add("In_UserId", MySqlDbType.Int32).Value = 0;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                Con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getlovvalues(int? lovid)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_ViewLovattributes", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "text";
                cmd.Parameters.Add("In_lovid", MySqlDbType.Int32).Value = lovid;
                cmd.Parameters.Add("In_Lovexlid", MySqlDbType.Int32).Value = 0;
                cmd.Parameters.Add("In_Lovexlname", MySqlDbType.VarChar).Value = 0;
                cmd.Parameters.Add("In_UserId", MySqlDbType.Int32).Value = 0;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Updatelov(ViewAttributeLOV_Model modelObj, List<ViewAttributeLOV_Model> lstobj)
        {
            try
            {
                int Result = 0;
                MySqlCommand cmd = new MySqlCommand("SP_ViewLovattributes", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "changeflag";
                cmd.Parameters.Add("In_lovid", MySqlDbType.Int32).Value = modelObj.masterid;
                cmd.Parameters.Add("In_Lovexlid", MySqlDbType.Int32).Value = 0;
                cmd.Parameters.Add("In_Lovexlname", MySqlDbType.VarChar).Value = 0;
                cmd.Parameters.Add("In_UserId", MySqlDbType.Int32).Value = 0;
                Con.Open();
                Result = cmd.ExecuteNonQuery();
                if (Result > 0)
                {
                    for (int i = 0; i < lstobj.Count; i++)
                    {
                        MySqlCommand cmd1 = new MySqlCommand("SP_ViewLovattributes", Con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "insert";
                        cmd1.Parameters.Add("In_lovid", MySqlDbType.Int32).Value = lstobj[i].masterid;
                        cmd1.Parameters.Add("In_Lovexlid", MySqlDbType.Int32).Value = lstobj[i].slno;
                        cmd1.Parameters.Add("In_Lovexlname", MySqlDbType.VarChar).Value = lstobj[i].lovtext;
                        cmd1.Parameters.Add("In_UserId", MySqlDbType.Int32).Value = modelObj.UserId;
                        Result = cmd1.ExecuteNonQuery();
                    }
                }
                Con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
