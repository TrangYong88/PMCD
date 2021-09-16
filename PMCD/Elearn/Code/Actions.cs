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
    public class Actions
    {
        private int _ActionId;
        private string _ActionName;
        private string _ActionDesc;
        private string _ActionUrl;
        private byte _ActionLevel;
        private int _ParentActionId;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        public int ActionId { get { return _ActionId; } set { _ActionId = value; } }
        public string ActionName { get { return _ActionName; } set { _ActionName = value; } }
        public string ActionDesc { get { return _ActionDesc; } set { _ActionDesc = value; } }
        public string ActionUrl { get { return _ActionUrl; } set { _ActionUrl = value; } }
        public byte ActionLevel { get { return _ActionLevel; } set { _ActionLevel = value; } }
        public int ParentActionId { get { return _ParentActionId; } set { _ParentActionId = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public Actions(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }

        //-------------------------------------------------------------------------------------
		public Actions(string constr, string Name, string Desc)
		{
			this.ActionId = 0;
			this.ActionName = Name;
			this.ActionDesc = Desc;
			db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
		}
        //-------------------------------------------------------------------------------------
        ~Actions()
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
            if ((!string.IsNullOrEmpty(this.ActionName)))
            {
                RetVal = "INSERT INTO V$Actions(ActionName, ActionDesc, ActionUrl, ActionLevel, ParentActionId, CrUserId, CrDateTime)";
                RetVal += " VALUES (N'" + this.ActionName + "'";
                RetVal += ",N'" + this.ActionDesc + "'";
                RetVal += ",N'" + this.ActionUrl + "'";
                RetVal += "," + this.ActionLevel.ToString();
                RetVal += "," + this.ParentActionId.ToString();
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
            if ((!string.IsNullOrEmpty(this.ActionName))
                )
            {
                RetVal = "UPDATE V$Actions SET ";
                RetVal += "ActionName=N'" + this.ActionName + "'";
                RetVal += ",ActionDesc=N'" + this.ActionDesc + "'";
                RetVal += ",ActionUrl=N'" + this.ActionUrl + "'";
                RetVal += ",ActionLevel=" + this.ActionLevel.ToString();
                RetVal += ",ParentActionId=" + this.ParentActionId.ToString();
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (ActionId=" + this.ActionId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal = "DELETE FROM UserActions";
                RetVal += " WHERE (ActionId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM Actions";
                RetVal += " WHERE (ParentActionId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM Actions";
                RetVal += " WHERE (ActionId=" + Id.ToString() + ")";
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
                        this.ActionId = Id;
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
        private List<Actions> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<Actions> l_Actions = new List<Actions>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {

                    Actions m_Actions = new Actions(db.ConnectionString);
                    m_Actions.ActionId = smartReader.GetInt32("ActionId");
                    m_Actions.ActionName = smartReader.GetString("ActionName");
                    m_Actions.ActionDesc = smartReader.GetString("ActionDesc");
                    m_Actions.ActionUrl = smartReader.GetString("ActionUrl");
                    m_Actions.ActionLevel = smartReader.GetByte("ActionLevel");
                    m_Actions.ParentActionId = smartReader.GetInt32("ParentActionId");
                    m_Actions.CrUserId = smartReader.GetInt32("CrUserId");
                    m_Actions.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_Actions.Add(m_Actions);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_Actions;
        }
        //------------------------------------------------------------------------------------
        public List<Actions> GetRootForUserId(string LogFilePath, string LogFileName, int UserId)
        {
            List<Actions> RetVal = new List<Actions>();
            try
            {
                if (UserId > 0)
                {
                   
                        string Condition = "(ParentActionId is Null)";
                        Condition += " AND (ActionId IN (SELECT ActionId FROM V$UserActions WHERE (UserId=" + UserId.ToString() + ")))";
                        RetVal = GetList(LogFilePath, LogFileName, Condition, "");
                    
                    
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //------------------------------------------------------------------------------------
        public StringBuilder GenMenuNew(string LogFilePath, string LogFileName, string constr, int UserId, string PRJ_ROOT, string ROOT_PATH, string fullName)
        {
            StringBuilder strMenu = new StringBuilder();
            int temp = 0;
            int tempChild = 0;
            try
            {
                strMenu.Append("[");
                List<Actions> l_UserNotRootActions = new List<Actions>();
                List<Actions> l_ActionsByUser = new List<Actions>();
                l_ActionsByUser = GetRootForUserId(LogFilePath, LogFileName, UserId);
                foreach (Actions mActions in l_ActionsByUser)
                {
                    ++temp;
                    strMenu.AppendFormat("[null, '{0}', null, null, null,", mActions.ActionDesc);
                    tempChild = 0;
                    l_UserNotRootActions = GetChildByUserId(LogFilePath, LogFileName, UserId, mActions.ActionId);
                    foreach (Actions actionChild in l_UserNotRootActions)
                    {
                        ++tempChild;
                        if (actionChild.ActionUrl.IndexOf("http://") == 0)
                        {
                            strMenu.AppendFormat(@"['<img src=""{0}"" />', '{1}', '{2}{3}', null,null]", ROOT_PATH + "admin/images/event_icon.png", actionChild.ActionDesc, ROOT_PATH, actionChild.ActionUrl);
                        }
                        else
                        {
                            strMenu.AppendFormat(@"['<img src=""{0}"" />', '{1}', '{2}{3}', null,null]", ROOT_PATH + "admin/images/event_icon.png", actionChild.ActionDesc, PRJ_ROOT, actionChild.ActionUrl);
                        }
                        if (tempChild < l_UserNotRootActions.Count)
                        {
                            strMenu.Append(",");
                        }
                    }
                    strMenu.Append("]");
                    if (temp < l_ActionsByUser.Count)
                    {
                        strMenu.Append(",_cmSplit,");
                    }
                }
                strMenu.Append("]");
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return strMenu;
        }
        //-------------------------------------------------------------------------------------------------------------------------------
        public List<Actions> GetChildByUserId(string LogFilePath, string LogFileName, int UserId, int ActionId)
        {
            List<Actions> RetVal = new List<Actions>();
            try
            {
                if (UserId > 0)
                {
                    if (ActionId > 0)
                    {
                        string Condition = "(ParentActionId = " + ActionId.ToString() + ")";
                        Condition += " AND (ActionId IN (SELECT ActionId FROM V$UserActions WHERE (UserId=" + UserId.ToString() + ")))";
                        RetVal = GetList(LogFilePath, LogFileName, Condition, "");
                    }
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------------------------
        public List<Actions> GetListByUrl(string LogFilePath, string LogFileName, int ActUserId, string Url)
        {
            List<Actions> RetVal = new List<Actions>();
            try
            {
                if (ActUserId > 0)
                {
                    if (!string.IsNullOrEmpty(Url))
                    {
                        string Condition = "('" + Url +"' like '%'+ActionUrl)";
                        Condition += " AND (ActionId IN (SELECT ActionId FROM V$UserActions WHERE (UserId=" + ActUserId.ToString() + ")))";
                        RetVal = GetList(LogFilePath, LogFileName, Condition, "");
                    }
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public Actions GetByUrl(string LogFilePath, string LogFileName, int ActUserId, string Url)
        {
            Actions RetVal = new Actions(db.ConnectionString);
            try
            {
                List<Actions> list = GetListByUrl(LogFilePath, LogFileName, ActUserId, Url);
                if (list.Count > 0)
                {
                    RetVal = (Actions)list[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //------------------------------------------------------------------------
        public List<Actions> GetListActionByUserId(string LogFilePath, string LogFileName, int UserId)
        {
            List<Actions> RetVal = new List<Actions>();
            try
            {
                string Sql = "SELECT V$Actions.* ";
                Sql += "FROM V$UserActions ";
                Sql += "INNER JOIN V$Users ON V$Users.UserId = V$UserActions.UserId ";
                Sql += "INNER JOIN V$Actions ON V$Actions.ActionId = V$UserActions.ActionId ";
                Sql += "AND V$Users.UserId =" + UserId.ToString();
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
        public List<Actions> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Actions> retVal = new List<Actions>();
            try
            {
                string Sql = "SELECT * FROM V$Actions";
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
        //--------------------------------------------------------------------------------------------------------------------
        public List<Actions> GetListByUserId(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Actions> retVal = new List<Actions>();
            try
            {
                string Sql = "SELECT V$Actions.ActionId FROM V$Actions";
                Sql += " INNER JOIN V$UserActions";
                Sql += " ON V$UserActions.ActionId = V$Actions.ActionId";
                Sql += " INNER JOIN V$Users";
                Sql += " ON V$UserActions.UserId = V$Users.UserId";
                Sql += " AND V$Actions.ActionUrl =N'AdmActions.aspx'";
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
        //--------------------------------------------------------------------------------------------------------------------
        public List<Actions> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Actions> GetList(string LogFilePath, string LogFileName, string KeyWord)
        {
            string Condition = "";
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "((ActionName = N'" + KeyWord + "') OR (ActionDesc = N'" + KeyWord + "'))";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Actions> GetList(string LogFilePath, string LogFileName, int UserId)
        {
            List<Actions> RetVal = new List<Actions>();
            try
            {
                if (UserId > 0)
                {
                    string Condition = " AND V$Users.UserId =" + UserId.ToString();
                    RetVal = GetListByUserId(LogFilePath, LogFileName, Condition, "");
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Actions> GetListOrderByName(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "ActionName");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Actions> GetListOrderByDesc(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "ActionDesc");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Actions> GetListByActionId(string LogFilePath, string LogFileName, int ActionId)
        {
            List<Actions> retVal = new List<Actions>();
            try
            {
                if (ActionId > 0)
                {
                    string Condition = "(ActionId=" + ActionId.ToString() + ")";
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
        public List<Actions> GetListByActionIds(string LogFilePath, string LogFileName, string ActionIds)
        {
            List<Actions> retVal = new List<Actions>();
            try
            {
                ActionIds = StringUtils.Static_InjectionString(ActionIds);
                if (!string.IsNullOrEmpty(ActionIds))
                {
                    string Condition = "(ActionId = " + ActionId.ToString() + ")";
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
        public Actions Get(string LogFilePath, string LogFileName, int ActionId)
        {
            Actions retVal = new Actions(db.ConnectionString);
            try
            {
                List<Actions> list = GetListByActionId(LogFilePath, LogFileName, ActionId);
                if (list.Count > 0)
                {
                    retVal = (Actions)list[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public static string Static_GetActionName(string LogFilePath, string LogFileName, string constr, int ActionId)
        {
            string retVal = "";
            Actions m_Actions = new Actions(constr);
            try
            {
                m_Actions = m_Actions.Get(LogFilePath, LogFileName, ActionId);
                if (!string.IsNullOrEmpty(m_Actions.ActionName))
                {
                    retVal = m_Actions.ActionName;
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_Actions.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public static string Static_GetActionName(string constr, int ActionId)
        {
            string LogFilePath = "";
            string LogFileName = "";
            return Static_GetActionName(LogFilePath, LogFileName, constr, ActionId);
        }
        //--------------------------------------------------------------------------------------------------------------------
        public static string Static_GetActionDesc(string LogFilePath, string LogFileName, string constr, int ActionId)
        {
            string retVal = "";
            Actions m_Actions = new Actions(constr);
            try
            {
                m_Actions = m_Actions.Get(LogFilePath, LogFileName, ActionId);
                if (!string.IsNullOrEmpty(m_Actions.ActionDesc))
                {
                    retVal = m_Actions.ActionDesc;
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_Actions.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public static List<Actions> Static_GetList(string LogFilePath, string LogFileName, string constr)
        {
            List<Actions> retVal = new List<Actions>();
            Actions m_Actions = new Actions(constr);
            try
            {
                retVal = m_Actions.GetList(LogFilePath, LogFileName);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_Actions.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Actions Get(List<Actions> l_Actions, int ActionId)
        {
            Actions RetVal = new Actions(db.ConnectionString);
            if (ActionId > 0)
            {
                foreach (Actions mActions in l_Actions)
                {
                    if (mActions.ActionId == ActionId)
                    {
                        RetVal = mActions;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetActionIds(List<Actions> l_Actions)
        {
            string retVal = "";
            foreach (Actions mActions in l_Actions)
            {
                retVal += mActions.ActionId.ToString() + ",";
            }
            if (retVal.EndsWith(","))
            {
                retVal = retVal.Substring(0, retVal.Length - 1);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Actions> Copy(List<Actions> l_Actions)
        {
            List<Actions> retVal = new List<Actions>();
            foreach (Actions mActions in l_Actions)
            {
                retVal.Add(mActions);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Actions> CopyAll(List<Actions> l_Actions)
        {
            Actions m_Actions = new Actions(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC);
            List<Actions> retVal = m_Actions.Copy(l_Actions);
            retVal.Insert(0, m_Actions);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Actions> CopyNa(List<Actions> l_Actions)
        {
            Actions m_Actions = new Actions(db.ConnectionString, StringUtils.NA, StringUtils.NA_DESC);
            List<Actions> retVal = m_Actions.Copy(l_Actions);
            retVal.Insert(0, m_Actions);
            return retVal;
        }
    }
}

