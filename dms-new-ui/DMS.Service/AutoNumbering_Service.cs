using DMS.Data;
using DMS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service
{
    public class AutoNumbering_Service
    {
        AutoNumbering_Data getAllautonumbering = new AutoNumbering_Data();

        //read the data
        public DataSet getAlldata()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = getAllautonumbering.getAllautonumbering();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public AutoNumbering_Model AutonumberingIUD(AutoNumbering_Model autonumbermodel)
        {
            return getAllautonumbering.AutoNumberIud(autonumbermodel);
        }
    }
}
