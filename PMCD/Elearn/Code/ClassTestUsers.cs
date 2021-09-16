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
    public class ClassTestUsers
    {
        private int _ClassTestUserId;
        private int _ClassTestId;
        private int _UserClassId;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public ClassTestUsers(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }

        //------------------------------------------------------------------------
        ~ClassTestUsers()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------

        public int ClassTestUserId { get { return _ClassTestUserId; } set { _ClassTestUserId = value; } }
        public int ClassTestId { get { return _ClassTestId; } set { _ClassTestId = value; } }
        public int UserClassId { get { return _UserClassId; } set { _UserClassId = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.ClassTestUserId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((this.ClassTestId > 0)
                && (this.UserClassId > 0)
                )
            {
                RetVal = "INSERT INTO V$ClassTestUsers(ClassTestId,UserClassId,CrUserId,CrDateTime)";
                RetVal += " VALUES (" + this.ClassTestId.ToString();
                RetVal += "," + this.UserClassId.ToString();
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
            if ((this.ClassTestUserId > 0)
                && (this.ClassTestId > 0)
                && (this.UserClassId > 0)
                )
            {
                RetVal = "UPDATE ClassTestUsers SET ";
                RetVal += " ClassTestId=" + this.ClassTestId.ToString();
                RetVal += ",UserClassId=" + this.UserClassId.ToString();
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (ClassTestUserId=" + this.ClassTestUserId.ToString() + ")";
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
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUsers";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId ";
                RetVal += " AND ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND ClassTestUsers.ClassTestUserId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE (ClassTestUserId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUsers";
                RetVal += " WHERE (ClassTestUserId=" + Id.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActClassTestUserId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActClassTestUserId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.ClassTestUserId = Id;
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
        public bool Update(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActClassTestUserId)
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
        public bool Delete(string LogFilePath, string LogFileName, int DistributedProcess, string IpAddress, int ActClassTestUserId, int Id)
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
        //------------------------------------------------------------------------
        private List<ClassTestUsers> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<ClassTestUsers> l_ClassTestUsers = new List<ClassTestUsers>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    ClassTestUsers m_ClassTestUsers = new ClassTestUsers(db.ConnectionString);
                    m_ClassTestUsers.ClassTestUserId = smartReader.GetInt32("ClassTestUserId");
                    m_ClassTestUsers.ClassTestId = smartReader.GetInt32("ClassTestId");
                    m_ClassTestUsers.UserClassId = smartReader.GetInt32("UserClassId");
                    m_ClassTestUsers.CrUserId = smartReader.GetInt32("CrUserId");
                    m_ClassTestUsers.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_ClassTestUsers.Add(m_ClassTestUsers);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_ClassTestUsers;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestUsers> GetList(string LogFilePath, string LogFileName, int ClassTestId)
        {
            string Condition = "";
            if (ClassTestId > 0)
            {
                Condition = "ClassTestId = " + ClassTestId.ToString();
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //------------------------------------------------------------------------
        public List<ClassTestUsers> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<ClassTestUsers> RetVal = new List<ClassTestUsers>();
            try
            {
                string Sql = "SELECT * FROM V$ClassTestUsers";
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
        public List<ClassTestUsers> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestUsers> GetList(string LogFilePath, string LogFileName, int ClassTestId, int UserClassId, string KeyWord)
        {
            string Condition = "";
            if (UserClassId > 0)
            {
                Condition = "(UserClassId = " + UserClassId.ToString() + ")";
                if (ClassTestId > 0)
                {
                    Condition += "AND (ClassTestId = " + ClassTestId.ToString() + ")";
                }
            }
            else
            {
                if (ClassTestId > 0)
                {
                    Condition = "(ClassTestId = " + ClassTestId.ToString() + ")";
                }
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestUsers> GetListOrderByName(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "ClassTestUserName");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<ClassTestUsers> GetListByClassTestUserId(string LogFilePath, string LogFileName, int ClassTestUserId)
        {
            List<ClassTestUsers> RetVal = new List<ClassTestUsers>();
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

        //------------------------------------------------------------------------
        public List<ClassTestUsers> GetListByClassTestId(string LogFilePath, string LogFileName, int ClassTestId)
        {
            List<ClassTestUsers> RetVal = new List<ClassTestUsers>();
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
        public ClassTestUsers Get(string LogFilePath, string LogFileName, int ClassTestUserId)
        {
            ClassTestUsers RetVal = new ClassTestUsers(db.ConnectionString);
            try
            {
                List<ClassTestUsers> l_ClassTestUsers = GetListByClassTestUserId(LogFilePath, LogFileName, ClassTestUserId);
                if (l_ClassTestUsers.Count > 0)
                {
                    RetVal = (ClassTestUsers)l_ClassTestUsers[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }

        //--------------------------------------------------------------------------------------------------------------------
        public ClassTestUsers Get(List<ClassTestUsers> l_ClassTestUsers, long ClassTestUserId)
        {
            ClassTestUsers RetVal = new ClassTestUsers(db.ConnectionString);
            foreach (ClassTestUsers mClassTestUsers in l_ClassTestUsers)
            {
                if (mClassTestUsers.ClassTestUserId == ClassTestUserId)
                {
                    RetVal = mClassTestUsers;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public string GetClassTestIds(List<ClassTestUsers> lClassTestUsers)
        {
            string RetVal = "";
            foreach (ClassTestUsers mClassTestUsers in lClassTestUsers)
            {
                RetVal += mClassTestUsers.ClassTestId.ToString() + ",";
            }
            return StringUtils.RemoveLastString(RetVal, ","); 
        }
        //------------------------------------------------------------------------------
        public ClassTestUsers GetUnique(List<ClassTestUsers> lClassTestUsers, int ClassTestId, int UserClassId)
        {
            ClassTestUsers RetVal = new ClassTestUsers(db.ConnectionString);
            if (UserClassId > 0)
            {
                if (ClassTestId > 0)
                {
                    foreach (ClassTestUsers mClassTestUsers in lClassTestUsers)
                    {
                        if ((mClassTestUsers.UserClassId == UserClassId) && (mClassTestUsers.ClassTestId == ClassTestId))
                        {
                            RetVal = mClassTestUsers;
                            break;
                        }
                    }
                }
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------
        public List<ClassTestUsers> GetListByUserClassId(string LogFilePath, string LogFileName, int UserClassId)
        {
            List<ClassTestUsers> RetVal = new List<ClassTestUsers>();
            try
            {
                if (UserClassId > 0)
                {
                    string Condition = "(UserClassId = " + UserClassId.ToString() + ")";
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
        public List<ClassTestUsers> Copy(List<ClassTestUsers> l_ClassTestUsers)
        {
            List<ClassTestUsers> retVal = new List<ClassTestUsers>();
            foreach (ClassTestUsers mClassTestUsers in l_ClassTestUsers)
            {
                retVal.Add(mClassTestUsers);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTestUsers> CopyAll(List<ClassTestUsers> l_ClassTestUsers)
        {
            ClassTestUsers m_ClassTestUsers = new ClassTestUsers(db.ConnectionString);
            List<ClassTestUsers> retVal = m_ClassTestUsers.Copy(l_ClassTestUsers);
            retVal.Insert(0, m_ClassTestUsers);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<ClassTestUsers> CopyNa(List<ClassTestUsers> l_ClassTestUsers)
        {
            ClassTestUsers m_ClassTestUsers = new ClassTestUsers(db.ConnectionString);
            List<ClassTestUsers> retVal = m_ClassTestUsers.Copy(l_ClassTestUsers);
            retVal.Insert(0, m_ClassTestUsers);
            return retVal;
        }
    }//end ClassTestUsers
}
