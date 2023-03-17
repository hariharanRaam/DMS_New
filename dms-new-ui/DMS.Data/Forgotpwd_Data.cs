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
    public class Forgotpwd_Data
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataTable Forgot(Forgot_Password objmdl,int Otp_Num)
        {

            DataTable dt = new DataTable();
            try
            {
                //var Result = 0;
                // Obmodel.Emp_Id = Convert.ToInt32(Session["Emp_Id"].ToString());

                MySqlCommand cmd = new MySqlCommand("SP_ForgetPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Emp_Id", MySqlDbType.Int32).Value = objmdl.Emp_Code;
                cmd.Parameters.Add("In_Email_Id", MySqlDbType.VarChar).Value = objmdl.Emp_EmailId;
                cmd.Parameters.Add("In_Otp_Num", MySqlDbType.Int32).Value = Otp_Num;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);


                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
