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
    public class Answers
    {
        private int _AnswerId;
        private int _QuestionId;
        private byte _AnswerPoint;
        private string _AnswerContent;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-------------------------------------------------------------------------------------
        public Answers(string constr)
        {
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
        }

        //------------------------------------------------------------------------
        ~Answers()
        {
        }
        //------------------------------------------------------------------------
        public virtual void Dispose()
        {
        }
        //------------------------------------------------------------------------

        public int AnswerId { get { return _AnswerId; } set { _AnswerId = value; } }
        public int QuestionId { get { return _QuestionId; } set { _QuestionId = value; } }
        public byte AnswerPoint { get { return _AnswerPoint; } set { _AnswerPoint = value; } }
        public string AnswerContent { get { return _AnswerContent; } set { _AnswerContent = value; } }
        public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
        public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //-------------------------------------------------------------------------------------
        public string toString()
        {
            string Retval = this.AnswerId + "";
            return Retval;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlInsert()
        {
            string RetVal = "";
            if ((this.QuestionId > 0)
                && (this.QuestionId > 0)
                && (this.AnswerPoint > 0)
                )
            {
                RetVal = "INSERT INTO V$Answers(QuestionId,AnswerPoint,AnswerContent,CrUserId,CrDateTime)";
                RetVal += " VALUES (" + this.QuestionId.ToString();
                RetVal += "," + this.AnswerPoint.ToString();
                RetVal += ",N'" + this.AnswerContent + "'";
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
            if ((this.AnswerId > 0)
                && (this.QuestionId > 0))
            {
                RetVal = "UPDATE Answers SET ";
                RetVal += " QuestionId=" + this.QuestionId.ToString();
                RetVal += ",AnswerPoint=" + this.AnswerPoint.ToString();
                RetVal += ",AnswerContent=N'" + this.AnswerContent + "'";
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (AnswerId=" + this.AnswerId.ToString() + ")";
            }
            return RetVal;
        }
        //------------------------------------------------------------------------------
        public Answers GetUnique(List<Answers> lAnswers, int QuestionId)
        {
            Answers RetVal = new Answers(db.ConnectionString);
            if (QuestionId > 0)
            {
                foreach (Answers mAnswers in lAnswers)
                {
                    if (mAnswers.QuestionId == QuestionId)
                    {
                        RetVal = mAnswers;
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
                RetVal += "DELETE FROM ClassTestUserQuestionAnswers";
                RetVal += " WHERE (ClassTestUserQuestionAnswers.AnswerId=" + Id.ToString() + ")";
                RetVal += " DELETE FROM Answers";
                RetVal += " WHERE (AnswerId=" + Id.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        public bool Insert(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActAnswerId)
        {
            bool RetVal = false;
            try
            {
                int Id = 0;
                if (db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActAnswerId, BuildSqlInsert(), ref Id))
                {
                    if (Id > 0)
                    {
                        this.AnswerId = Id;
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
        public bool Update(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActAnswerId)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActAnswerId, BuildSqlUpdate());
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public bool Delete(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActAnswerId, int Id)
        {
            bool RetVal = false;
            try
            {
                RetVal = db.SqlExecute(LogFilePath, LogFileName, IpAddress, ActAnswerId, BuildSqlDelete(Id));
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }
        //------------------------------------------------------------------------
        private List<Answers> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
        {
            SqlConnection con = db.getConnection();
            cmd.Connection = con;
            List<Answers> l_Answers = new List<Answers>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Answers m_Answers = new Answers(db.ConnectionString);
                    m_Answers.AnswerId = smartReader.GetInt32("AnswerId");
                    m_Answers.QuestionId = smartReader.GetInt32("QuestionId");
                    m_Answers.AnswerPoint = smartReader.GetByte("AnswerPoint");
                    m_Answers.AnswerContent = smartReader.GetString("AnswerContent");
                    m_Answers.CrUserId = smartReader.GetInt32("CrUserId");
                    m_Answers.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_Answers.Add(m_Answers);
                }
                smartReader.disposeReader(reader);
                db.closeConnection(con);
            }
            catch (SqlException ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return l_Answers;
        }
        //------------------------------------------------------------------------
        public List<Answers> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
        {
            List<Answers> RetVal = new List<Answers>();
            try
            {
                string Sql = "SELECT * FROM V$Answers";
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
        public List<Answers> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Answers> GetListByAnswerId(string LogFilePath, string LogFileName, int AnswerId)
        {
            List<Answers> RetVal = new List<Answers>();
            try
            {
                if (AnswerId > 0)
                {
                    string Condition = "(AnswerId = " + AnswerId.ToString() + ")";
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
        public List<Answers> GetListByQuestionId(string LogFilePath, string LogFileName, int QuestionId)
        {
            List<Answers> RetVal = new List<Answers>();
            try
            {
                if (QuestionId > 0)
                {
                    string Condition = "(QuestionId = " + QuestionId.ToString() + ")";
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
        public Answers Get(string LogFilePath, string LogFileName, int AnswerId)
        {
            Answers RetVal = new Answers(db.ConnectionString);
            try
            {
                List<Answers> l_Answers = GetListByAnswerId(LogFilePath, LogFileName, AnswerId);
                if (l_Answers.Count > 0)
                {
                    RetVal = (Answers)l_Answers[0];
                }
            }
            catch (Exception ex)
            {
                LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
            }
            return RetVal;
        }


        //--------------------------------------------------------------------------------------------------------------------
        public Answers Get(List<Answers> l_Answers, long AnswerId)
        {
            Answers RetVal = new Answers(db.ConnectionString);
            foreach (Answers mAnswers in l_Answers)
            {
                if (mAnswers.AnswerId == AnswerId)
                {
                    RetVal = mAnswers;
                    break;
                }
            }
            return RetVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public string GetQuestionIds(List<Answers> lAnswers)
        {
            string RetVal = "";
            foreach (Answers mAnswers in lAnswers)
            {
                RetVal += mAnswers.QuestionId.ToString() + ",";
            }
            return StringUtils.RemoveLastString(RetVal, ","); ;
        }


        //-------------------------------------------------------------------------------------------------------------
        public List<Answers> Copy(List<Answers> l_Answers)
        {
            List<Answers> retVal = new List<Answers>();
            foreach (Answers mAnswers in l_Answers)
            {
                retVal.Add(mAnswers);
            }
            return retVal;
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Answers> GetList(string LogFilePath, string LogFileName, int QuestionId)
        {
            string Condition = "";
            if (QuestionId > 0)
            {
                Condition = "QuestionId = " + QuestionId.ToString();
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Answers> CopyAll(List<Answers> l_Answers)
        {
            Answers m_Answers = new Answers(db.ConnectionString);
            List<Answers> retVal = m_Answers.Copy(l_Answers);
            retVal.Insert(0, m_Answers);
            return retVal;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Answers> CopyNa(List<Answers> l_Answers)
        {
            Answers m_Answers = new Answers(db.ConnectionString);
            List<Answers> retVal = m_Answers.Copy(l_Answers);
            retVal.Insert(0, m_Answers);
            return retVal;
        }
    }//end Answers
}
