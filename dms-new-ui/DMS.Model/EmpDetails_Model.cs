using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
    public class EmpDetails_Model
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmpCode { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmpName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Grade { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmpTitle { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Dept { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Unit { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmpDOJ { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmpAddress { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Pin { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string State { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Region { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmpMobileNo { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmpEmailId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmpRole { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmpStatus { get; set; }
    }
}
