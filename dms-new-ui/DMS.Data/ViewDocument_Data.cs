using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace DMS.Data
{
    public class ViewDocument_Data
    {
        public MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        public DataSet GetDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, string form)
        {
            DataSet ds = new DataSet();
            try
            {
                //MySqlConnection con = new MySqlConnection();
                con.Open();
                //MySqlCommand cmd = new MySqlCommand("Sp_GetDocAttributes1", con); Sp_GetDocSearch
               // MySqlCommand cmd = new MySqlCommand("SP_GetDocLinkGrid1Datas", con);
                MySqlCommand cmd = new MySqlCommand("Sp_GetDocSearch", con);
               // cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = 0;
                //cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = 0;
                //cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
                //cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_Dynamicfields", MySqlDbType.VarChar).Value = form;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;

                // return dt;
            }
            return ds;
        }

        public DataSet GetDocumentsmulti(string Dgroup1, string Dname1, string form)
        {
            //creating dataset for storing collection of tables.
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_GetDocSearch", con);
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.VarChar).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.VarChar).Value = Dname1;
                cmd.Parameters.Add("In_Dynamicfields", MySqlDbType.VarChar).Value = form;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetMultifile(string Attribid)
        {
            //creating dataset for storing collection of tables.
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_GetselectedFile", con);
                cmd.Parameters.Add("In_Attribid", MySqlDbType.VarChar).Value = Attribid;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataTable getmastervalue(int LovId,string LovType)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Getlovmastervalues", con);
                cmd.Parameters.Add("In_LovId", MySqlDbType.Int32).Value = LovId;
                cmd.Parameters.Add("In_LovType", MySqlDbType.VarChar).Value = LovType;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataSet GetLinkedDocuments( int? Dgroup1, int? Dname1, Int64 Attribid)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_GetLinkedFiles", con);
              //  cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
              //  cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_AttribID", MySqlDbType.Int64).Value = Attribid;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetRetrievalaudit(Int64 Attribid)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Pr_Get_RetrivalAudit", con);
                cmd.Parameters.Add("In_AttribID", MySqlDbType.Int64).Value = Attribid;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetInterlingDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_ShowInterFilingFiles", con);
                cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetInterlingLinkDocuments(int? Dgroup1, int? Dname1, Int64 Attribid)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_ShowInterFilingFiles1", con);
               // cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
              //  cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_AttribID", MySqlDbType.Int64).Value = Attribid;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }




        public DataTable GetDynamicFields( int? Dgroup1, int? Dname1)
        {
            DataTable dt = new DataTable();
            try
            {
                //MySqlConnection con = new MySqlConnection();
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_GetDynamicFileds", con);
              //  cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
               // cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataSet GetDropdown(string type, string actiontype)
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


        public DataSet RunProcedures(Dictionary<string, Object> values = null, string name = null)
        {

            try
            {
                MySqlCommand cmd = new MySqlCommand(name, con);
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

        public DataSet GetDocGroupChange(Int64 DocGroup_Id)
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
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDocNameChange(Int64 DocNameId)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            //string[] DocNameIds = DocNameId.Split(',');
            try
            {
                //for (int i = 0; i < DocNameIds.Length; i++)
                //{
                    Dictionary<string, Object> values = new Dictionary<string, object>();
                    values.Add("p_action", "GetDocName");
                    values.Add("p_Department", "0");
                    values.Add("p_Unit", "0");
                    values.Add("p_Docgroup", "0");
                    values.Add("p_Docname", DocNameId);
                    ds = RunProcedure(values);
                //}
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetDocNameChanges(string DocNameId)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            //string[] DocNameIds = DocNameId.Split(',');
            try
            {
                //for (int i = 0; i < DocNameIds.Length; i++)
                //{
                Dictionary<string, Object> values = new Dictionary<string, object>();
                string name = "";
                values.Add("p_action", "GetDocName");
                values.Add("p_Department", "0");
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", "0");
                values.Add("p_Docname", 0);
                values.Add("p_Docnames", DocNameId);
                name = "Pr_get_Docsearch";
               // values.Add("p_Docnames", "0");
                ds = RunProcedures(values, name);
                //}
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetDocGroupChanges(string DocGroup_Id)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                string name = "";
                values.Add("p_action", "GetDocGroup");
                values.Add("p_Department", "0");
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", DocGroup_Id);
                values.Add("p_Docname", "0");
                values.Add("p_Docnames", "0");
                name = "Pr_get_Docsearch";
                ds = RunProcedures(values, name);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SingleDownload(int? attachid, string mode)
        {
            string Result = "";
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("ViewDocumentsDownload", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Attachid", MySqlDbType.Int32).Value = attachid;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = mode;
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Result = dt.Rows[0][0].ToString();
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet Getmaildetails(int? Indexed_gid, int EmpID, string type)
        {
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_Getmaildetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_indexgid", MySqlDbType.Int32).Value = Indexed_gid;
                cmd.Parameters.Add("In_empgid", MySqlDbType.Int32).Value = EmpID;
                cmd.Parameters.Add("In_type", MySqlDbType.VarChar).Value = type;
                con.Open();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable showdynamicattributes(int group_gid, string group_type)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_Getdynamicattributesforlinkinterfiling", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("in_type", MySqlDbType.VarChar).Value = group_type;
                cmd.Parameters.Add("in_gid", MySqlDbType.Int32).Value = group_gid;
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

        public DataSet ShowAttributes(int? Dgroup1, int? Dname1, int? Attribid)
        {
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_GetDocsearchAttributes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_Dynamicfields", MySqlDbType.VarChar).Value = Attribid;
                con.Open();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ShowAttributesMultiple(int? Dgroup1, int? Dname1, string Attribid)
        {
            DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            try
            {
                string[] Attrib = Attribid.Split(',');
                con.Open();
                for (int i = 0; i < Attrib.Count(); i++)
                {
                     try
                    {
                        DataTable dt = new DataTable();
                        MySqlDataAdapter da = new MySqlDataAdapter();
                        MySqlCommand cmd = new MySqlCommand("SP_GetDocsearchAttributes", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                        cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                        cmd.Parameters.Add("In_Dynamicfields", MySqlDbType.VarChar).Value = Attrib[i];
                        cmd.CommandTimeout = 0;
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                        dt.TableName = Attrib[i];
                        ds.Tables.Add(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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
            return ds;
            
        }

        public DataSet getmultiplemaildetails(int _empid, string attributegids)
        {            
            try
            {
                DataSet ds = new DataSet();               
                MySqlCommand cmd = new MySqlCommand("SP_Getmultiplemaildetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("_empgid", MySqlDbType.Int32).Value = _empid;
                cmd.Parameters.Add("_indexgids", MySqlDbType.VarChar).Value = attributegids;
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

        public int SaveRequestRetrival(string Requestdate, string Requestno, int noofdcoc, int Employeeid, string Requireddate, string Description, string status,Int64 RequestType)
        {
            int Result = 0;
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_InsertRequestRetrival", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Request_No", MySqlDbType.VarChar).Value = Requestno;
                cmd.Parameters.Add("In_Request_Date", MySqlDbType.DateTime).Value = Requestdate;
                cmd.Parameters.Add("In_NoofDoc", MySqlDbType.Int32).Value = noofdcoc;
                cmd.Parameters.Add("In_RequestFrom", MySqlDbType.Int32).Value = Employeeid;
                cmd.Parameters.Add("In_Requireddate", MySqlDbType.DateTime).Value = Requireddate;
                cmd.Parameters.Add("In_RequestNote", MySqlDbType.VarChar).Value = Description;
                cmd.Parameters.Add("In_Status", MySqlDbType.VarChar).Value = status;
                cmd.Parameters.Add("In_RequestType", MySqlDbType.Int64).Value = RequestType;
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Result = Convert.ToInt32(dt.Rows[0][0].ToString());
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int ChildSaveRequestRetrival(int Requestgid, int attributeid, int Employeeid)
        {
            int Result = 0;
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_ChildInsertRequestRetrival", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Request_gid", MySqlDbType.Int32).Value = Requestgid;
                cmd.Parameters.Add("In_Attrib_Id", MySqlDbType.Int32).Value = attributeid;
                cmd.Parameters.Add("In_DespatchMode", MySqlDbType.VarChar).Value = null;
                cmd.Parameters.Add("In_DespatchDate", MySqlDbType.DateTime).Value = null;
                cmd.Parameters.Add("In_RequestFrom", MySqlDbType.Int32).Value = Employeeid;
                cmd.Parameters.Add("In_DespatchNote", MySqlDbType.VarChar).Value = null;
                cmd.Parameters.Add("In_Status", MySqlDbType.VarChar).Value = null;
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Result = Convert.ToInt32(dt.Rows[0][0].ToString());
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetRequestretrival(int Employeeid)
        {
            DataTable tab = new DataTable();
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetRequestRetrival", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Employeeid", MySqlDbType.Int32).Value = Employeeid;
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


        public DataSet GetFileTaken(Int64 Employeeid, String AttribId)
        {
            DataTable tab = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("Pr_get_RetrievalChkr", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_empid", MySqlDbType.Int32).Value = Employeeid;
                cmd.Parameters.Add("In_Action", MySqlDbType.String).Value = "FileTaken";
                cmd.Parameters.Add("In_Retrivid", MySqlDbType.String).Value = AttribId;
                cmd.Parameters.Add("In_DespatchMode", MySqlDbType.String).Value = "";
                cmd.Parameters.Add("In_Despatchdate", MySqlDbType.String).Value = "";
                cmd.Parameters.Add("In_DespatchNote", MySqlDbType.String).Value = "";
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

        public DataTable getDocnameDocgroupData(string Attribid1)
        {
                DataTable dtt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_DocNameGroupbyAttr", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Attribids", MySqlDbType.Text).Value = Attribid1;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dtt);
                con.Close();
            }
            catch (Exception e) { 
            
            }
            return dtt;
        }
    }
}
