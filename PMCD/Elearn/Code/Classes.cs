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
    public class Classes
    {
        private int _ClassId;
        private int _CurriculumId;
        private string _ClassCode;
        private string _ClassName;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public Classes(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
        public Classes(string constr, string Name, string Desc)
        {
            this.ClassId = 0;
            this.ClassCode = Name;
            this.ClassName = Desc;
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //------------------------------------------------------------------------
        ~Classes()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------
        public int ClassId { get { return _ClassId; } set { _ClassId = value; } }
        public int CurriculumId { get { return _CurriculumId; } set { _CurriculumId = value; } }
        public string ClassCode { get { return _ClassCode; } set { _ClassCode = value; } }
        public string ClassName { get { return _ClassName; } set { _ClassName = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.ClassId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((this.CurriculumId > 0)
                && (!string.IsNullOrEmpty(this.ClassCode))
                )
            {
                RetVal = "INSERT INTO V$Classes(CurriculumId,ClassCode,ClassName,CrUserId,CrDateTime)";
                RetVal += " VALUES (" + this.CurriculumId.ToString();
                RetVal += ",N'" + this.ClassCode + "'";
                RetVal += ",N'" + this.ClassName + "'";
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
            if ((this.ClassId > 0)
                && (this.CurriculumId > 0)
                && (!string.IsNullOrEmpty(this.ClassCode))
                )
            {
                RetVal = "UPDATE Classes SET ";
                RetVal += " CurriculumId=" + this.CurriculumId.ToString();
                RetVal += ",ClassCode=N'" + this.ClassCode + "'";
                RetVal += ",ClassName=N'" + this.ClassName + "'";
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (ClassId=" + this.ClassId.ToString() + ")";
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
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId ";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUsers,UserClasses, Classes";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND UserClasses.UserClassId = ClassTestUsers.UserClassId";
                RetVal += " AND Classes.ClassId = UserClasses.ClassId";
                RetVal += " AND Classes.ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE  ClassTestUserQuestions.ClassTestUserId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestUserId";
                RetVal += " FROM ClassTestUserQuestions, ClassTestUsers,UserClasses, Classes";
                RetVal += " WHERE ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND UserClasses.UserClassId = ClassTestUsers.UserClassId";
                RetVal += " AND Classes.ClassId = UserClasses.ClassId";
                RetVal += " AND Classes.ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUsers";
                RetVal += " WHERE  ClassTestUsers.UserClassId =";
                RetVal += " (SELECT TOP(1) ClassTestUsers.UserClassId";
                RetVal += " FROM ClassTestUsers,UserClasses, Classes";
                RetVal += " WHERE UserClasses.UserClassId = ClassTestUsers.UserClassId";
                RetVal += " AND Classes.ClassId = UserClasses.ClassId";
                RetVal += " AND Classes.ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM UserClasses";
                RetVal += " WHERE (ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUsers,ClassTests, Classes";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND ClassTests.ClassTestId = ClassTestUsers.UserClassId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Classes.ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUsers";
                RetVal += " WHERE  ClassTestUsers.UserClassId = ";
                RetVal += " (SELECT TOP(1) ClassTestUsers.UserClassId";
                RetVal += " FROM ClassTestUsers,ClassTests, Classes";
                RetVal += " WHERE ClassTests.ClassTestId = ClassTestUsers.UserClassId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Classes.ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTests";
                RetVal += " WHERE (ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestQuestions,ClassTests, Classes";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestQuestions.ClassTestQuestionId = ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " AND ClassTests.ClassTestId = ClassTestQuestions.ClassTestId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Classes.ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE  ClassTestUserQuestions.ClassTestQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " FROM ClassTestUserQuestions, ClassTestQuestions,ClassTests, Classes";
                RetVal += " WHERE ClassTestQuestions.ClassTestQuestionId = ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " AND ClassTests.ClassTestId = ClassTestQuestions.ClassTestId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Classes.ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestQuestions";
                RetVal += " WHERE  ClassTestQuestions.ClassTestId =";
                RetVal += " (SELECT TOP(1) ClassTestQuestions.ClassTestId";
                RetVal += " FROM ClassTestQuestions,ClassTests, Classes";
                RetVal += " WHERE ClassTests.ClassTestId = ClassTestQuestions.ClassTestId";
                RetVal += " AND Classes.ClassId = ClassTests.ClassId";
                RetVal += " AND Classes.ClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM Classes";
                RetVal += " WHERE (ClassId =" + Id.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActClassId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.ClassId = Id;
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
        public bool Update(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActClassId)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassId, BuildSqlUpdate());
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public bool Delete(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActClassId, int Id)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassId, BuildSqlDelete(Id));
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //------------------------------------------------------------------------
        private List<Classes> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<Classes> l_Classes = new List<Classes>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Classes m_Classes = new Classes(db.ConnectionString);
                    m_Classes.ClassId = smartReader.GetInt32("ClassId");
                    m_Classes.CurriculumId = smartReader.GetInt32("CurriculumId");
                    m_Classes.ClassCode = smartReader.GetString("ClassCode");
                    m_Classes.ClassName = smartReader.GetString("ClassName");
                    m_Classes.CrUserId = smartReader.GetInt32("CrUserId");
                    m_Classes.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_Classes.Add(m_Classes);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_Classes;
        }
        //------------------------------------------------------------------------
        public List<Classes> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Classes> RetVal = new List<Classes>();
            try
            {
                string Sql = "SELECT * FROM V$Classes";
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
        public List<Classes> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Classes Get(string LogFilePath, string LogFileName, int ClassId)
        {
            Classes RetVal = new Classes(db.ConnectionString);
            try
            {
                List<Classes> l_Classes = GetListByClassId(LogFilePath, LogFileName, ClassId);
                if (l_Classes.Count > 0)
                {
                    RetVal = (Classes)l_Classes[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Classes> GetList(string LogFilePath, string LogFileName, int CurriculumId, string KeyWord)
        {
            string Condition = "";           
            if (CurriculumId > 0)
            {
               Condition = "(CurriculumId = " + CurriculumId.ToString() + ")";
            }
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "(ClassCode = N'" + KeyWord + "')";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Classes> GetListOrderByName(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "ClassCode");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Classes> GetListByClassId(string LogFilePath, string LogFileName, int ClassId)
        {
            List<Classes> RetVal = new List<Classes>();
            try
            {
                if (ClassId > 0)
                {
                    string Condition = "(ClassId = " + ClassId.ToString() + ")";
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
        public List<Classes> GetListByClassCode(string LogFilePath, string LogFileName, string ClassCode)
        {
            List<Classes> RetVal = new List<Classes>();
            try
            {
                ClassCode = StringUtils.Static_InjectionString(ClassCode);
                if (!string.IsNullOrEmpty(ClassCode))
                {
                    string Condition = "(ClassCode =N'" + ClassCode + "')";
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

        public List<Classes> GetListUnique(string LogFilePath, string LogFileName, string ClassCode)
        {
            List<Classes> RetVal = new List<Classes>();
            try
            {
                if (!string.IsNullOrEmpty(ClassCode))
                {
                    string Condition = " (ClassCode = N'" + ClassCode + "')";
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
        public List<Classes> GetListByCurriculumId(string LogFilePath, string LogFileName, int CurriculumId)
        {
            List<Classes> RetVal = new List<Classes>();
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
        public Classes GetByClassCode(string LogFilePath, string LogFileName, string ClassCode)
        {
            Classes RetVal = new Classes(db.ConnectionString);
            try
            {
                List<Classes> l_Classes = GetListByClassCode(LogFilePath, LogFileName, ClassCode);
                if (l_Classes.Count > 0)
                {
                    RetVal = (Classes)l_Classes[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public int GetClassId(string ClassCode, string ClassName, List<Classes> l_Classes)
        {
            for (int i = 0; i < l_Classes.Count; i++)
            {
                if (l_Classes[i].ClassCode.Equals(ClassCode) && l_Classes[i].ClassName.Equals(ClassName))
                {
                    return l_Classes[i].ClassId;
                }
            }
            return 0;

        }
        //--------------------------------------------------------------------------------------------------------------------
        public Classes Get(List<Classes> l_Classes, long ClassId)
        {
            Classes RetVal = new Classes(db.ConnectionString);
            foreach (Classes mClasses in l_Classes)
            {
                if (mClasses.ClassId == ClassId)
                {
                    RetVal = mClasses;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public string GetCurriculumIds(List<Classes> lClasses)
        {
            string RetVal = "";
            foreach (Classes mClasses in lClasses)
            {
                RetVal += mClasses.CurriculumId.ToString() + ",";
            }
            return StringUtils.RemoveLastString(RetVal, ","); ;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Classes GetUnique(List<Classes> lClasses, string ClassCode)
        {
            Classes RetVal = new Classes(db.ConnectionString);

            if (!string.IsNullOrEmpty(ClassCode))
            {
                foreach (Classes mClasses in lClasses)
                {
                    if (mClasses.ClassCode == ClassCode)
                    {
                        RetVal = mClasses;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Classes GetUnique(string LogFilePath, string LogFileName, string ClassCode)
        {
            Classes RetVal = new Classes(db.ConnectionString);
            try
            {
                List<Classes> l_Classes = GetListUnique(LogFilePath, LogFileName, ClassCode);
                if (l_Classes.Count > 0)
                {
                    RetVal = (Classes)l_Classes[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Classes> Copy(List<Classes> l_Classes)
        {
            List<Classes> retVal = new List<Classes>();
            foreach (Classes mClasses in l_Classes)
            {
                retVal.Add(mClasses);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Classes> CopyAll(List<Classes> l_Classes)
        {
            Classes m_Classes = new Classes(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC);
            List<Classes> retVal = m_Classes.Copy(l_Classes);
            retVal.Insert(0, m_Classes);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Classes> CopyNa(List<Classes> l_Classes)
        {
            Classes m_Classes = new Classes(db.ConnectionString, StringUtils.NA, StringUtils.NA_DESC);
            List<Classes> retVal = m_Classes.Copy(l_Classes);
            retVal.Insert(0, m_Classes);
            return retVal;
        }
    }//end Classes
}
