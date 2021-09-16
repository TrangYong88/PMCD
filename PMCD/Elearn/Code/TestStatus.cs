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
    public class TestStatus
    {
        private byte _TestStatusId;
        private string _TestStatusName;
        private string _TestStatusDesc;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public TestStatus(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //------------------------------------------------------------------------
        ~TestStatus()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------
        public byte TestStatusId { get { return _TestStatusId; } set { _TestStatusId = value; } }
        public string TestStatusName { get { return _TestStatusName; } set { _TestStatusName = value; } }
        public string TestStatusDesc { get { return _TestStatusDesc; } set { _TestStatusDesc = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.TestStatusId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if (!string.IsNullOrEmpty(this.TestStatusName))
            {
                RetVal = "INSERT INTO V$TestStatus(TestStatusName,TestStatusDesc)";
                RetVal += " VALUES (";
                RetVal += "N'" + this.TestStatusName + "'";
                RetVal += ",N'" + this.TestStatusDesc + "'";
                RetVal += ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlUpdate()
        {
            string RetVal = "";
            if ((this.TestStatusId > 0)
                && (!string.IsNullOrEmpty(this.TestStatusDesc))
                && (!string.IsNullOrEmpty(this.TestStatusName))
                )
            {
                RetVal = "UPDATE TestStatus SET ";
                RetVal += "TestStatusName=N'" + this.TestStatusName + "'";
                RetVal += ",TestStatusDesc=N'" + this.TestStatusDesc + "'";
                RetVal += " WHERE (TestStatusId=" + this.TestStatusId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal += "DELETE FROM ClassTests";
                RetVal += " WHERE (TestStatusId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM TestStatus";
                RetVal += " WHERE (TestStatusId=" + Id.ToString() + ")";
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
                        this.TestStatusId = Convert.ToByte(Id);
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
        private List<TestStatus> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<TestStatus> l_TestStatus = new List<TestStatus>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    TestStatus m_TestStatus = new TestStatus(db.ConnectionString);
                    m_TestStatus.TestStatusId = smartReader.GetByte("TestStatusId");
                    m_TestStatus.TestStatusName = smartReader.GetString("TestStatusName");
                    m_TestStatus.TestStatusDesc = smartReader.GetString("TestStatusDesc");
                    l_TestStatus.Add(m_TestStatus);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_TestStatus;
        }
        //------------------------------------------------------------------------
        public List<TestStatus> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<TestStatus> RetVal = new List<TestStatus>();
            try
            {
                string Sql = "SELECT * FROM V$TestStatus";
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
        public List<TestStatus> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<TestStatus> GetList(string LogFilePath, string LogFileName, string KeyWord)
        {
            string Condition = "";
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "(TestStatusName = N'" + KeyWord + "')";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<TestStatus> GetListOrderByName(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "TestStatusName");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<TestStatus> GetListByTestStatusId(string LogFilePath, string LogFileName, int TestStatusId)
        {
            List<TestStatus> RetVal = new List<TestStatus>();
            try
            {
                if (TestStatusId > 0)
                {
                    string Condition = "(TestStatusId = " + TestStatusId.ToString() + ")";
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
        public List<TestStatus> GetListByTestStatusName(string LogFilePath, string LogFileName, string TestStatusName)
        {
            List<TestStatus> RetVal = new List<TestStatus>();
            try
            {
                TestStatusName = StringUtils.Static_InjectionString(TestStatusName);
                if (!string.IsNullOrEmpty(TestStatusName))
                {
                    string Condition = "(TestStatusName =N'" + TestStatusName + "')";
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

        public List<TestStatus> GetListUnique(string LogFilePath, string LogFileName, string TestStatusName)
        {
            List<TestStatus> RetVal = new List<TestStatus>();
            try
            {
                if (!string.IsNullOrEmpty(TestStatusName))
                {
                    string Condition = " (TestStatusName = N'" + TestStatusName + "')";
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
        public TestStatus Get(string LogFilePath, string LogFileName, int TestStatusId)
        {
            TestStatus RetVal = new TestStatus(db.ConnectionString);
            try
            {
                List<TestStatus> l_TestStatus = GetListByTestStatusId(LogFilePath, LogFileName, TestStatusId);
                if (l_TestStatus.Count > 0)
                {
                    RetVal = (TestStatus)l_TestStatus[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public TestStatus GetByTestStatusName(string LogFilePath, string LogFileName, string TestStatusName)
        {
            TestStatus RetVal = new TestStatus(db.ConnectionString);
            try
            {
                List<TestStatus> l_TestStatus = GetListByTestStatusName(LogFilePath, LogFileName, TestStatusName);
                if (l_TestStatus.Count > 0)
                {
                    RetVal = (TestStatus)l_TestStatus[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public TestStatus Get(List<TestStatus> l_TestStatus, long TestStatusId)
        {
            TestStatus RetVal = new TestStatus(db.ConnectionString);
            foreach (TestStatus mTestStatus in l_TestStatus)
            {
                if (mTestStatus.TestStatusId == TestStatusId)
                {
                    RetVal = mTestStatus;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public TestStatus GetUnique(List<TestStatus> lTestStatus, string TestStatusName)
        {
            TestStatus RetVal = new TestStatus(db.ConnectionString);

            if (!string.IsNullOrEmpty(TestStatusName))
            {
                foreach (TestStatus mTestStatus in lTestStatus)
                {
                    if (mTestStatus.TestStatusName == TestStatusName)
                    {
                        RetVal = mTestStatus;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public TestStatus GetUnique(string LogFilePath, string LogFileName, string TestStatusName)
        {
            TestStatus RetVal = new TestStatus(db.ConnectionString);
            try
            {
                List<TestStatus> l_TestStatus = GetListUnique(LogFilePath, LogFileName, TestStatusName);
                if (l_TestStatus.Count > 0)
                {
                    RetVal = (TestStatus)l_TestStatus[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TestStatus> Copy(List<TestStatus> l_TestStatus)
        {
            List<TestStatus> retVal = new List<TestStatus>();
            foreach (TestStatus mTestStatus in l_TestStatus)
            {
                retVal.Add(mTestStatus);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TestStatus> CopyAll(List<TestStatus> l_TestStatus)
        {
            TestStatus m_TestStatus = new TestStatus(db.ConnectionString);
            List<TestStatus> retVal = m_TestStatus.Copy(l_TestStatus);
            retVal.Insert(0, m_TestStatus);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TestStatus> CopyNa(List<TestStatus> l_TestStatus)
        {
            TestStatus m_TestStatus = new TestStatus(db.ConnectionString);
            List<TestStatus> retVal = m_TestStatus.Copy(l_TestStatus);
            retVal.Insert(0, m_TestStatus);
            return retVal;
        }
    }//end TestStatus
}
