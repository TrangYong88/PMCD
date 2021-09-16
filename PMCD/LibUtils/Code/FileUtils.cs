using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
namespace Lib.Utils
{
	public class FileUtils
	{
		//----------------------------------------------------------------------------------------
		public static string GenerateBackupFileName(string FileName)
		{
			string RetVal = GetFileName(FileName);
			RetVal += "_" + DateTimeUtils.Static_yyyymmddhhmissms(System.DateTime.Now);
			string Ext = GetFileExtention(FileName);
			if (!string.IsNullOrEmpty(Ext))
			{
				RetVal += "." + Ext;
			}
			return RetVal;
		}
		//----------------------------------------------------------------------------
		public static bool Compare(string LogFilePath, string LogFileName, string FullFileName1, string FullFileName2)
		{
			bool RetVal = false;
			try
			{
				string[] Lines1 = ReadLines(LogFilePath, LogFileName, FullFileName1);
				string[] Lines2 = ReadLines(LogFilePath, LogFileName, FullFileName2);
				RetVal = StringUtils.Compare(LogFilePath, LogFileName, Lines1, Lines2);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//Read content in File

		public static string[] ReadLines(string LogFilePath, string LogFileName, string FileName)
		{
			string[] RetVal = null;
			try
			{
				if (System.IO.File.Exists(FileName))
				{
					RetVal = System.IO.File.ReadAllLines(FileName);
				}
				else
				{
					LogFiles.WriteLog("Not exists " + " FileName=" + FileName, LogFilePath + "\\" + SystemConstants.LogFilePath_NotExists, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message + " FileName=" + FileName, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------
		public static byte PageCalculate(int PageIndex, int RowAmount, ref int RowCount, string SearchCriterion, int SumLines, byte SumHead, byte SumTail, ref int TotalRecords, ref int RowFrom, ref int RowTo)
		{
			byte RetVal = SearchTypes_GetId(SearchCriterion);
			TotalRecords = SumLines - (SumHead + SumTail);
			RowFrom = 0;
			RowTo = 0;
			if (RetVal > 0)
			{
				RowFrom = SumHead;
				RowTo = SumLines - SumTail;
				RowCount = 0;
			}
			else
			{
				if (RowAmount == 0)
				{
					RowFrom = SumHead;
					RowTo = SumLines - SumTail;
				}
				else
				{
					RowFrom = PageIndex * RowAmount + SumHead;
					RowTo = Math.Min(RowFrom + RowAmount, SumLines - SumTail);
				}
				RowCount = TotalRecords;
			}
			return RetVal;
		}
		//-----------------------------------
		public static byte SearchTypes_GetId(string SearchCriterion)
		{
			byte RetVal = 0;
			if (!string.IsNullOrEmpty(SearchCriterion))
			{
				if (SearchCriterion.Length > 10)
				{
					RetVal = 1;
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static bool Static_IsImage(string Extension)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Extension))
			{
				Extension = Extension.ToLower();
				switch (Extension)
				{
					case ".png":
						{
							RetVal = true;
							break;
						}
					case ".gif":
						{
							RetVal = true;
							break;
						}
					case ".jpeg":
						{
							RetVal = true;
							break;
						}
					case ".jpg":
						{
							RetVal = true;
							break;
						}
					case ".bmp":
						{
							RetVal = true;
							break;
						}
				}
			}
			return RetVal;
		}
		//---------------------------------------------
		public static bool Static_IsValidAttachedFileExtension(string Extension)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Extension))
			{
				Extension = Extension.ToLower();
				switch (Extension)
				{
					case ".rar":
						{
							RetVal = true;
							break;
						}
					case ".zip":
						{
							RetVal = true;
							break;
						}
					case ".doc":
						{
							RetVal = true;
							break;
						}
					case ".docx":
						{
							RetVal = true;
							break;
						}
					case ".pdf":
						{
							RetVal = true;
							break;
						}
				}
			}
			return RetVal;
		}
		//---------------------------------------------
		public static bool Static_IsValidForAttached(string FileName)
		{
			bool RetVal = false;
			FileName = FileName.ToLower();
			if (!string.IsNullOrEmpty(FileName))
			{
				if (FileName.EndsWith(".rar"))
				{
					RetVal = true;
				}
				else
				{
					if (FileName.EndsWith(".zip"))
					{
						RetVal = true;
					}
					else
					{
						if (FileName.EndsWith(".doc"))
						{
							RetVal = true;
						}
						else
						{
							if (FileName.EndsWith(".docx"))
							{
								RetVal = true;
							}
							else
							{
								if (FileName.EndsWith(".pdf"))
								{
									RetVal = true;
								}
							}
						}
					}
				}
			}
			return RetVal;
		}
		//---------------------------------------------
		public static void DeleteFile(string LogFilePath, string LogFileName, string FileName)
		{
			try
			{
				if (File.Exists(FileName))
				{
					File.Delete(FileName);
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
			}
		}
		//----------------------------------------------------------------------------
		public static void DeleteFile(string LogFilePath, string LogFileName, string SourcePath, string FileName)
		{

			try
			{
				FileName = (SourcePath.EndsWith("\\")) ? SourcePath + FileName : SourcePath + "\\" + FileName;
				DeleteFile(LogFilePath, LogFileName, FileName);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
			}
		}
		//----------------------------------------------------------------------------
		public static void MoveFile(string LogFilePath, string LogFileName, string SourcePath, string FileName, string DestPath)
		{

			try
			{
				string OriginalFile = (SourcePath.EndsWith("\\")) ? SourcePath + FileName : SourcePath + "\\" + FileName;
				if (File.Exists(OriginalFile))
				{
					string DestinationFile = (DestPath.EndsWith("\\")) ? DestPath + FileName : DestPath + "\\" + FileName;
					File.Move(OriginalFile, DestinationFile);
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog("SourcePath: " + SourcePath + " FileName: " + FileName + "DestPath: " + DestPath + " >>" + ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
			}
		}
		//----------------------------------------------------------------------------
		public static bool FileExists(string LogFilePath, string LogFileName, string FileName, FileInfo[] l_FileInfo)
		{
			bool retVal = false;
			try
			{
				if (l_FileInfo.Length > 0)
				{
					for (int i = 0; i < l_FileInfo.Length; i++)
					{
						if (l_FileInfo[i].Name == FileName)
						{
							retVal = true;
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return retVal;
		}
		//---------------------------------------------------
		public static bool MakeDir(string LogFilePath, string LogFileName, string DirectorName)
		{
			bool RetVal=Directory.Exists(DirectorName);
			try
			{
				if (!RetVal)
				{
					Directory.CreateDirectory(DirectorName);
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//-------------------------------------------------------
		public static void MakeDir(string LogFilePath, string LogFileName, string RootPath, DateTime time)
		{
			try
			{
				if (time != null)
				{
					if (Directory.Exists(RootPath))
					{
						RootPath = (RootPath.EndsWith("\\")) ? RootPath + time.Year.ToString() : RootPath + "\\" + time.Year.ToString();
						MakeDir(LogFilePath, LogFileName, RootPath);
						if (Directory.Exists(RootPath))
						{
							RootPath = RootPath + ((time.Month < 10) ? "0" + time.Month.ToString() : time.Month.ToString());
							MakeDir(LogFilePath, LogFileName, RootPath);
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
			}
		}
		//-------------------------------------------------------
		public static void MakeYYYY_MM(string LogFilePath, string LogFileName, ref string RootPath, DateTime time)
		{
			try
			{
				if (time != null)
				{
					if (Directory.Exists(RootPath))
					{
						RootPath = (RootPath.EndsWith("\\")) ? RootPath + time.Year.ToString() : RootPath + "\\" + time.Year.ToString();
						MakeDir(LogFilePath, LogFileName, RootPath);
						if (Directory.Exists(RootPath))
						{
							RootPath = RootPath + "\\" + ((time.Month < 10) ? "0" + time.Month.ToString() : time.Month.ToString());
							MakeDir(LogFilePath, LogFileName, RootPath);
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".FileUtils." + MethodBase.GetCurrentMethod().Name);
			}
		}
		//----------------------------------------------------------------------------------------
		public static string GetFileName(string FileName)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(FileName))
			{
				RetVal = (FileName.IndexOf("\\") >= 0) ? GetFileNameExtention(FileName) : FileName;
				int Pos = FileName.LastIndexOf('.');
				if (Pos > 0)
				{
					RetVal = FileName.Substring(0, Pos).Trim();
				}
			}
			return RetVal;
		}
		//----------------------------------------------------------------------------------------
		public static string GetFileNameExtention(string FileName)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(FileName))
			{
				string[] Tmp = FileName.Split('\\');
				if (Tmp != null)
				{
					if (Tmp.Length > 0)
					{
						RetVal = Tmp[Tmp.Length - 1];
					}
				}
			}
			return RetVal;
		}
		//----------------------------------------------------------------------------------------
		public static string GetFileExtention(string FileName)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(FileName))
			{
				string[] Tmp = FileName.Split('.');
				if (Tmp != null)
				{
					if (Tmp.Length > 1)
					{
						RetVal = Tmp[Tmp.Length - 1].Trim();
					}
				}
			}
			return RetVal;
		}
		//----------------------------------------------------------------------------------------
		public static bool CanUpload(string FileName)
		{
			bool RetVal = false;
			string Extention=GetFileExtention(FileName).ToLower();
			if ((string.IsNullOrEmpty(Extention))
				|| (Extention=="rar")
				|| (Extention == "zip")
				|| (Extention == "xls")
				|| (Extention == "xlsx")
				|| (Extention == "doc")
				|| (Extention == "docx")
				)
			{
				RetVal = true;
			}
			return RetVal;
		}
	}
}
