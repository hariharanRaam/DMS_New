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
   public  class Piechartdata
    {


        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        public DataSet getpdata()
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("SP_PieandBarchart", con);
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

        public DataSet getpdata1()
        {
            try
            {
                 DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("sp_barchart", con);
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

        public DataSet getdt()
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("SP_DashBoardDetails", con);
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


        public DataSet getpidata()
        {
            try
            {
                DataSet ds = new DataSet();
                //DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_PieandBarchartdatacount", con);
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


    }
}
