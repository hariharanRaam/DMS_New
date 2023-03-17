using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DMS.Model
{
  public  class Employee_Model
    {
       public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Employee Code Should not be blank!")]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Employee Name Should not be blank!")]
        public string EmployeeName { get; set; }
        public Int32 UserID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select title.!")]
        public Int64 TitleID { get; set; }
        public string Title { get; set; }

        //[Required(ErrorMessage = "Date of joining Should not be blank!")]
      public DateTime DOJ { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "MobileNo Should not be blank!")]
        [StringLength(13, MinimumLength = 10,ErrorMessage="Enter Valid Mobile No.!")]
        public string MobileNo { get; set; }

       public string LanNo { get; set; }

       [Required(ErrorMessage = "EmailID Should not be blank!")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string EmailID { get; set; }

       [Required(ErrorMessage = "Password Should not be blank!")]
        public string Password { get; set; }
       public string EmployeeType { get; set; }

        public string TypeName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select type.!")]
        public Int64 TypeID { get; set; }

       public string City { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select city.!")]
        public Int64 CityID { get; set; }

        public string State { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select state.!")]
        public Int64 StateID { get; set; }

       [Required(ErrorMessage = "Pin Code Should not be blank!")]
        public string Pin { get; set; }
        public Int64 PinID { get; set; }

       public string Region { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select region.!")]
        public Int64 RegionID { get; set; }

       public string Unit { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select unit.!")]
        public Int64 UnitID { get; set; }

       public string Dept_Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select department.!")]
        public Int64 Dept_Id { get; set; }

       public string Grade { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select grade.!")]
        public Int64 GradeID { get; set; }

        public string UserGroup { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select usergroup.!")]
        public Int64 UserGroupID { get; set; }

        public List<Employee_Model> TModel { get; set; }

        public Int64 emptitleId { get; set; }
        public Int64 emptypeId { get; set; }




    }
    }
 
