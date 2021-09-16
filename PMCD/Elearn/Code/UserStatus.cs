using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Lib.Database;
using Lib.Utils;
namespace Lib.Elearn
{
    public class UserStatus
    {
        private byte _UserStatusId;
        private string _UserStatusName;
        private string _UserStatusDesc;
        DBAccess db;
        //----------------------------------------------------------------
        public UserStatus(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
        public UserStatus(string constr, string Name, string Desc)
        {
            UserStatusId = 0;
            UserStatusName = Name;
            UserStatusDesc = Desc;
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //----------------------------------------------------------------
        public byte UserStatusId { get { return _UserStatusId; } set { _UserStatusId = value; } }
        public string UserStatusName { get { return _UserStatusName; } set { _UserStatusName = value; } }
        public string UserStatusDesc { get { return _UserStatusDesc; } set { _UserStatusDesc = value; } }
        //----------------------------------------------------------        
        private List<UserStatus> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<UserStatus> UserStatusList = new List<UserStatus>();
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    UserStatus m_UserStatus = new UserStatus(db.ConnectionString);
                    m_UserStatus.UserStatusId = smartReader.GetByte("UserStatusId");
                    m_UserStatus.UserStatusName = smartReader.GetString("UserStatusName");
                    m_UserStatus.UserStatusDesc = smartReader.GetString("UserStatusDesc");
                    UserStatusList.Add(m_UserStatus);
                }
                smartReader.DisposeReader(reader);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                con.Close();
            }
            return UserStatusList;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.UserStatusName)))
            {
                RetVal = "INSERT INTO V$UserStatus(UserStatusName, UserStatusDesc)";
                RetVal += " VALUES (N'" + this.UserStatusName + "'";
                RetVal += ",N'" + this.UserStatusDesc + "'";
                RetVal += ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlUpdate()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.UserStatusName))
                )
            {
                RetVal = "UPDATE V$UserStatus SET ";
                RetVal += "UserStatusName=N'" + this.UserStatusName + "'";
                RetVal += ",UserStatusDesc=N'" + this.UserStatusDesc + "'";
                RetVal += " WHERE (UserStatusId=" + this.UserStatusId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal = "DELETE FROM Users";
                RetVal += " WHERE (UserStatusId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM UserStatus";
                RetVal += " WHERE (UserStatusId=" + Id.ToString() + ")";
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
                        this.UserStatusId = Convert.ToByte(Id);
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
        public bool Delete(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserId, byte Id)
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
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserStatus> GetList(string LogFilePath, string LogFileName, string KeyWord)
        {
            string Condition = "";
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "((UserStatusName = N'" + KeyWord + "') OR (UserStatusDesc = N'" + KeyWord + "'))";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------
        public static List<UserStatus> Static_GetList(string LogFilePath, string LogFileName, string constr)
        {
            List<UserStatus> RetVal = new List<UserStatus>();
            UserStatus m_UserStatus = new UserStatus(constr);
            try
            {
                RetVal = m_UserStatus.GetList(LogFilePath, LogFileName);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_UserStatus.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------
        public List<UserStatus> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<UserStatus> RetVal = new List<UserStatus>();
            try
            {
                string Sql = "SELECT * FROM V$UserStatus";
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
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserStatus> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------
        public List<UserStatus> GetList(string LogFilePath, string LogFileName, byte UserStatusId)
        {
            List<UserStatus> RetVal = new List<UserStatus>();
            try
            {
                if (UserStatusId > 0)
                {
                    string Condition = "(UserStatusId =" + UserStatusId.ToString() + ")";
                    RetVal = GetList(LogFilePath, LogFileName, Condition, ""); ;
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------
        public UserStatus Get(string LogFilePath, string LogFileName, byte UserStatusId)
        {
            UserStatus RetVal = new UserStatus(db.ConnectionString);
            try
            {
                List<UserStatus> List = GetList(LogFilePath, LogFileName, UserStatusId);
                if (List.Count > 0)
                {
                    RetVal = (UserStatus)List[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------
        public UserStatus Get(List<UserStatus> lUserStatus, byte UserStatusId)
        {
            UserStatus RetVal = new UserStatus(db.ConnectionString);
            if (UserStatusId > 0)
            {
                foreach (UserStatus mUserStatus in lUserStatus)
                {
                    if (mUserStatus.UserStatusId == UserStatusId)
                    {
                        RetVal = mUserStatus;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserStatus> Copy(List<UserStatus> lUserStatus)
        {
            List<UserStatus> RetVal = new List<UserStatus>();
            foreach (UserStatus mUserStatus in lUserStatus)
            {
                RetVal.Add(mUserStatus);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserStatus> CopyAll(List<UserStatus> lUserStatus)
        {
            List<UserStatus> RetVal = Copy(lUserStatus);
            RetVal.Insert(0, new UserStatus(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC));
            return RetVal;
        }
    }//end UserStatus
}//end 