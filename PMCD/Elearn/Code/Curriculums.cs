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
    public class Curriculums
    {
        private int _CurriculumId;
        private string _CurriculumName;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public Curriculums(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //------------------------------------------------------------------------
        ~Curriculums()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------
        public int CurriculumId { get { return _CurriculumId; } set { _CurriculumId = value; } }
        public string CurriculumName { get { return _CurriculumName; } set { _CurriculumName = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.CurriculumId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if (!string.IsNullOrEmpty(this.CurriculumName))
            {
                RetVal = "INSERT INTO V$Curriculums(CurriculumName,CrUserId,CrDateTime)";
                RetVal += " VALUES (";
                RetVal += "N'" + this.CurriculumName + "'";
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
            if ((this.CurriculumId > 0)
                && (!string.IsNullOrEmpty(this.CurriculumName))
                )
            {
                RetVal = "UPDATE Curriculums SET ";
                RetVal += "CurriculumName=N'" + this.CurriculumName + "'";  
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (CurriculumId=" + this.CurriculumId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUsers,UserClasses, Classes, Curriculums";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND UserClasses.UserClassId = ClassTestUsers.UserClassId";
                RetVal += " AND Classes.ClassId = UserClasses.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE  ClassTestUserQuestions.ClassTestUserId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestUserId";
                RetVal += " FROM ClassTestUserQuestions, ClassTestUsers,UserClasses, Classes, Curriculums";
                RetVal += " WHERE ClassTestUsers.ClassTestUserId=ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND UserClasses.UserClassId = ClassTestUsers.UserClassId";
                RetVal += " AND Classes.ClassId = UserClasses.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUsers";
                RetVal += " WHERE  ClassTestUsers.UserClassId =";
                RetVal += " (SELECT TOP(1) ClassTestUsers.UserClassId";
                RetVal += " FROM ClassTestUsers,UserClasses, Classes, Curriculums";
                RetVal += " WHERE ClassTestUsers.UserClassId=UserClasses.UserClassId";
                RetVal += " AND Classes.ClassId = UserClasses.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM UserClasses";
                RetVal += " WHERE  UserClasses.ClassId =";
                RetVal += " (SELECT TOP(1) UserClasses.ClassId";
                RetVal += " FROM UserClasses, Classes, Curriculums";
                RetVal += " WHERE UserClasses.ClassId = Classes.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestQuestions,ClassTests, Classes, Curriculums";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestQuestions.ClassTestQuestionId = ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " AND ClassTests.ClassTestId = ClassTestQuestions.ClassTestId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE  ClassTestUserQuestions.ClassTestQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " FROM  ClassTestUserQuestions, ClassTestQuestions,ClassTests, Classes, Curriculums";
                RetVal += " WHERE ClassTestQuestions.ClassTestQuestionId = ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " AND ClassTests.ClassTestId = ClassTestQuestions.ClassTestId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestQuestions";
                RetVal += " WHERE  ClassTestQuestions.ClassTestId =";
                RetVal += " (SELECT TOP(1) ClassTestQuestions.ClassTestId ";
                RetVal += " FROM  ClassTestQuestions,ClassTests, Classes, Curriculums";
                RetVal += " WHERE ClassTests.ClassTestId = ClassTestQuestions.ClassTestId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUsers,ClassTests, Classes, Curriculums";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND ClassTests.ClassTestId = ClassTestUsers.ClassTestId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE  ClassTestUserQuestions.ClassTestUserId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestUserId";
                RetVal += " FROM ClassTestUserQuestions, ClassTestUsers,ClassTests, Classes, Curriculums";
                RetVal += " WHERE ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND ClassTests.ClassTestId = ClassTestUsers.ClassTestId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUsers";
                RetVal += " WHERE  ClassTestUsers.ClassTestId =";
                RetVal += " (SELECT TOP(1) ClassTestUsers.ClassTestId ";
                RetVal += " FROM ClassTestUsers,ClassTests, Classes, Curriculums";
                RetVal += " WHERE ClassTests.ClassTestId = ClassTestUsers.ClassTestId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTests";
                RetVal += " WHERE  ClassTests.ClassId =";
                RetVal += " (SELECT TOP(1) ClassTests.ClassId ";
                RetVal += " FROM ClassTests, Classes, Curriculums";
                RetVal += " WHERE Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Curriculums.CurriculumId = Classes.CurriculumId";
                RetVal += " AND Curriculums.CurriculumId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM Classes";
                RetVal += " WHERE (CurriculumId =" + Id.ToString() + ")";
                RetVal += "DELETE FROM QuestionCurriculums";
                RetVal += " WHERE (CurriculumId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM Curriculums";
                RetVal += " WHERE (CurriculumId=" + Id.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActUserId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActUserId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.CurriculumId = Id;
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
        public bool Update(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActUserId)
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
        public bool Delete(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActUserId, int Id)
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
        //------------------------------------------------------------------------
        private List<Curriculums> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<Curriculums> l_Curriculums = new List<Curriculums>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Curriculums m_Curriculums = new Curriculums(db.ConnectionString);
                    m_Curriculums.CurriculumId = smartReader.GetInt32("CurriculumId");
                    m_Curriculums.CurriculumName = smartReader.GetString("CurriculumName");
                    m_Curriculums.CrUserId = smartReader.GetInt32("CrUserId");
                    m_Curriculums.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_Curriculums.Add(m_Curriculums);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_Curriculums;
        }
        //------------------------------------------------------------------------
        public List<Curriculums> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Curriculums> RetVal = new List<Curriculums>();
            try
            {
                string Sql = "SELECT * FROM V$Curriculums";
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
        public List<Curriculums> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Curriculums> GetList(string LogFilePath, string LogFileName, string KeyWord)
        {
            string Condition = "";
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "(CurriculumName = N'" + KeyWord + "')";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Curriculums> GetListOrderByName(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "CurriculumName");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Curriculums> GetListByCurriculumId(string LogFilePath, string LogFileName, int CurriculumId)
        {
            List<Curriculums> RetVal = new List<Curriculums>();
            try
            {
                if (CurriculumId > 0)
                {
                    string Condition = "(CurriculumId = " + CurriculumId.ToString() + ")";
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
        public List<Curriculums> GetListByCurriculumName(string LogFilePath, string LogFileName, string CurriculumName)
        {
            List<Curriculums> RetVal = new List<Curriculums>();
            try
            {
                CurriculumName = StringUtils.Static_InjectionString(CurriculumName);
                if (!string.IsNullOrEmpty(CurriculumName))
                {
                    string Condition = "(CurriculumName =N'" + CurriculumName + "')";
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

        public List<Curriculums> GetListUnique(string LogFilePath, string LogFileName, string CurriculumName)
        {
            List<Curriculums> RetVal = new List<Curriculums>();
            try
            {
                if (!string.IsNullOrEmpty(CurriculumName))
                {
                    string Condition = " (CurriculumName = N'" + CurriculumName + "')";
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
        public Curriculums Get(string LogFilePath, string LogFileName, int CurriculumId)
        {
            Curriculums RetVal = new Curriculums(db.ConnectionString);
            try
            {
                List<Curriculums> l_Curriculums = GetListByCurriculumId(LogFilePath, LogFileName, CurriculumId);
                if (l_Curriculums.Count > 0)
                {
                    RetVal = (Curriculums)l_Curriculums[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Curriculums GetByCurriculumName(string LogFilePath, string LogFileName, string CurriculumName)
        {
            Curriculums RetVal = new Curriculums(db.ConnectionString);
            try
            {
                List<Curriculums> l_Curriculums = GetListByCurriculumName(LogFilePath, LogFileName, CurriculumName);
                if (l_Curriculums.Count > 0)
                {
                    RetVal = (Curriculums)l_Curriculums[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Curriculums Get(List<Curriculums> l_Curriculums, long CurriculumId)
        {
            Curriculums RetVal = new Curriculums(db.ConnectionString);
            foreach (Curriculums mCurriculums in l_Curriculums)
            {
                if (mCurriculums.CurriculumId == CurriculumId)
                {
                    RetVal = mCurriculums;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Curriculums GetUnique(List<Curriculums> lCurriculums, string CurriculumName)
        {
            Curriculums RetVal = new Curriculums(db.ConnectionString);

            if (!string.IsNullOrEmpty(CurriculumName))
            {
                foreach (Curriculums mCurriculums in lCurriculums)
                {
                    if (mCurriculums.CurriculumName == CurriculumName)
                    {
                        RetVal = mCurriculums;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Curriculums GetUnique(string LogFilePath, string LogFileName, string CurriculumName)
        {
            Curriculums RetVal = new Curriculums(db.ConnectionString);
            try
            {
                List<Curriculums> l_Curriculums = GetListUnique(LogFilePath, LogFileName, CurriculumName);
                if (l_Curriculums.Count > 0)
                {
                    RetVal = (Curriculums)l_Curriculums[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Curriculums> Copy(List<Curriculums> l_Curriculums)
        {
            List<Curriculums> retVal = new List<Curriculums>();
            foreach (Curriculums mCurriculums in l_Curriculums)
            {
                retVal.Add(mCurriculums);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Curriculums> CopyAll(List<Curriculums> l_Curriculums)
        {
            Curriculums m_Curriculums = new Curriculums(db.ConnectionString);
            List<Curriculums> retVal = m_Curriculums.Copy(l_Curriculums);
            retVal.Insert(0, m_Curriculums);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Curriculums> CopyNa(List<Curriculums> l_Curriculums)
        {
            Curriculums m_Curriculums = new Curriculums(db.ConnectionString);
            List<Curriculums> retVal = m_Curriculums.Copy(l_Curriculums);
            retVal.Insert(0, m_Curriculums);
            return retVal;
        }
    }//end Curriculums
}
