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
    public class TreeSearch_Data
    {
        public MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        public DataSet GetTreeData()
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_Set_HierarchyTreeView", con);
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

        public DataSet GetAttributes(Int64 DocgroupId, Int64 DocNameId, Int64 _empid)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Pr_get_AttributeTreeView", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("In_action", "Attribute");
                cmd.Parameters.AddWithValue("In_DocgroupId", DocgroupId);
                cmd.Parameters.AddWithValue("In_DocnameId", DocNameId);
                cmd.Parameters.AddWithValue("In_empid", _empid);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetDocuments(string Attributetypes1, string atrname1, string Docgroupid1, string Docnameid1, Int64 _empid, string Dyntextvalues1, string Conditions1, string operators1)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
               /* string[] Attributetypes = Attributetypes1.Split(',');
                string[] atrname = atrname1.Split(',');
                string[] Docgroupid = Docgroupid1.Split(',');
                string[] Docnameid = Docnameid1.Split(',');
                string[] Dyntextvalue = Dyntextvalues1.Split(',');
                if (Attributetypes.Count() == atrname.Count())
                {
                    for (int i = 0; i < Attributetypes.Count(); i++)
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("Pr_get_Documents", con);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("In_action", "Attribute");
                        cmd.Parameters.AddWithValue("In_Docgrp", Convert.ToInt64(Docgroupid[i]));
                        cmd.Parameters.AddWithValue("In_Docname", Convert.ToInt64(Docnameid[i]));
                        cmd.Parameters.AddWithValue("In_atrname", Convert.ToString(atrname[i]));
                        cmd.Parameters.AddWithValue("In_atrtype", Convert.ToString(Attributetypes[i]));
                        cmd.Parameters.AddWithValue("In_empid", _empid);
                        cmd.Parameters.AddWithValue("In_values",Convert.ToString(Dyntextvalue[i]));
                        da.Fill(ds);
                    }
                    con.Close();
                }
                else
                {
                    return ds;
                }*/

                con.Open();
                MySqlCommand cmd = new MySqlCommand("Pr_get_Documents", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("In_action", "Attribute");
                cmd.Parameters.AddWithValue("In_Docgrp", Docgroupid1);
                cmd.Parameters.AddWithValue("In_Docname", Docnameid1);
                cmd.Parameters.AddWithValue("In_atrname", atrname1);
                cmd.Parameters.AddWithValue("In_atrtype", Attributetypes1);
                cmd.Parameters.AddWithValue("In_empid", _empid);
                cmd.Parameters.AddWithValue("In_values", Dyntextvalues1);
                cmd.Parameters.AddWithValue("In_condition", Conditions1);
                cmd.Parameters.AddWithValue("In_Operator", operators1);
                da.Fill(ds);
                con.Close();
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
    }
}
