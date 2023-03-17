using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DMS.Data
{
    public class Retrival_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataSet GetChksummary(Int64 _empid, string _actionval)
        {
            DataSet ds = new DataSet();
            try
            {               
                MySqlCommand cmd = new MySqlCommand("Pr_get_RetrievalChkr", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("In_empid", _empid);
                cmd.Parameters.AddWithValue("In_Action", _actionval);
                cmd.Parameters.AddWithValue("In_Retrivid", '0');
                cmd.Parameters.AddWithValue("In_DespatchMode", "0");
                cmd.Parameters.AddWithValue("In_Despatchdate", "0");
                cmd.Parameters.AddWithValue("In_DespatchNote", "0");
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
            return ds;
        }


        public DataSet GetChkdetail(string Retrivid, Int64 _empid)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_RetrievalChkr", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("In_empid", _empid);
                cmd.Parameters.AddWithValue("In_Action", "Detail");
                cmd.Parameters.AddWithValue("In_Retrivid", Retrivid);
                cmd.Parameters.AddWithValue("In_DespatchMode", "0");
                cmd.Parameters.AddWithValue("In_Despatchdate", "0");
                cmd.Parameters.AddWithValue("In_DespatchNote", "0");
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
            return ds;
        }


        public DataTable updateRetrivalData(string Retrivid, string DespatchMode, string Despatchdate, string DespatchNote, Int64 _empid)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_RetrievalChkr", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("In_empid", _empid);
                cmd.Parameters.AddWithValue("In_Action", "Update");
                cmd.Parameters.AddWithValue("In_Retrivid", Retrivid);
                cmd.Parameters.AddWithValue("In_DespatchMode", DespatchMode);
                cmd.Parameters.AddWithValue("In_Despatchdate", Despatchdate);
                cmd.Parameters.AddWithValue("In_DespatchNote", DespatchNote);
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
            return dt;
        }
        public DataSet GetChkdetail(Int64 _empid)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_RetrievalChkr", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("In_empid", _empid);
                cmd.Parameters.AddWithValue("In_Action", "Retrievalchk");
                cmd.Parameters.AddWithValue("In_Retrivid", "0");
                cmd.Parameters.AddWithValue("In_DespatchMode", "0");
                cmd.Parameters.AddWithValue("In_Despatchdate", "0");
                cmd.Parameters.AddWithValue("In_DespatchNote", "0");
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
            return ds;
        }
        public DataTable updatereturnRetrivalData(string Retrivid, string Despatchdate, string DespatchNote, Int64 _empid)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_RetrievalChkr", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("In_empid", _empid);
                cmd.Parameters.AddWithValue("In_Action", "UpdateRtrn");
                cmd.Parameters.AddWithValue("In_Retrivid", Retrivid);
                cmd.Parameters.AddWithValue("In_DespatchMode", '0');
                cmd.Parameters.AddWithValue("In_Despatchdate", Despatchdate);
                cmd.Parameters.AddWithValue("In_DespatchNote", DespatchNote);
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
            return dt;
        }

    }
}
