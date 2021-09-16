using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using Lib.Utils;
using Lib.Database;
namespace Lib.Elearn
{
    public class ClassTestQuestions
    {
        private int _ClassTestQuestionId;
        private int _QuestionId;
        private int _ClassTestId;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        public int ClassTestQuestionId { get { return _ClassTestQuestionId; } set { _ClassTestQuestionId = value; } }
        public int QuestionId { get { return _QuestionId; } set { _QuestionId = value; } }
        public int ClassTestId { get { return _ClassTestId; } set { _ClassTestId = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public ClassTestQuestions(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
        ~ClassTestQuestions()
        {
        }
        //-------------------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if (this.ClassTestId > 0 && this.QuestionId > 0)
            {
                RetVal = "INSERT INTO V$ClassTestQuestions(QuestionId, ClassTestId, CrUserId, CrDateTime)";
                RetVal += " VALUES (" + this.QuestionId.ToString();
                RetVal += "," + this.ClassTestId.ToString();
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
            if (this.QuestionId > 0 && this.ClassTestId > 0
                )
            {
                RetVal = "UPDATE V$ClassTestQuestions SET ";
                RetVal += "QuestionId=" + this.QuestionId.ToString();
                RetVal += ",ClassTestId=" + this.ClassTestId.ToString();
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (ClassTestQuestionId=" + this.ClassTestQuestionId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int QuestionId, int ClassTestId)
        {
            string RetVal = "";
            if (QuestionId > 0 && ClassTestId > 0)
            {
                RetVal = "DELETE FROM ClassTestQuestions";
                RetVal += " WHERE (QuestionId=" + QuestionId.ToString() + ")";
                RetVal += " AND (ClassTestId=" + ClassTestId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int ClassTestQuestionId)
        {
            string RetVal = "";
            if (ClassTestQuestionId > 0)
            {
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestQuestions";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId = ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestQuestions.ClassTestQuestionId =" + ClassTestQuestionId.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE (ClassTestQuestionId=" + ClassTestQuestionId.ToString() + ")";
                RetVal += " DELETE FROM ClassTestQuestions";
                RetVal += " WHERE (ClassTestQuestionId=" + ClassTestQuestionId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActClassTestId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassTestId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.ClassTestQuestionId = Id;
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
        public bool Update(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActClassTestId)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassTestId, BuildSqlUpdate());
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public bool Delete(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActClassTestId, int Id)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassTestId, BuildSqlDelete(Id));
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private List<ClassTestQuestions> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<ClassTestQuestions> l_ClassTestQuestions = new List<ClassTestQuestions>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {

                    ClassTestQuestions m_ClassTestQuestions = new ClassTestQuestions(db.ConnectionString);
                    m_ClassTestQuestions.ClassTestQuestionId = smartReader.GetInt32("ClassTestQuestionId");
                    m_ClassTestQuestions.QuestionId = smartReader.GetInt32("QuestionId");
                    m_ClassTestQuestions.ClassTestId = smartReader.GetInt32("ClassTestId");
                    m_ClassTestQuestions.CrUserId = smartReader.GetInt32("CrUserId");
                    m_ClassTestQuestions.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_ClassTestQuestions.Add(m_ClassTestQuestions);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_ClassTestQuestions;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestQuestions> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<ClassTestQuestions> retVal = new List<ClassTestQuestions>();
            try
            {
                string Sql = "SELECT * FROM V$ClassTestQuestions";
                Sql += " INNER JOIN V$ClassTests";
                Sql += " ON V$ClassTestQuestions.ClassTestId = V$ClassTests.ClassTestId";
                Sql += " INNER JOIN V$Questions";
                Sql += " ON V$ClassTestQuestions.QuestionId = V$Questions.QuestionId";
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
                retVal = Init(LogFilePath, LogFileName, cmd);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;

        }
        //------------------------------------------------------------------------------
        public ClassTestQuestions GetUnique(List<ClassTestQuestions> lClassTestQuestions, int ClassTestId, int QuestionId)
        {
            ClassTestQuestions RetVal = new ClassTestQuestions(db.ConnectionString);
            if (ClassTestId > 0)
            {
                if (QuestionId > 0)
                {
                    foreach (ClassTestQuestions mClassTestQuestions in lClassTestQuestions)
                    {
                        if ((mClassTestQuestions.ClassTestId == ClassTestId) && (mClassTestQuestions.QuestionId == QuestionId))
                        {
                            RetVal = mClassTestQuestions;
                            break;
                        }
                    }
                }
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------
        public List<ClassTestQuestions> GetListByClassTestId(string LogFilePath, string LogFileName, int ClassTestId)
        {
            List<ClassTestQuestions> RetVal = new List<ClassTestQuestions>();
            try
            {
                if (ClassTestId > 0)
                {
                    string Condition = "(ClassTestId = " + ClassTestId.ToString() + ")";
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
        public List<ClassTestQuestions> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestQuestions> GetList(string LogFilePath, string LogFileName, int ClassTestId, byte QuestionTypeId, byte QuestionLevelId, string KeyWord)
        {
            string Condition = "";
            if (ClassTestId > 0)
            {
                Condition += " AND (V$ClassTestQuestions.ClassTestId = " + ClassTestId.ToString() + ")";
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
        public List<ClassTestQuestions> GetList(string LogFilePath, string LogFileName, int ClassTestId)
        {
            string Condition = "";
            if (ClassTestId > 0)
            {
                Condition += " AND (V$ClassTestQuestions.ClassTestId = " + ClassTestId.ToString() + ")";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestQuestions> GetListByClassTestQuestionId(string LogFilePath, string LogFileName, int ClassTestQuestionId)
        {
            List<ClassTestQuestions> retVal = new List<ClassTestQuestions>();
            try
            {
                if (ClassTestQuestionId > 0)
                {
                    string Condition = "(ClassTestQuestionId=" + ClassTestQuestionId.ToString() + ")";
                    retVal = GetList(LogFilePath, LogFileName, Condition, "");
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestQuestions> GetListByClassTestQuestionIds(string LogFilePath, string LogFileName, string ClassTestQuestionIds)
        {
            List<ClassTestQuestions> retVal = new List<ClassTestQuestions>();
            try
            {
                ClassTestQuestionIds = StringUtils.Static_InjectionString(ClassTestQuestionIds);
                if (!string.IsNullOrEmpty(ClassTestQuestionIds))
                {
                    string Condition = "(ClassTestQuestionId IN (" + ClassTestQuestionIds + "))";
                    retVal = GetList(LogFilePath, LogFileName, Condition, "");
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public ClassTestQuestions Get(string LogFilePath, string LogFileName, int ClassTestQuestionId)
        {
            ClassTestQuestions retVal = new ClassTestQuestions(db.ConnectionString);
            try
            {
                List<ClassTestQuestions> list = GetListByClassTestQuestionId(LogFilePath, LogFileName, ClassTestQuestionId);
                if (list.Count > 0)
                {
                    retVal = (ClassTestQuestions)list[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public static List<ClassTestQuestions> Static_GetList(string LogFilePath, string LogFileName, string constr)
        {
            List<ClassTestQuestions> retVal = new List<ClassTestQuestions>();
            ClassTestQuestions m_ClassTestQuestions = new ClassTestQuestions(constr);
            try
            {
                retVal = m_ClassTestQuestions.GetList(LogFilePath, LogFileName);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_ClassTestQuestions.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public ClassTestQuestions Get(List<ClassTestQuestions> l_ClassTestQuestions, int ClassTestQuestionId)
        {
            ClassTestQuestions RetVal = new ClassTestQuestions(db.ConnectionString);
            if (ClassTestQuestionId > 0)
            {
                foreach (ClassTestQuestions mClassTestQuestions in l_ClassTestQuestions)
                {
                    if (mClassTestQuestions.ClassTestQuestionId == ClassTestQuestionId)
                    {
                        RetVal = mClassTestQuestions;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetClassTestQuestionIds(List<ClassTestQuestions> l_ClassTestQuestions)
        {
            string retVal = "";
            foreach (ClassTestQuestions mClassTestQuestions in l_ClassTestQuestions)
            {
                if (!StringUtils.IsMember(retVal.Split(','), mClassTestQuestions.QuestionId.ToString()))
                {
                    retVal += mClassTestQuestions.ClassTestQuestionId.ToString() + ",";
                }
            }
            return StringUtils.RemoveLastString(retVal, ",");
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTestQuestions> Copy(List<ClassTestQuestions> l_ClassTestQuestions)
        {
            List<ClassTestQuestions> retVal = new List<ClassTestQuestions>();
            foreach (ClassTestQuestions mClassTestQuestions in l_ClassTestQuestions)
            {
                retVal.Add(mClassTestQuestions);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTestQuestions> CopyAll(List<ClassTestQuestions> l_ClassTestQuestions)
        {
            ClassTestQuestions m_ClassTestQuestions = new ClassTestQuestions(db.ConnectionString);
            List<ClassTestQuestions> retVal = m_ClassTestQuestions.Copy(l_ClassTestQuestions);
            retVal.Insert(0, m_ClassTestQuestions);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTestQuestions> CopyNa(List<ClassTestQuestions> l_ClassTestQuestions)
        {
            ClassTestQuestions m_ClassTestQuestions = new ClassTestQuestions(db.ConnectionString);
            List<ClassTestQuestions> retVal = m_ClassTestQuestions.Copy(l_ClassTestQuestions);
            retVal.Insert(0, m_ClassTestQuestions);
            return retVal;
        }
    }
}

