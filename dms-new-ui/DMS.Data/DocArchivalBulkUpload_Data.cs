using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Model;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Web;
using System.IO;


namespace DMS.Data
{
    public class DocArchivalBulkUpload_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataTable getattributes(DocArchival_Model ModelObj)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_Getattributesdetails", con);
               // cmd.Parameters.Add("_dept", MySqlDbType.Int32).Value = ModelObj.DeptId;
                cmd.Parameters.Add("_Action", MySqlDbType.VarChar).Value = "Attributeonly";
                cmd.Parameters.Add("_docgroup", MySqlDbType.Int32).Value = ModelObj.CatgId;
                cmd.Parameters.Add("_docname", MySqlDbType.Int32).Value = ModelObj.SubCatgId;
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
            finally
            { con.Close(); }

        }

        //public DataTable savebulkupload(HttpPostedFileBase flexcelfile, HttpPostedFileBase[] flbulkfiles, DocArchival_Model ModelObj, DataTable dtexceldata)
        //{
        //    int res = 0;
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        int Count = 0;
        //        foreach (HttpPostedFileBase file in flbulkfiles)
        //        {
        //            dt.Rows.Clear();
        //            string filepath1 = "";
        //            //Checking file is available to save.  
        //            if (file != null)
        //            {
        //                string filename = file.FileName;
        //                string filenamenew = Path.GetFileNameWithoutExtension(file.FileName);
        //                FileInfo fi = new FileInfo(file.FileName);
        //                string fileextension = fi.Extension;
        //                string filePath = ConfigurationManager.AppSettings["Path"].ToString();
        //                con.Open();
        //                MySqlCommand cmd = new MySqlCommand("Sp_CheckDocExistorNot", con);
        //                cmd.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = filename;
        //                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                da.Fill(dt);
        //                Count = Convert.ToInt32(dt.Rows[0]["Count"].ToString());
        //                if (Count > 0)
        //                {
        //                    filenamenew = filenamenew + "(" + Count + ")" + fileextension;
        //                    filepath1 = Path.Combine(filePath, Path.GetFileName(filenamenew));
        //                    file.SaveAs(filepath1);
        //                    res = 1;
        //                }
        //                else
        //                {
        //                    filenamenew = filenamenew + "(" + Count + ")" + fileextension;
        //                    filepath1 = Path.Combine(filePath, Path.GetFileName(filenamenew));
        //                    file.SaveAs(filepath1);
        //                    res = 1;
        //                }
        //                if (res == 1)
        //                {
        //                    MySqlCommand cmd1 = new MySqlCommand("SP_MultipleDocumentArchival", con);
        //                    cmd1.CommandType = CommandType.StoredProcedure;
        //                    cmd1.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = filename;
        //                    cmd1.Parameters.Add("In_FileVersion", MySqlDbType.VarChar).Value = Count;
        //                    cmd1.Parameters.Add("In_FileNameVersion", MySqlDbType.VarChar).Value = filenamenew;
        //                    cmd1.Parameters.Add("In_FilePath", MySqlDbType.VarChar).Value = filepath1;
        //                    cmd1.Parameters.Add("In_FileExtension", MySqlDbType.VarChar).Value = fileextension;
        //                    cmd1.Parameters.Add("In_FileType", MySqlDbType.VarChar).Value = fileextension;

        //                    cmd1.Parameters.Add("In_Dep_id", MySqlDbType.VarChar).Value = ModelObj.DeptId;
        //                    cmd1.Parameters.Add("In_Unit_id", MySqlDbType.VarChar).Value = ModelObj.UnitId;
        //                    cmd1.Parameters.Add("In_Dgroup_id", MySqlDbType.VarChar).Value = ModelObj.CatgId;
        //                    cmd1.Parameters.Add("In_Dname_id", MySqlDbType.VarChar).Value = ModelObj.SubCatgId;
        //                    cmd1.Parameters.Add("In_Doc_Arch_Created_By", MySqlDbType.Int32).Value = ModelObj.UserId;
        //                    res = cmd1.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public int SaveSingleDocDetails(DocArchival_Model ModelObj, HttpPostedFileBase file)
        {
            int Result = 0;
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
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
                    MySqlCommand cmd2 = new MySqlCommand("SP_Savebulkarchival", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = filename;
                    cmd2.Parameters.Add("In_FileVersion", MySqlDbType.VarChar).Value = Count;
                    cmd2.Parameters.Add("In_FileNameVersion", MySqlDbType.VarChar).Value = filenamenew;
                    cmd2.Parameters.Add("In_FilePath", MySqlDbType.VarChar).Value = filepath1;
                    cmd2.Parameters.Add("In_FileExtension", MySqlDbType.VarChar).Value = fileextension;
                    cmd2.Parameters.Add("In_FileType", MySqlDbType.VarChar).Value = fileextension;

                    //cmd2.Parameters.Add("In_Dep_id", MySqlDbType.VarChar).Value = ModelObj.DeptId;
                    //cmd2.Parameters.Add("In_Unit_id", MySqlDbType.VarChar).Value = ModelObj.UnitId;
                    cmd2.Parameters.Add("In_Dgroup_id", MySqlDbType.VarChar).Value = ModelObj.CatgId;
                    cmd2.Parameters.Add("In_Dname_id", MySqlDbType.VarChar).Value = ModelObj.SubCatgId;
                    cmd2.Parameters.Add("In_Doc_Arch_Created_By", MySqlDbType.Int32).Value = ModelObj.UserId;
                    //Result = cmd2.ExecuteNonQuery();
                    MySqlDataAdapter da1 = new MySqlDataAdapter(cmd2);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    da1.Fill(dt1);
                    con.Close();
                    Result = Convert.ToInt32(dt1.Rows[0][0].ToString());
                }
                return Result;
            }
            catch (Exception ex)
            {
               // throw ex;
                return 0;
            }
        }

        public int saveexclefile(HttpPostedFileBase excelfile, int userid)
        {
            int Result = 0;
            try
            {
                string filename = excelfile.FileName;
                FileInfo fi = new FileInfo(excelfile.FileName);
                string fileextension = fi.Extension;
                string filePath = ConfigurationManager.AppSettings["Path2"].ToString();
                //string filewithpath = "";
                //filewithpath = Path.Combine(filePath, Path.GetFileName(filename));
                //excelfile.SaveAs(filewithpath);

                MySqlCommand cmd = new MySqlCommand("SP_Savebulkuploadexcelfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("in_filename", MySqlDbType.VarChar).Value = filename;
                cmd.Parameters.Add("in_filetype", MySqlDbType.VarChar).Value = fileextension;
                cmd.Parameters.Add("in_filepath", MySqlDbType.VarChar).Value = filePath;
                cmd.Parameters.Add("in_userid", MySqlDbType.Int32).Value = userid;
                con.Open();
                Result = cmd.ExecuteNonQuery();
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getfilenamecheck_data(DocArchival_Model ModelObj, string _filename){
         int Result = 0;
            try
            {
                  DataTable dt = new DataTable();
                 MySqlCommand cmd = new MySqlCommand("SP_filenamecheck", con);
                 MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_docgroup_id", MySqlDbType.VarChar).Value = ModelObj.CatgId;
                cmd.Parameters.Add("In_docname_id", MySqlDbType.VarChar).Value = ModelObj.SubCatgId;
                cmd.Parameters.Add("In_filename", MySqlDbType.VarChar).Value = _filename;
                con.Open();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Result = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        public string validateLOV(int dtlov, string userdata)
        {
            string validationmessage = "";
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_Validate_LOV_Master", con);
                cmd.Parameters.Add("_lovid", MySqlDbType.Int32).Value = dtlov;
                cmd.Parameters.Add("_lov_value", MySqlDbType.VarChar).Value = userdata;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                validationmessage = dt.Rows[0][0].ToString();
                return validationmessage;
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

        public string validateattribute( DocArchival_Model ModelObj, String _wherecondition = "")
        {
            string result = "";
            DataTable dt = new DataTable();
            try
            {
                con.Open();
               // String _wherecondition = "";
                //copyofdtexceldata.Columns.Remove("File Name");
                //for (int i = 0; i < copyofdtexceldata.Rows.Count; i++)
                //{
                //    for (int j = 0; j < copyofdtexceldata.Columns.Count - 1; j++)
                //    {
                //        if (copyofdtexceldata.Columns.Count - 2 == j)
                //        {
                //            _wherecondition += "`" + copyofdtexceldata.Columns[j].ToString().Trim() + "`" + "=" + "'" + copyofdtexceldata.Rows[i][j].ToString().Trim() + "'";
                //        }
                //        else
                //        {
                //            _wherecondition += "`" + copyofdtexceldata.Columns[j].ToString().Trim() + "`" + "=" + "'" + copyofdtexceldata.Rows[i][j].ToString().Trim() + "'" + " " + "AND" + " ";
                //        }

                //    }

                    MySqlCommand cmd = new MySqlCommand("SP_ValidateAttributesbulkfile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                   // cmd.Parameters.Add("_dept", MySqlDbType.Int32).Value = ModelObj.DeptId;
                   // cmd.Parameters.Add("_unit", MySqlDbType.Int32).Value = ModelObj.UnitId;
                    cmd.Parameters.Add("_docgroup", MySqlDbType.Int32).Value = ModelObj.CatgId;
                    cmd.Parameters.Add("_docname", MySqlDbType.Int32).Value = ModelObj.SubCatgId;
                    cmd.Parameters.Add("_condition", MySqlDbType.VarChar).Value = _wherecondition;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        result = dt.Rows[0][0].ToString();
                        if (result == "duplicate not found")
                        {
                            _wherecondition = "";
                            dt.Clear();
                          //  continue;
                        }
                        else
                        {
                         //   break;
                            _wherecondition = "";
                            dt.Clear();
                        }
                    }
              //  }
            }
            catch (Exception ex)
            {
               // result =  ex.Message.ToString();
                result = "Attributes Values are in Incorrect Their Attribute Type or Length";
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}
