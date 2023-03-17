using DMS.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Model;
using System.Reflection;

namespace DMS.Service
{
    public class StorageAttributes_Service
    {

        //log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ConfigureAttributesController)); 
        Storageattribute_Data Strobjdata = new Storageattribute_Data();

        public DataSet SaveStorageAttri(string Str_Name, int Str_Length, string Str_Type, string Str_Mandotry, int Storage_orderid, int Dgroup_id, int DName_id, int UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = Strobjdata.SaveStorageattrib(Str_Name, Str_Length, Str_Type, Str_Mandotry, Storage_orderid, Dgroup_id, DName_id, UserID);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //01-04-2019 update and Delete

        public DataSet UpdateStorageAttri(Int64 attrgid, string Str_Name, int Str_Length, string Str_Type, string Str_Mandotry, Int32 orderid, int Dgroup_id, int DName_id, int UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = Strobjdata.UpdateStorageattrib(attrgid, Str_Name, Str_Length, Str_Type, Str_Mandotry, orderid, Dgroup_id, DName_id, UserID);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //for delete
        public DataSet DeleteStorageAttri(int strid)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = Strobjdata.DeleteStorageAttri(strid);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //


        public DataSet CheckSaveStorageattrib( int? DgroupID, int? DNameID)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = Strobjdata.CheckSaveStr( DgroupID, DNameID);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //30-03-2019

        public DataSet AttributeBind(int groupid)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = Strobjdata.AttributeBind(groupid);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //04-04-2019 View and add same as
        public DataTable GSameasAttributeValues(int Docname_id)
        {
            return Strobjdata.GSameasAttributeValues(Docname_id);
        }


        public DataTable GetSameasAttributeplusdata(int Docname_id)
        {
            return Strobjdata.GetSameasAttributeplusdata(Docname_id);
        }

        //



        //save mandotry fields
        public DataSet saveStorageAtt(int DgroupID, int DNameID, int UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = Strobjdata.savemandotry( DgroupID, DNameID, UserID);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
