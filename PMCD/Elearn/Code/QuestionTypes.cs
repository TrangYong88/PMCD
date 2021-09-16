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
    public class QuestionTypes
    {
        private byte _QuestionTypeId;
        private string _QuestionTypeName;
        private string _QuestionTypeDesc;
        DBAccess db;
        //----------------------------------------------------------------
        public QuestionTypes(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //-------------------------------------------------------------------------------------
        public QuestionTypes(string constr, string Name, string Desc)
        {
            QuestionTypeId = 0;
            QuestionTypeName = Name;
            QuestionTypeDesc = Desc;
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }
        //----------------------------------------------------------------
        public byte QuestionTypeId { get { return _QuestionTypeId; } set { _QuestionTypeId = value; } }
        public string QuestionTypeName { get { return _QuestionTypeName; } set { _QuestionTypeName = value; } }
        public string QuestionTypeDesc { get { return _QuestionTypeDesc; } set { _QuestionTypeDesc = value; } }
        //----------------------------------------------------------        
        private List<QuestionTypes> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<QuestionTypes> QuestionTypeList = new List<QuestionTypes>();
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    QuestionTypes m_QuestionTypes = new QuestionTypes(db.ConnectionString);
                    m_QuestionTypes.QuestionTypeId = smartReader.GetByte("QuestionTypeId");
                    m_QuestionTypes.QuestionTypeName = smartReader.GetString("QuestionTypeName");
                    m_QuestionTypes.QuestionTypeDesc = smartReader.GetString("QuestionTypeDesc");
                    QuestionTypeList.Add(m_QuestionTypes);
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
            return QuestionTypeList;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.QuestionTypeName)))
            {
                RetVal = "INSERT INTO V$QuestionTypes(QuestionTypeName, QuestionTypeDesc)";
                RetVal += " VALUES (N'" + this.QuestionTypeName + "'";
                RetVal += ",N'" + this.QuestionTypeDesc + "'";
                RetVal += ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlUpdate()
        {
            string RetVal = "";
            if ((!string.IsNullOrEmpty(this.QuestionTypeName))
                )
            {
                RetVal = "UPDATE V$QuestionTypes SET ";
                RetVal += "QuestionTypeName=N'" + this.QuestionTypeName + "'";
                RetVal += ",QuestionTypeDesc=N'" + this.QuestionTypeDesc + "'";
                RetVal += " WHERE (QuestionTypeId=" + this.QuestionTypeId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal = "DELETE FROM Questions";
                RetVal += " WHERE (QuestionTypeId=" + Id.ToString() + ")";
                RetVal += "DELETE FROM QuestionTypes";
                RetVal += " WHERE (QuestionTypeId=" + Id.ToString() + ")";
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
                        this.QuestionTypeId = Convert.ToByte(Id);
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
        public List<QuestionTypes> GetList(string LogFilePath, string LogFileName, string KeyWord)
        {
            string Condition = "";
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "((QuestionTypeName = N'" + KeyWord + "') OR (QuestionTypeDesc = N'" + KeyWord + "'))";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //--------------------------------------------------------------
        public static List<QuestionTypes> Static_GetList(string LogFilePath, string LogFileName, string constr)
        {
            List<QuestionTypes> RetVal = new List<QuestionTypes>();
            QuestionTypes m_QuestionTypes = new QuestionTypes(constr);
            try
            {
                RetVal = m_QuestionTypes.GetList(LogFilePath, LogFileName);
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_QuestionTypes.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------
        public List<QuestionTypes> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<QuestionTypes> RetVal = new List<QuestionTypes>();
            try
            {
                string Sql = "SELECT * FROM V$QuestionTypes";
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
        public List<QuestionTypes> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------
        public List<QuestionTypes> GetList(string LogFilePath, string LogFileName, byte QuestionTypeId)
        {
            List<QuestionTypes> RetVal = new List<QuestionTypes>();
            try
            {
                if (QuestionTypeId > 0)
                {
                    string Condition = "(QuestionTypeId =" + QuestionTypeId.ToString() + ")";
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
        public QuestionTypes Get(string LogFilePath, string LogFileName, byte QuestionTypeId)
        {
            QuestionTypes RetVal = new QuestionTypes(db.ConnectionString);
            try
            {
                List<QuestionTypes> List = GetList(LogFilePath, LogFileName, QuestionTypeId);
                if (List.Count > 0)
                {
                    RetVal = (QuestionTypes)List[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //-------------------------------------------------------------
        public QuestionTypes Get(List<QuestionTypes> lQuestionTypes, byte QuestionTypeId)
        {
            QuestionTypes RetVal = new QuestionTypes(db.ConnectionString);
            if (QuestionTypeId > 0)
            {
                foreach (QuestionTypes mQuestionTypes in lQuestionTypes)
                {
                    if (mQuestionTypes.QuestionTypeId == QuestionTypeId)
                    {
                        RetVal = mQuestionTypes;
                        break;
                    }
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionTypes> Copy(List<QuestionTypes> lQuestionTypes)
        {
            List<QuestionTypes> RetVal = new List<QuestionTypes>();
            foreach (QuestionTypes mQuestionTypes in lQuestionTypes)
            {
                RetVal.Add(mQuestionTypes);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<QuestionTypes> CopyAll(List<QuestionTypes> lQuestionTypes)
        {
            List<QuestionTypes> RetVal = Copy(lQuestionTypes);
            RetVal.Insert(0, new QuestionTypes(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC));
            return RetVal;
        }
    }//end QuestionTypes
}//end 