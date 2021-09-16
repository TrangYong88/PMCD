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
    public class ClassTestUserQuestions
    {
        private int _ClassTestUserQuestionId;
        private int _ClassTestQuestionId;
        private int _ClassTestUserId;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        public int ClassTestUserQuestionId { get { return _ClassTestUserQuestionId; } set { _ClassTestUserQuestionId = value; } }
        public int ClassTestQuestionId { get { return _ClassTestQuestionId; } set { _ClassTestQuestionId = value; } }
        public int ClassTestUserId { get { return _ClassTestUserId; } set { _ClassTestUserId = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public ClassTestUserQuestions(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
        ~ClassTestUserQuestions()
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
            if (this.ClassTestUserId > 0 && this.ClassTestQuestionId > 0)
            {
                RetVal = "INSERT INTO V$ClassTestUserQuestions(ClassTestQuestionId, ClassTestUserId, CrUserId, CrDateTime)";
                RetVal += " VALUES (" + this.ClassTestQuestionId.ToString();
                RetVal += "," + this.ClassTestUserId.ToString();
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
            if (this.ClassTestQuestionId > 0 && this.ClassTestUserId > 0
                )
            {
                RetVal = "UPDATE V$ClassTestUserQuestions SET ";
                RetVal += "ClassTestQuestionId=" + this.ClassTestQuestionId.ToString();
                RetVal += ",ClassTestUserId=" + this.ClassTestUserId.ToString();
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (ClassTestUserQuestionId=" + this.ClassTestUserQuestionId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int ClassTestQuestionId, int ClassTestUserId)
        {
            string RetVal = "";
            if (ClassTestQuestionId > 0 && ClassTestUserId > 0)
            {
                RetVal = "DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE (ClassTestQuestionId=" + ClassTestQuestionId.ToString() + ")";
                RetVal += " AND (ClassTestUserId=" + ClassTestUserId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int ClassTestUserQuestionId)
        {
            string RetVal = "";
            if (ClassTestUserQuestionId > 0)
            {
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUserQuestions";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId = ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestUserQuestions.ClassTestUserQuestionId =" + ClassTestUserQuestionId.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE (ClassTestUserQuestionId=" + ClassTestUserQuestionId.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE (ClassTestUserQuestionId=" + ClassTestUserQuestionId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActClassTestUserId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassTestUserId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.ClassTestUserQuestionId = Id;
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
        public bool Update(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActClassTestUserId)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassTestUserId, BuildSqlUpdate());
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public bool Delete(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActClassTestUserId, int Id)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassTestUserId, BuildSqlDelete(Id));
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private List<ClassTestUserQuestions> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<ClassTestUserQuestions> l_ClassTestUserQuestions = new List<ClassTestUserQuestions>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {

                    ClassTestUserQuestions m_ClassTestUserQuestions = new ClassTestUserQuestions(db.ConnectionString);
                    m_ClassTestUserQuestions.ClassTestUserQuestionId = smartReader.GetInt32("ClassTestUserQuestionId");
                    m_ClassTestUserQuestions.ClassTestQuestionId = smartReader.GetInt32("ClassTestQuestionId");
                    m_ClassTestUserQuestions.ClassTestUserId = smartReader.GetInt32("ClassTestUserId");
                    m_ClassTestUserQuestions.CrUserId = smartReader.GetInt32("CrUserId");
                    m_ClassTestUserQuestions.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_ClassTestUserQuestions.Add(m_ClassTestUserQuestions);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_ClassTestUserQuestions;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestUserQuestions> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<ClassTestUserQuestions> retVal = new List<ClassTestUserQuestions>();
            try
            {
                string Sql = "SELECT * FROM V$ClassTestUserQuestions";
                Sql += " INNER JOIN V$ClassTests";
                Sql += " ON V$ClassTestUserQuestions.ClassTestUserId = V$ClassTests.ClassTestUserId";
                Sql += " INNER JOIN V$Questions";
                Sql += " ON V$ClassTestUserQuestions.ClassTestQuestionId = V$Questions.ClassTestQuestionId";
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
                retVal = Init(LogFilePath, LogFileName, cmd);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;

        }
        //------------------------------------------------------------------------------
        public ClassTestUserQuestions GetUnique(List<ClassTestUserQuestions> lClassTestUserQuestions, int ClassTestUserId, int ClassTestQuestionId)
        {
            ClassTestUserQuestions RetVal = new ClassTestUserQuestions(db.ConnectionString);
            if (ClassTestUserId > 0)
            {
                if (ClassTestQuestionId > 0)
                {
                    foreach (ClassTestUserQuestions mClassTestUserQuestions in lClassTestUserQuestions)
                    {
                        if ((mClassTestUserQuestions.ClassTestUserId == ClassTestUserId) && (mClassTestUserQuestions.ClassTestQuestionId == ClassTestQuestionId))
                        {
                            RetVal = mClassTestUserQuestions;
                            break;
                        }
                    }
                }
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------
        public List<ClassTestUserQuestions> GetListByClassTestUserId(string LogFilePath, string LogFileName, int ClassTestUserId)
        {
            List<ClassTestUserQuestions> RetVal = new List<ClassTestUserQuestions>();
            try
            {
                if (ClassTestUserId > 0)
                {
                    string Condition = "(ClassTestUserId = " + ClassTestUserId.ToString() + ")";
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
        public List<ClassTestUserQuestions> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestUserQuestions> GetList(string LogFilePath, string LogFileName, int ClassTestUserId, byte QuestionTypeId, byte QuestionLevelId, string KeyWord)
        {
            string Condition = "";
            if (ClassTestUserId > 0)
            {
                Condition += " AND (V$ClassTestUserQuestions.ClassTestUserId = " + ClassTestUserId.ToString() + ")";
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
        public List<ClassTestUserQuestions> GetList(string LogFilePath, string LogFileName, int ClassTestUserId)
        {
            string Condition = "";
            if (ClassTestUserId > 0)
            {
                Condition += " AND (V$ClassTestUserQuestions.ClassTestUserId = " + ClassTestUserId.ToString() + ")";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestUserQuestions> GetListByClassTestUserQuestionId(string LogFilePath, string LogFileName, int ClassTestUserQuestionId)
        {
            List<ClassTestUserQuestions> retVal = new List<ClassTestUserQuestions>();
            try
            {
                if (ClassTestUserQuestionId > 0)
                {
                    string Condition = "(ClassTestUserQuestionId=" + ClassTestUserQuestionId.ToString() + ")";
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
        public List<ClassTestUserQuestions> GetListByClassTestUserQuestionIds(string LogFilePath, string LogFileName, string ClassTestUserQuestionIds)
        {
            List<ClassTestUserQuestions> retVal = new List<ClassTestUserQuestions>();
            try
            {
                ClassTestUserQuestionIds = StringUtils.Static_InjectionString(ClassTestUserQuestionIds);
                if (!string.IsNullOrEmpty(ClassTestUserQuestionIds))
                {
                    string Condition = "(ClassTestUserQuestionId IN (" + ClassTestUserQuestionIds + "))";
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
        public ClassTestUserQuestions Get(string LogFilePath, string LogFileName, int ClassTestUserQuestionId)
        {
            ClassTestUserQuestions retVal = new ClassTestUserQuestions(db.ConnectionString);
            try
            {
                List<ClassTestUserQuestions> list = GetListByClassTestUserQuestionId(LogFilePath, LogFileName, ClassTestUserQuestionId);
                if (list.Count > 0)
                {
                    retVal = (ClassTestUserQuestions)list[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public static List<ClassTestUserQuestions> Static_GetList(string LogFilePath, string LogFileName, string constr)
        {
            List<ClassTestUserQuestions> retVal = new List<ClassTestUserQuestions>();
            ClassTestUserQuestions m_ClassTestUserQuestions = new ClassTestUserQuestions(constr);
            try
            {
                retVal = m_ClassTestUserQuestions.GetList(LogFilePath, LogFileName);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_ClassTestUserQuestions.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public ClassTestUserQuestions Get(List<ClassTestUserQuestions> l_ClassTestUserQuestions, int ClassTestUserQuestionId)
        {
            ClassTestUserQuestions RetVal = new ClassTestUserQuestions(db.ConnectionString);
            if (ClassTestUserQuestionId > 0)
            {
                foreach (ClassTestUserQuestions mClassTestUserQuestions in l_ClassTestUserQuestions)
                {
                    if (mClassTestUserQuestions.ClassTestUserQuestionId == ClassTestUserQuestionId)
                    {
                        RetVal = mClassTestUserQuestions;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetClassTestUserQuestionIds(List<ClassTestUserQuestions> l_ClassTestUserQuestions)
        {
            string retVal = "";
            foreach (ClassTestUserQuestions mClassTestUserQuestions in l_ClassTestUserQuestions)
            {
                if (!StringUtils.IsMember(retVal.Split(','), mClassTestUserQuestions.ClassTestQuestionId.ToString()))
                {
                    retVal += mClassTestUserQuestions.ClassTestUserQuestionId.ToString() + ",";
                }
            }
            return StringUtils.RemoveLastString(retVal, ",");
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTestUserQuestions> Copy(List<ClassTestUserQuestions> l_ClassTestUserQuestions)
        {
            List<ClassTestUserQuestions> retVal = new List<ClassTestUserQuestions>();
            foreach (ClassTestUserQuestions mClassTestUserQuestions in l_ClassTestUserQuestions)
            {
                retVal.Add(mClassTestUserQuestions);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTestUserQuestions> CopyAll(List<ClassTestUserQuestions> l_ClassTestUserQuestions)
        {
            ClassTestUserQuestions m_ClassTestUserQuestions = new ClassTestUserQuestions(db.ConnectionString);
            List<ClassTestUserQuestions> retVal = m_ClassTestUserQuestions.Copy(l_ClassTestUserQuestions);
            retVal.Insert(0, m_ClassTestUserQuestions);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTestUserQuestions> CopyNa(List<ClassTestUserQuestions> l_ClassTestUserQuestions)
        {
            ClassTestUserQuestions m_ClassTestUserQuestions = new ClassTestUserQuestions(db.ConnectionString);
            List<ClassTestUserQuestions> retVal = m_ClassTestUserQuestions.Copy(l_ClassTestUserQuestions);
            retVal.Insert(0, m_ClassTestUserQuestions);
            return retVal;
        }
    }
}

