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
    public class UserActions
    {
        private int _UserActionId; 
        private int _ActionId;
        private int _UserId;
        private int _CrUserId;
		private DateTime _CrDateTime;
        private DBAccess db;
        public int UserActionId { get { return _UserActionId; } set { _UserActionId = value; } }
        public int ActionId { get { return _ActionId; } set { _ActionId = value; } }
        public int UserId { get { return _UserId; } set { _UserId = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
		public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public UserActions(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        } 
        //-------------------------------------------------------------------------------------
        ~UserActions()
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
            if (this.UserId > 0 && this.ActionId > 0)
            {
                RetVal = "INSERT INTO V$UserActions(ActionId, UserId, CrUserId, CrDateTime)";
                RetVal += " VALUES (" + this.ActionId.ToString() ;
                RetVal += "," + this.UserId.ToString() ;
                RetVal += "," + this.CrUserId.ToString() ;
                RetVal += ",CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlUpdate()
        {
            string RetVal = "";
            if (this.ActionId > 0 && this.UserId > 0
                )
            {
                RetVal = "UPDATE V$UserActions SET ";
                RetVal += "ActionId=" + this.ActionId.ToString() ;
                RetVal += ",UserId=" + this.UserId.ToString() ;
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (UserActionId=" + this.UserActionId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int ActionId, int UserId)
        {
            string RetVal = "";
            if (ActionId > 0 && UserId > 0)
            {
                RetVal = "DELETE FROM UserActions";
                RetVal += " WHERE (ActionId=" + ActionId.ToString() + ")";
                RetVal += " AND (UserId=" + UserId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int UserActionId)
        {
            string RetVal = "";
            if (UserActionId > 0)
            {
                RetVal = "DELETE FROM UserActions";
                RetVal += " WHERE (UserActionId=" + UserActionId.ToString() + ")";
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
                        this.UserActionId = Id;
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
        //-------------------------------------------------------------------------------------
        private List<UserActions> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<UserActions> l_UserActions = new List<UserActions>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {

                    UserActions m_UserActions = new UserActions(db.ConnectionString);
                    m_UserActions.UserActionId = smartReader.GetInt32("UserActionId");
                    m_UserActions.ActionId = smartReader.GetInt32("ActionId");
                    m_UserActions.UserId = smartReader.GetInt32("UserId");
                    m_UserActions.CrUserId = smartReader.GetInt32("CrUserId");
                    m_UserActions.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_UserActions.Add(m_UserActions);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_UserActions;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserActions> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<UserActions> retVal = new List<UserActions>();
            try
            {
                string Sql = "SELECT * FROM V$UserActions";
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
                retVal = Init(LogFilePath, LogFileName, cmd);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;

        }
        //------------------------------------------------------------------------------
        public UserActions GetUnique(List<UserActions> lUserActions, int UserId, short ActionId)
        {
            UserActions RetVal = new UserActions(db.ConnectionString);
            if (UserId > 0)
            {
                if (ActionId > 0)
                {
                    foreach (UserActions mUserActions in lUserActions)
                    {
                        if ((mUserActions.UserId == UserId) && (mUserActions.ActionId == ActionId))
                        {
                            RetVal = mUserActions;
                            break;
                        }
                    }
                }
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------
        public List<UserActions> GetListByUserId(string LogFilePath, string LogFileName, int UserId)
        {
            List<UserActions> RetVal = new List<UserActions>();
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
        public List<UserActions> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserActions> GetList(string LogFilePath, string LogFileName, int UserId)
        {
            string Condition = "";
            if (UserId > 0)
            {
                Condition = "(UserId = " + UserId.ToString() + ")";
            } 
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<UserActions> GetListByUserActionId(string LogFilePath, string LogFileName, int UserActionId)
        {
            List<UserActions> retVal = new List<UserActions>();
            try
            {
                if (UserActionId > 0)
                {
                    string Condition = "(UserActionId=" + UserActionId.ToString() + ")";
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
        public List<UserActions> GetListByUserActionIds(string LogFilePath, string LogFileName, string UserActionIds)
        {
            List<UserActions> retVal = new List<UserActions>();
            try
            {
                UserActionIds = StringUtils.Static_InjectionString(UserActionIds);
                if (!string.IsNullOrEmpty(UserActionIds))
                {
                    string Condition = "(UserActionId IN (" + UserActionIds + "))";
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
        public UserActions Get(string LogFilePath, string LogFileName, int UserActionId)
        {
            UserActions retVal = new UserActions(db.ConnectionString);
            try
            {
                List<UserActions> list = GetListByUserActionId(LogFilePath, LogFileName, UserActionId);
                if (list.Count > 0)
                {
                    retVal = (UserActions)list[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        } 
        //--------------------------------------------------------------------------------------------------------------------
        public static List<UserActions> Static_GetList(string LogFilePath, string LogFileName, string constr)
        {
            List<UserActions> retVal = new List<UserActions>();
            UserActions m_UserActions = new UserActions(constr);
            try
            {
                retVal = m_UserActions.GetList(LogFilePath, LogFileName);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_UserActions.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public UserActions Get(List<UserActions> l_UserActions, int UserActionId)
        {
            UserActions RetVal = new UserActions(db.ConnectionString);
            if (UserActionId > 0)
            {
                foreach (UserActions mUserActions in l_UserActions)
                {
                    if (mUserActions.UserActionId == UserActionId)
                    {
                        RetVal = mUserActions;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetUserActionIds(List<UserActions> l_UserActions)
        {
            string retVal = "";
            foreach (UserActions mUserActions in l_UserActions)
            {
                if (!StringUtils.IsMember(retVal.Split(','), mUserActions.ActionId.ToString()))
                { 
                    retVal += mUserActions.UserActionId.ToString() + ","; 
                }
            }
            return StringUtils.RemoveLastString(retVal, ",");
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<UserActions> Copy(List<UserActions> l_UserActions)
        {
            List<UserActions> retVal = new List<UserActions>();
            foreach (UserActions mUserActions in l_UserActions)
            {
                retVal.Add(mUserActions);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<UserActions> CopyAll(List<UserActions> l_UserActions)
        {
            UserActions m_UserActions = new UserActions(db.ConnectionString);
            List<UserActions> retVal = m_UserActions.Copy(l_UserActions);
            retVal.Insert(0, m_UserActions);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<UserActions> CopyNa(List<UserActions> l_UserActions)
        {
            UserActions m_UserActions = new UserActions(db.ConnectionString);
            List<UserActions> retVal = m_UserActions.Copy(l_UserActions);
            retVal.Insert(0, m_UserActions);
            return retVal;
        }
    }
}

