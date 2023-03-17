using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using DMS.Model;
using System.IO;
using System.Web;

namespace DMS.Data
{
    public class Login_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataTable Getusername(Login_Model Objmodel)
        {
            
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_Login", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_UserName", MySqlDbType.VarChar).Value = Objmodel.UserName;
                cmd.Parameters.Add("In_Password", MySqlDbType.VarChar).Value = Objmodel.Password;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
           }        
            finally
            { con.Close(); }
        }

        public DataTable Cpwd(change_password Obmodel)
        {

            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_ChangePasword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Emp_Id", MySqlDbType.Int32).Value = Obmodel.Emp_Id;
                cmd.Parameters.Add("In_OldPassword", MySqlDbType.VarChar).Value = Obmodel.OldPassword;
                cmd.Parameters.Add("In_NewPassword", MySqlDbType.VarChar).Value = Obmodel.NewPassword;
                cmd.Parameters.Add("In_CPassword", MySqlDbType.VarChar).Value = Obmodel.Cpassword;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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

        public DataTable updatepassword(string Emp_Code, int Otp_Num, string Useremailid)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_ForgetPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_UserID", MySqlDbType.VarChar).Value = Emp_Code;
                cmd.Parameters.Add("In_OTPNum", MySqlDbType.Int32).Value = Otp_Num;
                cmd.Parameters.Add("In_Useremailid", MySqlDbType.VarChar).Value = Useremailid;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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

    }
}
