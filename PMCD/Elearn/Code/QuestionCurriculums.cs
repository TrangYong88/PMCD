using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Lib.Utils;
using Lib.Database;
namespace Lib.Elearn
{
    public class QuestionCurriculums
    {
        private int _QuestionCurriculumId;
        private int _QuestionId;
        private int _CurriculumId;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public QuestionCurriculums(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        
        //------------------------------------------------------------------------
        ~QuestionCurriculums()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------

        public int QuestionCurriculumId { get { return _QuestionCurriculumId; } set { _QuestionCurriculumId = value; } }
        public int QuestionId { get { return _QuestionId; } set { _QuestionId = value; } }
        public int CurriculumId { get { return _CurriculumId; } set { _CurriculumId = value; } } 
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.QuestionCurriculumId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((this.QuestionId > 0)
                && (this.CurriculumId > 0)
                )
            {
                RetVal = "INSERT INTO V$QuestionCurriculums(QuestionId,CurriculumId,CrUserId,CrDateTime)";
                RetVal += " VALUES (" + this.QuestionId.ToString();
                RetVal += "," + this.CurriculumId.ToString();
                RetVal += "," + this.CrUserId.ToString();
                RetVal += ",CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlUpdate()
        {
            string RetVal = "";
            if ((this.QuestionCurriculumId > 0)
                && (this.QuestionId > 0)
                && (this.CurriculumId > 0)
                )
            {
                RetVal = "UPDATE QuestionCurriculums SET ";
                RetVal += " QuestionId=" + this.QuestionId.ToString();
                RetVal += ",CurriculumId=" + this.CurriculumId.ToString();  
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (QuestionCurriculumId=" + this.QuestionCurriculumId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal += "DELETE FROM QuestionCurriculums";
                RetVal += " WHERE (QuestionCurriculumId=" + Id.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActQuestionCurriculumId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActQuestionCurriculumId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.QuestionCurriculumId = Id;
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
        public bool Update(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActQuestionCurriculumId)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActQuestionCurriculumId, BuildSqlUpdate());
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public bool Delete(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActQuestionCurriculumId, int Id)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActQuestionCurriculumId, BuildSqlDelete(Id));
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //------------------------------------------------------------------------
        private List<QuestionCurriculums> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<QuestionCurriculums> l_QuestionCurriculums = new List<QuestionCurriculums>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    QuestionCurriculums m_QuestionCurriculums = new QuestionCurriculums(db.ConnectionString);
                    m_QuestionCurriculums.QuestionCurriculumId = smartReader.GetInt32("QuestionCurriculumId");
                    m_QuestionCurriculums.QuestionId = smartReader.GetInt32("QuestionId");
                    m_QuestionCurriculums.CurriculumId = smartReader.GetInt32("CurriculumId");
                    m_QuestionCurriculums.CrUserId = smartReader.GetInt32("CrUserId");
                    m_QuestionCurriculums.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_QuestionCurriculums.Add(m_QuestionCurriculums);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_QuestionCurriculums;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionCurriculums> GetList(string LogFilePath, string LogFileName,int CurriculumId, byte QuestionTypeId, byte QuestionLevelId, string KeyWord)
        {
            string Condition = "";
            if (CurriculumId > 0)
            {
                Condition += " AND (V$QuestionCurriculums.CurriculumId = " + CurriculumId.ToString() + ")";
            }
            if (QuestionLevelId > 0)
            {
                Condition += " AND (V$QuestionLevels.QuestionLevelId = " + QuestionLevelId.ToString() + ")";
                if (QuestionTypeId > 0)
                {
                    Condition += " AND (V$QuestionTypes.QuestionTypeId = " + QuestionTypeId.ToString() + ")";
                }
            }
            else
            {
                if (QuestionTypeId > 0)
                {
                    Condition += " AND (V$QuestionTypes.QuestionTypeId = " + QuestionTypeId.ToString() + ")";
                }
            }

            if (!string.IsNullOrEmpty(KeyWord))
            {
                Condition += " AND (V$Questions.QuestionName = N'" + KeyWord + "')";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionCurriculums> GetList(string LogFilePath, string LogFileName, int CurriculumId)
        {
            string Condition = "";
            if (CurriculumId > 0)
            {
                Condition += " AND V$QuestionCurriculums.CurriculumId = " + CurriculumId.ToString();
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //------------------------------------------------------------------------
        public List<QuestionCurriculums> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<QuestionCurriculums> RetVal = new List<QuestionCurriculums>();
            try
            {
                string Sql = "SELECT * FROM V$QuestionCurriculums";
                Sql += " INNER JOIN V$Curriculums";
                Sql += " ON V$QuestionCurriculums.CurriculumId = V$Curriculums.CurriculumId";
                Sql += " INNER JOIN V$Questions";
                Sql += " ON V$QuestionCurriculums.QuestionId = V$Questions.QuestionId";
                Sql += " INNER JOIN V$QuestionLevels";
                Sql += " ON V$QuestionLevels.QuestionlevelId = V$Questions.QuestionLevelId";
                Sql += " INNER JOIN V$QuestionTypes";
                Sql += " ON V$QuestionTypes.QuestionTypeId = V$Questions.QuestionTypeId";
                if (!string.IsNullOrEmpty(Condition))
                {
                    Sql += Condition;
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
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionCurriculums> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionCurriculums> GetList(string LogFilePath, string LogFileName, int QuestionId, int CurriculumId)
        {
            string Condition = "";
            if (CurriculumId > 0)
            {
                Condition = "(CurriculumId = " + CurriculumId.ToString() + ")";
                if (QuestionId > 0)
                {
                    Condition += "AND (QuestionId = " + QuestionId.ToString() + ")";
                }
            }
            else
            {
                if (QuestionId > 0)
                {
                    Condition = "(QuestionId = " + QuestionId.ToString() + ")";
                }
            }            
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionCurriculums> GetListOrderByName(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "QuestionCurriculumName");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionCurriculums> GetListByQuestionCurriculumId(string LogFilePath, string LogFileName, int QuestionCurriculumId)
        {
            List<QuestionCurriculums> RetVal = new List<QuestionCurriculums>();
            try
            {
                if (QuestionCurriculumId > 0)
                {
                    string Condition = "(QuestionCurriculumId = " + QuestionCurriculumId.ToString() + ")";
                    RetVal = GetList(LogFilePath, LogFileName, Condition, "");
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        
        //------------------------------------------------------------------------
        public List<QuestionCurriculums> GetListByQuestionId(string LogFilePath, string LogFileName, int QuestionId)
        {
            List<QuestionCurriculums> RetVal = new List<QuestionCurriculums>();
            try
            {
                if (QuestionId > 0)
                {
                    string Condition = "(QuestionId = " + QuestionId.ToString() + ")";
                    RetVal = GetList(LogFilePath, LogFileName, Condition, "");
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public QuestionCurriculums Get(string LogFilePath, string LogFileName, int QuestionCurriculumId)
        {
            QuestionCurriculums RetVal = new QuestionCurriculums(db.ConnectionString);
            try
            {
                List<QuestionCurriculums> l_QuestionCurriculums = GetListByQuestionCurriculumId(LogFilePath, LogFileName, QuestionCurriculumId);
                if (l_QuestionCurriculums.Count > 0)
                {
                    RetVal = (QuestionCurriculums)l_QuestionCurriculums[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }

        //--------------------------------------------------------------------------------------------------------------------
        public QuestionCurriculums Get(List<QuestionCurriculums> l_QuestionCurriculums, long QuestionCurriculumId)
        {
            QuestionCurriculums RetVal = new QuestionCurriculums(db.ConnectionString);
            foreach (QuestionCurriculums mQuestionCurriculums in l_QuestionCurriculums)
            {
                if (mQuestionCurriculums.QuestionCurriculumId == QuestionCurriculumId)
                {
                    RetVal = mQuestionCurriculums;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public string GetQuestionIds(List<QuestionCurriculums> lQuestionCurriculums)
        {
            string RetVal = "";
            foreach (QuestionCurriculums mQuestionCurriculums in lQuestionCurriculums)
            {
                RetVal += mQuestionCurriculums.QuestionId.ToString() + ",";
            }
            return StringUtils.RemoveLastString(RetVal, ","); ;
        }
        //------------------------------------------------------------------------------
        public QuestionCurriculums GetUnique(List<QuestionCurriculums> lQuestionCurriculums, int CurriculumId, int QuestionId)
        {
            QuestionCurriculums RetVal = new QuestionCurriculums(db.ConnectionString);
            if (CurriculumId > 0)
            {
                if (QuestionId > 0)
                {
                    foreach (QuestionCurriculums mQuestionCurriculums in lQuestionCurriculums)
                    {
                        if ((mQuestionCurriculums.CurriculumId == CurriculumId) && (mQuestionCurriculums.QuestionId == QuestionId))
                        {
                            RetVal = mQuestionCurriculums;
                            break;
                        }
                    }
                }
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------
        public List<QuestionCurriculums> GetListByCurriculumId(string LogFilePath, string LogFileName, int CurriculumId)
        {
            List<QuestionCurriculums> RetVal = new List<QuestionCurriculums>();
            try
            {
                if (CurriculumId > 0)
                {
                    string Condition = " AND (V$QuestionCurriculums.CurriculumId = " + CurriculumId.ToString() + ")";
                    RetVal = GetList(LogFilePath, LogFileName, Condition, "");
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        
        //-------------------------------------------------------------------------------------------------------------
        public List<QuestionCurriculums> Copy(List<QuestionCurriculums> l_QuestionCurriculums)
        {
            List<QuestionCurriculums> retVal = new List<QuestionCurriculums>();
            foreach (QuestionCurriculums mQuestionCurriculums in l_QuestionCurriculums)
            {
                retVal.Add(mQuestionCurriculums);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<QuestionCurriculums> CopyAll(List<QuestionCurriculums> l_QuestionCurriculums)
        {
            QuestionCurriculums m_QuestionCurriculums = new QuestionCurriculums(db.ConnectionString);
            List<QuestionCurriculums> retVal = m_QuestionCurriculums.Copy(l_QuestionCurriculums);
            retVal.Insert(0, m_QuestionCurriculums);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<QuestionCurriculums> CopyNa(List<QuestionCurriculums> l_QuestionCurriculums)
        {
            QuestionCurriculums m_QuestionCurriculums = new QuestionCurriculums(db.ConnectionString);
            List<QuestionCurriculums> retVal = m_QuestionCurriculums.Copy(l_QuestionCurriculums);
            retVal.Insert(0, m_QuestionCurriculums);
            return retVal;
        }
    }//end QuestionCurriculums
}
