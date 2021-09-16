using System;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
namespace Lib.Utils
{
	public class DateTimeUtils
	{
		public DateTimeUtils()
		{
		}
		//---------------------------------------------------------------------------
		public static bool Equals(DateTime mDateTime, string yyyy_mm_dd_hh_mi_ss_ms)
		{
			bool RetVal = false;
			try
			{
				if (!string.IsNullOrEmpty(yyyy_mm_dd_hh_mi_ss_ms))
				{
					if (mDateTime > BeginDate())
					{
						string TmpStr=Static_yyyymmddhhmissms(mDateTime, "-", ":");
						if (TmpStr == yyyy_mm_dd_hh_mi_ss_ms)
						{
							RetVal = true;
						}
						else
						{
							DateTime Tmp;
							if (DateTime.TryParse(yyyy_mm_dd_hh_mi_ss_ms,out Tmp))
							{
								string TmpStr1 = Static_yyyymmddhhmissms(Tmp, "-", ":");
								RetVal = (mDateTime == Tmp);
							}
						}
					}
				}
			}
			catch
			{
			}
			return RetVal;
		}
		public static List<DateTime> Generate(DateTime DateTimeFrom, DateTime DateTimeTo)
		{
			List<DateTime> RetVal = new List<DateTime>();
			while (DateTimeFrom <= DateTimeTo)
			{
				RetVal.Add(DateTimeFrom);
				DateTimeFrom = DateTimeFrom.AddDays(1);
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------
		public static string HHmi(short Minute,char TimeDelimeter)
		{
			string Delimeter = TimeDelimeter.ToString();
			if (string.IsNullOrEmpty(Delimeter))
			{
				Delimeter = ":";
			}
			return (Minute / 60).ToString().PadLeft(2, '0') + Delimeter + (Minute % 60).ToString().PadLeft(2, '0');
		}
		//---------------------------------------------------------------------------
		public static string HHmi(short Minute)
		{
			return HHmi(Minute, ':');
		}
		//----------------------------------------------------
		public static string HHmi(DateTime date)
		{
			string RetVal = "";
				if (date != null)
				{
					if (date.Year > 1900)
					{
						RetVal = date.ToString("HH:MI");
					}
				}
			return RetVal;
		}
		//---------------------------------------------------------------------------
		public static bool HHmiToMinute(string HourMinute,char Delimeter, out short HHMI)
		{
			bool RetVal = false;
			HHMI = 0;
			try
			{
				string[] HourMinuteArr = HourMinute.Split(Delimeter);
				if (HourMinuteArr.Length == 2)
				{
					byte Hour = 0;
					byte.TryParse(HourMinuteArr[0], out Hour);
					if ((Hour < 24) && (Hour >= 0))
					{
						byte Minute = 0;
						byte.TryParse(HourMinuteArr[1], out Minute);
						if ((Hour < 24) && (Hour >= 0))
						{
							RetVal = true;
							HHMI =(short)(Hour * 60 + Minute);
						}
					}
				}
			}
			catch
			{
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------
		public static bool HHmiToMinute(string HourMinute, out short HHMI)
		{
			return HHmiToMinute(HourMinute, ':', out HHMI);
		}
		//---------------------------------------------------------------------------
		public static uint SwapEndianness(ulong x)
		{
			return (uint)(((x & 0x000000ff) << 24) +
										 ((x & 0x0000ff00) << 8) +
										 ((x & 0x00ff0000) >> 8) +
										 ((x & 0xff000000) >> 24));
		}
		//---------------------------------------------------------------------------
		public static DateTime DateTimeParser(string LogFilePath, string LogFileName, byte[] ntpData, byte Position)
		{
			DateTime RetVal = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);//**UTC** time
			try
			{				
				ulong intPart = BitConverter.ToUInt32(ntpData, Position);//Get the seconds part
				ulong fractPart = BitConverter.ToUInt32(ntpData, Position + 4);//Get the seconds fraction
				//Convert From big-endian to little-endian
				intPart = SwapEndianness(intPart);
				fractPart = SwapEndianness(fractPart);
				ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
				RetVal = RetVal.AddMilliseconds((ulong)milliseconds);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + "DateTimeUtilss" + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------
		public static bool SetDateTime(string LogFilePath, string LogFileName, DateTime DateTimeValue, ref byte[] ntpData, byte Position)
		{
			bool RetVal = false;
			try
			{
				DateTime DateTimeValueUtc = DateTimeValue.ToUniversalTime();
				long Tick = DateTimeValueUtc.Ticks;
				uint intPart = (uint)(Tick / 1000000);
				uint fractPart=(uint)(Tick%1000000);
				byte[] Value = BitConverter.GetBytes(intPart);
				uint intPart1 = BitConverter.ToUInt32(Value, 0);
				DateTime UtcTimeOrig = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);//**UTC** time
				//A single tick represents one hundred nanoseconds or one ten-millionth of a second. There are 10,000 ticks in a millisecond, or 10 million ticks in a second.
				//UtcTimeOrig.Ticks
				// intPart = BitConverter.ToUInt32(ntpData, Position);//Get the seconds part
				// fractPart = BitConverter.ToUInt32(ntpData, Position + 4);//Get the seconds fraction
				////Convert From big-endian to little-endian
				//intPart = SwapEndianness(intPart);
				//fractPart = SwapEndianness(fractPart);
				//ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
				RetVal = true;// RetVal.AddMilliseconds((long)milliseconds);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + "DateTimeUtilss" + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//--------------------------------------------------------------------
		public static DateTime BeginDate()
		{
			return Convert.ToDateTime("1900-01-01 00:00:00.000");
		}
		//--------------------------------------------------------------------
		public static bool IsDateValid(DateTime mDateTime)
		{
			return (mDateTime > BeginDate());
		}
		//--------------------------------------------------------------------
		public static short BeginYear()
		{
			return (short)BeginDate().Year;
		}
		//--------------------------------------------------------------------
		public static DateTime BeginYearDate(int Year)
		{
			return Convert.ToDateTime(Year.ToString() + "-01-01 00:00:00.000");
		}
		//--------------------------------------------------------------------
		public static DateTime BeginYearDate(DateTime CurrentDate)
		{
			return BeginYearDate(CurrentDate.Year);
		}
		//--------------------------------------------------------------------
		public static DateTime LastYearDate(DateTime CurrentDate)
		{
			return Convert.ToDateTime(CurrentDate.Year.ToString() + "-12-31 00:00:00.000");
		}
		//--------------------------------------------------------------------
		public static DateTime BeginYearDateTime(DateTime CurrentDate)
		{
			return CurrentDate.AddDays(1 - CurrentDate.DayOfYear);
		}
		//--------------------------------------------------------------------
		public static DateTime BeginWeekDateTime(DateTime CurrentDate)
		{
			//Sunday is first day of week
			return CurrentDate.AddDays(-(double)CurrentDate.DayOfWeek);
		}
		//--------------------------------------------------------------------
		public static DateTime BeginWeekDateTimeMondayFirst(DateTime CurrentDate)
		{
			//Monday is first day of week
			return CurrentDate.AddDays(1-(double)CurrentDate.DayOfWeek);
		}
		//--------------------------------------------------------------------
		public static DateTime BeginWeekDateMondayFirst(int Year, byte WeekNum)
		{
			DateTime BeginDateOfYear=BeginYearDate(Year);
			return BeginWeekDateTimeMondayFirst(BeginDateOfYear.AddDays((WeekNum-1) * 7));
		}
		//--------------------------------------------------------------------
		public static byte WeekOfYear(DateTime CurrentDate)
		{
			CultureInfo cul = CultureInfo.CurrentCulture;
			return (byte)cul.Calendar.GetWeekOfYear(CurrentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		}
		//--------------------------------------------------------------------
		public static string Static_MMYYYY(DateTime dateTime, string Delimiter)
		{
			string RetVal = "";
			if (dateTime != null)
			{
				if (dateTime != DateTime.MinValue)
				{
					if (dateTime.Month <= 9) RetVal = "0";
					RetVal = RetVal + dateTime.Month.ToString() + Delimiter;
					RetVal = RetVal + dateTime.Year.ToString();
				}
			}
			return RetVal;
		}
		//--------------------------------------------------------------------
		public static string Static_DDMMYYYY(DateTime dateTime)
		{
			return Static_DDMMYYYY(dateTime, "/");
		}
		//--------------------------------------------------------------------
		public static string Static_DDMMYYYY(DateTime dateTime, string Delimiter)
		{
			string RetVal = "";
			if (dateTime>BeginDate())
			{
					RetVal = dateTime.Day.ToString().PadLeft(2,'0') + Delimiter;
					RetVal += Static_MMYYYY(dateTime, Delimiter);
			}
			return RetVal;
		}
		//--------------------------------------------------------------------
		public static string Static_DDMMYYYYHH(DateTime dateTime, string Delimiter)
		{
			string RetVal = Static_DDMMYYYY(dateTime, Delimiter);
			try
			{
				if (!string.IsNullOrEmpty(RetVal))
				{
					RetVal = RetVal + " ";
					if (dateTime.Hour <= 9)
					{
						RetVal = RetVal + "0";
					}
					RetVal = RetVal + dateTime.Hour.ToString();
				}
			}
			catch
			{

			}
			return RetVal;
		}
		//--------------------------------------------------------------------
		public static string Static_DateTimeToString(byte ReportTypeId, DateTime dateTime, string Delimiter)
		{
			string RetVal = "";
			if (ReportTypeId == 3)
			{
				RetVal = Static_MMYYYY(dateTime, Delimiter);
			}
			else
			{
				if (ReportTypeId == 2)
				{
					RetVal = Static_DDMMYYYY(dateTime, Delimiter);
				}
				else
				{
					if (ReportTypeId == 1)
					{
						RetVal = Static_DDMMYYYYHH(dateTime, Delimiter);
					}
					else
					{
						RetVal = Static_ddmmyyyyhhmissms(dateTime);
					}
				}
			}
			return RetVal;
		}
		//--------------------------------------------------------------------
		public static string Static_DateTimeToString(byte ReportTypeId, DateTime dateTime)
		{
			string Delimiter = "/";
			return Static_DateTimeToString(ReportTypeId, dateTime, Delimiter);
		}
		public static string Static_HHhMI(short HHMI)
		{
			string RetVal = "";
			RetVal = (HHMI / 60).ToString() + "h" + (HHMI % 60).ToString();
			return RetVal;
		}
		public static string Static_HHhMI(DateTime dateTime)
		{
			string RetVal = "";
			try
			{
				if (dateTime != null)
				{
					if (dateTime != DateTime.MinValue)
					{
						if (dateTime.Hour <= 9) RetVal = "0";
						RetVal = RetVal + dateTime.Hour.ToString() + "h";
						if (dateTime.Minute <= 9) RetVal = RetVal + "0";
						RetVal = RetVal + dateTime.Minute.ToString();
					}
				}
			}
			catch
			{

			}
			return RetVal;
		}
		//------------------------------------------------------
		public static string DayOfWeek_Vn(string DayOfWeek_En)
		{
			string RetVal = "";
			switch (DayOfWeek_En)
			{
				case "Sunday":
					RetVal = "Chủ Nhật";
					break;
				case "Monday":
					RetVal = "Thứ Hai";
					break;
				case "Tuesday":
					RetVal = "Thứ Ba";
					break;
				case "Wednesday":
					RetVal = "Thứ Tư";
					break;
				case "Thursday":
					RetVal = "Thứ Năm";
					break;
				case "Friday":
					RetVal = "Thứ Sáu";
					break;
				case "Saturday":
					RetVal = "Thứ Bảy";
					break;
				default:
					break;
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static byte DayOfWeekToNumber(DayOfWeek mDayOfWeek)
		{
			byte RetVal = 0;
			switch (mDayOfWeek.ToString())
			{
				case "Sunday":
					RetVal = 0;
					break;
				case "Monday":
					RetVal = 2;
					break;
				case "Tuesday":
					RetVal = 3;
					break;
				case "Wednesday":
					RetVal = 4;
					break;
				case "Thursday":
					RetVal = 5;
					break;
				case "Friday":
					RetVal = 6;
					break;
				case "Saturday":
					RetVal = 7;
					break;
				default:
					break;
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static string getLongDate(string dd_MM_yyyy)
		{
			try
			{
				DateTime dateTime = DateTime.ParseExact(dd_MM_yyyy, "dd-MM-yyyy", null);
				string day = DayOfWeek_Vn(Convert.ToString(dateTime.DayOfWeek));
				day = day + ", " + dd_MM_yyyy;
				return day;
			}
			catch
			{
				return "";
			}
		}
		//-------------------------------------------------------------------------------------------------------------------
		public static string GetDayOfWeek()
		{
			string RetVal = "";
			try
			{
				RetVal = DateTime.Now.DayOfWeek.ToString().ToUpper();
				RetVal = RetVal.Substring(0, 3);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------------------------------------
		public string GetBeforeDayOfWeek()
		{
			string RetVal = "";
			try
			{
				RetVal = DateTime.Now.AddDays((double)-1).DayOfWeek.ToString().ToUpper();
				RetVal = RetVal.Substring(0, 3);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string ToSqlServerDateTime(DateTime mDateTime)
		{
			string RetVal = "NULL";
			if (mDateTime!=null)
			{
				if (mDateTime>BeginDate())
				{
					RetVal = "CONVERT(DATETIME,N'" + Static_yyyymmddhhmissms(mDateTime, "-", ":") + "',121)";
				}
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string ddmmyyyyhhmiss(DateTime dateTime)
		{
			string day = "";
			try
			{
				if (dateTime == DateTime.MinValue)
				{
					day = "";
				}
				else
				{
					day = dateTime.ToString("dd/MM/yyyy") + " ";
					if (dateTime.Hour <= 9) day = day + "0" + dateTime.Hour.ToString() + ":";
					else day = day + dateTime.Hour.ToString() + ":";
					if (dateTime.Minute <= 9) day = day + "0" + dateTime.Minute.ToString() + ":";
					else day = day + dateTime.Minute.ToString() + ":";
					if (dateTime.Second <= 9) day = day + "0" + dateTime.Second.ToString();
					else day = day + dateTime.Second.ToString();
				}
			}
			catch
			{

			}
			return day;
		}
		//---------------------------------------------------------------------
		public string hhmi_weekday_ddmmyyyy(DateTime dateTime)
		{
			try
			{
				string day = dateTime.Hour.ToString() + "h";
				if (dateTime.Hour <= 9) day = "0" + day;
				if (dateTime.Minute <= 9) day = day + "0" + dateTime.Minute.ToString() + " ";
				else day = day + dateTime.Minute.ToString() + " ";
				switch (Convert.ToString(dateTime.DayOfWeek))
				{
					case "Sunday":
						day = day + "Chủ Nhật";
						break;
					case "Monday":
						day = day + "Thứ Hai";
						break;
					case "Tuesday":
						day = day + "Thứ Ba";
						break;
					case "Wednesday":
						day = day + "Thứ Tư";
						break;
					case "Thursday":
						day = day + "Thứ Năm";
						break;
					case "Friday":
						day = "Thứ Sáu";
						break;
					case "Saturday":
						day = day + "Thứ Bảy";
						break;
					default:
						break;
				}
				day = day + " (" + dateTime.ToString("dd/MM/yyyy") + ")";
				return day;
			}
			catch
			{
				return "";
			}
		}
		//---------------------------------------------------------------------
		public string ddmmyyyyhhmissms(DateTime dateTime)
		{
			string day;
			try
			{
				day = dateTime.ToString("dd/MM/yyyy") + " ";
				if (dateTime.Hour <= 9) day = day + "0" + dateTime.Hour.ToString() + ":";
				else day = day + dateTime.Hour.ToString() + ":";
				if (dateTime.Minute <= 9) day = day + "0" + dateTime.Minute.ToString() + ":";
				else day = day + dateTime.Minute.ToString() + ":";
				if (dateTime.Second <= 9) day = day + "0" + dateTime.Second.ToString() + ".";
				else day = day + dateTime.Second.ToString() + ".";
				if (dateTime.Millisecond <= 9)
				{
					day = day + "00" + dateTime.Millisecond.ToString();
				}
				else
				{
					if (dateTime.Millisecond <= 99)
					{
						day = day + "0" + dateTime.Millisecond.ToString();
					}
					else
					{
						day = day + dateTime.Millisecond.ToString();
					}
				}
			}
			catch
			{
				day = "";
			}
			return day;
		}
		//---------------------------------------------------------------------
		public static string Static_ddmmyyyyhhmissms(DateTime mDateTime)
		{
			string RetVal = "";
			if (mDateTime > BeginDate())
			{
				RetVal = mDateTime.ToString("dd/MM/yyyy") + " ";
				RetVal += mDateTime.Hour.ToString().PadLeft(2, '0') + ":";
				RetVal += mDateTime.Minute.ToString().PadLeft(2, '0') + ":";
				RetVal += mDateTime.Second.ToString().PadLeft(2, '0') + ".";
				RetVal += mDateTime.Millisecond.ToString().PadLeft(3, '0');
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Static_ddmmyyyyhh(DateTime dateTime, string Desc)
		{
			string day = Desc;
			try
			{
				if (dateTime.Year > 2000)
				{
					day = dateTime.ToString("dd/MM/yyyy") + " ";
					if (dateTime.Hour <= 9)
					{
						day = day + "0";
					}
					day = day + dateTime.Hour.ToString();
				}
			}
			catch
			{

			}
			return day;
		}
		//---------------------------------------------------------------------
		public static string Static_ddmmyyyyhh(DateTime dateTime)
		{
			string Desc = "Tất cả";
			return Static_ddmmyyyyhh(dateTime, Desc);
		}
		//---------------------------------------------------------------------
		public static string Static_ddmmyyyyhhmi(DateTime dateTime, string Desc)
		{
			string day = Desc;
			try
			{
				if (dateTime.Year > 2000)
				{
					day = dateTime.ToString("dd/MM/yyyy") + " ";
					if (dateTime.Hour <= 9)
					{
						day = day + "0";
					}
					day = day + dateTime.Hour.ToString() + ":";
					if (dateTime.Minute <= 9)
					{
						day = day + "0";
					}
					day = day + dateTime.Minute.ToString();
				}
			}
			catch
			{

			}
			return day;
		}
		//---------------------------------------------------------------------
		public static string Static_ddmmyyyyhhmi(DateTime dateTime)
		{
			string Desc = "";
			return Static_ddmmyyyyhhmi(dateTime, Desc);
		}
		//---------------------------------------------------------------------
		public static string Static_ddmmyyyyhhmiss(DateTime dateTime, string Desc)
		{
			string day = Desc;
			try
			{
				if (dateTime.Year > 2000)
				{
					day = dateTime.ToString("dd/MM/yyyy") + " ";
					if (dateTime.Hour <= 9) day = day + "0" + dateTime.Hour.ToString() + ":";
					else day = day + dateTime.Hour.ToString() + ":";
					if (dateTime.Minute <= 9) day = day + "0" + dateTime.Minute.ToString() + ":";
					else day = day + dateTime.Minute.ToString() + ":";
					if (dateTime.Second <= 9) day = day + "0" + dateTime.Second.ToString();
					else day = day + dateTime.Second.ToString();
				}
			}
			catch
			{
				day = "";
			}
			return day;
		}
		//---------------------------------------------------------------------
		public static string Static_ddmmyyyyhhmiss(DateTime dateTime)
		{
			string Desc = "";
			return Static_ddmmyyyyhhmiss(dateTime, Desc);
		}
		//---------------------------------------------------------------------
		public static string Static_mmyyyy(DateTime dateTime, string Desc)
		{
			string day = Desc;
			try
			{
				if (dateTime.Year > 2000)
				{
					day = dateTime.Month.ToString() + "/" + dateTime.Year.ToString();
					if (dateTime.Month <= 9) day = "0" + day;
				}
			}
			catch
			{
				day = "";
			}
			return day;
		}
		//---------------------------------------------------------------------
		public static string Static_yyyymm(DateTime dateTime, string Delimiter)
		{
			string RetVal = "";
			RetVal = dateTime.Year.ToString() + Delimiter;
			if (dateTime.Month <= 9) RetVal += "0";
			RetVal += dateTime.Month.ToString();
			return RetVal;
		}

		//---------------------------------------------------------------------
		public static string ToVariableString(DateTime dateTime, string Desc, byte Type)
		{
			string day = Desc;
			try
			{
				if (dateTime.Year > 2000)
				{
					switch (Type)
					{
						case 1: day = Static_ddmmyyyyhhmiss(dateTime, Desc); break;
						case 2: day = dateTime.ToString("dd/MM/yyyy"); break;
						case 3: day = Static_mmyyyy(dateTime, Desc); break;
						default: day = Static_ddmmyyyyhhmissms(dateTime); break;
					}
				}
			}
			catch
			{
				day = "";
			}
			return day;
		}
		//---------------------------------------------------------------------
		/*
		 * Hàm này trả về kiểu string của ngày tháng năm dạng ngắn, ví dụ: 14/04/2006
		 */
		public static string getShortDate(DateTime dateTime)
		{
			try
			{
				return dateTime.ToString("dd/MM/yyyy");
			}
			catch
			{
				return "";
			}
		}
		/*
		 * Hàm này trả về kiểu string của ngày tháng năm dạng đầy đủ, ví dụ: Thứ Sáu, 14/04/2006 14:15 PM
		 */
		public static string getLongDateTime(DateTime dateTime)
		{
			try
			{
				string day = "";
				switch (Convert.ToString(dateTime.DayOfWeek))
				{
					case "Sunday":
						day = "Chủ Nhật";
						break;
					case "Monday":
						day = "Thứ Hai";
						break;
					case "Tuesday":
						day = "Thứ Ba";
						break;
					case "Wednesday":
						day = "Thứ Tư";
						break;
					case "Thursday":
						day = "Thứ Năm";
						break;
					case "Friday":
						day = "Thứ Sáu";
						break;
					case "Saturday":
						day = "Thứ Bảy";
						break;
					default:
						break;
				}
				day = day + ", " + dateTime.ToString("dd/MM/yyyy - HH:mm"); // Khong co PM/AM
				return day;
			}
			catch
			{
				return "";
			}
		}

		/*
		 * Hàm này trả về kiểu string của ngày tháng năm dạng đầy đủ, ví dụ: Thứ Sáu, 14/04/2006
		 */
		public static string getLongDate(DateTime dateTime)
		{
			try
			{
				string day = "";
				switch (Convert.ToString(dateTime.DayOfWeek))
				{
					case "Sunday":
						day = "Chủ Nhật";
						break;
					case "Monday":
						day = "Thứ Hai";
						break;
					case "Tuesday":
						day = "Thứ Ba";
						break;
					case "Wednesday":
						day = "Thứ Tư";
						break;
					case "Thursday":
						day = "Thứ Năm";
						break;
					case "Friday":
						day = "Thứ Sáu";
						break;
					case "Saturday":
						day = "Thứ Bảy";
						break;
					default:
						break;
				}
				day = day + ", " + dateTime.ToString("dd/MM/yyyy"); // Khong co PM/AM
				return day;
			}
			catch
			{
				return "";
			}
		}
		/*
		 * Hàm này trả về kiểu string của giờ phút và ngày tháng năm dạng đầy đủ, ví dụ:  14:15' PM, Thứ Sáu, 14/04/2006
		 */
		public string getLongTimeDate(DateTime dateTime)
		{
			try
			{
				string day = "";
				switch (Convert.ToString(dateTime.DayOfWeek))
				{
					case "Sunday":
						day = "Chủ Nhật";
						break;
					case "Monday":
						day = "Thứ Hai";
						break;
					case "Tuesday":
						day = "Thứ Ba";
						break;
					case "Wednesday":
						day = "Thứ Tư";
						break;
					case "Thursday":
						day = "Thứ Năm";
						break;
					case "Friday":
						day = "Thứ Sáu";
						break;
					case "Saturday":
						day = "Thứ Bảy";
						break;
					default:
						break;
				}
				//day = dateTime.ToString("HH:mm") +"' "+ dateTime.ToString("tt")+ ", " + day + ", " + dateTime.ToString("dd/MM/yyyy"); //Co PM/AM
				day = dateTime.ToString("HH:mm") + "' " + ", " + day + ", " + dateTime.ToString("dd/MM/yyyy"); // Khong co PM/AM
				return day;
			}
			catch
			{
				return "";
			}
		}

		public static string addMunite(string startTime, int addmunite)
		{
			int hour = 12;
			int munite = 12;
			int second = 12;

			string[] element = startTime.Split(':');

			hour = Convert.ToInt32(element[0]);
			munite = Convert.ToInt32(element[1]);
			second = Convert.ToInt32(element[2]);

			DateTime date = new DateTime(2006, 9, 18, hour, munite, second);

			date = date.AddMinutes(addmunite);

			return addZero(date.Hour) + ":" + addZero(date.Minute) + ":" + addZero(date.Second);
		}

		private static string addZero(int param)
		{
			if (param >= 0 && param <= 9)
				return "0" + param;
			else return param.ToString();
		}
		/*
		 * Hàm này trả về ngày cuối cùng trong tháng
		 */
		public int getLastDate(int month, bool x)
		{
			try
			{
				int day = 0;
				switch (month)
				{
					case 1:
						day = 31;
						break;
					case 3:
						day = 31;
						break;
					case 5:
						day = 31;
						break;
					case 7:
						day = 31;
						break;
					case 8:
						day = 31;
						break;
					case 10:
						day = 31;
						break;
					case 12:
						day = 31;
						break;
					case 2:
						if (x)
						{
							day = 29;
						}
						else
						{
							day = 28;
						}
						break;
					case 4:
						day = 30;
						break;
					case 6:
						day = 30;
						break;
					case 9:
						day = 30;
						break;
					case 11:
						day = 30;
						break;
					default:
						break;
				}
				return day;
			}
			catch
			{
				return 0;
			}
		}
		//---------------------------------------------------------------------
		public static DateTime RoundToTenMs(DateTime mDateTime)
		{
			DateTime RetVal = mDateTime;
			if (mDateTime>BeginDate())
			{
				string tmp = mDateTime.ToString("yyyy-MM-dd HH:mm:ss.ff") + "0";
				try
				{
					if (!DateTime.TryParse(tmp,out RetVal))
					{
						RetVal = mDateTime;
					}
				}
				catch(Exception ex)
				{
					string s = ex.Message;
				}
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static DateTime BeginDate(DateTime mDateTime)
		{
			return Convert.ToDateTime(mDateTime.ToString("yyyy-MM-dd"));
		}
		//---------------------------------------------------------------------
		public static string BeginMonth(DateTime dateTime)
		{
			return BeginMonth(dateTime, "/");
		}
		//---------------------------------------------------------------------
		public static string BeginMonth(DateTime dateTime, string delimiter)
		{
			string RetVal = "";
			try
			{
				if (delimiter == null) delimiter = "";
				if (dateTime != null)
				{
					RetVal = "01" + delimiter;
					if (dateTime.Month <= 9) RetVal += "0";
					RetVal += dateTime.Month.ToString() + delimiter;
					RetVal += dateTime.Year.ToString();
				}
			}
			catch (Exception ex)
			{
				RetVal = ex.Message;
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Static_YYYYMMDD(DateTime dateTime, string DateDelimiter)
		{
			string day = "";
			try
			{
				if (DateDelimiter == null) DateDelimiter = "";
				if (dateTime == DateTime.MinValue)
				{
					day = "";
				}
				else
				{
					day = dateTime.Year.ToString() + DateDelimiter;
					if (dateTime.Month <= 9)
					{
						day = day + "0";
					}
					day = day + dateTime.Month.ToString() + DateDelimiter;
					if (dateTime.Day <= 9)
					{
						day = day + "0";
					}
					day = day + dateTime.Day.ToString();
				}
			}
			catch
			{

			}
			return day;
		}
		//---------------------------------------------------------------------
		public static string Static_YYYYMMDD(DateTime dateTime)
		{
			string DateDelimiter = "";
			return Static_YYYYMMDD(dateTime, DateDelimiter);
		}
		//---------------------------------------------------------------------
		public static string Static_YYYYMMDDHH(DateTime dateTime)
		{
			string DateDelimiter = "";
			string RetVal = Static_YYYYMMDD(dateTime, DateDelimiter);
			if (dateTime.Hour <= 9)
			{
				RetVal += "0";
			}
			RetVal += dateTime.Hour.ToString();
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Static_YYYYMMDDHHMI(DateTime dateTime)
		{
			string RetVal = Static_YYYYMMDDHH(dateTime);
			if (dateTime.Minute <= 9)
			{
				RetVal += "0";
			}
			RetVal += dateTime.Minute.ToString();
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Static_YYMMDDHHMISS(DateTime dateTime, string DateDelimiter, string TimeDelimiter)
		{
			string day = "";
			try
			{
				if (DateDelimiter == null) DateDelimiter = "";
				if (TimeDelimiter == null) TimeDelimiter = "";
				string Gap = (DateDelimiter.Length > 0) ? " " : "";
				if (dateTime == DateTime.MinValue)
				{
					day = "";
				}
				else
				{
					day = dateTime.Year.ToString().Substring(2, 2) + DateDelimiter;
					if (dateTime.Month <= 9) day = day + "0" + dateTime.Month.ToString();
					else day = day + dateTime.Month.ToString();
					day = day + DateDelimiter;
					if (dateTime.Day <= 9) day = day + "0" + dateTime.Day.ToString();
					else day = day + dateTime.Day.ToString();
					day = day + Gap;
					if (dateTime.Hour <= 9) day = day + "0" + dateTime.Hour.ToString();
					else day = day + dateTime.Hour.ToString();
					day = day + TimeDelimiter;
					if (dateTime.Minute <= 9) day = day + "0" + dateTime.Minute.ToString();
					else day = day + dateTime.Minute.ToString();
					day = day + TimeDelimiter;
					if (dateTime.Second <= 9) day = day + "0" + dateTime.Second.ToString();
					else day = day + dateTime.Second.ToString();
				}
			}
			catch
			{

			}
			return day;
		}
		//---------------------------------------------------------------------
		public static string Static_yyyymmddhhmiss(DateTime dateTime, string DateDelimiter, string TimeDelimiter)
		{
			string RetVal = "";
			if (DateDelimiter == null) DateDelimiter = "";
			if (TimeDelimiter == null) TimeDelimiter = "";
			string Gap = (DateDelimiter.Length > 0) ? " " : "";
			if (dateTime > BeginDate())
			{
				RetVal = dateTime.Year.ToString() + DateDelimiter;
				RetVal += dateTime.Month.ToString().PadLeft(2, '0') + DateDelimiter;
				RetVal += dateTime.Day.ToString().PadLeft(2, '0') + Gap;
				RetVal += dateTime.Hour.ToString().PadLeft(2, '0') + TimeDelimiter;
				RetVal += dateTime.Minute.ToString().PadLeft(2, '0') + TimeDelimiter;
				RetVal += dateTime.Second.ToString().PadLeft(2, '0');
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Static_yyyymmddhhmiss(DateTime dateTime)
		{
			string DateDelimiter = "";
			string TimeDelimiter = "";
			return Static_yyyymmddhhmiss(dateTime, DateDelimiter, TimeDelimiter);
		}
		//---------------------------------------------------------------------
		public static string Static_yyyymmddhhmissms(DateTime dateTime)
		{
			string RetVal = Static_yyyymmddhhmiss(dateTime);
			if (!string.IsNullOrEmpty(RetVal))
			{
				RetVal += dateTime.Millisecond.ToString().PadLeft(3, '0');
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Static_yyyymmddhhmissms(DateTime mDateTime, string DateDelimiter, string TimeDelimiter)
		{
			string RetVal = "";
			if (mDateTime > DateTimeUtils.BeginDate())
			{
				RetVal=Static_yyyymmddhhmiss(mDateTime, DateDelimiter, TimeDelimiter);
				double ffff = 0;
				int Ms = (double.TryParse(mDateTime.ToString("ffff"), out ffff)) ? (int)Math.Round(ffff / 10, 0, MidpointRounding.AwayFromZero) : mDateTime.Millisecond;
				RetVal += "." + Ms.ToString().PadLeft(3, '0');
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Static_yyyymmddhhmissfffffff(DateTime mDateTime, string DateDelimiter
			, string TimeDelimiter)
		{
			string RetVal = "";
			if (mDateTime > DateTimeUtils.BeginDate())
			{
				RetVal = Static_yyyymmddhhmiss(mDateTime, DateDelimiter, TimeDelimiter);
				if (!string.IsNullOrEmpty(DateDelimiter))
				{
					RetVal += ".";
				}
				RetVal += mDateTime.ToString("fffffff");
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static DateTime Static_yyyymmddhhmissToDateTime(string dateTime, string DateDelimiter, string TimeDelimiter)
		{
			DateTime RetVal = System.DateTime.Now;
			try
			{
				if (!string.IsNullOrEmpty(dateTime))
				{
					if (dateTime.Length == 14)
					{
						if (DateDelimiter == null) DateDelimiter = "";
						if (TimeDelimiter == null) TimeDelimiter = "";
						string Gap = (DateDelimiter.Length > 0) ? " " : "";
						string yyyy = dateTime.Substring(0, 4);
						string mm = dateTime.Substring(4, 2);
						string dd = dateTime.Substring(6, 2);
						string hh = dateTime.Substring(8, 2);
						string mi = dateTime.Substring(10, 2);
						string ss = dateTime.Substring(12, 2);
						string s = yyyy + "-" + mm + "-" + dd + " " + hh + ":" + mi + ":" + ss;
						RetVal = Convert.ToDateTime(s);
					}
				}
			}
			catch
			{

			}
			return RetVal;
		}
		public static double getMilliseconds(DateTime dt)
		{
			try
			{
				double d = dt.ToUniversalTime().Ticks / TimeSpan.TicksPerMillisecond;
				return d;
			}
			catch
			{
				return -1;
			}
		}
		//----------------------------------------------------
		public static string Static_DateTimeToDDMMYYYY(DateTime date, string Default)
		{
			string RetVal = Default;
			try
			{
				if (date != null)
				{
					if (date.Year > 1900)
					{
						RetVal = date.ToString("dd/MM/yyyy");
					}
				}
			}
			catch
			{
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static string Static_VnDayDDMMYYYY(DateTime date, string Default)
		{
			return DayOfWeek_Vn(Convert.ToString(date.DayOfWeek)) + ", " + Static_DateTimeToDDMMYYYY(date, Default);
		}

		//-----------------------------------------------------------------------------------
		public static bool isDate(string str)
		{
			bool RetVal = false;
			string[] strMonths = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
			foreach (string s in strMonths)
			{
				if (str.Contains(s))
				{
					RetVal = true;
					break;
				}
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------
		public static DateTime convertStr(string strDate)
		{
			string pattern = "MMMM dd";
			return convertStr(strDate, pattern);
		}
		//-----------------------------------------------------------------------------------
		public static DateTime convertStr(string strDate, string pattern)
		{
			DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
			dtfi.ShortDatePattern = pattern;
			dtfi.DateSeparator = " ";
			DateTime objDate = Convert.ToDateTime(strDate, dtfi);
			return objDate;
		}
		//-----------------------------------------------------------------------------------
		public static string DDMMYYYY(byte DD, byte MM, short YYYY, string Delimiter)
		{
			string RetVal = "";
			if (Delimiter == null)
			{
				Delimiter = "";
			}
			if ((DD > 0) && (MM > 0) && (YYYY > 0))
			{
				if (DD <= 9)
				{
					RetVal = RetVal + "0";
				}
				RetVal = RetVal + DD.ToString() + Delimiter;
				if (MM <= 9)
				{
					RetVal = RetVal + "0";
				}
				RetVal = RetVal + MM.ToString() + Delimiter + YYYY.ToString();
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------
		public static string DDMM(byte DD, byte MM, string Delimiter)
		{
			string RetVal = "";
			if (Delimiter == null)
			{
				Delimiter = "";
			}
			if ((DD > 0) && (MM > 0))
			{
				if (DD <= 9)
				{
					RetVal = RetVal + "0";
				}
				RetVal += DD.ToString() + Delimiter;
				if (MM <= 9)
				{
					RetVal +="0";
				}
				RetVal +=MM.ToString();
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------
		public static string DDMMYYYY(DateTime date, string Delimiter)
		{
			string RetVal = "";
			if (date!=null)
			{
				byte DD = (byte)date.Day;
				byte MM = (byte)date.Month;
				short YYYY = (short)date.Year;
				RetVal = DDMMYYYY(DD, MM, YYYY, Delimiter);
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------
		public static string DDMM(DateTime date, string Delimiter)
		{
			string RetVal = "";
			if (date != null)
			{
				byte DD = (byte)date.Day;
				byte MM = (byte)date.Month;
				RetVal = DDMM(DD, MM, Delimiter);
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------
		public static long DateTimeToTimestamp(DateTime value)
		{
			long RetVal = 0;
			try
			{
				DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				TimeSpan elapsedTime = value - Epoch;
				RetVal = (long)elapsedTime.TotalSeconds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------
		public static DateTime TryParseDateTimeNow(string VnDateTime)
		{
			DateTime RetVal = System.DateTime.Now;
			if (!string.IsNullOrEmpty(VnDateTime))
			{
				IFormatProvider culture = new CultureInfo("fr-FR", true);
				if (!DateTime.TryParse(VnDateTime, culture, DateTimeStyles.NoCurrentDateDefault, out RetVal))
				{
					RetVal = System.DateTime.Now;
				}
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------
		public static bool TryParseDateTime(string VnDateTime, ref DateTime RetDateTime)
		{
			bool RetVal=false;
			if (!string.IsNullOrEmpty(VnDateTime))
			{
				IFormatProvider culture = new CultureInfo("fr-FR", true);
				RetVal = DateTime.TryParse(VnDateTime, culture, DateTimeStyles.NoCurrentDateDefault, out RetDateTime);
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------
		public static DateTime ParseDateTime(string VnDateTime)
		{
			DateTime RetVal = BeginDate();
			if (!TryParseDateTime(VnDateTime, ref  RetVal))
			{
				RetVal = BeginDate();
			}
			return RetVal;
		}
	}
}