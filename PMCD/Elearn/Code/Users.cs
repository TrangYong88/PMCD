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
    public class Users
    {
        private int _UserId;
        private byte _UserStatusId;
        private string _UserName;
        private string _UserPass;
        private string _FirstName;
        private string _MiddleName;
        private string _LastName; 
        private DateTime _Birthday;
        private string _Address;
        private byte _GenderId;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public Users(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
		public Users(string constr, string Name, string Desc)
		{
			this.UserId = 0;
			this.UserName = Name;
			this.UserPass = Desc;
			db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
		}
        //------------------------------------------------------------------------
        ~Users()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------
        
        public int UserId { get { return _UserId; } set { _UserId = value; } }
        public byte UserStatusId { get { return _UserStatusId; } set { _UserStatusId = value; } }
        public string UserName { get { return _UserName; } set { _UserName = value; } }
        public string UserPass { get { return _UserPass; } set { _UserPass = value; } }
        public string FirstName { get { return _FirstName; } set { _FirstName = value; } }
        public string MiddleName { get { return _MiddleName; } set { _MiddleName = value; } }
        public string LastName { get { return _LastName; } set { _LastName = value; } }
        public string FirstMiddleName
        {
            get
            {
                string RetVal = this.FirstName;
                if (!string.IsNullOrEmpty(this.MiddleName))
                {
                    RetVal += " " + this.MiddleName;
                }
                return RetVal;
            }
        }
        public string FullName
        {
            get
            {
                string RetVal = this.FirstMiddleName;
                if (!string.IsNullOrEmpty(this.LastName))
                {
                    RetVal += " " + this.LastName;
                }
                return RetVal;
            }
        }
        public string UserNameFullName
        {
            get
            {
                string RetVal = "";
                if (!string.IsNullOrEmpty(this.UserName))
                {
                    RetVal = this.UserName.Trim() + " (" + this.FullName + ")";
                }
                return RetVal;
            }
        }
        public DateTime Birthday { get { return _Birthday; } set { _Birthday = value; } }
        public string Address { get { return _Address; } set { _Address = value; } }
        public byte GenderId { get { return _GenderId; } set { _GenderId = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.UserId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((this.UserStatusId > 0)
                && (!string.IsNullOrEmpty(this.UserName))
                )
            {
                RetVal = "INSERT INTO V$Users(UserStatusId,UserName,UserPass,FirstName,MiddleName,LastName,Birthday,Address,GenderId,CrUserId,CrDateTime)";
                RetVal += " VALUES (" + this.UserStatusId.ToString();
                RetVal += ",N'" + this.UserName + "'";
                RetVal += ",N'" + this.UserPass + "'";
                RetVal += ",N'" + this.FirstName + "'";
                RetVal += ",N'" + this.MiddleName + "'";
                RetVal += ",N'" + this.LastName + "'";
                RetVal += ",CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.Birthday, "-", ":") + "',121)";
                RetVal += ",N'" + this.Address + "'";
                RetVal += "," + this.GenderId.ToString();
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
            if ((this.UserId > 0)
                && (this.UserStatusId > 0)
                && (!string.IsNullOrEmpty(this.UserName))
                )
            {
                RetVal = "UPDATE Users SET ";
                RetVal += " UserStatusId=" + this.UserStatusId.ToString();
                RetVal += ",UserName=N'" + this.UserName + "'";
                RetVal += ",UserPass=N'" + this.UserPass + "'";
                RetVal += ",FirstName=N'" + this.FirstName + "'";
                RetVal += ",MiddleName=N'" + this.MiddleName + "'";
                RetVal += ",LastName=N'" + this.LastName + "'";
                RetVal += ",Birthday = CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.Birthday, "-", ":") + "',121)";
                RetVal += ",Address=N'" + this.Address + "'";
                RetVal += ",GenderId=" + this.GenderId.ToString();
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (UserId=" + this.UserId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal += "DELETE FROM UserActions";
                RetVal += " WHERE (UserId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUsers,UserClasses, Users";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserQuestionId = ClassTestUserQuestionAnswers.ClassTestUserQuestionId";
                RetVal += " AND ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId";
                RetVal += " AND UserClasses.UserClassId = ClassTestUsers.UserClassId";
                RetVal += " AND Users.UserId = UserClasses.UserId";
                RetVal += " AND Users.UserId = " + Id.ToString() +")";
                RetVal += " DELETE FROM ClassTestUserQuestions";
                RetVal += " WHERE  ClassTestUserQuestions.ClassTestUserId =";
                RetVal += " (SELECT TOP(1) ClassTestUserQuestions.ClassTestUserId";
                RetVal += " FROM ClassTestUserQuestions, ClassTestUsers, UserClasses, Users";
                RetVal += " WHERE ClassTestUserQuestions.ClassTestUserId = ClassTestUsers.ClassTestUserId";
                RetVal += " AND UserClasses.UserClassId = ClassTestUsers.UserClassId";
                RetVal += " AND Users.UserId = UserClasses.UserId";
                RetVal += " AND Users.UserId = " + Id.ToString() +")";
                RetVal += " DELETE FROM ClassTestUsers";
                RetVal += " WHERE ClassTestUsers.ClassTestUserId =";
                RetVal += " (SELECT TOP(1) ClassTestUsers.ClassTestUserId";
                RetVal += " FROM ClassTestUsers, UserClasses, Users";
                RetVal += " WHERE UserClasses.UserClassId = ClassTestUsers.UserClassId";
                RetVal += " AND Users.UserId = UserClasses.UserId";
                RetVal += " AND Users.UserId = " + Id.ToString() +")";
                RetVal += " DELETE FROM UserClasses";
                RetVal += " WHERE (UserId = " + Id.ToString() + ")";
                RetVal += " DELETE FROM Users";
                RetVal += " WHERE (UserId=" + Id.ToString() + ")";
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
                        this.UserId = Id;
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
        public bool Delete(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserId, int Id)
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
        private List<Users> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<Users> l_Users = new List<Users>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Users m_Users = new Users(db.ConnectionString);
                    m_Users.UserId = smartReader.GetInt32("UserId");
                    m_Users.UserStatusId = smartReader.GetByte("UserStatusId");
                    m_Users.UserName = smartReader.GetString("UserName");
                    m_Users.UserPass = smartReader.GetString("UserPass");
                    m_Users.FirstName = smartReader.GetString("FirstName");
                    m_Users.MiddleName = smartReader.GetString("MiddleName");
                    m_Users.LastName = smartReader.GetString("LastName");
                    m_Users.Birthday = smartReader.GetDateTime("Birthday");
                    m_Users.Address = smartReader.GetString("Address");
                    m_Users.GenderId = smartReader.GetByte("GenderId");
                    m_Users.CrUserId = smartReader.GetInt32("CrUserId");
                    m_Users.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_Users.Add(m_Users);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_Users;
        } 
        //------------------------------------------------------------------------
        public List<Users> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Users> RetVal = new List<Users>();
            try
            {
                string Sql = "SELECT * FROM V$Users";
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
        //------------------------------------------------------------------------
        public List<Users> GetListByUserId(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Users> RetVal = new List<Users>();
            try
            {
                string Sql = "SELECT V$Users.UserId FROM V$Users";
                Sql += " INNER JOIN V$UserActions";
                Sql += " ON V$UserActions.UserId = V$Users.UserId";
                Sql += " INNER JOIN V$Actions";
                Sql += " ON V$UserActions.ActionId = V$Actions.ActionId";
                Sql += " AND V$Actions.ActionUrl =N'AdmActions.aspx'";
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
        public List<Users> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Users> GetList(string LogFilePath, string LogFileName, byte GenderId, string KeyWord)
        {
            string Condition = "";
            if (GenderId > 0)
            {
                Condition = "(GenderId = " + GenderId.ToString() + ")";
            }
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "(UserName = N'" + KeyWord + "')";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Users> GetList(string LogFilePath, string LogFileName, int UserId)
        {
            string Condition = "";
            if (UserId > 0)
            {
                Condition = "(UserId = " + UserId.ToString() + ")";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Users> GetListOrderByName(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "UserName");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Users> GetListByUserId(string LogFilePath, string LogFileName, int UserId)
        {
            List<Users> RetVal = new List<Users>();
            try
            {
                if (UserId > 0)
                {
                    string Condition = "(UserId = " + UserId.ToString() + ")";
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
        public List<Users> GetListByUserName(string LogFilePath, string LogFileName, string UserName)
        {
            List<Users> RetVal = new List<Users>();
            try
            {
                UserName = StringUtils.Static_InjectionString(UserName);
                if (StringUtils.IsValidUserName(UserName))
                {
                    string Condition = "(UserName =N'" + UserName + "')";
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
       
        public List<Users> GetListUnique(string LogFilePath, string LogFileName, string UserName)
        {
            List<Users> RetVal = new List<Users>();
            try
            { 
                    if (!string.IsNullOrEmpty(UserName))
                    {
                        string Condition = " (UserName = N'" + UserName + "')";
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
        public List<Users> GetListByUserStatusId(string LogFilePath, string LogFileName, int UserStatusId)
        {
            List<Users> RetVal = new List<Users>();
            try
            {
                if (UserStatusId > 0)
                {
                    string Condition = "(UserStatusId = " + UserStatusId.ToString() + ")";
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
        public Users Get(string LogFilePath, string LogFileName, int UserId)
        {
            Users RetVal = new Users(db.ConnectionString);
            try
            {
                List<Users> l_Users = GetListByUserId(LogFilePath, LogFileName, UserId);
                if (l_Users.Count > 0)
                {
                    RetVal = (Users)l_Users[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Users GetByUserName(string LogFilePath, string LogFileName, string UserName)
        {
            Users RetVal = new Users(db.ConnectionString);
            try
            {
                List<Users> l_Users = GetListByUserName(LogFilePath, LogFileName, UserName);
                if (l_Users.Count > 0)
                {
                    RetVal = (Users)l_Users[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public int GetUserId(string UserName, string UserPass, List<Users> l_Users)
        {
            for (int i = 0; i < l_Users.Count; i++)
            {
                if (l_Users[i].UserName.Equals(UserName) && l_Users[i].UserPass.Equals(UserPass))
                {
                    return l_Users[i].UserId;
                }
            }
            return 0;  
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Users Get(List<Users> l_Users, long UserId)
        {
            Users RetVal = new Users(db.ConnectionString);
            foreach (Users mUsers in l_Users)
            {
                if (mUsers.UserId == UserId)
                {
                    RetVal = mUsers;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public string GetUserStatusIds(List<Users> lUsers)
        {
            string RetVal = "";
            foreach (Users mUsers in lUsers)
            {
                RetVal += mUsers.UserStatusId.ToString() + ",";
            }
            return StringUtils.RemoveLastString(RetVal, ","); ;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Users GetUnique(List<Users> lUsers, string UserName)
        {
            Users RetVal = new Users(db.ConnectionString);
            
                if (!string.IsNullOrEmpty(UserName))
                {
                    foreach (Users mUsers in lUsers)
                    { 
                            if (mUsers.UserName == UserName)
                            {
                                RetVal = mUsers;
                                break;
                            }  
                    }
                }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public Users GetUnique(string LogFilePath, string LogFileName, string UserName)
        {
            Users RetVal = new Users(db.ConnectionString);
            try
            {
                List<Users> l_Users = GetListUnique(LogFilePath, LogFileName , UserName);
                if (l_Users.Count > 0)
                {
                    RetVal = (Users)l_Users[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Users> Copy(List<Users> l_Users)
        {
            List<Users> retVal = new List<Users>();
            foreach (Users mUsers in l_Users)
            {
                retVal.Add(mUsers);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Users> CopyAll(List<Users> l_Users)
        {
            Users m_Users = new Users(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC);
            List<Users> retVal = m_Users.Copy(l_Users);
            retVal.Insert(0, m_Users);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Users> CopyNa(List<Users> l_Users)
        {
            Users m_Users = new Users(db.ConnectionString, StringUtils.NA, StringUtils.NA_DESC);
            List<Users> retVal = m_Users.Copy(l_Users);
            retVal.Insert(0, m_Users);
            return retVal;
        }
    }//end Users
}
