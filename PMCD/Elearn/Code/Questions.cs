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
    public class Questions
    {
        private int _QuestionId;
        private byte _QuestionTypeId;
        private byte _QuestionLevelId;
        private string _QuestionName;
        private string _QuestionDesc;
        private byte _QuestionMaxTime;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public Questions(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
        public Questions(string constr, string Name, string Desc)
        {
            this.QuestionId = 0;
            this.QuestionName = Name;
            this.QuestionDesc = Desc;
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //------------------------------------------------------------------------
        ~Questions()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------

        public int QuestionId { get { return _QuestionId; } set { _QuestionId = value; } }
        public byte QuestionTypeId { get { return _QuestionTypeId; } set { _QuestionTypeId = value; } }
        public byte QuestionLevelId { get { return _QuestionLevelId; } set { _QuestionLevelId = value; } }
        public string QuestionName { get { return _QuestionName; } set { _QuestionName = value; } }
        public string QuestionDesc { get { return _QuestionDesc; } set { _QuestionDesc = value; } }
        public byte QuestionMaxTime { get { return _QuestionMaxTime; } set { _QuestionMaxTime = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.QuestionId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((this.QuestionTypeId > 0)
                && (!string.IsNullOrEmpty(this.QuestionName))
                )
            {
                RetVal = "INSERT INTO V$Questions(QuestionTypeId,QuestionLevelId,QuestionName,QuestionDesc,QuestionMaxTime,CrUserId,CrDateTime)";
                RetVal += " VALUES (" + this.QuestionTypeId.ToString();
                RetVal += "," + this.QuestionLevelId.ToString();
                RetVal += ",N'" + this.QuestionName + "'";
                RetVal += ",N'" + this.QuestionDesc + "'";  
                RetVal += "," + this.QuestionMaxTime.ToString();
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
            if ((this.QuestionId > 0)
                && (this.QuestionTypeId > 0)
                && (!string.IsNullOrEmpty(this.QuestionName))
                )
            {
                RetVal = "UPDATE Questions SET ";
                RetVal += " QuestionTypeId=" + this.QuestionTypeId.ToString();
                RetVal += ",QuestionLevelId=" + this.QuestionLevelId.ToString();
                RetVal += ",QuestionName=N'" + this.QuestionName + "'";
                RetVal += ",QuestionDesc=N'" + this.QuestionDesc + "'";
                RetVal += ",QuestionMaxTime=" + this.QuestionMaxTime.ToString();
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (QuestionId=" + this.QuestionId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal += "DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE ClassTestUserQuestionAnswers.AnswerId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.AnswerId";
                RetVal += " FROM ClassTestUserQuestionAnswers";
                RetVal += " INNER JOIN Answers";
                RetVal += " ON ClassTestUserQuestionAnswers.AnswerId = Answers.AnswerId";
                RetVal += " INNER JOIN Questions";
                RetVal += " ON Questions.QuestionId = Answers.QuestionId";
                RetVal += " AND Questions.QuestionId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM Answers";
                RetVal += " WHERE QuestionId=" + Id.ToString() ;
                RetVal += " DELETE FROM QuestionCurriculums";
                RetVal += " WHERE QuestionCurriculums.QuestionId=" + Id.ToString();
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers";
                RetVal += " INNER JOIN ClassTestUserQuestions";
                RetVal += " ON ClassTestUserQuestionAnswers.ClassTestUserQuestionId = ClassTestUserQuestions.ClassTestUserQuestionId";
                RetVal += " INNER JOIN ClassTestQuestions";
                RetVal += " ON ClassTestUserQuestions.ClassTestQuestionId = ClassTestQuestions.ClassTestQuestionId";
                RetVal += " INNER JOIN Questions";
                RetVal += " ON ClassTestQuestions.QuestionId = Questions.QuestionId";
                RetVal += " AND Questions.QuestionId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " FROM ClassTestUserQuestions";
                RetVal += " INNER JOIN ClassTestQuestions";
                RetVal += " ON ClassTestUserQuestions.ClassTestQuestionId = ClassTestQuestions.ClassTestQuestionId";
                RetVal += " INNER JOIN Questions";
                RetVal += " ON Questions.QuestionId = ClassTestQuestions.QuestionId";
                RetVal += " AND Questions.QuestionId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestQuestions";
                RetVal += " WHERE QuestionId =" + Id.ToString();
                RetVal += " DELETE FROM Questions";
                RetVal += " WHERE QuestionId= " + Id.ToString();
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActQuestionId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActQuestionId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.QuestionId = Id;
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
        public bool Update(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActQuestionId)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActQuestionId, BuildSqlUpdate());
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public bool Delete(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActQuestionId, int Id)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActQuestionId, BuildSqlDelete(Id));
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //------------------------------------------------------------------------
        private List<Questions> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<Questions> l_Questions = new List<Questions>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Questions m_Questions = new Questions(db.ConnectionString);
                    m_Questions.QuestionId = smartReader.GetInt32("QuestionId");
                    m_Questions.QuestionTypeId = smartReader.GetByte("QuestionTypeId");
                    m_Questions.QuestionLevelId = smartReader.GetByte("QuestionLevelId");
                    m_Questions.QuestionName = smartReader.GetString("QuestionName");
                    m_Questions.QuestionDesc = smartReader.GetString("QuestionDesc");
                    m_Questions.QuestionMaxTime = smartReader.GetByte("QuestionMaxTime");
                    m_Questions.CrUserId = smartReader.GetInt32("CrUserId");
                    m_Questions.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_Questions.Add(m_Questions);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_Questions;
        }
        //------------------------------------------------------------------------
        public List<Questions> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Questions> RetVal = new List<Questions>();
            try
            {
                string Sql = "SELECT * FROM V$Questions";
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
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Questions> GetList(string LogFilePath, string LogFileName, int ClassTestId)
        {
            string Condition = "";
            if (ClassTestId > 0)
            {
                Condition = " AND (V$ClassTests.ClassTestId = " + ClassTestId.ToString() + ")";
            }
            return GetList2(LogFilePath, LogFileName, Condition, "");
        }
        //------------------------------------------------------------------------
        public List<Questions> GetList3(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Questions> RetVal = new List<Questions>();
            try
            {
                string Sql = "SELECT * FROM V$Questions";
                Sql += " INNER JOIN V$QuestionCurriculums";
                Sql += " ON V$QuestionCurriculums.QuestionId = V$Questions.QuestionId";
                Sql += " INNER JOIN V$Curriculums";
                Sql += " ON V$Curriculums.CurriculumId = V$QuestionCurriculums.CurriculumId";
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
        //------------------------------------------------------------------------
        public List<Questions> GetList2(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Questions> RetVal = new List<Questions>();
            try
            {
                string Sql = "SELECT * FROM V$Questions";
                Sql += " INNER JOIN V$QuestionCurriculums";
                Sql += " ON V$QuestionCurriculums.QuestionId = V$Questions.QuestionId";
                Sql += " INNER JOIN V$Curriculums";
                Sql += " ON V$Curriculums.CurriculumId = V$QuestionCurriculums.CurriculumId";
                Sql += " INNER JOIN V$Classes";
                Sql += " ON V$Classes.CurriculumId = V$Curriculums.CurriculumId";
                Sql += " INNER JOIN V$ClassTests";
                Sql += " ON V$ClassTests.ClassId = V$Classes.ClassId";
                Sql += " INNER JOIN V$QuestionLevels";
                Sql += " ON V$QuestionLevels.QuestionlevelId = V$Questions.QuestionLevelId";
                Sql += " INNER JOIN V$QuestionTypes";
                Sql += " ON V$QuestionTypes.QuestionTypeId = V$Questions.QuestionTypeId";
                if (!string.IsNullOrEmpty(Condition))
                {
                    Sql += Condition ;
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
        public List<Questions> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Questions> GetList(string LogFilePath, string LogFileName, byte QuestionTypeId, byte QuestionLevelId, string KeyWord)
        {
            string Condition = "";
            if (QuestionTypeId > 0)
            {
                Condition += " (QuestionTypeId = " + QuestionTypeId.ToString() + ")";
                if (QuestionLevelId > 0)
                {
                    Condition += " AND (QuestionLevelId = " + QuestionLevelId.ToString() + ")";
                }
            }
            else
            {
                if (QuestionLevelId > 0)
                {
                    Condition += " (QuestionLevelId = " + QuestionLevelId.ToString() + ")";
                }
            }

            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "(QuestionName = N'" + KeyWord + "')";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Questions> GetList(string LogFilePath, string LogFileName, int ClassTestId, byte QuestionTypeId, byte QuestionLevelId, string KeyWord)
        {
            string Condition = "";
            if (ClassTestId > 0)
            {
                Condition += " AND (V$ClassTests.ClassTestId = " + ClassTestId.ToString() + ")";
            }
            if (QuestionLevelId > 0)
            {
                Condition += " AND (V$QuestionLevels.QuestionLevelId = " + QuestionLevelId.ToString() + ")";
                if (QuestionTypeId > 0)
                {
                    Condition += "AND (V$QuestionTypes.QuestionTypeId = " + QuestionTypeId.ToString() + ")";
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
                Condition += " AND (V$Questions.QuestionName =N'"+KeyWord+"')";
            }
            return GetList2(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Questions> GetListOrderByName(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "QuestionName");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Questions> GetListByQuestionId(string LogFilePath, string LogFileName, int QuestionId)
        {
            List<Questions> RetVal = new List<Questions>();
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
        public List<Questions> GetListByQuestionName(string LogFilePath, string LogFileName, string QuestionName)
        {
            List<Questions> RetVal = new List<Questions>();
            try
            {
                QuestionName = StringUtils.Static_InjectionString(QuestionName);
                if (!string.IsNullOrEmpty(QuestionName))
                {
                    string Condition = "(QuestionName =N'" + QuestionName + "')";
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

        public List<Questions> GetListUnique(string LogFilePath, string LogFileName, string QuestionName)
        {
            List<Questions> RetVal = new List<Questions>();
            try
            {
                if (!string.IsNullOrEmpty(QuestionName))
                {
                    string Condition = " (QuestionName = N'" + QuestionName + "')";
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
        public List<Questions> GetListByQuestionTypeId(string LogFilePath, string LogFileName, int QuestionTypeId)
        {
            List<Questions> RetVal = new List<Questions>();
            try
            {
                if (QuestionTypeId > 0)
                {
                    string Condition = "(QuestionTypeId = " + QuestionTypeId.ToString() + ")";
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
        public Questions Get(string LogFilePath, string LogFileName, int QuestionId)
        {
            Questions RetVal = new Questions(db.ConnectionString);
            try
            {
                List<Questions> l_Questions = GetListByQuestionId(LogFilePath, LogFileName, QuestionId);
                if (l_Questions.Count > 0)
                {
                    RetVal = (Questions)l_Questions[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Questions GetByQuestionName(string LogFilePath, string LogFileName, string QuestionName)
        {
            Questions RetVal = new Questions(db.ConnectionString);
            try
            {
                List<Questions> l_Questions = GetListByQuestionName(LogFilePath, LogFileName, QuestionName);
                if (l_Questions.Count > 0)
                {
                    RetVal = (Questions)l_Questions[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public int GetQuestionId(string QuestionName, string QuestionDesc, List<Questions> l_Questions)
        {
            for (int i = 0; i < l_Questions.Count; i++)
            {
                if (l_Questions[i].QuestionName.Equals(QuestionName) && l_Questions[i].QuestionDesc.Equals(QuestionDesc))
                {
                    return l_Questions[i].QuestionId;
                }
            }
            return 0;

        }
        //--------------------------------------------------------------------------------------------------------------------
        public Questions Get(List<Questions> l_Questions, long QuestionId)
        {
            Questions RetVal = new Questions(db.ConnectionString);
            foreach (Questions mQuestions in l_Questions)
            {
                if (mQuestions.QuestionId == QuestionId)
                {
                    RetVal = mQuestions;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public string GetQuestionTypeIds(List<Questions> lQuestions)
        {
            string RetVal = "";
            foreach (Questions mQuestions in lQuestions)
            {
                RetVal += mQuestions.QuestionTypeId.ToString() + ",";
            }
            return StringUtils.RemoveLastString(RetVal, ","); ;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Questions GetUnique(List<Questions> lQuestions, string QuestionName)
        {
            Questions RetVal = new Questions(db.ConnectionString);

            if (!string.IsNullOrEmpty(QuestionName))
            {
                foreach (Questions mQuestions in lQuestions)
                {
                    if (mQuestions.QuestionName == QuestionName)
                    {
                        RetVal = mQuestions;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Questions GetUnique(string LogFilePath, string LogFileName, string QuestionName)
        {
            Questions RetVal = new Questions(db.ConnectionString);
            try
            {
                List<Questions> l_Questions = GetListUnique(LogFilePath, LogFileName, QuestionName);
                if (l_Questions.Count > 0)
                {
                    RetVal = (Questions)l_Questions[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Questions> Copy(List<Questions> l_Questions)
        {
            List<Questions> retVal = new List<Questions>();
            foreach (Questions mQuestions in l_Questions)
            {
                retVal.Add(mQuestions);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Questions> CopyAll(List<Questions> l_Questions)
        {
            Questions m_Questions = new Questions(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC);
            List<Questions> retVal = m_Questions.Copy(l_Questions);
            retVal.Insert(0, m_Questions);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Questions> CopyNa(List<Questions> l_Questions)
        {
            Questions m_Questions = new Questions(db.ConnectionString, StringUtils.NA, StringUtils.NA_DESC);
            List<Questions> retVal = m_Questions.Copy(l_Questions);
            retVal.Insert(0, m_Questions);
            return retVal;
        }
    }//end Questions
}
