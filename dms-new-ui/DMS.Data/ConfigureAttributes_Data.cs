using DMS.Model;
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
    public class ConfigureAttributes_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        public DataSet SaveConfigAttri(string Atr_Name, int Atr_Length, string Atr_Type, string Atr_Mandotry, int Lov_Id, int Attrib_orderId, int attrautonumberid, string atr_keyval, int Dgroup_id, int Dname_id, string position)
        {
            DataSet ds = new DataSet();
            try
            {
                if (Atr_Type == "Lov Name")
                {
                    //lov id to be save in dms_mst_tattributeslov
                    Dictionary<string, Object> values = new Dictionary<string, object>();
                    values.Add("In_AtrLov_Name", Atr_Name);
                    values.Add("In_Lov_Id", Lov_Id);
                    values.Add("In_AtrLov_Type", Atr_Type);
                    values.Add("In_AtrLov_Mandotry", Atr_Mandotry);
                    values.Add("In_Lov_orderid", Attrib_orderId);
                    values.Add("In_Dgroup_id", Dgroup_id);
                    values.Add("In_Dname_id", Dname_id);
                    values.Add("In_position", position);
                    values.Add("In_Atr_Keyval", atr_keyval);
                    ds = RunProc("Sp_Config_AttributeLov", values);
                    //con.RunProc("Sp_Config_AttributeLov", values);          

                }
                else if (Atr_Type == "AutoNumber")
                {
                    //lov id to be save in dms_mst_tattributeslov
                    Dictionary<string, Object> values = new Dictionary<string, object>();
                    values.Add("In_AtrLov_Name", Atr_Name);
                    values.Add("In_Lov_Id", attrautonumberid);
                    values.Add("In_AtrLov_Type", Atr_Type);
                    values.Add("In_AtrLov_Mandotry", Atr_Mandotry);
                    values.Add("In_Lov_orderid", Attrib_orderId);
                    values.Add("In_Dgroup_id", Dgroup_id);
                    values.Add("In_Dname_id", Dname_id);
                    values.Add("In_position", position);
                    values.Add("In_Atr_Keyval", atr_keyval);
                    ds = RunProc("Sp_Config_AttributeLov", values);
                    //con.RunProc("Sp_Config_AttributeLov", values); 
                
                }
                else
                {
                    //lov id to be save in dms_mst_tattributes
                    Dictionary<string, Object> values = new Dictionary<string, object>();
                    values.Add("In_Atr_Name", Atr_Name);
                    values.Add("In_Atr_Length", Atr_Length);
                    values.Add("In_Atr_Type", Atr_Type);
                    values.Add("In_Atr_Mandotry", Atr_Mandotry);
                    values.Add("In_Attrib_orderId", Attrib_orderId);
                    values.Add("In_Dgroup_id", Dgroup_id);
                    values.Add("In_Dname_id", Dname_id);
                    values.Add("In_position", position);
                    values.Add("In_Atr_Keyval", atr_keyval);
                    ds = RunProc("Sp_Config_Attribute", values);
                    //con.RunProc("FindDropDown", values);       

                }
                return ds;
            }

            catch (Exception ex)
            {

                throw (ex);

            }
        }

        public DataSet checkSaveConfigAttri( int Dgroup_id, int Dname_id)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                //values.Add("In_Dep_id", Dep_id);
                //values.Add("In_Unit_id", Unit_id);
                values.Add("In_Dgroup_id", Dgroup_id);
                values.Add("In_Dname_id", Dname_id);
                ds = RunProc("sp_Checkattributealreadyexist", values);
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
                cmd.CommandTimeout = 0;
                //cmd.CommandTimeout = int.MaxValue;
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

        public DataSet GetDropdown(string type, string actiontype,string userid)
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
                values.Add("p_userid", userid);
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet RunProcedure(Dictionary<string, Object> values = null)
        {

            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_SingFileddl", con);
                cmd.CommandTimeout = 0;
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


        public DataSet Header_Data(Headers_Model HeaderObjs)
        {

            try
            {
                MySqlCommand cmd2 = new MySqlCommand("Pr_get_Headers", con);
                cmd2.CommandTimeout = 0;
                DataSet ds = new DataSet();

                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add("In_orglvl1_code", MySqlDbType.VarChar).Value = HeaderObjs.Orglvl1code;
                cmd2.Parameters.Add("In_orglvl2_code", MySqlDbType.VarChar).Value = HeaderObjs.Orglvl2code;
                cmd2.Parameters.Add("In_orglvl3_code", MySqlDbType.VarChar).Value = HeaderObjs.Orglvl3code;
                cmd2.Parameters.Add("In_orglvl4_code", MySqlDbType.VarChar).Value = HeaderObjs.Orglvl4code;
                //if (values != null)
                //{
                //    foreach (string key in values.Keys)
                //    {
                //        cmd.Parameters.AddWithValue(key, values[key]);
                //    }
                //}
                MySqlDataAdapter da = new MySqlDataAdapter(cmd2);
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

        public DataSet GetDocGroupChange(Int64 DocGroup_Id, string userId)
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
                values.Add("p_userid", userId);
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDocNameChange(Int64 DocNameId,string userId)
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
                values.Add("p_userid", userId);
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}