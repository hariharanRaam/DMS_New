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
    public class PhysicalArchival_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        
        public DataSet GetIndexedDocuments(int? Dgroup1, int? Dname1, string Dbcon, string type, string activeflag, string from_date, string to_date)
        {
            try
            {
                
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("SP_Getstorageattribforgrid_New", con);
                cmd.CommandType = CommandType.StoredProcedure;
              //  cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
              //  cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_Dbcondition", MySqlDbType.VarChar).Value = Dbcon;
                cmd.Parameters.Add("In_Type", MySqlDbType.VarChar).Value = type;
                cmd.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = activeflag;
                cmd.Parameters.Add("In_From_Date", MySqlDbType.VarChar).Value = from_date;
                cmd.Parameters.Add("In_To_Date", MySqlDbType.VarChar).Value = to_date;
                
                con.Open();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 300;
                da.Fill(ds);
                con.Close();
                return ds;
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

        public DataSet GetIndexedRetrivals(int? Dgroup1, int? Dname1, string attrib_ids, string grid_mode)
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("get_retrieval_attributes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //  cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
                //  cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_attributes", MySqlDbType.Text).Value = attrib_ids;
                cmd.Parameters.Add("In_grid_mode", MySqlDbType.VarChar).Value = grid_mode;
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

        public DataSet Initialvalues( int? docgroup, int? docname, string Aaction, int? attribgid)
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("SP_GetdynamicStorageattributes", con);
                cmd.CommandType = CommandType.StoredProcedure;
             //   cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = deptid;
             //   cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = unit;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = docgroup;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = docname;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = Aaction;
                cmd.Parameters.Add("In_Attribgid", MySqlDbType.Int32).Value = attribgid;
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

        public int SaveStorageAttributes(PhysicalArchival_Model modelObj, List<PhysicalArchival_Model> ModelObjList)
        {
            try
            {

                int Result = 0;
                con.Open();
                for (int i = 0; i < ModelObjList.Count; i++)
                {
                    MySqlCommand cmd = new MySqlCommand("SP_Savephysicalattributes", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("In_AttribId", MySqlDbType.Int32).Value = ModelObjList[i].Atrgid;
                    cmd.Parameters.Add("In_SatrName", MySqlDbType.VarChar).Value = ModelObjList[i].Satrname;
                    cmd.Parameters.Add("In_SatrLength", MySqlDbType.VarChar).Value = ModelObjList[i].Satrlen;
                    cmd.Parameters.Add("In_SatrType", MySqlDbType.VarChar).Value = ModelObjList[i].Satrtype;
                    cmd.Parameters.Add("In_SatrMandotry", MySqlDbType.VarChar).Value = ModelObjList[i].SatrMand;
                    cmd.Parameters.Add("In_SatrDesc", MySqlDbType.VarChar).Value = ModelObjList[i].DynamicVal;
                    cmd.Parameters.Add("In_UserId", MySqlDbType.Int32).Value = modelObj.UserId;
                    cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = modelObj.action;
                    cmd.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = modelObj.activeflag;
                    Result = cmd.ExecuteNonQuery(); 
                }                            
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable checkvalidone(int? GridID)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_checkvalidone", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_AttribID", MySqlDbType.Int32).Value = GridID;
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

        public DataSet getattributes(PhysicalArchival_Model ModelObj)
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("SP_Getattributesdetails", con);
                cmd.Parameters.Add("_docgroup", MySqlDbType.Int32).Value = ModelObj.CatgId;
                cmd.Parameters.Add("_docname", MySqlDbType.Int32).Value = ModelObj.SubCatgId;
                cmd.Parameters.Add("_Action", MySqlDbType.String).Value = "Attributes";
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

        public DataTable SaveStorageAttribute(PhysicalArchival_Model ModelObj)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_Set_Storageattributesdetails", con);
             //   cmd.Parameters.Add("_dept", MySqlDbType.Int64).Value = ModelObj.DeptId;
             //   cmd.Parameters.Add("_unit", MySqlDbType.Int64).Value = ModelObj.UnitId;
                cmd.Parameters.Add("_docgroup", MySqlDbType.Int64).Value = ModelObj.CatgId;
                cmd.Parameters.Add("_docname", MySqlDbType.Int64).Value = ModelObj.SubCatgId;
                if (ModelObj.Attributescol == null)
                {
                    cmd.Parameters.Add("_AttrbCol", MySqlDbType.String).Value = "";
                }
                else {
                    cmd.Parameters.Add("_AttrbCol", MySqlDbType.String).Value = ModelObj.Attributescol;
                }
                if (ModelObj.Attributesval == null)
                {
                   cmd.Parameters.Add("_AttrbVal", MySqlDbType.String).Value = "";
                }else{
                   cmd.Parameters.Add("_AttrbVal", MySqlDbType.String).Value = ModelObj.Attributesval;
                }
                cmd.Parameters.Add("_StrAttrCol", MySqlDbType.String).Value = (ModelObj.StorageAttribcol == null ? "":ModelObj.StorageAttribcol);
                cmd.Parameters.Add("_StrAttrVal", MySqlDbType.String).Value = (ModelObj.StorageAttribval == null ? "":ModelObj.StorageAttribval);
                cmd.Parameters.Add("FileName", MySqlDbType.String).Value = ModelObj.FileName;
                cmd.Parameters.Add("In_UserId", MySqlDbType.Int64).Value = ModelObj.UserId;
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

        public DataTable SaveStorageAttributeNew(PhysicalArchival_Model ModelObj)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_Set_StorageattributesdetailsNew", con);
                //   cmd.Parameters.Add("_dept", MySqlDbType.Int64).Value = ModelObj.DeptId;
                //   cmd.Parameters.Add("_unit", MySqlDbType.Int64).Value = ModelObj.UnitId;
                cmd.Parameters.Add("_docgroup", MySqlDbType.Int64).Value = ModelObj.CatgId;
                cmd.Parameters.Add("_docname", MySqlDbType.Int64).Value = ModelObj.SubCatgId;
                cmd.Parameters.Add("_StrAttrCol", MySqlDbType.String).Value = (ModelObj.Attributescol == null ? "" : ModelObj.Attributescol);
                cmd.Parameters.Add("_StrAttrVal", MySqlDbType.String).Value = (ModelObj.Attributesval == null ? "" : ModelObj.Attributesval);
                cmd.Parameters.Add("FileName", MySqlDbType.String).Value = ModelObj.FileName;
                cmd.Parameters.Add("In_UserId", MySqlDbType.Int64).Value = ModelObj.UserId;
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

    }
}
