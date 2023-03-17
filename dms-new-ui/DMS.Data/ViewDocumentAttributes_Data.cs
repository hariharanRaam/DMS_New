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
    public class ViewDocumentAttributes_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataSet getdynamicattributes()
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("SP_GetDynamicMastersName", con);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable Getattributes(int GroupAtrID)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_Getattributevalues", con);
            cmd.Parameters.Add("In_Atr_ID", MySqlDbType.Int32).Value = GroupAtrID;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public DataTable Getattribute(Int64 Attrbid, Int64 Dognameid, Int64 _empid)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_GetEditattributevalue", con);
                cmd.Parameters.Add("In_action", MySqlDbType.String).Value = "GetAttributes";
                cmd.Parameters.Add("In_Atr_ID", MySqlDbType.Int64).Value = Attrbid;
               // cmd.Parameters.Add("In_DeptID", MySqlDbType.Int64).Value = 0;
              //  cmd.Parameters.Add("In_UnitID", MySqlDbType.Int64).Value = 0;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int64).Value = 0;
                cmd.Parameters.Add("In_DNameID", MySqlDbType.Int64).Value = 0;
                cmd.Parameters.Add("In_dynamictxt", MySqlDbType.String).Value = "0";
                cmd.Parameters.Add("In_dynamictxtlov", MySqlDbType.String).Value = "0";
            //    cmd.Parameters.Add("In_dynamictxtautonumber", MySqlDbType.String).Value = "0";
                //cmd.Parameters.Add("In_dynamictxtupdate", MySqlDbType.String).Value = "0";
                //cmd.Parameters.Add("In_dynamictxtlovupdate", MySqlDbType.String).Value = "0";
                cmd.Parameters.Add("In_empid", MySqlDbType.Int64).Value = _empid;
                cmd.Parameters.Add("In_docnamegid", MySqlDbType.Int64).Value = Dognameid;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return dt;
        }


        public DataTable Setattribute(Int64 DgroupID, Int64 DNameID, string dynamictxt, string dynamictxtlov, Int64 _empid)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_GetEditattributevalue", con);
                con.Open();
                cmd.Parameters.Add("In_action", MySqlDbType.String).Value = "SetAttributes";
                cmd.Parameters.Add("In_Atr_ID", MySqlDbType.Int64).Value = 0;
               // cmd.Parameters.Add("In_DeptID", MySqlDbType.Int64).Value = DeptID;
               // cmd.Parameters.Add("In_UnitID", MySqlDbType.Int64).Value = UnitID;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int64).Value = DgroupID;
                cmd.Parameters.Add("In_DNameID", MySqlDbType.Int64).Value = DNameID;
                cmd.Parameters.Add("In_dynamictxt", MySqlDbType.String).Value = dynamictxt;
                cmd.Parameters.Add("In_dynamictxtlov", MySqlDbType.String).Value = dynamictxtlov;
               // cmd.Parameters.Add("In_dynamictxtautonumber", MySqlDbType.String).Value = dynamictxtautonumber;
                //cmd.Parameters.Add("In_dynamictxtupdate", MySqlDbType.String).Value = dynamictxtupdate;
                //cmd.Parameters.Add("In_dynamictxtlovupdate", MySqlDbType.String).Value = dynamictxtlovupdate;
                cmd.Parameters.Add("In_empid", MySqlDbType.Int64).Value = _empid;
                cmd.Parameters.Add("In_docnamegid", MySqlDbType.Int64).Value = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return dt;
        }

        public DataTable Geteditattribute(Int64 docnameId, Int64 docgrpId, Int64 unitId, Int64 depId, Int64 docnameId1, Int64 _empid)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_GetEditattributevalue", con);
                cmd.Parameters.Add("In_action", MySqlDbType.String).Value = "GetAttributes";
                cmd.Parameters.Add("In_Atr_ID", MySqlDbType.Int64).Value = 0;
                cmd.Parameters.Add("In_DeptID", MySqlDbType.Int64).Value = depId;
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int64).Value = unitId;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int64).Value = docgrpId;
                cmd.Parameters.Add("In_DNameID", MySqlDbType.Int64).Value = docnameId;
                cmd.Parameters.Add("In_dynamictxt", MySqlDbType.String).Value = 0;
                cmd.Parameters.Add("In_dynamictxtlov", MySqlDbType.String).Value = 0;
           //     cmd.Parameters.Add("In_dynamictxtautonumber", MySqlDbType.String).Value = 0;
                //cmd.Parameters.Add("In_dynamictxtupdate", MySqlDbType.String).Value = dynamictxtupdate;
                //cmd.Parameters.Add("In_dynamictxtlovupdate", MySqlDbType.String).Value = dynamictxtlovupdate;
                cmd.Parameters.Add("In_empid", MySqlDbType.Int64).Value = _empid;
                cmd.Parameters.Add("In_docnamegid", MySqlDbType.Int64).Value = docnameId1;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
    }
}
