using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Data;

namespace DMS.Service
{
    public class DespatchtRetrievalServices
    {
        Retrival_Data Data = new Retrival_Data();
        public DataSet GetChksummary(Int64 _empid,string _actionval)
        {
            DataSet ds = new DataSet();
            ds = Data.GetChksummary(_empid, _actionval);
            return ds;
        }

        public DataSet GetChkdetail(string Retrivid, Int64 _empid)
        {
            DataSet ds = new DataSet();
            ds = Data.GetChkdetail(Retrivid, _empid);
            return ds;
        }

        public DataTable updatereturnRetrivalData(string Retrivid, string Despatchdate, string DespatchNote, Int64 _empid)
        {
            DataTable dt = new DataTable();
            dt = Data.updatereturnRetrivalData(Retrivid, Despatchdate, DespatchNote, _empid);
            return dt;
        }

        public DataTable updateRetrivalData(string Retrivid, string DespatchMode, string Despatchdate, string DespatchNote, Int64 _empid)
        {
            DataTable dt = new DataTable();
            dt = Data.updateRetrivalData(Retrivid,DespatchMode,Despatchdate,DespatchNote,_empid);
            return dt;
        }
        public DataSet GetChkdetail(Int64 _empid)
        {
            DataSet ds = new DataSet();
            ds = Data.GetChkdetail(_empid);
            return ds;
        }
    }
}
