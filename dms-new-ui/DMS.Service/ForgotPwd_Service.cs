using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using DMS.Model;
using DMS.Data;
 
namespace DMS.Service
{
   public class ForgotPwd_Service
    {
       public DataTable Forgot(Forgot_Password objmdl,int Otp_Num)
       {
           Forgotpwd_Data obmdl = new Forgotpwd_Data();
           return obmdl.Forgot(objmdl, Otp_Num);
       }
    }
}
