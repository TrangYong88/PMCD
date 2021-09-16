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
    public class Ranks
    {
        private byte _RankId;
        private string _RankName;
        private string _RankDesc;
        DBAccess db;
        //----------------------------------------------------------------
        public Ranks(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
        public Ranks(string constr, string Name, string Desc)
        {
            RankId = 0;
            RankName = Name;
            RankDesc = Desc;
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //----------------------------------------------------------------
        public byte RankId { get { return _RankId; } set { _RankId = value; } }
        public string RankName { get { return _RankName; } set { _RankName = value; } }
        public string RankDesc { get { return _RankDesc; } set { _RankDesc = value; } }
        //----------------------------------------------------------        
        private List<Ranks> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<Ranks> RankList = new List<Ranks>();
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Ranks m_Ranks = new Ranks(db.ConnectionString);
                    m_Ranks.RankId = smartReader.GetByte("RankId");
                    m_Ranks.RankName = smartReader.GetString("RankName");
                    m_Ranks.RankDesc = smartReader.GetString("RankDesc");
                    RankList.Add(m_Ranks);
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
            return RankList;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.RankName)))
            {
                RetVal = "INSERT INTO V$Ranks(RankName, RankDesc)";
                RetVal += " VALUES (N'" + this.RankName + "'";
                RetVal += ",N'" + this.RankDesc + "'";
                RetVal += ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlUpdate()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.RankName))
                )
            {
                RetVal = "UPDATE V$Ranks SET ";
                RetVal += "RankName=N'" + this.RankName + "'";
                RetVal += ",RankDesc=N'" + this.RankDesc + "'";
                RetVal += " WHERE (RankId=" + this.RankId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal = "DELETE FROM Questions";
                RetVal += " WHERE (RankId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM Ranks";
                RetVal += " WHERE (RankId=" + Id.ToString() + ")";
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
                        this.RankId = Convert.ToByte(Id);
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
        public List<Ranks> GetList(string LogFilePath, string LogFileName, string KeyWord)
        {
            string Condition = "";
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "((RankName = N'" + KeyWord + "') OR (RankDesc = N'" + KeyWord + "'))";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------
        public static List<Ranks> Static_GetList(string LogFilePath, string LogFileName, string constr)
        {
            List<Ranks> RetVal = new List<Ranks>();
            Ranks m_Ranks = new Ranks(constr);
            try
            {
                RetVal = m_Ranks.GetList(LogFilePath, LogFileName);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_Ranks.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------
        public List<Ranks> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Ranks> RetVal = new List<Ranks>();
            try
            {
                string Sql = "SELECT * FROM V$Ranks";
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
        public List<Ranks> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------
        public List<Ranks> GetList(string LogFilePath, string LogFileName, byte RankId)
        {
            List<Ranks> RetVal = new List<Ranks>();
            try
            {
                if (RankId > 0)
                {
                    string Condition = "(RankId =" + RankId.ToString() + ")";
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
        public Ranks Get(string LogFilePath, string LogFileName, byte RankId)
        {
            Ranks RetVal = new Ranks(db.ConnectionString);
            try
            {
                List<Ranks> List = GetList(LogFilePath, LogFileName, RankId);
                if (List.Count > 0)
                {
                    RetVal = (Ranks)List[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------
        public Ranks Get(List<Ranks> lRanks, byte RankId)
        {
            Ranks RetVal = new Ranks(db.ConnectionString);
            if (RankId > 0)
            {
                foreach (Ranks mRanks in lRanks)
                {
                    if (mRanks.RankId == RankId)
                    {
                        RetVal = mRanks;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Ranks> Copy(List<Ranks> lRanks)
        {
            List<Ranks> RetVal = new List<Ranks>();
            foreach (Ranks mRanks in lRanks)
            {
                RetVal.Add(mRanks);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Ranks> CopyAll(List<Ranks> lRanks)
        {
            List<Ranks> RetVal = Copy(lRanks);
            RetVal.Insert(0, new Ranks(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC));
            return RetVal;
        }
    }//end Ranks
}//end 