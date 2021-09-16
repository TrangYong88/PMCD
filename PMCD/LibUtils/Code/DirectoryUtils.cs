using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;
namespace Lib.Utils
{
	public class DirectoryUtils
	{
		public static string[] ListDirectories(string LogFilePath, string LogFileName, string path, string SearchPattern)
		{
			string[] RetVal = new string[0];
			try
			{
				RetVal = (string.IsNullOrEmpty(SearchPattern)) ? Directory.GetDirectories(path, "*", SearchOption.AllDirectories) : Directory.GetDirectories(path, SearchPattern, SearchOption.AllDirectories);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".DirectoryUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//----------------------------------------------------------------------------
		public static string[] ListFiles(string LogFilePath, string LogFileName, string path, string SearchPattern)
		{
			string[] RetVal = new string[0];
			try
			{
				if (Directory.Exists(path))
				{
					RetVal = Directory.GetFiles(path, SearchPattern);
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".DirectoryUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//----------------------------------------------------------------------------
		public static string[] ListFiles(string LogFilePath, string LogFileName, string path)
		{//Return Full FileName and Path
			string[] RetVal = new string[0];
			try
			{
				if (Directory.Exists(path))
				{
					RetVal = Directory.GetFiles(path);
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".DirectoryUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//----------------------------------------------------------------------------
		public static bool CompareDirectories(string LogFilePath, string LogFileName, string RootPath1, string RootPath2, string SearchPattern, out string LogContent)
		{
			bool RetVal = true;
			LogContent = "";
			try
			{
				foreach (string Path1 in ListDirectories(LogFilePath, LogFileName, RootPath1, SearchPattern))
				{
					if (string.IsNullOrEmpty(Path1))
					{
						LogContent += "Path1 is empty" + Path1 + Environment.NewLine;
					}
					else
					{
						string Path2 = RootPath2 + StringUtils.SubString(LogFilePath, LogFileName, Path1, RootPath1.Length, 0);
						if (Directory.Exists(Path2))
						{
							string[] ListFiles1 = ListFiles(LogFilePath, LogFileName, Path1);
							if (ListFiles1.Length > 0)
							{
								bool InDirFlag = true;
								string InnneLogContent = "";
								foreach (string FullFileName1 in ListFiles1)
								{
									string FullFileName2 = RootPath2 + StringUtils.SubString(LogFilePath, LogFileName, FullFileName1, RootPath1.Length, 0);
									if (File.Exists(FullFileName2))
									{
										if (!FileUtils.Compare(LogFilePath, LogFileName, FullFileName1, FullFileName2))
										{
											RetVal = false;
											InDirFlag = false;
											InnneLogContent += FullFileName1 + " <> " + FullFileName2 + Environment.NewLine;
										}
									}
									else
									{
										RetVal = false;
										LogContent += FullFileName2 + " not found in compare with " + FullFileName1 + Environment.NewLine;
									}
								}
								if (InDirFlag)
								{
									LogContent += ListFiles1.Length.ToString() + " file found in " + Path1 + " OK" + Environment.NewLine;
								}
								else
								{
									LogContent += ListFiles1.Length.ToString() + " file found in " + Path1 + " differ:" + Environment.NewLine + InnneLogContent;

								}
							}
							//else
							//{
							//    LogContent += "No file found in " + Path1 + Environment.NewLine;
							//}
						}
						else
						{
							RetVal = false;
							LogContent += Path2 + " not found, Path1=" + Path1 + Environment.NewLine;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".DirectoryUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//----------------------------------------------------------------------------
		public static bool CreateDirectory(string LogFilePath, string LogFileName, string DirectoryName)
		{
			bool RetVal = false;
			try
			{
				if (!string.IsNullOrEmpty(DirectoryName))
				{
					RetVal = Directory.Exists(DirectoryName);
					if (!RetVal)
					{
						Directory.CreateDirectory(DirectoryName);
						RetVal = Directory.Exists(DirectoryName);
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".DirectoryUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//----------------------------------------------------------------------
		public static int MoveToBak(string LogFilePath, string LogFileName, string FilePath, string BkFilePath, string SearchPattern)
		{
			int RetVal = 0;
			try
			{
				DirectoryInfo dir = new DirectoryInfo(FilePath);
				if (dir.Exists)
				{
					FileInfo[] l_FileInfo = (string.IsNullOrEmpty(SearchPattern)) ? dir.GetFiles() : dir.GetFiles(SearchPattern);
					if (l_FileInfo.Length > 0)
					{
						if (DirectoryUtils.CreateDirectory(LogFilePath, LogFileName, BkFilePath))
						{
							if (!BkFilePath.EndsWith("\\"))
							{
								BkFilePath += "\\";
							}
							foreach (FileInfo mFileInfo in l_FileInfo)
							{
								string BkFullName = BkFilePath + mFileInfo.Name;
								if (File.Exists(BkFullName))
								{
									string BkFullName2 = BkFilePath + FileUtils.GenerateBackupFileName(mFileInfo.Name);
									File.Move(BkFullName, BkFullName2);
								}
								File.Move(mFileInfo.FullName, BkFullName);
								RetVal++;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".DirectoryUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
	}
}
