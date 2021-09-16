using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
namespace Lib.Utils
{
	public class LogFiles
	{
		private static object o = new object();
		private static Mutex mut = new Mutex();
		private static StreamWriter m_StreamWriter;
		private static string PathLogFileMO;

		private static void InitLogFile(string DirLogPath)
		{
			if (!Directory.Exists(DirLogPath))
			{
				Directory.CreateDirectory(DirLogPath);
			}
			string FileNameMO = "MO_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log";
			PathLogFileMO = DirLogPath + FileNameMO;
			if (!File.Exists(PathLogFileMO))
			{
				m_StreamWriter = File.CreateText(PathLogFileMO);
			}
			else
			{
				m_StreamWriter = File.AppendText(PathLogFileMO);
			}
		}
		//-----------------------------------------------------
		private static void InitLogFile(string DirLogPath, string fileName)
		{
			if (!Directory.Exists(DirLogPath))
			{
				Directory.CreateDirectory(DirLogPath);
			}
			string FileNameMO = fileName + "_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log";
			PathLogFileMO = DirLogPath + FileNameMO;
			lock (m_StreamWriter)
			{
				if (!File.Exists(PathLogFileMO))
				{
					m_StreamWriter = File.CreateText(PathLogFileMO);
				}
				else
				{
					m_StreamWriter = File.AppendText(PathLogFileMO);
				}
			}
		}

		public static void WriteLogOld(string textlog, string DirLogPath)
		{
			try
			{
				if (!DirLogPath.EndsWith("\\")) DirLogPath = DirLogPath + "\\";

				InitLogFile(DirLogPath);

				string nowPathLogFileMO = DirLogPath + "MO_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log";

				if (PathLogFileMO.ToUpper() != nowPathLogFileMO.ToUpper())
				{
					lock (m_StreamWriter)
					{
						PathLogFileMO = nowPathLogFileMO;
						m_StreamWriter.Close();
						m_StreamWriter = File.CreateText(nowPathLogFileMO);
						m_StreamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + textlog);
						m_StreamWriter.Flush();
					}
				}
				else
				{
					lock (m_StreamWriter)
					{
						m_StreamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + textlog);
						m_StreamWriter.Flush();
					}
				}
				m_StreamWriter.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void WriteLogOld(string textlog, string DirLogPath, string fileName)
		{
			try
			{
				if (!DirLogPath.EndsWith("\\")) DirLogPath = DirLogPath + "\\";

				InitLogFile(DirLogPath, fileName);

				if (!Directory.Exists(DirLogPath))
				{
					Directory.CreateDirectory(DirLogPath);
				}
				string FileNameMO = fileName + "_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log";

				PathLogFileMO = DirLogPath + FileNameMO;

				string nowPathLogFileMO = DirLogPath + fileName + "_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log";
				if (PathLogFileMO.ToUpper() != nowPathLogFileMO.ToUpper())
				{
					lock (m_StreamWriter)
					{
						PathLogFileMO = nowPathLogFileMO;
						m_StreamWriter.Close();
						m_StreamWriter = File.CreateText(nowPathLogFileMO);
						m_StreamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + textlog);
						m_StreamWriter.Flush();
					}
				}
				else
				{
					lock (m_StreamWriter)
					{
						m_StreamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + textlog);
						m_StreamWriter.Flush();
					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//---------------------------------------------------------

		public static void WriteLog(string textlog, string DirLogPath)
		{
			try
			{
				if (!DirLogPath.EndsWith("\\")) DirLogPath = DirLogPath + "\\";

				if (!Directory.Exists(DirLogPath))
				{
					Directory.CreateDirectory(DirLogPath);
				}
				string nowPathLogFileMO = DirLogPath + "MO_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log";
				if (!File.Exists(PathLogFileMO))
				{
					//m_StreamWriter = File.CreateText(PathLogFileMO);
					FileStream fs = File.Create(PathLogFileMO);
					fs.Close();
				}
				mut.WaitOne();
				lock (o)
				{
					m_StreamWriter = File.AppendText(PathLogFileMO);
					m_StreamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + textlog);
					m_StreamWriter.Flush();
					m_StreamWriter.Close();
				}
				mut.ReleaseMutex();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//----------------------------------------------------------------------------
		public static void WriteOrAppend(string LineHeader, string LineContent, string Path, string FileName)
		{
			try
			{
				if (!string.IsNullOrEmpty(FileName))
				{
					if (!Path.EndsWith("\\")) Path = Path + "\\";
					string PathFileName = Path + FileName;
					if (!string.IsNullOrEmpty(LineHeader))
					{
						LineContent = LineHeader + "-" + LineContent;
					}
					if (!Directory.Exists(Path))
					{
						Directory.CreateDirectory(Path);
					}
					if (!File.Exists(PathFileName))
					{
						FileStream fs = File.Create(PathFileName);
						fs.Close();
					}
					mut.WaitOne();
					lock (o)
					{
						m_StreamWriter = File.AppendText(PathFileName);
						m_StreamWriter.WriteLine(LineContent);
						m_StreamWriter.Flush();
						m_StreamWriter.Close();
					}
					mut.ReleaseMutex();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//----------------------------------------------------------------------------
		public static void WriteOrAppend(string LineContent, string Path, string FileName)
		{
			string LineHeader = "";
			WriteOrAppend(LineHeader, LineContent, Path, FileName);
		}
		//----------------------------------------------------------------------------
		public static void WriteLog(string LineContent, string Path, string FileName)
		{
			try
			{
				DateTime dateTime = DateTime.Now;
				string LineHeader = DateTimeUtils.Static_yyyymmddhhmissms(dateTime);
				FileName = FileName + DateTimeUtils.Static_YYYYMMDDHH(dateTime) + ".log";
				WriteOrAppend(LineHeader, LineContent, Path, FileName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//----------------------------------------------------------------------------
		//public static void WriteLogSecond(string LogFilePath, string FileName, string LineContent)
		//{

		//  try
		//  {
		//    if (FileUtils.MakeDir(LogFilePath, LogFileName, DirectorName))
		//    {
		//      DateTime dateTime = DateTime.Now;
		//      string LineHeader = DateTimeUtils.Static_yyyymmddhhmissms(dateTime);
		//      FileName = FileName + DateTimeUtils.Static_YYYYMMDDHH(dateTime) + ".log";
		//      WriteOrAppend(LineHeader, LineContent, Path, FileName);
		//    }
		//  }
		//  catch 
		//  {
				
		//  }
		//}
	}
}
