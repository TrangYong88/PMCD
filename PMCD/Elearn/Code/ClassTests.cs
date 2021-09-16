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
    public class ClassTests
    {
        private int _ClassTestId;
        private byte _TestTypeId;
        private int _ClassId;
        private DateTime _BeginDateTime;
        private DateTime _EndDateTime;
        private byte _TestStatusId;
        private Int16 _MaxTime;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public ClassTests(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        
        //------------------------------------------------------------------------
        ~ClassTests()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------

        public int ClassTestId { get { return _ClassTestId; } set { _ClassTestId = value; } }
        public byte TestTypeId { get { return _TestTypeId; } set { _TestTypeId = value; } }
        public int ClassId { get { return _ClassId; } set { _ClassId = value; } }
        public DateTime BeginDateTime { get { return _BeginDateTime; } set { _BeginDateTime = value; } }
        public DateTime EndDateTime { get { return _EndDateTime; } set { _EndDateTime = value; } }
        public byte TestStatusId { get { return _TestStatusId; } set { _TestStatusId = value; } }
        public Int16 MaxTime { get { return _MaxTime; } set { _MaxTime = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.ClassTestId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((this.TestTypeId > 0)
                && (this.ClassId > 0)
                && (this.TestStatusId > 0)
                )
            {
                RetVal = "INSERT INTO V$ClassTests(TestTypeId,ClassId,BeginDateTime,EndDateTime,TestStatusId,MaxTime,CrUserId,CrDateTime)";
                RetVal += " VALUES (" + this.TestTypeId.ToString();
                RetVal += "," + this.ClassId.ToString();
                RetVal += ",CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.BeginDateTime, "-", ":") + "',121)";
                RetVal += ",CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.EndDateTime, "-", ":") + "',121)";
                RetVal += "," + this.TestStatusId.ToString();
                RetVal += "," + this.MaxTime.ToString();
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
            if ((this.ClassTestId > 0)
                && (this.TestTypeId > 0)
                && (this.ClassId > 0)
                )
            {
                RetVal = "UPDATE ClassTests SET ";
                RetVal += " TestTypeId=" + this.TestTypeId.ToString();
                RetVal += ",ClassId=" + this.ClassId.ToString();
                RetVal += ",BeginDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.BeginDateTime, "-", ":") + "',121)";
                RetVal += ",EndDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.EndDateTime, "-", ":") + "',121)";
                RetVal += ",TestStatusId=" + this.TestStatusId.ToString() ;
                RetVal += ",MaxTime=" + this.MaxTime.ToString();
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (ClassTestId=" + this.ClassTestId.ToString() + ")";
            }
            return RetVal;
        }
        //------------------------------------------------------------------------------
        public ClassTests GetUnique(List<ClassTests> lClassTests, int ClassId)
        {
            ClassTests RetVal = new ClassTests(db.ConnectionString);
            if (ClassId > 0)
            {
                foreach (ClassTests mClassTests in lClassTests)
                    {
                        if (mClassTests.ClassId == ClassId)
                        {
                            RetVal = mClassTests;
                            break;
                        }
                    }
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
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestQuestions,ClassTests";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestQuestions.ClassTestQuestionId = ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " AND ClassTestQuestions.ClassTestId = ClassTests.ClassTestId";
                RetVal += " AND ClassTests.ClassTestId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions ";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " FROM ClassTestUserQuestions, ClassTestQuestions, ClassTests";
                RetVal += " WHERE ClassTestQuestions.ClassTestQuestionId = ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " AND ClassTestQuestions.ClassTestId = ClassTests.ClassTestId";
                RetVal += " AND ClassTests.ClassTestId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestQuestions ";
                RetVal += " WHERE (ClassTestId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUsers,ClassTests";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId ";
                RetVal += " AND ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND ClassTestUsers.ClassTestId = ClassTests.ClassTestId";
                RetVal += " AND ClassTests.ClassTestId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions ";
                RetVal += " WHERE  ClassTestUserQuestions.ClassTestQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestQuestionId";
                RetVal += " FROM ClassTestUserQuestions, ClassTestUsers,ClassTests";
                RetVal += " WHERE ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND ClassTestUsers.ClassTestId = ClassTests.ClassTestId";
                RetVal += " AND ClassTests.ClassTestId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUsers ";
                RetVal += " WHERE (ClassTestId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTests ";
                RetVal += " WHERE (ClassTestId =" + Id.ToString() + ")";    
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
                        this.ClassTestId = Id;
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
        //------------------------------------------------------------------------
        private List<ClassTests> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<ClassTests> l_ClassTests = new List<ClassTests>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    ClassTests m_ClassTests = new ClassTests(db.ConnectionString);
                    m_ClassTests.ClassTestId = smartReader.GetInt32("ClassTestId");
                    m_ClassTests.TestTypeId = smartReader.GetByte("TestTypeId");
                    m_ClassTests.ClassId = smartReader.GetInt32("ClassId");
                    m_ClassTests.BeginDateTime = smartReader.GetDateTime("BeginDateTime");
                    m_ClassTests.EndDateTime = smartReader.GetDateTime("EndDateTime");
                    m_ClassTests.TestStatusId = smartReader.GetByte("TestStatusId");
                    m_ClassTests.MaxTime = smartReader.GetInt16("MaxTime");
                    m_ClassTests.CrUserId = smartReader.GetInt32("CrUserId");
                    m_ClassTests.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_ClassTests.Add(m_ClassTests);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_ClassTests;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTests> GetList(string LogFilePath, string LogFileName, int ClassId, byte TestStatusId)
        {
            string Condition = "";
            if (ClassId > 0)
            {
                Condition = "(ClassId = " + ClassId.ToString() + ")";
                if (TestStatusId > 0)
                {
                    Condition += "AND (TestStatusId = " + TestStatusId.ToString() + ")";
                }
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //------------------------------------------------------------------------
        public List<ClassTests> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<ClassTests> RetVal = new List<ClassTests>();
            try
            {
                string Sql = "SELECT * FROM V$ClassTests";
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
        public List<ClassTests> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTests> GetList(string LogFilePath, string LogFileName, byte TestStatusId, int ClassId,  string KeyWord)
        {
            string Condition = "";
            if (TestStatusId > 0)
            {
                Condition = "(TestStatusId = " + TestStatusId.ToString() + ")";
                if (ClassId > 0)
                {
                    Condition += "AND (ClassId = " + ClassId.ToString() + ")";
                }
            }
            else
            {
                if (ClassId > 0)
                {
                    Condition = "(ClassId = " + ClassId.ToString() + ")";
                }
            } 
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTests> GetListByClassTestId(string LogFilePath, string LogFileName, int ClassTestId)
        {
            List<ClassTests> RetVal = new List<ClassTests>();
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
        
        //------------------------------------------------------------------------
        public List<ClassTests> GetListByTestTypeId(string LogFilePath, string LogFileName, int TestTypeId)
        {
            List<ClassTests> RetVal = new List<ClassTests>();
            try
            {
                if (TestTypeId > 0)
                {
                    string Condition = "(TestTypeId = " + TestTypeId.ToString() + ")";
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
        public ClassTests Get(string LogFilePath, string LogFileName, int ClassTestId)
        {
            ClassTests RetVal = new ClassTests(db.ConnectionString);
            try
            {
                List<ClassTests> l_ClassTests = GetListByClassTestId(LogFilePath, LogFileName, ClassTestId);
                if (l_ClassTests.Count > 0)
                {
                    RetVal = (ClassTests)l_ClassTests[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        
        
        //--------------------------------------------------------------------------------------------------------------------
        public ClassTests Get(List<ClassTests> l_ClassTests, long ClassTestId)
        {
            ClassTests RetVal = new ClassTests(db.ConnectionString);
            foreach (ClassTests mClassTests in l_ClassTests)
            {
                if (mClassTests.ClassTestId == ClassTestId)
                {
                    RetVal = mClassTests;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public string GetTestTypeIds(List<ClassTests> lClassTests)
        {
            string RetVal = "";
            foreach (ClassTests mClassTests in lClassTests)
            {
                RetVal += mClassTests.TestTypeId.ToString() + ",";
            }
            return StringUtils.RemoveLastString(RetVal, ","); ;
        }
        
        
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTests> Copy(List<ClassTests> l_ClassTests)
        {
            List<ClassTests> retVal = new List<ClassTests>();
            foreach (ClassTests mClassTests in l_ClassTests)
            {
                retVal.Add(mClassTests);
            }
            return retVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTests> GetList(string LogFilePath, string LogFileName, int ClassId)
        {
            string Condition = "";
            if (ClassId > 0)
            {
                Condition = "ClassId = " + ClassId.ToString();
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTests> CopyAll(List<ClassTests> l_ClassTests)
        {
            ClassTests m_ClassTests = new ClassTests(db.ConnectionString);
            List<ClassTests> retVal = m_ClassTests.Copy(l_ClassTests);
            retVal.Insert(0, m_ClassTests);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTests> CopyNa(List<ClassTests> l_ClassTests)
        {
            ClassTests m_ClassTests = new ClassTests(db.ConnectionString);
            List<ClassTests> retVal = m_ClassTests.Copy(l_ClassTests);
            retVal.Insert(0, m_ClassTests);
            return retVal;
        }
    }//end ClassTests
}
