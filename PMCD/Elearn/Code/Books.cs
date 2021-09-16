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
	public class Books
	{
		private int _BookId;
		private string _BookName;
		private string _BookDesc;
		private byte _BookKindId;
		private int _CrUserId;
		private DateTime _CrDateTime;
		private DBAccess db;
		public int BookId { get { return _BookId; } set { _BookId = value; } }
		public string BookName { get { return _BookName; } set { _BookName = value; } }
		public string BookDesc { get { return _BookDesc; } set { _BookDesc = value; } }
		public byte BookKindId { get { return _BookKindId; } set { _BookKindId = value; } }
		public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
		public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
        //public string Access = "Data Source=DESKTOP-D7N3OKS;Initial Catalog=ScoringSystem;Persist Security Info=True;User ID=trangyong; Password = admin";
        //-------------------------------------------------------------------------------------
		public Books(string constr)
		{
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
		}
		//-------------------------------------------------------------------------------------
		public Books(string constr, string Name, string Desc)
		{
			this.BookId = 0;
			this.BookName = Name;
			this.BookDesc = Desc;
			db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
		}
		//-------------------------------------------------------------------------------------
		~Books()
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
            if ((this.BookKindId > 0)
                && (!string.IsNullOrEmpty(this.BookName))
                )
            {
                RetVal = "INSERT INTO Books(BookName, BookDesc, BookKindId, CrUserId, CrDateTime)";
                RetVal += " VALUES (N'" + this.BookName + "'";
                RetVal += ",N'" + this.BookDesc + "'";
                RetVal += "," + this.BookKindId.ToString();
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
            if ((this.BookKindId > 0) 
                && (!string.IsNullOrEmpty(this.BookName))
                )
            {
                RetVal = "UPDATE Books SET ";
                RetVal += "BookName=N'" + this.BookName + "'";
                RetVal += ",BookDesc=N'" + this.BookDesc + "'";
                RetVal += ",BookKindId=" + this.BookKindId.ToString();
                RetVal += ",CrUserId=" + this.CrUserId.ToString();
                RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
                RetVal += " WHERE (BookId=" + this.BookId.ToString() + ")";
            }
            return RetVal;
        }
        //-------------------------------------------------------------------------------------
        private string BuildSqlDelete(int Id)
        {
            string RetVal = "";
            if (Id > 0)
            {
                RetVal = "DELETE FROM Books";
                RetVal += " WHERE (BookId=" + Id.ToString() + ")";
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
                        this.BookId = Id;
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
        private List<Books> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
		{
			SqlConnection con = db.getConnection();
			cmd.Connection = con;
			List<Books> l_Books = new List<Books>();
			try
			{
				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				SmartDataReader smartReader = new SmartDataReader(reader);
				while (smartReader.Read())
				{

					Books m_Books = new Books(db.ConnectionString);
					m_Books.BookId = smartReader.GetInt32("BookId");
					m_Books.BookName = smartReader.GetString("BookName");
					m_Books.BookDesc = smartReader.GetString("BookDesc");
					m_Books.BookKindId = smartReader.GetByte("BookKindId");
					m_Books.CrUserId = smartReader.GetInt32("CrUserId");
					m_Books.CrDateTime = smartReader.GetDateTime("CrDateTime");
					l_Books.Add(m_Books);
				}
				smartReader.disposeReader(reader);
				db.closeConnection(con);
			}
			catch (SqlException ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return l_Books;
		}
		//--------------------------------------------------------------------------------------------------------------------
        public List<Books> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
		{
			List<Books> retVal = new List<Books>();
			try
			{
				string Sql = "SELECT * FROM V$Books";
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
        public List<Books> GetList(string LogFilePath, string LogFileName)
        {
            return GetList(LogFilePath, LogFileName, "", "");
        }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Books> GetList(string LogFilePath, string LogFileName, byte BookKindId, string KeyWord)
        {
            string Condition = "";
            if (BookKindId > 0)
            {
                Condition = "(BookKindId = "+ BookKindId.ToString() +")";
            }
            if (!string.IsNullOrEmpty(KeyWord))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ";
                }
                Condition += "((BookName = N'" + KeyWord + "') OR (BookDesc = N'" + KeyWord + "'))";
            }
            return GetList(LogFilePath, LogFileName, Condition, "");
        }
		//--------------------------------------------------------------------------------------------------------------------
		public List<Books> GetListOrderByName(string LogFilePath, string LogFileName)
		{
            return GetList(LogFilePath, LogFileName, "", "BookName");
		}
		//--------------------------------------------------------------------------------------------------------------------
		public List<Books> GetListOrderByDesc(string LogFilePath, string LogFileName)
		{
            return GetList(LogFilePath, LogFileName, "", "BookDesc");
		}
		//--------------------------------------------------------------------------------------------------------------------
		public List<Books> GetListByBookId(string LogFilePath, string LogFileName, int BookId)
		{
			List<Books> retVal = new List<Books>();
			try
			{
				if (BookId > 0)
				{
                    string Condition = "(BookId=" + BookId.ToString() + ")";
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
		public List<Books> GetListByBookIds(string LogFilePath, string LogFileName, string BookIds)
		{
			List<Books> retVal = new List<Books>();
			try
			{
				BookIds = StringUtils.Static_InjectionString(BookIds);
				if (!string.IsNullOrEmpty(BookIds))
				{
                    string Condition = "(BookId = " + BookId.ToString() + ")";
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
		public Books Get(string LogFilePath, string LogFileName, int BookId)
		{
			Books retVal = new Books(db.ConnectionString);
			try
			{
				List<Books> list = GetListByBookId(LogFilePath, LogFileName, BookId);
				if (list.Count > 0)
				{
					retVal = (Books)list[0];
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return retVal;
		}
		//--------------------------------------------------------------------------------------------------------------------
		public static string Static_GetBookName(string LogFilePath, string LogFileName, string constr, int BookId)
		{
			string retVal = "";
			Books m_Books = new Books(constr);
			try
			{
				m_Books = m_Books.Get(LogFilePath, LogFileName, BookId);
				if (!string.IsNullOrEmpty(m_Books.BookName))
				{
					retVal = m_Books.BookName;
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_Books.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return retVal;
		}
		//--------------------------------------------------------------------------------------------------------------------
		public static string Static_GetBookName(string constr, int BookId)
		{
			string LogFilePath="";
			string LogFileName="";
			return Static_GetBookName(LogFilePath, LogFileName,constr, BookId);
		}
		//--------------------------------------------------------------------------------------------------------------------
		public static string Static_GetBookDesc(string LogFilePath, string LogFileName, string constr, int BookId)
		{
			string retVal = "";
			Books m_Books = new Books(constr);
			try
			{
				m_Books = m_Books.Get(LogFilePath, LogFileName, BookId);
				if (!string.IsNullOrEmpty(m_Books.BookDesc))
				{
					retVal = m_Books.BookDesc;
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_Books.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return retVal;
		}
		//--------------------------------------------------------------------------------------------------------------------
		public static List<Books> Static_GetList(string LogFilePath, string LogFileName, string constr)
		{
			List<Books> retVal = new List<Books>();
			Books m_Books = new Books(constr);
			try
			{
				retVal = m_Books.GetList(LogFilePath, LogFileName);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + m_Books.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return retVal;
		}
		//-------------------------------------------------------------------------------------------------------------
		public Books Get(List<Books> l_Books, int BookId)
		{
			Books RetVal = new Books(db.ConnectionString);
			if (BookId > 0)
			{
				foreach (Books mBooks in l_Books)
				{
					if (mBooks.BookId == BookId)
					{
						RetVal = mBooks;
						break;
					}
				}
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------------------------------
		public string GetBookIds(List<Books> l_Books)
		{
			string retVal = "";
			foreach (Books mBooks in l_Books)
			{
					retVal += mBooks.BookId.ToString()+",";
			}
			if (retVal.EndsWith(","))
			{
				retVal = retVal.Substring(0, retVal.Length-1);
			}
			return retVal;
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<Books> Copy(List<Books> l_Books)
		{
			List<Books> retVal = new List<Books>();
			foreach (Books mBooks in l_Books)
			{
				retVal.Add(mBooks);
			}
			return retVal;
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<Books> CopyAll(List<Books> l_Books)
		{
			Books m_Books = new Books(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC);
			List<Books> retVal = m_Books.Copy(l_Books);
			retVal.Insert(0, m_Books);
			return retVal;
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<Books> CopyNa(List<Books> l_Books)
		{
			Books m_Books = new Books(db.ConnectionString, StringUtils.NA, StringUtils.NA_DESC);
			List<Books> retVal = m_Books.Copy(l_Books);
			retVal.Insert(0, m_Books);
			return retVal;
		}
	}
}

