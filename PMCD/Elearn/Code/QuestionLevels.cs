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
    public class QuestionLevels
    {
        private byte _QuestionLevelId;
        private string _QuestionLevelName;
        private string _QuestionLevelDesc;
        DBAccess db;
        //----------------------------------------------------------------
        public QuestionLevels(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
        public QuestionLevels(string constr, string Name, string Desc)
        {
            QuestionLevelId = 0;
            QuestionLevelName = Name;
            QuestionLevelDesc = Desc;
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //----------------------------------------------------------------
        public byte QuestionLevelId { get { return _QuestionLevelId; } set { _QuestionLevelId = value; } }
        public string QuestionLevelName { get { return _QuestionLevelName; } set { _QuestionLevelName = value; } }
        public string QuestionLevelDesc { get { return _QuestionLevelDesc; } set { _QuestionLevelDesc = value; } }
        //----------------------------------------------------------        
        private List<QuestionLevels> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<QuestionLevels> QuestionLevelList = new List<QuestionLevels>();
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    QuestionLevels m_QuestionLevels = new QuestionLevels(db.ConnectionString);
                    m_QuestionLevels.QuestionLevelId = smartReader.GetByte("QuestionLevelId");
                    m_QuestionLevels.QuestionLevelName = smartReader.GetString("QuestionLevelName");
                    m_QuestionLevels.QuestionLevelDesc = smartReader.GetString("QuestionLevelDesc");
                    QuestionLevelList.Add(m_QuestionLevels);
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
            return QuestionLevelList;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.QuestionLevelName)))
            {
                RetVal = "INSERT INTO V$QuestionLevels(QuestionLevelName, QuestionLevelDesc)";
                RetVal += " VALUES (N'" + this.QuestionLevelName + "'";
                RetVal += ",N'" + this.QuestionLevelDesc + "'";
                RetVal += ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlUpdate()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.QuestionLevelName))
                )
            {
                RetVal = "UPDATE V$QuestionLevels SET ";
                RetVal += "QuestionLevelName=N'" + this.QuestionLevelName + "'";
                RetVal += ",QuestionLevelDesc=N'" + this.QuestionLevelDesc + "'";
                RetVal += " WHERE (QuestionLevelId=" + this.QuestionLevelId.ToString() + ")";
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
                RetVal += " WHERE (QuestionLevelId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM QuestionLevels";
                RetVal += " WHERE (QuestionLevelId=" + Id.ToString() + ")";
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
                        this.QuestionLevelId = Convert.ToByte(Id);
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
        public List<QuestionLevels> GetList(string LogFilePath, string LogFileName, string KeyWord)
        {
            string Condition = "";
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "((QuestionLevelName = N'" + KeyWord + "') OR (QuestionLevelDesc = N'" + KeyWord + "'))";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------
        public static List<QuestionLevels> Static_GetList(string LogFilePath, string LogFileName, string constr)
        {
            List<QuestionLevels> RetVal = new List<QuestionLevels>();
            QuestionLevels m_QuestionLevels = new QuestionLevels(constr);
            try
            {
                RetVal = m_QuestionLevels.GetList(LogFilePath, LogFileName);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_QuestionLevels.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------
        public List<QuestionLevels> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<QuestionLevels> RetVal = new List<QuestionLevels>();
            try
            {
                string Sql = "SELECT * FROM V$QuestionLevels";
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
        public List<QuestionLevels> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------
        public List<QuestionLevels> GetList(string LogFilePath, string LogFileName, byte QuestionLevelId)
        {
            List<QuestionLevels> RetVal = new List<QuestionLevels>();
            try
            {
                if (QuestionLevelId > 0)
                {
                    string Condition = "(QuestionLevelId =" + QuestionLevelId.ToString() + ")";
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
        public QuestionLevels Get(string LogFilePath, string LogFileName, byte QuestionLevelId)
        {
            QuestionLevels RetVal = new QuestionLevels(db.ConnectionString);
            try
            {
                List<QuestionLevels> List = GetList(LogFilePath, LogFileName, QuestionLevelId);
                if (List.Count > 0)
                {
                    RetVal = (QuestionLevels)List[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------
        public QuestionLevels Get(List<QuestionLevels> lQuestionLevels, byte QuestionLevelId)
        {
            QuestionLevels RetVal = new QuestionLevels(db.ConnectionString);
            if (QuestionLevelId > 0)
            {
                foreach (QuestionLevels mQuestionLevels in lQuestionLevels)
                {
                    if (mQuestionLevels.QuestionLevelId == QuestionLevelId)
                    {
                        RetVal = mQuestionLevels;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionLevels> Copy(List<QuestionLevels> lQuestionLevels)
        {
            List<QuestionLevels> RetVal = new List<QuestionLevels>();
            foreach (QuestionLevels mQuestionLevels in lQuestionLevels)
            {
                RetVal.Add(mQuestionLevels);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionLevels> CopyAll(List<QuestionLevels> lQuestionLevels)
        {
            List<QuestionLevels> RetVal = Copy(lQuestionLevels);
            RetVal.Insert(0, new QuestionLevels(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC));
            return RetVal;
        }
    }//end QuestionLevels
}//end 