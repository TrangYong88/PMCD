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
    public class TestTypes
    {
        private byte _TestTypeId;
        private string _TestTypeName;
        private int _TestTypeQuatityTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public TestTypes(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        } 
        //------------------------------------------------------------------------
        ~TestTypes()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------
        public byte TestTypeId { get { return _TestTypeId; } set { _TestTypeId = value; } }
        public string TestTypeName { get { return _TestTypeName; } set { _TestTypeName = value; } }
        public int TestTypeQuatityTime { get { return _TestTypeQuatityTime; } set { _TestTypeQuatityTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.TestTypeId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if (!string.IsNullOrEmpty(this.TestTypeName))
            {
                RetVal = "INSERT INTO V$TestTypes(TestTypeName,TestTypeQuatityTime)";
                RetVal += " VALUES ("; 
                RetVal += "N'" + this.TestTypeName + "'";
                RetVal += "," + this.TestTypeQuatityTime.ToString();
                RetVal += ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlUpdate()
        {
            string RetVal = "";
            if ((this.TestTypeId > 0)
                && (this.TestTypeQuatityTime > 0)
                && (!string.IsNullOrEmpty(this.TestTypeName))
                )
            {
                RetVal = "UPDATE TestTypes SET ";
                RetVal += "TestTypeName=N'" + this.TestTypeName + "'";
                RetVal += ",TestTypeQuatityTime=" + this.TestTypeQuatityTime.ToString(); 
                RetVal += " WHERE (TestTypeId=" + this.TestTypeId.ToString() + ")";
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
                RetVal += " WHERE (TestTypeId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM TestTypes";
                RetVal += " WHERE (TestTypeId=" + Id.ToString() + ")";
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
                        this.TestTypeId = Convert.ToByte(Id);
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
        private List<TestTypes> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<TestTypes> l_TestTypes = new List<TestTypes>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    TestTypes m_TestTypes = new TestTypes(db.ConnectionString);
                    m_TestTypes.TestTypeId = smartReader.GetByte("TestTypeId");
                    m_TestTypes.TestTypeName = smartReader.GetString("TestTypeName");
                    m_TestTypes.TestTypeQuatityTime = smartReader.GetInt32("TestTypeQuatityTime");
                    l_TestTypes.Add(m_TestTypes);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_TestTypes;
        }
        //------------------------------------------------------------------------
        public List<TestTypes> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<TestTypes> RetVal = new List<TestTypes>();
            try
            {
                string Sql = "SELECT * FROM V$TestTypes";
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
        public List<TestTypes> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<TestTypes> GetList(string LogFilePath, string LogFileName, string KeyWord)
        {
            string Condition = "";
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "(TestTypeName = N'" + KeyWord + "')";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<TestTypes> GetListOrderByName(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "TestTypeName");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<TestTypes> GetListByTestTypeId(string LogFilePath, string LogFileName, int TestTypeId)
        {
            List<TestTypes> RetVal = new List<TestTypes>();
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
        public List<TestTypes> GetListByTestTypeName(string LogFilePath, string LogFileName, string TestTypeName)
        {
            List<TestTypes> RetVal = new List<TestTypes>();
            try
            {
                TestTypeName = StringUtils.Static_InjectionString(TestTypeName);
                if (!string.IsNullOrEmpty(TestTypeName))
                {
                    string Condition = "(TestTypeName =N'" + TestTypeName + "')";
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

        public List<TestTypes> GetListUnique(string LogFilePath, string LogFileName, string TestTypeName)
        {
            List<TestTypes> RetVal = new List<TestTypes>();
            try
            {
                if (!string.IsNullOrEmpty(TestTypeName))
                {
                    string Condition = " (TestTypeName = N'" + TestTypeName + "')";
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
        public TestTypes Get(string LogFilePath, string LogFileName, int TestTypeId)
        {
            TestTypes RetVal = new TestTypes(db.ConnectionString);
            try
            {
                List<TestTypes> l_TestTypes = GetListByTestTypeId(LogFilePath, LogFileName, TestTypeId);
                if (l_TestTypes.Count > 0)
                {
                    RetVal = (TestTypes)l_TestTypes[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public TestTypes GetByTestTypeName(string LogFilePath, string LogFileName, string TestTypeName)
        {
            TestTypes RetVal = new TestTypes(db.ConnectionString);
            try
            {
                List<TestTypes> l_TestTypes = GetListByTestTypeName(LogFilePath, LogFileName, TestTypeName);
                if (l_TestTypes.Count > 0)
                {
                    RetVal = (TestTypes)l_TestTypes[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public TestTypes Get(List<TestTypes> l_TestTypes, long TestTypeId)
        {
            TestTypes RetVal = new TestTypes(db.ConnectionString);
            foreach (TestTypes mTestTypes in l_TestTypes)
            {
                if (mTestTypes.TestTypeId == TestTypeId)
                {
                    RetVal = mTestTypes;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public TestTypes GetUnique(List<TestTypes> lTestTypes, string TestTypeName)
        {
            TestTypes RetVal = new TestTypes(db.ConnectionString);

            if (!string.IsNullOrEmpty(TestTypeName))
            {
                foreach (TestTypes mTestTypes in lTestTypes)
                {
                    if (mTestTypes.TestTypeName == TestTypeName)
                    {
                        RetVal = mTestTypes;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public TestTypes GetUnique(string LogFilePath, string LogFileName, string TestTypeName)
        {
            TestTypes RetVal = new TestTypes(db.ConnectionString);
            try
            {
                List<TestTypes> l_TestTypes = GetListUnique(LogFilePath, LogFileName, TestTypeName);
                if (l_TestTypes.Count > 0)
                {
                    RetVal = (TestTypes)l_TestTypes[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TestTypes> Copy(List<TestTypes> l_TestTypes)
        {
            List<TestTypes> retVal = new List<TestTypes>();
            foreach (TestTypes mTestTypes in l_TestTypes)
            {
                retVal.Add(mTestTypes);
            }
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TestTypes> CopyAll(List<TestTypes> l_TestTypes)
        {
            TestTypes m_TestTypes = new TestTypes(db.ConnectionString);
            List<TestTypes> retVal = m_TestTypes.Copy(l_TestTypes);
            retVal.Insert(0, m_TestTypes);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TestTypes> CopyNa(List<TestTypes> l_TestTypes)
        {
            TestTypes m_TestTypes = new TestTypes(db.ConnectionString);
            List<TestTypes> retVal = m_TestTypes.Copy(l_TestTypes);
            retVal.Insert(0, m_TestTypes);
            return retVal;
        }
    }//end TestTypes
}
