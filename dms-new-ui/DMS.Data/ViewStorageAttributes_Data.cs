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
    public class ViewStorageAttributes_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataSet GetDynamicStorageAttributes()
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("SP_GetStorageAttributeDynamicName", con);
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

        public DataTable GetStorageAttributeValues(int GroupAtrID)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_viewstorageattributevalues", con);
            cmd.Parameters.Add("In_Satrid", MySqlDbType.Int32).Value = GroupAtrID;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        //30-03-2019

        public DataTable GetStorageAttributeValuesEdit(int GroupAtrID)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_GetDocumentattributevalues", con);
            cmd.Parameters.Add("IN_Satr_GId", MySqlDbType.Int32).Value = GroupAtrID;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


    }
}
