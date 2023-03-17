using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using DMS.Model;

namespace DMS.Data
{
    public class BasicReport_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        public DataSet GetBasicReportDetails(string Master, Int64 MasterID, Int64 UserID)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_GetBasicReportDetails", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("in_action", MySqlDbType.VarChar).Value = Master;
                cmd.Parameters.Add("in_masterid", MySqlDbType.Int64).Value = MasterID;
                cmd.Parameters.Add("in_userid", MySqlDbType.Int64).Value = UserID;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
        }

        public DataSet GetUploadedFiles(BasicReport_Model modelObj)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetUploadedFileReportNew", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_DocId", MySqlDbType.Int64).Value = modelObj.SubCateID;
                cmd.Parameters.Add("In_DocGrpID", MySqlDbType.Int64).Value = modelObj.CateID;
                cmd.Parameters.Add("In_DocAttDetails", MySqlDbType.String).Value = modelObj.Atr_Conditions;
                cmd.Parameters.Add("In_Atrloadtype", MySqlDbType.VarChar).Value = modelObj.Atr_load_action;
                cmd.Parameters.Add("In_From_Date", MySqlDbType.VarChar).Value = modelObj.FromDate;
                cmd.Parameters.Add("In_To_Date", MySqlDbType.VarChar).Value = modelObj.Todate;
                cmd.Parameters.Add("In_Doc_Stat", MySqlDbType.VarChar).Value = modelObj.Docstat;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
                //return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
        }

        public DataTable GetAttributes(BasicReport_Model modelObj)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_Get_UserAttributes", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.String).Value = modelObj.inAction;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int64).Value = modelObj.SubCateID;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int64).Value = modelObj.CateID;
             //   cmd.Parameters.Add("In_UnitID", MySqlDbType.Int64).Value = modelObj.UnitID;
                cmd.Parameters.Add("In_LovID", MySqlDbType.Int64).Value = modelObj.DeptID;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
        }

        public DataSet GetIndexedFileDetails(BasicReport_Model modelObj)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_DocumentList", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_DocId", MySqlDbType.Int64).Value = modelObj.SubCateID;
                cmd.Parameters.Add("In_DocGrpID", MySqlDbType.Int64).Value = modelObj.CateID;
                cmd.Parameters.Add("In_DocAttDetails", MySqlDbType.String).Value = modelObj.Atr_Conditions;
                cmd.Parameters.Add("In_Atrloadtype", MySqlDbType.VarChar).Value = modelObj.Atr_load_action;
                cmd.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = modelObj.activeflag;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
        }

        public DataSet GetIndexedDocumentDetails(BasicReport_Model modelObj)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("pr_get_DocumentSearch", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_DocId", MySqlDbType.Int64).Value = modelObj.SubCateID;
                cmd.Parameters.Add("In_DocGrpID", MySqlDbType.Int64).Value = modelObj.CateID;
                cmd.Parameters.Add("In_DocAttDetails", MySqlDbType.String).Value = modelObj.Atr_Conditions;
                cmd.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = modelObj.activeflag;
                //cmd.Parameters.Add("In_Atrloadtype", MySqlDbType.VarChar).Value = modelObj.Atr_load_action;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
        }
    }
}
