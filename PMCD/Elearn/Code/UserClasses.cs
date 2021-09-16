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
    public class UserClasses
    {
        private int _UserClassId;
        private int _UserId;
        private int _ClassId;
        private byte _UserClassRow;
        private byte _UserClassColumn;
        private byte _RankId;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public UserClasses(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }

        //------------------------------------------------------------------------
        ~UserClasses()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------

        public int UserClassId { get { return _UserClassId; } set { _UserClassId = value; } }
        public int UserId { get { return _UserId; } set { _UserId = value; } }
        public int ClassId { get { return _ClassId; } set { _ClassId = value; } }
        public byte UserClassRow { get { return _UserClassRow; } set { _UserClassRow = value; } }
        public byte UserClassColumn { get { return _UserClassColumn; } set { _UserClassColumn = value; } }
        public byte RankId { get { return _RankId; } set { _RankId = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.UserClassId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((this.UserId > 0)
                && (this.ClassId > 0)
                && (this.RankId > 0)
                )
            {
                RetVal = "INSERT INTO V$UserClasses(UserId,ClassId,UserClassRow,UserClassColumn,RankId,CrUserId,CrDateTime)";
                RetVal += " VALUES (" + this.UserId.ToString();
                RetVal += "," + this.ClassId.ToString();
                RetVal += "," + this.UserClassRow.ToString();
                RetVal += "," + this.UserClassColumn.ToString();
                RetVal += "," + this.RankId.ToString();
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
            if ((this.UserClassId > 0)
                && (this.UserId > 0)
                && (this.ClassId > 0)
                )
            {
                RetVal = "UPDATE UserClasses SET ";
                RetVal += " UserId=" + this.UserId.ToString();
                RetVal += ",ClassId=" + this.ClassId.ToString();
                RetVal += ",UserClassRow=" + this.UserClassRow.ToString();
                RetVal += ",UserClassColumn=" + this.UserClassColumn.ToString();
                RetVal += ",RankId=" + this.RankId.ToString();
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (UserClassId=" + this.UserClassId.ToString() + ")";
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
                RetVal += " WHERE ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers ";
                RetVal += " INNER JOIN ClassTestUserQuestions";
                RetVal += " ON ClassTestUserQuestionAnswers.ClassTestUserQuestionId = ClassTestUserQuestions.ClassTestUserQuestionId";
                RetVal += " INNER JOIN ClassTestUsers ";
                RetVal += " ON ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " INNER JOIN UserClasses";
                RetVal += " ON ClassTestUsers.UserClassId = UserClasses.UserClassId";
                RetVal += " AND UserClasses.UserClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestUserId";
                RetVal += " FROM ClassTestUserQuestions";
                RetVal += " INNER JOIN ClassTestUsers";
                RetVal += " ON ClassTestUserQuestions.ClassTestUserId = ClassTestUsers.ClassTestUserId";
                RetVal += " INNER JOIN UserClasses";
                RetVal += " ON UserClasses.UserClassId = ClassTestUsers.UserClassId";
                RetVal += " AND UserClasses.UserClassId =" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUsers";
                RetVal += " WHERE UserClassId =" + Id.ToString();
                RetVal += " DELETE FROM UserClasses";
                RetVal += " WHERE UserClassId =" + Id.ToString();

            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserClassId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActUserClassId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.UserClassId = Id;
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
        public bool Update(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserClassId)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActUserClassId, BuildSqlUpdate());
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public bool Delete(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserClassId, int Id)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActUserClassId, BuildSqlDelete(Id));
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //------------------------------------------------------------------------
        private List<UserClasses> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<UserClasses> l_UserClasses = new List<UserClasses>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    UserClasses m_UserClasses = new UserClasses(db.ConnectionString);
                    m_UserClasses.UserClassId = smartReader.GetInt32("UserClassId");
                    m_UserClasses.UserId = smartReader.GetInt32("UserId");
                    m_UserClasses.ClassId = smartReader.GetInt32("ClassId");
                    m_UserClasses.UserClassRow = smartReader.GetByte("UserClassRow");
                    m_UserClasses.UserClassColumn = smartReader.GetByte("UserClassColumn");
                    m_UserClasses.RankId = smartReader.GetByte("RankId");
                    m_UserClasses.CrUserId = smartReader.GetInt32("CrUserId");
                    m_UserClasses.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_UserClasses.Add(m_UserClasses);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_UserClasses;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserClasses> GetList(string LogFilePath, string LogFileName, int ClassId, string SeachKeyword)
        {
            string Condition = "";
            if (ClassId > 0)
            {
                Condition = " AND (V$UserClasses.ClassId = " + ClassId.ToString() + ")";
                if (!string.IsNullOrEmpty(SeachKeyword))
                {
                    Condition += " AND (V$Users.LastName =N'" + SeachKeyword + "')";
  
                }
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserClasses> GetListByUserClassId(string LogFilePath, string LogFileName, int UserClassId)
        {
            List<UserClasses> RetVal = new List<UserClasses>();
            try
            {
                string Condition = "";
                if (UserClassId > 0)
                {
                    Condition += " AND (UserClassId = " + UserClassId.ToString() + ")";
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
        public List<UserClasses> GetList(string LogFilePath, string LogFileName, int ClassId)
        {
            string Condition = "";
            if (ClassId > 0)
            {
                Condition = " AND (V$UserClasses.ClassId = " + ClassId.ToString() + ")";
            }
            return GetListUserClass(LogFilePath, LogFileName, Condition, "");
        }
        //------------------------------------------------------------------------
        public List<UserClasses> GetListByClassId(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<UserClasses> RetVal = new List<UserClasses>();
            try
            {
                string Sql = "SELECT * FROM V$UserClasses";
                Sql += " INNER JOIN V$Classes";
                Sql += " ON (V$UserClasses.ClassId = V$Classes.ClassId)";
                Sql += "INNER JOIN V$ClassTests";
                Sql += " ON (V$ClassTests.ClassId = V$Classes.ClassId)";
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
        public List<UserClasses> GetListUserClass(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<UserClasses> RetVal = new List<UserClasses>();
            try
            {
                string Sql = "SELECT * FROM V$UserClasses";
                Sql += " INNER JOIN V$Classes";
                Sql += " ON (V$UserClasses.ClassId = V$Classes.ClassId)";
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
        public List<UserClasses> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<UserClasses> RetVal = new List<UserClasses>();
            try
            {
                string Sql = "SELECT * FROM V$UserClasses";
                Sql += " INNER JOIN V$Users";
                Sql += " ON (V$UserClasses.UserId = V$Users.UserId)";
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
        public List<UserClasses> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserClasses> GetList(string LogFilePath, string LogFileName, int ClassTestId, byte UserClassRow, byte UserClassColumn)
        {
            List<UserClasses> RetVal = new List<UserClasses>();
            try
            {
                string Condition = "";
                if (ClassTestId > 0)
                {
                    Condition += " AND (V$ClassTests.ClassTestId = " + ClassTestId.ToString() + ")";  
                }
                if (UserClassRow > 0)
                {
                    Condition += " AND (V$UserClasses.UserClassRow = " + UserClassRow.ToString() + ")";
                }
                if (UserClassColumn > 0)
                {
                    Condition += " AND (V$UserClasses.UserClassColumn = " + UserClassColumn.ToString() + ")";
                }
                RetVal = GetListByClassId(LogFilePath, LogFileName, Condition, "");
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }

        //------------------------------------------------------------------------
        public List<UserClasses> GetListByUserId(string LogFilePath, string LogFileName, int UserId)
        {
            List<UserClasses> RetVal = new List<UserClasses>();
            try
            {
                if (UserId > 0)
                {
                    string Condition = " AND (V$UserClasses.ClassId = " + UserId.ToString() + ")";
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
        public UserClasses Get(string LogFilePath, string LogFileName, int UserClassId)
        {
            UserClasses RetVal = new UserClasses(db.ConnectionString);
            try
            {
                List<UserClasses> l_UserClasses = GetListByUserClassId(LogFilePath, LogFileName, UserClassId);
                if (l_UserClasses.Count > 0)
                {
                    RetVal = (UserClasses)l_UserClasses[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserClasses> GetListByUserClassesId(string LogFilePath, string LogFileName, int UserClassId)
        {
            List<UserClasses> retVal = new List<UserClasses>();
            try
            {
                if (UserClassId > 0)
                {
                    string Condition = "(UserClassId=" + UserClassId.ToString() + ")";
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
        public UserClasses Get(List<UserClasses> l_UserClasses, long UserClassId)
        {
            UserClasses RetVal = new UserClasses(db.ConnectionString);
            foreach (UserClasses mUserClasses in l_UserClasses)
            {
                if (mUserClasses.UserClassId == UserClassId)
                {
                    RetVal = mUserClasses;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public string GetUserIds(List<UserClasses> lUserClasses)
        {
            string RetVal = "";
            foreach (UserClasses mUserClasses in lUserClasses)
            {
                RetVal += mUserClasses.UserId.ToString() + ",";
            }
            return StringUtils.RemoveLastString(RetVal, ","); ;
        }


        //-------------------------------------------------------------------------------------------------------------
        public List<UserClasses> Copy(List<UserClasses> l_UserClasses)
        {
            List<UserClasses> retVal = new List<UserClasses>();
            foreach (UserClasses mUserClasses in l_UserClasses)
            {
                retVal.Add(mUserClasses);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<UserClasses> CopyAll(List<UserClasses> l_UserClasses)
        {
            UserClasses m_UserClasses = new UserClasses(db.ConnectionString);
            List<UserClasses> retVal = m_UserClasses.Copy(l_UserClasses);
            retVal.Insert(0, m_UserClasses);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<UserClasses> CopyNa(List<UserClasses> l_UserClasses)
        {
            UserClasses m_UserClasses = new UserClasses(db.ConnectionString);
            List<UserClasses> retVal = m_UserClasses.Copy(l_UserClasses);
            retVal.Insert(0, m_UserClasses);
            return retVal;
        }
    }//end UserClasses
}
