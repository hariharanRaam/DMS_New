using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DMS.Model
{
    public class Login_Model
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class change_password
    {
        [Required(ErrorMessage = "Please Enter The Emp Id ")]
        public int Emp_Id { get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        [StringLength(6, ErrorMessage = "Must be 6 characters", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*\d)[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = " Password does not match")]
        public string Cpassword { get; set; }
    }

    public class Forgot_Password
    {
        [Required(ErrorMessage = "Please Enter The Emp Id ")]
        // public int Emp_Id { get; set; }
        public string Emp_Code { get; set; }
        [Required(ErrorMessage = "Please Enter Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Emp_EmailId { get; set; }
        public string FromEmailId { get; set; }
        public string Password { get; set; }
        public int SMTPPort { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string ForgotEmailUrl { get; set; }

    }


}
