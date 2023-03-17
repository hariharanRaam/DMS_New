using DMS.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service
{
    public class PiechartService
    {
        Piechartdata pdata = new Piechartdata();

        public DataSet GetDta()
        {
            return pdata.getpdata();
        }

        public DataSet GetDta1()
        {
            return pdata.getpdata1();
        }
        public DataSet GetDt()
        {
            return pdata.getdt();
        }

        public DataSet getdata()
        {
            Piechartdata pidata = new Piechartdata();
            return pidata.getpidata();
        }

    }

    
       
}
