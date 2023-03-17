using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using DMS.Model;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
//using Microsoft
namespace DMS.Data
{
    public class DocArchival_Data
    {

        public static class FileSizeFormatter
        {
            // Load all suffixes in an array  
            static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
            public static string FormatSize(Int64 bytes)
            {
                int counter = 0;
                decimal number = (decimal)bytes;
                while (Math.Round(number / 1024) >= 1)
                {
                    number = number / 1024;
                    counter++;
                }
                return string.Format("{0:n1}{1}", number, suffixes[counter]);
            }
        }  
        /***** Multiple document archival/Uploading Data method - START *****/
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public  int  SaveMultipleDocDetails(DocArchival_Model ModelObj, List<HttpPostedFileBase> FileUpload1)
        {
            int res = 0;
            try
            {
                DataTable dt = new DataTable();
                int Count = 0;
                foreach (HttpPostedFileBase file in FileUpload1)
                {
                    dt.Rows.Clear();
                    string filepath1 = "";
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        string filename = file.FileName;
                        string filenamenew = Path.GetFileNameWithoutExtension(file.FileName);
                        FileInfo fi = new FileInfo(file.FileName);
                        string fileextension = fi.Extension;
                        long filesize = file.ContentLength;
                        ModelObj.FileSize = FileSizeFormatter.FormatSize(file.ContentLength);
                        string filePath = ConfigurationManager.AppSettings["Path"].ToString();
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("Sp_CheckDocExistorNot", con);
                        cmd.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = filename;
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(dt);
                        Count = Convert.ToInt32(dt.Rows[0]["Count"].ToString());
                        if (Count > 0)
                        {
                            filenamenew = filenamenew + "(" + Count + ")" + fileextension;
                            filepath1 = Path.Combine(filePath, Path.GetFileName(filenamenew));
                            //using (var client = new HttpClient())
                            //{
                            //    var formcontent = new MultipartFormDataContent();
                            //    formcontent.Add(new StreamContent(file.InputStream), "files", filepath1);
                            //    string url = ConfigurationManager.AppSettings["ApiUrl"].ToString();
                            //    client.BaseAddress = new Uri(url);
                            //    HttpContent content = new StringContent(JsonConvert.SerializeObject(""), System.Text.UTF8Encoding.UTF8, "application/json");
                            //    content.Headers.Add("subDirectory", filepath1);
                            //    var response = client.PostAsync(url, formcontent);
                            //    int i = 0;
                            //    //deptsModel1.filedata = response.Result;

                            //}
                            file.SaveAs(filepath1);
                            res = 1;
                        }
                        else
                        {
                            filenamenew = filenamenew + "(" + Count + ")" + fileextension;
                            filepath1 = Path.Combine(filePath, Path.GetFileName(filenamenew));
                            string url = ConfigurationManager.AppSettings["ApiUrl"].ToString();
                            //using (var httpClient = new HttpClient())
                            //{
                            //    var form = new MultipartFormDataContent();
                            //    var fileName = Path.GetFileName(file.FileName);
                            //    MemoryStream target = new MemoryStream();
                            //    file.InputStream.CopyTo(target);
                            //    byte[] fileData = target.ToArray();
                             

                            //  //  file.InputStream.Read(fileData,0,file.ContentLength);


                            //    ByteArrayContent byteContent = new ByteArrayContent(fileData);

                            //    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                            //    form.Add(byteContent, "formFiles", file.FileName);

                            //    var result = httpClient.PostAsync(url, form).ConfigureAwait(false).GetAwaiter().GetResult().Content.ReadAsStringAsync().Result;
                            //} 
                           
                            file.SaveAs(filepath1);
                            res = 1;
                        }
                        if (res == 1)
                        {
                            MySqlCommand cmd1 = new MySqlCommand("SP_MultipleDocumentArchival", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = filename;
                            cmd1.Parameters.Add("In_FileVersion", MySqlDbType.VarChar).Value = Count;
                            cmd1.Parameters.Add("In_FileNameVersion", MySqlDbType.VarChar).Value = filenamenew;
                            cmd1.Parameters.Add("In_FilePath", MySqlDbType.VarChar).Value = filepath1;
                            cmd1.Parameters.Add("In_FileExtension", MySqlDbType.VarChar).Value = fileextension;
                            cmd1.Parameters.Add("In_FileType", MySqlDbType.VarChar).Value = fileextension;

                           // cmd1.Parameters.Add("In_Dep_id", MySqlDbType.VarChar).Value = ModelObj.DeptId;
                           // cmd1.Parameters.Add("In_Unit_id", MySqlDbType.VarChar).Value = ModelObj.UnitId;
                            cmd1.Parameters.Add("In_Dgroup_id", MySqlDbType.VarChar).Value = ModelObj.CatgId;
                            cmd1.Parameters.Add("In_Dname_id", MySqlDbType.VarChar).Value = ModelObj.SubCatgId;
                            cmd1.Parameters.Add("In_Doc_Arch_Created_By", MySqlDbType.Int32).Value = ModelObj.UserId;
                            cmd1.Parameters.Add("In_Doc_Arch_FileSize", MySqlDbType.VarChar).Value = ModelObj.FileSize;
                            cmd1.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = ModelObj.activeflag;
                            res = cmd1.ExecuteNonQuery();
                            con.Close();
                        }

                    }
                }
                return res;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /***** Multiple document archival/Uploading Data method - END *****/

        /***** Single document archival/Uploading Data method - START *****/
        public int SaveSingleDocDetails(DocArchival_Model ModelObj, HttpPostedFileBase file)
        {
            int Result = 0;
            try
            {
                DataTable dt = new DataTable();
                int Count = 0;
                string filename = file.FileName;
                string filenamenew = Path.GetFileNameWithoutExtension(file.FileName);
                FileInfo fi = new FileInfo(file.FileName);
                string fileextension = fi.Extension;
                string filePath = ConfigurationManager.AppSettings["Path"].ToString();
                string filepath1 = "";
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_CheckDocExistorNot", con);
                cmd.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = filename;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                Count = Convert.ToInt32(dt.Rows[0]["Count"].ToString());
                if (Count > 0)
                {
                    filenamenew = filenamenew + "(" + Count + ")" + fileextension;
                    filepath1 = Path.Combine(filePath, Path.GetFileName(filenamenew));
                    file.SaveAs(filepath1);
                    Result = 1;                  
                }
                else
                {
                    filenamenew = filenamenew + "(" + Count + ")" + fileextension;
                    filepath1 = Path.Combine(filePath, Path.GetFileName(filenamenew));
                    file.SaveAs(filepath1);
                    Result = 1;
                }
                if (Result == 1)
                {
                    MySqlCommand cmd2 = new MySqlCommand("SP_MultipleDocumentArchival", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = filename;
                    cmd2.Parameters.Add("In_FileVersion", MySqlDbType.VarChar).Value = Count;
                    cmd2.Parameters.Add("In_FileNameVersion", MySqlDbType.VarChar).Value = filenamenew;
                    cmd2.Parameters.Add("In_FilePath", MySqlDbType.VarChar).Value = filepath1;
                    cmd2.Parameters.Add("In_FileExtension", MySqlDbType.VarChar).Value = fileextension;
                    cmd2.Parameters.Add("In_FileType", MySqlDbType.VarChar).Value = fileextension;

                 //   cmd2.Parameters.Add("In_Dep_id", MySqlDbType.VarChar).Value = ModelObj.DeptId;
                 //   cmd2.Parameters.Add("In_Unit_id", MySqlDbType.VarChar).Value = ModelObj.UnitId;
                    cmd2.Parameters.Add("In_Dgroup_id", MySqlDbType.VarChar).Value = ModelObj.CatgId;
                    cmd2.Parameters.Add("In_Dname_id", MySqlDbType.VarChar).Value = ModelObj.SubCatgId;
                    cmd2.Parameters.Add("In_Doc_Arch_Created_By", MySqlDbType.Int32).Value = ModelObj.UserId;
                    cmd2.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = ModelObj.activeflag;
                    Result = cmd2.ExecuteNonQuery();
                    con.Close();
                }
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /***** Single document archival/Uploading Data method - END *****/

        public DataSet GetDropdown(string type, string actiontype,Int64 empid)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", actiontype);
                values.Add("p_Department", "0");
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", "0");
                values.Add("p_Docname", "0");
                values.Add("p_userid", empid);
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDropdownNew(string type, string condition_1, string condition_2, string condition_3, string condition_4, Int64 empid)
        {

            DataTable dtt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_SingFileddl_New", con);
                cmd.Parameters.Add("p_action", MySqlDbType.VarChar).Value = type;
                cmd.Parameters.Add("condition_1", MySqlDbType.Text).Value = condition_1;
                cmd.Parameters.Add("condition_2", MySqlDbType.Text).Value = condition_2;
                cmd.Parameters.Add("condition_3", MySqlDbType.Text).Value = condition_3;
                cmd.Parameters.Add("condition_4", MySqlDbType.Text).Value = condition_4;
                cmd.Parameters.Add("p_userid", MySqlDbType.Int64).Value = empid;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dtt);
                con.Close();
            }
            catch (Exception er) { 
            
            }
            return dtt;
        }

        public DataSet RunProcedure(Dictionary<string, Object> values = null)
        {

            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_SingFileddl", con);
                DataSet ds = new DataSet();

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

        public DataSet GetDeptCatgLst(int RefId, string RefType)
        {
            DataSet ds = new DataSet();
            try
            {

                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("Ref_ID", RefId);
                values.Add("Ref_TYPE", RefType);
                ds = RunProc(values); 
                //con.RunProc("FindDropDown", values);

                return ds;
            }
            catch (Exception ex)
            {

                throw (ex);

            }
        }

        public DataSet RunProc(Dictionary<string, Object> values = null)
        {

            try
            {
                MySqlCommand cmd = new MySqlCommand("FindDropDown", con);
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

        public DataSet GetDepartmentChange(Int64 Department_id)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", "GetDept");
                values.Add("p_Department", Department_id);
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", "0");
                values.Add("p_Docname", "0");
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUnitChange(Int64 Unit_Id)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", "GetUnit");
                values.Add("p_Department", "0");
                values.Add("p_Unit", Unit_Id);
                values.Add("p_Docgroup", "0");
                values.Add("p_Docname", "0");
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDocGroupChange(Int64 DocGroup_Id, Int64 empid)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", "GetDocGroup");
                values.Add("p_Department", "0");
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", DocGroup_Id);
                values.Add("p_Docname", "0");
                values.Add("p_userid", empid);
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDocNameChange(Int64 DocNameId,Int64 empid)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", "GetDocName");
                values.Add("p_Department", "0");
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", "0");
                values.Add("p_Docname", DocNameId);
                values.Add("p_userid", empid);
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DeleteScannedFile(Int64? id, string mode, DocArchival_Model ModelObj)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_DeleteScannedDocuments", con);
                cmd.Parameters.Add("In_DocID", MySqlDbType.Int64).Value = id;
                cmd.Parameters.Add("In_Modifiedby", MySqlDbType.Int32).Value = ModelObj.UserId;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetselectDropdown(int? docid, string type)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_GetScannedFile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("In_DocID", MySqlDbType.Int32).Value = docid;
            cmd.Parameters.Add("In_Type", MySqlDbType.VarChar).Value = type;
            con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public int EditScannedDocwithfile(DocArchival_Model ModelObj, HttpPostedFileBase file)
        {
            int Result = 0;
            try
            {
                con.Open();
                DataTable deleteDT = new DataTable();
                MySqlCommand cmd4 = new MySqlCommand("SP_GetDeleteDocName", con);
                cmd4.Parameters.Add("In_DocId", MySqlDbType.Int32).Value = ModelObj.FileID;
                MySqlDataAdapter da4 = new MySqlDataAdapter(cmd4);
                cmd4.CommandType = CommandType.StoredProcedure;
                da4.Fill(deleteDT);
                if(deleteDT.Rows.Count>0)
                {
                    string filefullPath = deleteDT.Rows[0]["DOC_Arch_FilePath"].ToString();
                    if (System.IO.File.Exists(filefullPath))
                    {
                        System.IO.File.Delete(filefullPath);
                    }
                }

                DataTable dt = new DataTable();
                int Count = 0;
                string filename = file.FileName;
                string filenamenew = Path.GetFileNameWithoutExtension(file.FileName);
                FileInfo fi = new FileInfo(file.FileName);
                string fileextension = fi.Extension;
                string filePath = ConfigurationManager.AppSettings["Path"].ToString();
                string filepath1 = "";
               
                MySqlCommand cmd = new MySqlCommand("Sp_CheckDocExistorNot", con);
                cmd.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = filename;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                Count = Convert.ToInt32(dt.Rows[0]["Count"].ToString());
                if (Count > 0)
                {
                    filenamenew = filenamenew + "(" + Count + ")" + fileextension;
                    filepath1 = Path.Combine(filePath, Path.GetFileName(filenamenew));
                    file.SaveAs(filepath1);
                    Result = 1;
                }
                else
                {
                    filenamenew = filenamenew + "(" + Count + ")" + fileextension;
                    filepath1 = Path.Combine(filePath, Path.GetFileName(filenamenew));
                    file.SaveAs(filepath1);
                    Result = 1;
                }
                if (Result == 1)
                {
                    MySqlCommand cmd2 = new MySqlCommand("SP_EditScannedDocuments", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add("In_DocID", MySqlDbType.Int32).Value = ModelObj.FileID;
                    cmd2.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = filename;
                    cmd2.Parameters.Add("In_FileVersion", MySqlDbType.VarChar).Value = Count;
                    cmd2.Parameters.Add("In_FileNameVersion", MySqlDbType.VarChar).Value = filenamenew;
                    cmd2.Parameters.Add("In_FilePath", MySqlDbType.VarChar).Value = filepath1;
                    cmd2.Parameters.Add("In_FileExtension", MySqlDbType.VarChar).Value = fileextension;
                    cmd2.Parameters.Add("In_FileType", MySqlDbType.VarChar).Value = fileextension;

                   // cmd2.Parameters.Add("In_Dep_id", MySqlDbType.VarChar).Value = ModelObj.DeptId;
                   // cmd2.Parameters.Add("In_Unit_id", MySqlDbType.VarChar).Value = ModelObj.UnitId;
                    cmd2.Parameters.Add("In_Dgroup_id", MySqlDbType.VarChar).Value = ModelObj.CatgId;
                    cmd2.Parameters.Add("In_Dname_id", MySqlDbType.VarChar).Value = ModelObj.SubCatgId;
                    cmd2.Parameters.Add("In_Doc_Arch_Modified_By", MySqlDbType.Int32).Value = ModelObj.UserId;
                    cmd2.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = ModelObj.activeflag;
                    Result = cmd2.ExecuteNonQuery();
                    con.Close();
                }
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int EditScannedDocwithoutfile(DocArchival_Model ModelObj)
        {
             int Result = 0;
             try
             {
                 con.Open();
                 MySqlCommand cmd = new MySqlCommand("SP_EditScannedDocWithoutFile", con);
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Parameters.Add("In_DocID", MySqlDbType.Int32).Value = ModelObj.FileID;
               //  cmd.Parameters.Add("In_Dep_id", MySqlDbType.VarChar).Value = ModelObj.DeptId;
               //  cmd.Parameters.Add("In_Unit_id", MySqlDbType.VarChar).Value = ModelObj.UnitId;
                 cmd.Parameters.Add("In_Dgroup_id", MySqlDbType.VarChar).Value = ModelObj.CatgId;
                 cmd.Parameters.Add("In_Dname_id", MySqlDbType.VarChar).Value = ModelObj.SubCatgId;
                 cmd.Parameters.Add("In_DocModifiedBy", MySqlDbType.Int32).Value = ModelObj.UserId;
                 cmd.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = ModelObj.activeflag;
                 Result = cmd.ExecuteNonQuery();
                 con.Close();
                 return Result;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
        }

        public string checkfile(DocArchival_Model ModelObj, HttpPostedFileBase file)
        {
            string message ="";
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_Check_Uploading_File", con);
                cmd.Parameters.Add("in_FileName", MySqlDbType.VarChar).Value = file.FileName;
              //  cmd.Parameters.Add("in_DepartmentID", MySqlDbType.Int32).Value = ModelObj.DeptId;
              //  cmd.Parameters.Add("in_UnitID", MySqlDbType.Int32).Value = ModelObj.UnitId;
                cmd.Parameters.Add("in_DocgroupID", MySqlDbType.Int32).Value = ModelObj.CatgId;
                cmd.Parameters.Add("in_DocnameID", MySqlDbType.Int32).Value = ModelObj.SubCatgId;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    message = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return message;
        }

        public string checkfilemultiple(DocArchival_Model ModelObj, List<HttpPostedFileBase> FileUpload1)
        {
            string message = "";
            try
            {
                con.Open();

                foreach (HttpPostedFileBase file in FileUpload1)
                {
                    DataTable dt = new DataTable();
                    MySqlCommand cmd = new MySqlCommand("SP_Check_Uploading_File", con);
                    cmd.Parameters.Add("in_FileName", MySqlDbType.VarChar).Value = file.FileName;
                  //  cmd.Parameters.Add("in_DepartmentID", MySqlDbType.Int32).Value = ModelObj.DeptId;
                 //   cmd.Parameters.Add("in_UnitID", MySqlDbType.Int32).Value = ModelObj.UnitId;
                    cmd.Parameters.Add("in_DocgroupID", MySqlDbType.Int32).Value = ModelObj.CatgId;
                    cmd.Parameters.Add("in_DocnameID", MySqlDbType.Int32).Value = ModelObj.SubCatgId;
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        message = dt.Rows[0][0].ToString();
                    }
                    if (message == "exist")
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return message;
        }

        public string checkfilemultiplecache(DocArchival_Model ModelObj, HttpPostedFileBase FileUpload1)
        {
            string message = "";
            try
            {
                con.Open();

                
                    DataTable dt = new DataTable();
                    MySqlCommand cmd = new MySqlCommand("SP_Check_Uploading_File", con);
                    cmd.Parameters.Add("in_FileName", MySqlDbType.VarChar).Value = FileUpload1.FileName;
                    //  cmd.Parameters.Add("in_DepartmentID", MySqlDbType.Int32).Value = ModelObj.DeptId;
                    //   cmd.Parameters.Add("in_UnitID", MySqlDbType.Int32).Value = ModelObj.UnitId;
                    cmd.Parameters.Add("in_DocgroupID", MySqlDbType.Int32).Value = ModelObj.CatgId;
                    cmd.Parameters.Add("in_DocnameID", MySqlDbType.Int32).Value = ModelObj.SubCatgId;
                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        message = dt.Rows[0][0].ToString();
                    }
                }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return message;
        }
    }
}
