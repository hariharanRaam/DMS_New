using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
    public class AutoNumbering_Model
    {
        public string autonumber_rowid { get; set; }
        public string autonumber_prefix { get; set; }
        public string autonumber_year { get; set; }
        public string autonumber_month { get; set; }
        public string autonumber_date { get; set; }
        public string autonumber_sequence { get; set; }
        public string modifed_by { get; set; }
        public string mode { get; set; }
        public string createdby { get; set; }
        public string resultcode { get; set; }
        public string resultstatus { get; set; }
    }
}
