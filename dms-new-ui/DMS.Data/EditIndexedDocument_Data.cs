using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace DMS.Data
{
    public class EditIndexedDocument_Data
    {
        //ConnectionString
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataSet getindexedrecodrs()
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("Sp_GetEditIndexedDocuments", con);
                cmd.CommandType=CommandType.StoredProcedure;
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
        public DataSet Getindexedrecordsforapproval()
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("Sp_GetForApprovalEditedIndexedDocs", con);
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

        //public DataSet approvetoupdatefile(int? id)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        DataTable dt = new DataTable();
        //        //DataTable dt1 = new DataTable();
        //        MySqlCommand cmd = new MySqlCommand("Sp_GetneedtoUpdateRecords", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = id;
        //        con.Open();
        //        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        int count =dt.Rows.Count;
        //        int res;
        //        if (count > 0)
        //        {
        //            for (int i = 0; i < count; i++)
        //            {
        //                //dt1.Rows.Clear();
        //                int AttributeId =Convert.ToInt32(dt.Rows[i]["Attrib_Id"].ToString());
        //                int AttributedtlId = Convert.ToInt32(dt.Rows[i]["Attribdtl_Id"].ToString());
        //                string AttributeDesc = dt.Rows[i]["EAttribdtl_Description"].ToString();
        //                MySqlCommand cmd1 = new MySqlCommand("Sp_UpdateIndexedFiles", con);
        //                cmd1.CommandType = CommandType.StoredProcedure;
        //                cmd1.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Approve";
        //                cmd1.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value =AttributeId ;
        //                cmd1.Parameters.Add("In_AttribdtlId", MySqlDbType.Int32).Value = AttributedtlId;
        //                cmd1.Parameters.Add("In_AttribDesc", MySqlDbType.VarChar).Value = AttributeDesc;
        //                res = cmd1.ExecuteNonQuery();  
        //            }
        //            MySqlCommand cmd2 = new MySqlCommand("Sp_GetForApprovalEditedIndexedDocs", con);
        //            cmd2.CommandType = CommandType.StoredProcedure;
        //            MySqlDataAdapter da2 = new MySqlDataAdapter(cmd2);
        //            da2.Fill(ds); 
        //        }
        //        con.Close();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataSet Reject(int? id)
        //{
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    //DataTable dt1 = new DataTable();
        //    MySqlCommand cmd = new MySqlCommand("Sp_GetneedtoUpdateRecords", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = id;
        //    con.Open();
        //    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    int count = dt.Rows.Count;
        //    int res;
        //    if (count > 0)
        //    {
        //        for (int i = 0; i < count; i++)
        //        {
        //            int AttributeId = Convert.ToInt32(dt.Rows[i]["Attrib_Id"].ToString());
        //            int AttributedtlId = Convert.ToInt32(dt.Rows[i]["Attribdtl_Id"].ToString());
        //            string AttributeDesc = dt.Rows[i]["EAttribdtl_Description"].ToString();
        //            MySqlCommand cmd1 = new MySqlCommand("Sp_UpdateIndexedFiles", con);
        //            cmd1.CommandType = CommandType.StoredProcedure;
        //            cmd1.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Reject";
        //            cmd1.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = AttributeId;
        //            cmd1.Parameters.Add("In_AttribdtlId", MySqlDbType.Int32).Value = AttributedtlId;
        //            cmd1.Parameters.Add("In_AttribDesc", MySqlDbType.VarChar).Value = AttributeDesc;                    
        //            res = cmd1.ExecuteNonQuery();
        //        }
        //        MySqlCommand cmd2 = new MySqlCommand("Sp_GetForApprovalEditedIndexedDocs", con);
        //        cmd2.CommandType = CommandType.StoredProcedure;
        //        MySqlDataAdapter da2 = new MySqlDataAdapter(cmd2);
        //        da2.Fill(ds); 
        //    }
        //    con.Close();
        //    return ds;   
        //}

    }
}
