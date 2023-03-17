using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Data
{
    public class Storageattribute_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        public DataSet SaveStorageattrib(string Str_Name, int Str_Length, string Str_Type, string Str_Mandotry, int Storage_orderid, int Dgroup_id, int DName_id, int UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("In_Satr_Name", Str_Name);
                values.Add("In_Satr_Length", Str_Length);
                values.Add("In_Satr_Type", Str_Type);
                values.Add("In_Satr_Mandotry", Str_Mandotry);
                values.Add("In_Storage_orderid", Storage_orderid);
             //   values.Add("In_Dep_id", Dept_id);
            //    values.Add("In_Unit_id", Unit_id);
                values.Add("In_Dgroup_id", Dgroup_id);
                values.Add("In_Dname_id", DName_id);
                values.Add("In_UserId", UserID);
                ds = RunProc("Sp_SaveStorageAttribute", values);
                return ds;
            }
            catch (Exception ex)
            {

                throw (ex);

            }
        }

        //01-04-2019
        public DataSet UpdateStorageattrib(Int64 attrgid, string Str_Name, int Str_Length, string Str_Type, string Str_Mandotry,Int32 orderid, int Dgroup_id, int DName_id, int UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("In_Satr_Id", attrgid);
                values.Add("In_Storage_orderid", orderid);
                values.Add("In_Satr_Name", Str_Name);
                values.Add("In_Satr_Length", Str_Length);
                values.Add("In_Satr_Type", Str_Type);
                values.Add("In_Satr_Mandotry", Str_Mandotry);
             //   values.Add("In_Dep_id", Dept_id);
             //   values.Add("In_Unit_id", Unit_id);
                values.Add("In_Dgroup_id", Dgroup_id);
                values.Add("In_Dname_id", DName_id);
                values.Add("In_UserId", UserID);
                ds = RunProc("Sp_UpdateStorageAttribute", values);
                return ds;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        //for delete //03-04-2019

        public DataSet DeleteStorageAttri(int strid)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("In_Satr_Id", strid);
                ds = RunProc("Sp_DeleteStorageAttribute", values);
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
                con.Open();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet CheckSaveStr(int? DgroupID, int? DNameID)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
               // values.Add("In_Dep_id", DeptID);
               // values.Add("In_Unit_id", UnitID);
                values.Add("In_Dgroup_id", DgroupID);
                values.Add("In_DName_id", DNameID);
                ds = RunProc("sp_CheckStorageAttributealreadyexist", values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //30-03-2019
        public DataSet AttributeBind(int groupid)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("P_Groupgid", groupid);
                ds = RunProc("SP_GetStorageAttributeddl", values);
                return ds;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //04-04-2019 View and Add same as

        public DataTable GSameasAttributeValues(int Docname_id)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_viewsameasattributevalues", con);
            cmd.Parameters.Add("In_Dname_Id", MySqlDbType.Int32).Value = Docname_id;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public DataTable GetSameasAttributeplusdata(int Docname_id)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_viewsameasattributevalues", con);
            cmd.Parameters.Add("In_Dname_Id", MySqlDbType.Int32).Value = Docname_id;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        //

        public DataSet savemandotry(int DgroupID, int DNameID, int UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
             //   values.Add("In_Dep_id", DeptID);
             //   values.Add("In_Unit_id", UnitID);
                values.Add("In_Dgroup_id", DgroupID);
                values.Add("In_DName_id", DNameID);
                values.Add("In_UserId", UserID);
                ds = RunProc("Sp_StorageAttributeMandotryfields", values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
