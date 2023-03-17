
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
    public class Employee_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataSet Saveemdtl(List<Model.Employee_Model> lstmodel, DataTable dt1)//to check excel data id valid for masters
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataTable dt2 = new DataTable();
            string text = "";
            try
            {
                int ret = 0;
                int Errorcount = 0;
                dt1.Columns.Add("remarks");
                for (int i = 0; i < lstmodel.Count; i++)
                {
                    string EmpCode = "0";
                    MySqlCommand cmd1 = new MySqlCommand("sp_CheckEmployeebulkupload", Con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = lstmodel[i].Dept_Id;
                    cmd1.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = lstmodel[i].UnitID;
                    cmd1.Parameters.Add("In_GradeID", MySqlDbType.Int32).Value = lstmodel[i].Dept_Id;
                    cmd1.Parameters.Add("In_EmpTitleID", MySqlDbType.Int32).Value = lstmodel[i].TitleID;
                    cmd1.Parameters.Add("In_CityID", MySqlDbType.Int32).Value = lstmodel[i].CityID;
                    cmd1.Parameters.Add("In_StateID", MySqlDbType.Int32).Value = lstmodel[i].StateID;
                    cmd1.Parameters.Add("In_RegionID", MySqlDbType.Int32).Value = lstmodel[i].RegionID;
                    cmd1.Parameters.Add("In_UserGroupID", MySqlDbType.Int32).Value = lstmodel[i].UserGroupID;
                    cmd1.Parameters.Add("In_EmpTypeID", MySqlDbType.Int32).Value = lstmodel[i].emptypeId;
                    EmpCode = lstmodel[i].EmployeeCode;
                    MySqlDataAdapter da1 = new MySqlDataAdapter(cmd1);
                    Con.Open();
                    ret = cmd1.ExecuteNonQuery();
                    da1.Fill(ds);
                    Con.Close();
                    text = ds.Tables[0].Rows[i][0].ToString();
                    // string testing = EmpCode + "_" + dt2.Rows[0][0].ToString();
                    if (text != "0")
                    {
                        dt1.Rows[i][28] = text;
                        Errorcount++;
                    }
                    else
                    {
                        dt1.Rows[i][28] = "Success";
                    }
                }
                if (Errorcount == 0)
                {
                    if (text == "0")
                    {
                        Con.Open();
                        for (int i = 0; i < lstmodel.Count; i++)//valid master id to be insert 
                        {

                            MySqlCommand cmd = new MySqlCommand("SP_EmployeeSaveUpdateDelete", Con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "BInsert";
                            cmd.Parameters.Add("In_EmpID", MySqlDbType.Int32).Value = "0";
                            cmd.Parameters.Add("In_EmpCode", MySqlDbType.VarChar).Value = lstmodel[i].EmployeeCode;
                            cmd.Parameters.Add("In_EmpName", MySqlDbType.VarChar).Value = lstmodel[i].EmployeeName;
                            cmd.Parameters.Add("In_EmpTitle", MySqlDbType.VarChar).Value = lstmodel[i].TitleID;
                            cmd.Parameters.Add("In_GradeID", MySqlDbType.Int32).Value = lstmodel[i].GradeID;
                            cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = lstmodel[i].Dept_Id;
                            cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = lstmodel[i].UnitID;
                            cmd.Parameters.Add("In_DOJ", MySqlDbType.DateTime).Value = lstmodel[i].DOJ;
                            cmd.Parameters.Add("In_Address", MySqlDbType.VarChar).Value = lstmodel[i].Address;
                            cmd.Parameters.Add("In_CityID", MySqlDbType.Int32).Value = lstmodel[i].CityID;
                            cmd.Parameters.Add("In_PinID", MySqlDbType.Int32).Value = lstmodel[i].PinID;
                            cmd.Parameters.Add("In_StateID", MySqlDbType.Int32).Value = lstmodel[i].StateID;
                            cmd.Parameters.Add("In_RegionID", MySqlDbType.Int32).Value = lstmodel[i].RegionID;
                            cmd.Parameters.Add("In_MobileNo", MySqlDbType.VarChar).Value = lstmodel[i].MobileNo;
                            cmd.Parameters.Add("In_LanNo", MySqlDbType.VarChar).Value = lstmodel[i].LanNo;
                            cmd.Parameters.Add("In_EmailID", MySqlDbType.VarChar).Value = lstmodel[i].EmailID;
                            cmd.Parameters.Add("In_CreatedBy", MySqlDbType.Int32).Value = lstmodel[i].UserID;
                            cmd.Parameters.Add("In_EmpType", MySqlDbType.VarChar).Value = lstmodel[i].emptypeId;
                            cmd.Parameters.Add("In_Password", MySqlDbType.VarChar).Value = lstmodel[i].Password;
                            cmd.Parameters.Add("In_UserGroupID", MySqlDbType.Int32).Value = lstmodel[i].UserGroupID;
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                            da.Fill(ds1);
                        }
                        Con.Close();
                    }
                }
                else
                {
                    DataTable dtNew = new DataTable();
                    dtNew = dt1.DefaultView.ToTable();
                    dtNew.TableName = "Table3";
                    ds.Tables.Add(dtNew);
                     
                   return ds;
                }
                //ds1.Tables.Add(dtNew);
                return ds1;
            }
            catch (Exception ex)
            {
                throw ex;
                //return ret;
            }
        }

        //public DataSet GetFile(string Filename)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        MySqlCommand cmd = new MySqlCommand(" ", Con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("In_LovName", MySqlDbType.VarChar).Value = Filename;
        //        cmd.Parameters.Add("In_excelId", MySqlDbType.Int32).Value = 0;
        //        cmd.Parameters.Add("In_excelName", MySqlDbType.VarChar).Value = "NULL";
        //        cmd.Parameters.Add("Actionval", MySqlDbType.VarChar).Value = "GetData";
        //        Con.Open();
        //        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        Con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }

        //    return ds;

        //}

        public DataTable exceldata(string data)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_GetEmployeebulkMasterDropDowns", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = data;
                Con.Open();
                       MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                       da.Fill(dt);
                       //dt.WriteXml("excel.xls",);
                
                      Con.Close();
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            return dt;
        }



        public DataSet GetFile(string Filename)
        {
            throw new NotImplementedException();
        }
    }
}

