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
	public class UploadFiles
	{
		private int _UploadFileId;
		private int _StudentExamQuestionId;
		private string _FileName;
		private int _AuthorUserId;
		private byte _MarkPoint;
		private int _CrUserId;
		private DateTime _CrDateTime;
		private DBAccess db;
		//-------------------------------------------------------------------------------------
		public UploadFiles(string constr)
		{
			db = new DBAccess((string.IsNullOrEmpty(constr)) ? ElearnConstants.ELEARN_CONSTR : constr);
		}
		//------------------------------------------------------------------------
		~UploadFiles()
		{
		}
		//------------------------------------------------------------------------
		public virtual void Dispose()
		{
		}
		//------------------------------------------------------------------------
		public int UploadFileId { get { return _UploadFileId; } set { _UploadFileId = value; } }
		public int StudentExamQuestionId { get { return _StudentExamQuestionId; } set { _StudentExamQuestionId = value; } }
		public string FileName { get { return _FileName; } set { _FileName = value; } }
		public int AuthorUserId { get { return _AuthorUserId; } set { _AuthorUserId = value; } }
		public byte MarkPoint { get { return _MarkPoint; } set { _MarkPoint = value; } }
		public int CrUserId { get { return _CrUserId; } set { _CrUserId = value; } }
		public DateTime CrDateTime { get { return _CrDateTime; } set { _CrDateTime = value; } }
		//-------------------------------------------------------------------------------------
		private string BuildSqlInsert()
		{
			string RetVal = "";
			if ((this.StudentExamQuestionId > 0)
				&& (!string.IsNullOrEmpty(this.FileName ))
				)
			{
				RetVal = "INSERT INTO UploadFiles(StudentExamQuestionId, FileName, AuthorUserId, MarkPoint, CrUserId, CrDateTime)";
				RetVal += " VALUES (" + this.StudentExamQuestionId.ToString();
				RetVal += ",N'" + this.FileName+"'";
				RetVal += "," + this.AuthorUserId.ToString();
				RetVal += "," + this.MarkPoint.ToString();
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
			if ((this.UploadFileId > 0)
				&& (this.StudentExamQuestionId > 0)
				&& (!string.IsNullOrEmpty(this.FileName))
				)
			{
				RetVal = "UPDATE UploadFiles SET ";
				RetVal += " StudentExamQuestionId=" + this.StudentExamQuestionId.ToString();
				RetVal += ",FileName=N'" + this.FileName+"'";
				RetVal += ",AuthorUserId=" + this.AuthorUserId.ToString();
				RetVal += ",MarkPoint=" + this.MarkPoint.ToString();
				RetVal += ",CrUserId=" + this.CrUserId.ToString();
				RetVal += ",CrDateTime=CONVERT(DATETIME,N'" + DateTimeUtils.Static_yyyymmddhhmissms(this.CrDateTime, "-", ":") + "',121)";
				RetVal += " WHERE (UploadFileId=" + this.UploadFileId.ToString() + ")";
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------
		private string BuildSqlDelete(long Id)
		{
			string RetVal = "";
			if (Id > 0)
			{
				RetVal = "DELETE FROM UploadFiles";
				RetVal += " WHERE (UploadFileId=" + Id.ToString() + ")";
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
						this.UploadFileId = Id;
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
		//---------------------------------------------------------------------------------------------------------
		public bool Insert(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserId
			, int StudentExamQuestionId, string FileName, byte MarkPoint)
		{
			this.StudentExamQuestionId = StudentExamQuestionId;
			this.FileName = FileName;
			this.AuthorUserId = ActUserId;
			this.MarkPoint = MarkPoint;
			this.CrUserId = ActUserId;
			this.CrDateTime = System.DateTime.Now;
			return this.Insert(LogFilePath, LogFileName, DistributedProcess, IpAddress, ActUserId);
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
		public bool Delete(string LogFilePath, string LogFileName, byte DistributedProcess, string IpAddress, int ActUserId, long Id)
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
		private List<UploadFiles> Init(string LogFilePath, string LogFileName, SqlCommand cmd)
		{
			SqlConnection con = db.getConnection();
			cmd.Connection = con;
			List<UploadFiles> l_UploadFiles = new List<UploadFiles>();
			try
			{
				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				SmartDataReader smartReader = new SmartDataReader(reader);
				while (smartReader.Read())
				{
					UploadFiles m_UploadFiles = new UploadFiles(db.ConnectionString);
					m_UploadFiles.UploadFileId = smartReader.GetInt32("UploadFileId");
					m_UploadFiles.StudentExamQuestionId = smartReader.GetInt32("StudentExamQuestionId");
					m_UploadFiles.FileName = smartReader.GetString("FileName");
					m_UploadFiles.AuthorUserId = smartReader.GetInt32("AuthorUserId");
					m_UploadFiles.MarkPoint = smartReader.GetByte("MarkPoint");
					m_UploadFiles.CrUserId = smartReader.GetInt32("CrUserId");
					m_UploadFiles.CrDateTime = smartReader.GetDateTime("CrDateTime");
					l_UploadFiles.Add(m_UploadFiles);
				}
				smartReader.disposeReader(reader);
				db.closeConnection(con);
			}
			catch (SqlException ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return l_UploadFiles;
		}
		//------------------------------------------------------------------------
		public List<UploadFiles> GetList(string LogFilePath, string LogFileName, string Condition, string OrderBy)
		{
			List<UploadFiles> RetVal = new List<UploadFiles>();
			try
			{
				string Sql = "SELECT * FROM V$UploadFiles";
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
		public List<UploadFiles> GetList(string LogFilePath, string LogFileName)
		{
			return GetList(LogFilePath, LogFileName, "", "");
		}
		//--------------------------------------------------------------------------------------------------------------------
		public List<UploadFiles> GetListByUploadFileId(string LogFilePath, string LogFileName, int UploadFileId)
		{
			List<UploadFiles> RetVal = new List<UploadFiles>();
			try
			{
				if (UploadFileId > 0)
				{
					string Condition = "(UploadFileId = " + UploadFileId.ToString() + ")";
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
		public List<UploadFiles> GetListUnique(string LogFilePath, string LogFileName, int StudentExamQuestionId, string FileName)
		{
			List<UploadFiles> RetVal = new List<UploadFiles>();
			try
			{
				if (StudentExamQuestionId > 0)
				{
					if (!string.IsNullOrEmpty(FileName))
					{
						string Condition = "(StudentExamQuestionId = " + StudentExamQuestionId.ToString() + ")";
						Condition += " AND (FileName = N'" + FileName + "')";
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
		//------------------------------------------------------------------------
		public List<UploadFiles> GetListByStudentExamQuestionId(string LogFilePath, string LogFileName, int StudentExamQuestionId)
		{
			List<UploadFiles> RetVal = new List<UploadFiles>();
			try
			{
				if (StudentExamQuestionId > 0)
				{
					string Condition = "(StudentExamQuestionId = " + StudentExamQuestionId.ToString() + ")";
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
		public List<UploadFiles> GetListByFileName(string LogFilePath, string LogFileName, string FileName)
		{
			List<UploadFiles> RetVal = new List<UploadFiles>();
			try
			{
				if (!string.IsNullOrEmpty(FileName))
				{
					string Condition = "(FileName = N'" + FileName + "')";
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
		public UploadFiles Get(string LogFilePath, string LogFileName, int UploadFileId)
		{
			UploadFiles RetVal = new UploadFiles(db.ConnectionString);
			try
			{
				List<UploadFiles> l_UploadFiles = GetListByUploadFileId(LogFilePath, LogFileName, UploadFileId);
				if (l_UploadFiles.Count > 0)
				{
					RetVal = (UploadFiles)l_UploadFiles[0];
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//--------------------------------------------------------------------------------------------------------------------
		public UploadFiles Get(List<UploadFiles> l_UploadFiles, long UploadFileId)
		{
			UploadFiles RetVal = new UploadFiles(db.ConnectionString);
			foreach (UploadFiles mUploadFiles in l_UploadFiles)
			{
				if (mUploadFiles.UploadFileId == UploadFileId)
				{
					RetVal = mUploadFiles;
					break;
				}
			}
			return RetVal;
		}
		//--------------------------------------------------------------------------------------------------------------------
		public string GetStudentExamQuestionIds(List<UploadFiles> lUploadFiles)
		{
			string RetVal = "";
			foreach (UploadFiles mUploadFiles in lUploadFiles)
			{
				RetVal += mUploadFiles.StudentExamQuestionId.ToString() + ",";
			}
			return StringUtils.RemoveLastString(RetVal, ","); ;
		}
		//--------------------------------------------------------------------------------------------------------------------
		public UploadFiles GetUnique(List<UploadFiles> lUploadFiles, int StudentExamQuestionId, string FileName)
		{
			UploadFiles RetVal = new UploadFiles(db.ConnectionString);
			if (StudentExamQuestionId > 0)
			{
				if (!string.IsNullOrEmpty(FileName))
				{
					foreach (UploadFiles mUploadFiles in lUploadFiles)
					{
						if (mUploadFiles.StudentExamQuestionId == StudentExamQuestionId)
						{
							if (mUploadFiles.FileName == FileName)
							{
								RetVal = mUploadFiles;
								break;
							}
						}
					}
				}
			}
			return RetVal;
		}
		//--------------------------------------------------------------------------------------------------------------------
		public UploadFiles GetUnique(string LogFilePath, string LogFileName, int StudentExamQuestionId, string FileName)
		{
			UploadFiles RetVal = new UploadFiles(db.ConnectionString);
			try
			{
				List<UploadFiles> l_UploadFiles = GetListUnique(LogFilePath, LogFileName, StudentExamQuestionId, FileName);
				if (l_UploadFiles.Count > 0)
				{
					RetVal = (UploadFiles)l_UploadFiles[0];
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//--------------------------------------------------------------------------------------------------------------------
		public UploadFiles GetUnique(string LogFilePath, string LogFileName, ref List<UploadFiles> lUploadFiles, int StudentExamQuestionId, string FileName)
		{
			UploadFiles RetVal = GetUnique(lUploadFiles, StudentExamQuestionId, FileName);
			if (RetVal.UploadFileId <= 0)
			{
				RetVal = GetUnique(LogFilePath, LogFileName, StudentExamQuestionId, FileName);
				if (RetVal.UploadFileId > 0)
				{
					lUploadFiles.Add(RetVal);
				}
			}
			return RetVal;
		}		
	}//end UploadFiles
}
