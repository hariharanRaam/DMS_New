using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.IO;
using System.Web;
using DMS.Model;

namespace DMS.Data
{

    public class SetDocAttributes_Data
    {
        //MySql Connection String.
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        public DataSet GetDeptCatgLst(int RefId, string RefType)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("Ref_ID", RefId);
                values.Add("Ref_TYPE", RefType);
                ds = RunProc("FindDropDown", values);
                //con.RunProc("FindDropDown", values);

                return ds;
            }
            catch (Exception ex)
            {

                throw (ex);

            }
        }

        public DataSet InitValues(int id)
        {
            DataSet ds = new DataSet();
            try
            {

                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("In_DocArchId", id);
                ds = RunProc("SP_Initialattributes", values);
                //con.RunProc("FindDropDown", values);

                return ds;
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public DataSet RunProc(string SpName, Dictionary<string, Object> values = null)
        {

            try
            {
                MySqlCommand cmd = new MySqlCommand(SpName, con);
                DataSet ds = new DataSet();
                //cmd.CommandText = command;
                cmd.CommandType = CommandType.StoredProcedure;
                if (values != null)
                {
                    foreach (string key in values.Keys)
                    {
                        cmd.Parameters.AddWithValue(key, values[key]);
                    }
                }
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                //con.Open();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int SaveDetails(SetDocAttributes_Model deptsModel, List<SetDocAttributes_Model> ModelObjList1)
        {
            try
            {
                int Result = 0;
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_SetFileAttribute", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_DocArchId", MySqlDbType.Int32).Value = deptsModel.HideDocArchId;
                cmd.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = deptsModel.FileName;
                cmd.Parameters.Add("In_FileType", MySqlDbType.VarChar).Value = deptsModel.FileType;
                cmd.Parameters.Add("In_VersionName", MySqlDbType.VarChar).Value = deptsModel.VersionName;
                cmd.Parameters.Add("In_VersionDate", MySqlDbType.Date).Value = deptsModel.VersionDate;
                cmd.Parameters.Add("In_Signature", MySqlDbType.VarChar).Value = deptsModel.Signature;
                cmd.Parameters.Add("In_FilePath", MySqlDbType.VarChar).Value = deptsModel.Filepathto;
                cmd.Parameters.Add("In_UserId", MySqlDbType.Int32).Value = ModelObjList1[0].UserId;
                cmd.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = 'Y';
                Result = cmd.ExecuteNonQuery();

                if (Result == 1)
                {
                    for (int i = 0; i < ModelObjList1.Count; i++)
                    {
                        MySqlCommand cmd1 = new MySqlCommand("SP_SetFileAttributeDtl", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add("In_DocId", MySqlDbType.Int32).Value = deptsModel.HideDocArchId;
                        cmd1.Parameters.Add("In_AtrName", MySqlDbType.VarChar).Value = ModelObjList1[i].AtrLabel1;
                        cmd1.Parameters.Add("In_AtrLength", MySqlDbType.VarChar).Value = ModelObjList1[i].AtrLen;
                        cmd1.Parameters.Add("In_AtrType", MySqlDbType.VarChar).Value = ModelObjList1[i].AtrType;
                        cmd1.Parameters.Add("In_AtrMandotry", MySqlDbType.VarChar).Value = ModelObjList1[i].AtrMand;
                        cmd1.Parameters.Add("In_AtrLovId", MySqlDbType.Int32).Value = ModelObjList1[i].AtrLovId;
                        if (ModelObjList1[i].AtrType=="Datetime")
                        { cmd1.Parameters.Add("Atr_Description", MySqlDbType.VarChar).Value = ModelObjList1[i].AtrText1.Substring(0,10); }
                        else
                        { 
                        cmd1.Parameters.Add("Atr_Description", MySqlDbType.VarChar).Value = ModelObjList1[i].AtrText1;
                        }
                        cmd1.Parameters.Add("In_UserId", MySqlDbType.Int32).Value = ModelObjList1[i].UserId;
                        Result = cmd1.ExecuteNonQuery();
                    }

                }

                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public int UpdateProperties(SetDocAttributes_Model deptsModel, List<SetDocAttributes_Model> ModelObjList1)
        {
            try
            {

                int Result = 0;
                con.Open();
                for (int i = 0; i < ModelObjList1.Count; i++)
                {
                    MySqlCommand cmd1 = new MySqlCommand("SP_EditFileAttributeDtl", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = deptsModel.HideDocArchId;
                    cmd1.Parameters.Add("In_AtrName", MySqlDbType.VarChar).Value = ModelObjList1[i].AtrLabel1;
                    cmd1.Parameters.Add("In_AtrDescription", MySqlDbType.VarChar).Value = ModelObjList1[i].AtrText1;
                    cmd1.Parameters.Add("In_UserId", MySqlDbType.Int32).Value = ModelObjList1[i].UserId;
                    Result = cmd1.ExecuteNonQuery();
                }
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable getmastervalue(int LovId, string type)
        {
            DataTable dt = new DataTable();
            con.Open();
            MySqlCommand cmd = new MySqlCommand("Getlovmastervalues", con);
            // MySqlCommand cmd = new MySqlCommand("SP_GetDocLinkGrid1Datas", con);
            cmd.Parameters.Add("In_LovId", MySqlDbType.Int32).Value = LovId;
            cmd.Parameters.Add("In_LovType", MySqlDbType.VarChar).Value = type;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public DataSet Initeditvalues(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("In_AttribId", id);
                ds = RunProc("SP_EditInitialattributes", values);
                return ds;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public int Deletefile(SetDocAttributes_Model deptsModel)
        {
            try
            {

                int Result = 0;
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SP_DeleteIndexedDoc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = deptsModel.HideDocArchId;
                Result = cmd.ExecuteNonQuery();
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //public DataSet storagedetail(int? attribgid)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        MySqlCommand cmd = new MySqlCommand("SP_GetdynamicStorageattributes", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //       // cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = deptid;
        //       // cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = unit;
        //       // cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = docgroup;
        //      //  cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = docname;
        //       // cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = Aaction;
        //        cmd.Parameters.Add("In_Attribgid", MySqlDbType.Int32).Value = attribgid;
        //        con.Open();
        //        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        con.Close();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        public DataTable ValidateAttributes(SetDocAttributes_Model deptsModel, List<SetDocAttributes_Model> ModelObjList1)
        {
            DataTable dt2 = new DataTable();
            try
            {
                String _wherecondition = "";
                for (int i = 0; i < ModelObjList1.Count; i++)
                {
                    if (ModelObjList1.Count == i + 1)
                    {
                        _wherecondition += "`" + ModelObjList1[i].AtrLabel1.Trim() + "`" + "=" + "'" + ModelObjList1[i].AtrText1.Trim() + "'";
                    }
                    else
                    {
                        _wherecondition += "`" + ModelObjList1[i].AtrLabel1.Trim() + "`" + "=" + "'" + ModelObjList1[i].AtrText1.Trim() + "'" + " " + "AND" + " ";
                    }
                }
                con.Open();
                MySqlCommand cmd2 = new MySqlCommand("SP_Validate_Indexing_Attributes", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add("_attribid", MySqlDbType.Int32).Value = deptsModel.HideDocArchId;
                cmd2.Parameters.Add("_condition", MySqlDbType.VarChar).Value = _wherecondition;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd2);
                cmd2.CommandType = CommandType.StoredProcedure;
                da.Fill(dt2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return dt2;
        }


        public DataTable Check_Document_Under_Modification(string Doc_GID)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_Get_Document_Modified_Details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Doc_GID", MySqlDbType.VarChar).Value = Doc_GID;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                con.Open();
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
