using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using DMS.Model;
using DMS.Data;
using System.Net.Mail;
using System.Net;
 
namespace DMS.Service
{
   public class Login_Service
    {
       Login_Data Objdata = new Login_Data();
       Forgot_Password fpwdobj = new Forgot_Password();
       public DataTable Getusername(Login_Model Objmodel)
       {
          
           return  Objdata.Getusername(Objmodel);
       }

       public DataTable Cpwd(change_password Obmodel)
       {  
           return Objdata.Cpwd(Obmodel);  
       }

       public string EmailManager(string Emp_Code, string Useremailid)
       {         
           fpwdobj.FromEmailId = System.Configuration.ConfigurationManager.AppSettings["FromEmailId"];
           fpwdobj.Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
           fpwdobj.SMTPPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
           fpwdobj.Host = System.Configuration.ConfigurationManager.AppSettings["Host"];
           fpwdobj.ForgotEmailUrl = System.Configuration.ConfigurationManager.AppSettings["ForgotEmailUrl"];
          // all_Employee.ForgotEmailUrl = System.Configuration.ConfigurationManager.AppSettings["ForgotEmailUrl"];
           string Email_id = fpwdobj.FromEmailId;
           int Otp_Num = GenerateRandomNo();
           DataTable dt = new DataTable();
           dt = Objdata.updatepassword(Emp_Code, Otp_Num, Useremailid);
           string message = dt.Rows[0]["message"].ToString();
           if (message == "Success")
           {
               var sub = "DMS Forgot Password OTP ";

               var body = "<p style='font-family:Calibri;font-size:15px'> Please Find Below of  your one Time Password Of Web . <p/>  <table><tr><td style='font-family:Calibri;font-size:15px'>Otp</td> <td>:</td> <td  style='font-family:Calibri;font-size:15px'> " + Convert.ToString(Otp_Num) + "</td> </tr></table> <p style='font-family:Calibri;font-size:15px'> Please Use This To Login your DMS. <a href=" + fpwdobj.ForgotEmailUrl + ">" + fpwdobj.ForgotEmailUrl + "</a><p/>";


              // var sub = "OTP";
              // var body = Convert.ToString(Otp_Num);
               var smtp = new SmtpClient
               {

                   Host = fpwdobj.Host,
                   Port = fpwdobj.SMTPPort,
                   EnableSsl = true,
                   DeliveryMethod = SmtpDeliveryMethod.Network,
                   UseDefaultCredentials = false,
                   Credentials = new NetworkCredential(fpwdobj.FromEmailId, fpwdobj.Password)
               };
               using (var mess = new MailMessage(fpwdobj.FromEmailId, Useremailid)
               {
                   IsBodyHtml = true,
                   Subject = sub,
                   Body = body
               })
               {
                   smtp.Send(mess);
               }
           }        
           return message;
       }

       public int GenerateRandomNo()
       {
           int _min = 1000;
           int _max = 9999;
           Random _rdm = new Random();
           return _rdm.Next(_min, _max);
       }


    }
}
