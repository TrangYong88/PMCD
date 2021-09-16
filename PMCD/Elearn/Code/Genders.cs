using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Lib.Database;
using Lib.Utils;
namespace Lib.Elearn
{
    public class Genders
    {
        private byte _GenderId;
        private string _GenderName;
        private string _GenderDesc;
        DBAccess db;
        //----------------------------------------------------------------
        public Genders(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
        public Genders(string constr, string Name, string Desc)
        {
            GenderId = 0;
            GenderName = Name;
            GenderDesc = Desc;
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //----------------------------------------------------------------
        public byte GenderId { get { return _GenderId; } set { _GenderId = value; } }
        public string GenderName { get { return _GenderName; } set { _GenderName = value; } }
        public string GenderDesc { get { return _GenderDesc; } set { _GenderDesc = value; } }
        //----------------------------------------------------------        
        private List<Genders> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<Genders> GenderList = new List<Genders>();
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Genders m_Genders = new Genders(db.ConnectionString);
                    m_Genders.GenderId = smartReader.GetByte("GenderId");
                    m_Genders.GenderName = smartReader.GetString("GenderName");
                    m_Genders.GenderDesc = smartReader.GetString("GenderDesc");
                    GenderList.Add(m_Genders);
                }
                smartReader.DisposeReader(reader);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                con.Close();
            }
            return GenderList;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.GenderName)))
            {
                RetVal = "INSERT INTO V$Genders(GenderName, GenderDesc)";
                RetVal += " VALUES (N'" + this.GenderName + "'";
                RetVal += ",N'" + this.GenderDesc + "'";
                RetVal += ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlUpdate()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.GenderName))
                )
            {
                RetVal = "UPDATE V$Genders SET ";
                RetVal += "GenderName=N'" + this.GenderName + "'";
                RetVal += ",GenderDesc=N'" + this.GenderDesc + "'";
                RetVal += " WHERE (GenderId=" + this.GenderId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal = "DELETE FROM Users";
                RetVal += " WHERE (GenderId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM Genders";
                RetVal += " WHERE (GenderId=" + Id.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActUserId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.GenderId = Convert.ToByte(Id);
                        RetVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }

        //-----------------------------------------------------------------------------------------------------------------
        public bool Update(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserId)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActUserId, BuildSqlUpdate());
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public bool Delete(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserId, byte Id)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActUserId, BuildSqlDelete(Id));
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Genders> GetList(string LogFilePath, string LogFileName, string KeyWord)
        {
            string Condition = "";
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "((GenderName = N'" + KeyWord + "') OR (GenderDesc = N'" + KeyWord + "'))";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------
        public static List<Genders> Static_GetList(string LogFilePath, string LogFileName, string constr)
        {
            List<Genders> RetVal = new List<Genders>();
            Genders m_Genders = new Genders(constr);
            try
            {
                RetVal = m_Genders.GetList(LogFilePath, LogFileName);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_Genders.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------
        public List<Genders> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Genders> RetVal = new List<Genders>();
            try
            {
                string Sql = "SELECT * FROM V$Genders";
                if (!string.IsNullOrEmpty(Condition))
                {
                    Sql += " WHERE (" + Condition + ")";
                }
                if (!string.IsNullOrEmpty(OrderBy))
                {
                    Sql += " ORDER BY " + OrderBy;
                }
                SqlCommand cmd = new SqlCommand(Sql);
                cmd.CommandType = CommandType.Text;
                RetVal = Init(LogFilePath, LogFileName, cmd);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Genders> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------
        public List<Genders> GetList(string LogFilePath, string LogFileName, byte GenderId)
        {
            List<Genders> RetVal = new List<Genders>();
            try
            {
                if (GenderId > 0)
                {
                    string Condition = "(GenderId =" + GenderId.ToString() + ")";
                    RetVal = GetList(LogFilePath, LogFileName, Condition, ""); ;
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------
        public Genders Get(string LogFilePath, string LogFileName, byte GenderId)
        {
            Genders RetVal = new Genders(db.ConnectionString);
            try
            {
                List<Genders> List = GetList(LogFilePath, LogFileName, GenderId);
                if (List.Count > 0)
                {
                    RetVal = (Genders)List[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------
        public Genders Get(List<Genders> lGenders, byte GenderId)
        {
            Genders RetVal = new Genders(db.ConnectionString);
            if (GenderId > 0)
            {
                foreach (Genders mGenders in lGenders)
                {
                    if (mGenders.GenderId == GenderId)
                    {
                        RetVal = mGenders;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Genders> Copy(List<Genders> lGenders)
        {
            List<Genders> RetVal = new List<Genders>();
            foreach (Genders mGenders in lGenders)
            {
                RetVal.Add(mGenders);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Genders> CopyAll(List<Genders> lGenders)
        {
            List<Genders> RetVal = Copy(lGenders);
            RetVal.Insert(0, new Genders(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC));
            return RetVal;
        }
    }//end Genders
}//end 