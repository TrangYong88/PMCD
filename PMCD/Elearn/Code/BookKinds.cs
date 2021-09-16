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
	public class BookKinds
	{
		private byte _BookKindId;
		private string _BookKindName;
		private string _BookKindDesc;
		DBAccess db;
		//-------------------------------------------------------------------------------------
		public BookKinds(string constr)
		{
            db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
		}
		//-------------------------------------------------------------------------------------
		~BookKinds()
		{
		}
		//-------------------------------------------------------------------------------------
		public virtual void Dispose()
		{
		}
		//---------------------------------------------------------------------------------
		public BookKinds(string constr, string Name, string Desc)
		{
			BookKindId = 0;
			BookKindName = Name;
			BookKindDesc = Desc;
			db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
		}
		public byte BookKindId		{			get			{				return _BookKindId;			}			set			{				_BookKindId = value;			}		}
		public string BookKindName		{			get			{				return _BookKindName;			}			set			{				_BookKindName = value;			}		}
		public string BookKindDesc		{			get			{				return _BookKindDesc;			}			set			{				_BookKindDesc = value;			}		}
		//----------------------------------------------------------        
		private List<BookKinds> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
		{
			SqlConnection con = db.getConnection();
			cmd.Connection = con;
			List<BookKinds> l_BookKinds = new List<BookKinds>();
			try
			{
				if (con.State == ConnectionState.Closed) con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				SmartDataReader smartReader = new SmartDataReader(reader);
				while (smartReader.Read())
				{
					BookKinds m_BookKinds = new BookKinds(db.ConnectionString);
					m_BookKinds.BookKindId = smartReader.GetByte("BookKindId");
					m_BookKinds.BookKindName = smartReader.GetString("BookKindName");
					m_BookKinds.BookKindDesc = smartReader.GetString("BookKindDesc");
					l_BookKinds.Add(m_BookKinds);
				}
				smartReader.DisposeReader(reader);
			}
			catch (SqlException ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			finally
			{
				con.Close();
			}
			return l_BookKinds;
		}
		//--------------------------------------------------------------
		public List<BookKinds> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
		{
			List<BookKinds> RetVal = new List<BookKinds>();
			try
			{
				string Sql = "SELECT * FROM V$BookKinds";
				if (!string.IsNullOrEmpty(Condition))
				{
					Sql += " WHERE (" + Condition + ")";
				}
				if (!string.IsNullOrEmpty(OrderBy))
				{
					Sql += " ORDER BY " + OrderBy;
				}
				SqlCommand cmd = new SqlCommand(Sql);
				RetVal = Init(LogFilePath, LogFileName, cmd);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//--------------------------------------------------------------
		public List<BookKinds> GetList(string LogFilePath, string LogFileName)
		{
			string Condition = "";
			string OrderBy = "";
			return GetList(LogFilePath, LogFileName, Condition, OrderBy);
		}
		//--------------------------------------------------------------
		public static List<BookKinds> Static_GetList(string LogFilePath, string LogFileName, string constr)
		{
			List<BookKinds> RetVal = new List<BookKinds>();
			BookKinds m_BookKinds = new BookKinds(constr);
			try
			{
				RetVal = m_BookKinds.GetList(LogFilePath, LogFileName);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + m_BookKinds.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//--------------------------------------------------------------
		public static List<BookKinds> Static_GetListAll(string LogFilePath, string LogFileName, string constr)
		{
			List<BookKinds> RetVal = new List<BookKinds>();
			BookKinds m_BookKinds = new BookKinds(constr, StringUtils.ALL, StringUtils.ALL_DESC);
			try
			{
				RetVal = m_BookKinds.GetList(LogFilePath, LogFileName);
				RetVal.Insert(0, m_BookKinds);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + m_BookKinds.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//--------------------------------------------------------------
		public List<BookKinds> GetListNA(string LogFilePath, string LogFileName, string constr)
		{
			List<BookKinds> RetVal = new List<BookKinds>();
			BookKinds m_BookKinds = new BookKinds(constr, StringUtils.NA, StringUtils.NA_DESC);
			try
			{
				RetVal = m_BookKinds.GetList(LogFilePath, LogFileName);
				RetVal.Insert(0, m_BookKinds);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + m_BookKinds.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//--------------------------------------------------------------
		public List<BookKinds> GetChoose(string LogFilePath, string LogFileName)
		{
			List<BookKinds> RetVal = new List<BookKinds>();
			try
			{
				string sql = "SELECT * FROM V$BookKinds";
				SqlCommand cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;
				RetVal = Init(LogFilePath, LogFileName, cmd);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//--------------------------------------------------------------
		public List<BookKinds> GetListByBookKindId(string LogFilePath, string LogFileName, byte BookKindId)
		{
			List<BookKinds> RetVal = new List<BookKinds>();
			try
			{
				if (BookKindId > 0)
				{
					string sql = "SELECT * FROM V$BookKinds WHERE (BookKindId = " + BookKindId.ToString() + ")";
					SqlCommand cmd = new SqlCommand(sql);
					cmd.CommandType = CommandType.Text;
					RetVal = Init(LogFilePath, LogFileName, cmd);
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//-------------------------------------------------------------
		public BookKinds Get(string LogFilePath, string LogFileName, byte BookKindId)
		{
			BookKinds RetVal = new BookKinds(db.ConnectionString);
			try
			{
				if (BookKindId > 0)
				{
					List<BookKinds> l_BookKinds = RetVal.GetListByBookKindId(LogFilePath, LogFileName, BookKindId);
					if (l_BookKinds.Count > 0)
					{
						RetVal = (BookKinds)l_BookKinds[0];
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//-------------------------------------------------------------
		public BookKinds Get(List<BookKinds> l_BookKinds, byte BookKindId)
		{
			BookKinds RetVal = new BookKinds(db.ConnectionString);
			foreach(BookKinds mBookKinds in l_BookKinds)
			{
				if (mBookKinds.BookKindId==BookKindId)
				{
					RetVal = mBookKinds;
					break;
				}
			}
			return RetVal;
		}
		//-------------------------------------------------------------
		public static string Static_GetDesc(string LogFilePath, string LogFileName, string constr, byte BookKindId)
		{
			string RetVal = "";
			BookKinds m_BookKinds = new BookKinds(constr);
			try
			{
				m_BookKinds = m_BookKinds.Get(LogFilePath, LogFileName, BookKindId);
				if (!string.IsNullOrEmpty(m_BookKinds.BookKindDesc))
				{
					RetVal = m_BookKinds.BookKindDesc.Trim();
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + m_BookKinds.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------------------------------
		public BookKinds Copy(BookKinds mBookKinds)
		{
			BookKinds RetVal = new BookKinds(db.ConnectionString);
			RetVal.BookKindId = mBookKinds.BookKindId;
			RetVal.BookKindName = mBookKinds.BookKindName;
			RetVal.BookKindDesc = mBookKinds.BookKindDesc;
			return RetVal;
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<BookKinds> Copy(List<BookKinds> l_BookKinds)
		{
			List<BookKinds> RetVal = new List<BookKinds>();
			foreach (BookKinds mBookKinds in l_BookKinds)
			{
				RetVal.Add(mBookKinds);
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<BookKinds> CopyAll(List<BookKinds> l_BookKinds)
		{
			BookKinds m_BookKinds = new BookKinds(db.ConnectionString, StringUtils.ALL, StringUtils.ALL_DESC);
			List<BookKinds> RetVal = m_BookKinds.Copy(l_BookKinds);
			RetVal.Insert(0, m_BookKinds);
			return RetVal;
		}
	}
}
