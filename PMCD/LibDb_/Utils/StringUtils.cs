using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
namespace Lib.Utils
{
	public class StringUtils
	{
		public static string SpaceInHtml = "&nbsp;";
		public static string SYMBOL_DETERMINE = "->";
		public static string ALL = "All";
		public static string ALL_DESC = "Tất cả";
		public static string NA = "NA";
		public static string NA_DESC = "Không có";
		public static string Choose = "Choose";
		public static string ChooseDesc = "Chọn";
		public static string OTHER = "OTHER";
		public static string OTHER_DESC = "Khác...";


		public static string CHOOSE_ARTICLE = "Chose Article";
		public static string CHOOSE_ARTICLE_TITLE = "Chọn bài viết...";
		public static string CHOOSE_CATEGORY = "Chose Category";
		public static string CHOOSE_CATEGORY_DESC = "Chọn chuyên mục...";
		public static string ENTIRE_COUNTRY = "Entire";
		public static string ENTIRE_COUNTRY_DESC = "T.Quốc";
		public static string CHOOSE_PROVINCE = "Chose Province";
		public static string CHOOSE_PROVINCE_DESC = "Chọn Tỉnh/T.Phố";
		public static string CHOOSE_DISTRICT = "Chose District";
		public static string CHOOSE_DISTRICT_DESC = "Chọn Quận/Huyện";

		public static char Delimiter = '#';
		public static string colorLv1 = "blue";
		public static string colorLv2 = "DarkBlue";
		public static string colorLv3 = "DarkRed";
		public static string colorLv4 = "DarkViolet";
		public static string colorLv5 = "IndianRed";
		public static string colorLvOrther = "DimGray";
		public static string END_URL = "-";
		//public static string ALPHABET = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
		//public static string ALPHABET_Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		//public static string SPECIAL_CHAR = "@#$^&*()";
		public static string alphabet = "abcdefghijklmnopqrstuvwxyz";
		public static string alphabetUpper = alphabet.ToUpper();
		public static string ALPHABET = alphabet + alphabetUpper;
		public static string DIGISTS = "0123456789";
		public static string SPECIAL_CHAR = "!@#$^&*()_";
		public StringUtils()
		{
		}
		//----------------------------------------------------
		public static bool Compare(string LogFilePath, string LogFileName, string[] str1, string[] str2)
		{
			bool RetVal = false;
			try
			{
				if (str1 != null)
				{
					if (str2 != null)
					{
						if (str1.Length == str2.Length)
						{
							RetVal = true;
							for (int i = 0; i < str1.Length; i++)
							{
								if (str1[i] != str2[i])
								{
									RetVal = false;
									break;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".StringUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//------------------------------Standarlized-------------------------------------------------------
		public static bool IsDigists(string intput)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(intput))
			{
				RetVal = true;
				for (int i = 0; i < intput.Length; i++)
				{
					if (!DIGISTS.Contains(intput[i].ToString()))
					{
						RetVal = false;
						break;
					}
				}
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static bool IsValidIsdn(string Str)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Str))
			{
				Str = Str.Replace(" ", "");
				int Len = Str.Length;
				if ((Len >= 5) && (Len <= 15))
				{
					RetVal = IsDigists(Str);
				}
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static bool ContainsOneCharOfString(string Pattern, string Str)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Str))
			{
				for (int i = 0; i < Str.Length; i++)
				{
					if (Pattern.Contains(Str[i].ToString()))
					{
						RetVal = true;
						break;
					}
				}

			}
			return RetVal;
		}
		//----------------------------------------------------
		public static bool IsValidUserPass(string UserName, string UserPass, ref string strlog)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(UserName))
			{
				if (!string.IsNullOrEmpty(UserPass))
				{
					if (UserPass.ToLower().IndexOf(UserName) >= 0)
					{
						strlog = "Password contains user name";
					}
					else
					{
						RetVal = IsValidUserPass(UserPass, ref strlog);
					}
				}
				else
				{
					strlog = "Empty password";
				}
			}
			else
			{
				strlog = "Empty User name";
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static bool IsValidUserPass(string UserPass, ref string strlog)
		{
			bool RetVal = false;
			strlog = "";
			if (string.IsNullOrEmpty(UserPass))
			{
				strlog = "Mật khẩu rỗng";
			}
			else
			{
				if (UserPass.Length >= 8)
				{
					if (UserPass == UserPass.Replace(" ", ""))
					{
						if (ContainsOneCharOfString(alphabet, UserPass))
						{
							if (ContainsOneCharOfString(alphabetUpper, UserPass))
							{
								if (ContainsOneCharOfString(DIGISTS, UserPass))
								{
									if (ContainsOneCharOfString(SPECIAL_CHAR, UserPass))
									{
										RetVal = true;
									}
									else
									{
										strlog = "Không thấy ký tự đặc biệt trong mật khẩu"; ;
									}
								}
								else
								{
									strlog = "Không thấy ký chữ số trong mật khẩu";
								}
							}
							else
							{
								strlog = "Không thấy ký tự in hoa trong mật khẩu";
							}
						}
						else
						{
							strlog = "Không thấy ký tự in thường trong mật khẩu";
						}
					}
					else
					{
						strlog = "Mật khẩu không được chứa ký tự trống";
					}
				}
				else
				{
					strlog = "Chiều dài mật khẩu tối thiểu 8 ký tự";
				}
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static int Int32ReadCell(DataRow mDataRow, int Col)
		{
			int RetVal = 0;
			try
			{
				string Value = ReadCell(mDataRow, Col);
				if (!string.IsNullOrEmpty(Value))
				{
					int.TryParse(Value, out RetVal);
				}
			}
			catch
			{
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static short Int16ReadCell(DataRow mDataRow, int Col)
		{
			short RetVal = 0;
			try
			{
				string Value = ReadCell(mDataRow, Col);
				if (!string.IsNullOrEmpty(Value))
				{
					short.TryParse(Value, out RetVal);
				}
			}
			catch
			{
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static byte ByteReadCell(DataRow mDataRow, int Col)
		{
			byte RetVal = 0;
			try
			{
				string Value = ReadCell(mDataRow, Col);
				if (!string.IsNullOrEmpty(Value))
				{
					byte.TryParse(Value, out RetVal);
				}
			}
			catch
			{ 
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static string ReadCell(DataRow mDataRow, int Col)
		{
			string RetVal = "";
			if (Col >= 0)
			{
				if (mDataRow != null)
				{
					if (mDataRow.ItemArray.Length > Col)
					{
						RetVal = (mDataRow.ItemArray[Col] == null) ? "" : mDataRow.ItemArray[Col].ToString().Trim();
					}
				}
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static string SubString(string LogFilePath, string LogFileName, string str, int StartIndex, int Length)
		{
			string RetVal = "";
			try
			{
				if (!string.IsNullOrEmpty(str))
				{
					int Len = str.Length;
					if (Len >= StartIndex)
					{
						if (Length > 0)
						{
							if (Len >= StartIndex + Length)
							{
								RetVal = str.Substring(StartIndex, Length);
							}
							else
							{
								RetVal = str.Substring(StartIndex);
							}
						}
						else
						{
							RetVal = str.Substring(StartIndex);
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".StringUtils." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static string SubStringLeft(string str, int Length)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(str))
			{
				if (Length > 0)
				{
					int Len = str.Length;
					if (Len > Length)
					{
						RetVal = str.Substring(0, Length);
					}
					else
					{
						RetVal = str.Trim();
					}
				}
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static string SubStringRight(string str, int Length)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(str))
			{
				if (Length > 0)
				{
					int Len = str.Length;

					if (Len > Length)
					{
						RetVal = str.Substring(Len - Length, Length);
					}
					else
					{
						RetVal = str;
					}
				}
			}
			return RetVal.Trim();
		}
		//----------------------------------------------------
		public static string SubStringRight(string str, string Separator)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(str))
			{
				if (!string.IsNullOrEmpty(Separator))
				{
					int Pos = str.LastIndexOf(Separator);
					if (Pos >= 0)
					{
						RetVal = SubStringRight(str, Pos + Separator.Length);
					}
					else
					{
						RetVal = str;
					}
				}
			}
			return RetVal.Trim();
		}
		//-------------------------------------------------------------------------------------
		public static string GetElement(string Str, int Index)
		{
			string RetVal = "";
			if (Index >= 0)
			{
				RetVal = GetElement(Str, Index, ',');
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------
		public static string GetElement(string Str, int Index,char Delimeter)
		{
			string RetVal = "";
			if (Index >= 0)
			{
				if (!string.IsNullOrEmpty(Str))
				{
					string[] a_string = Str.Split(Delimeter);
					RetVal = GetElement(a_string, Index);
				}
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------
		public static string GetElement(string[] a_string, int Index)
		{
			string RetVal = "";
			if (Index >= 0)
			{
				if (a_string != null)
				{
					if (a_string.Length > Index)
					{
						RetVal = a_string[Index];
					}
				}
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------
		public static string ToString(List<string> lstring, string Gap)
		{
			string RetVal = "";
			if (lstring != null)
			{
				if (lstring.Count > 0)
				{
					foreach (string mstring in lstring)
					{
						RetVal += mstring + Gap;
					}
					if (!string.IsNullOrEmpty(Gap))
					{
						RetVal = StringUtils.RemoveLastString(RetVal, ",");
					}
				}
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------
		public static string ToString(string[] lstring, string Gap)
		{
			string RetVal = "";
			if (lstring != null)
			{
				if (lstring.Length > 0)
				{
					for (int i = 0; i < lstring.Length; i++)
					{
						RetVal += lstring[i].ToString() + Gap;
					}
					if (!string.IsNullOrEmpty(Gap))
					{
						RetVal = StringUtils.RemoveLastString(RetVal, ",");
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static List<string> ToList(string Str)
		{
			List<string> RetVal = new List<string>();
			if (!string.IsNullOrEmpty(Str))
			{
				for (int i = 0; i < Str.Length; i++)
				{
					RetVal.Add(Str[i].ToString());
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static List<string> Copy(List<string> lstring)
		{
			List<string> RetVal = new List<string>();
			if (lstring != null)
			{
				if (lstring.Count > 0)
				{
					for (int i = 0; i < lstring.Count; i++)
					{
						RetVal.Add(lstring[i]);
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static List<string> Random(List<string> lstring)
		{
			List<string> RetVal = new List<string>();
			if (lstring != null)
			{
				if (lstring.Count > 0)
				{
					for (int i = 0; i < lstring.Count; i++)
					{
						switch (i % 3)
						{
							case 0:
								{
									RetVal.Insert(0, lstring[i]);
									break;
								}
							case 1:
								{
									RetVal.Add(lstring[i]);
									break;
								}
							default:
								{
									RetVal.Insert(NumberUtil.RandNumber(0, i), lstring[i]);
									break;
								}
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static bool CompareStrict(List<string> lstring1, List<string> lstring2)
		{
			bool RetVal = false;
			if (lstring1.Count > 0)
			{
				if (lstring2.Count > 0)
				{
					if (lstring1.Count == lstring2.Count)
					{
						for (int i = 0; i < lstring1.Count; i++)
						{
							RetVal = (lstring1[i] == lstring2[i]);
							if (!RetVal)
							{
								break;
							}
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static bool Compare(List<string> lstring1, List<string> lstring2)
		{
			bool RetVal = Contains(lstring1, lstring2);
			if (RetVal)
			{
				RetVal = Contains(lstring2, lstring1);
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static bool Contains(List<string> lstring1, string[] lstring2)
		{
			bool RetVal = false;
			if (lstring1 != null)
			{
				if (lstring2 != null)
				{
					if (lstring1.Count > 0)
					{
						if (lstring2.Length > 0)
						{
							foreach (string mstring in lstring2)
							{
								RetVal = lstring1.Contains(mstring);
								if (!RetVal)
								{
									break;
								}
							}
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static bool Contains(List<string> lstring1, List<string> lstring2)
		{
			bool RetVal = false;
			if (lstring1 != null)
			{
				if (lstring2 != null)
				{
					if (lstring1.Count > 0)
					{
						if (lstring2.Count > 0)
						{
							foreach (string mstring in lstring2)
							{
								RetVal = lstring1.Contains(mstring);
								if (!RetVal)
								{
									break;
								}
							}
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static List<string> Union(List<string> lstring1, List<string> lstring2)
		{
			List<string> RetVal = Copy(lstring1);
			if (lstring1 != null)
			{
				if (lstring2 != null)
				{
					if (lstring2.Count > 0)
					{
						foreach (string mstring in lstring2)
						{
							if (!RetVal.Contains(mstring))
							{
								RetVal.Add(mstring);
							}
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static List<string> Mix(List<string> lstring1, List<string> lstring2)
		{
			List<string> RetVal = Copy(lstring1);
			if (lstring1 != null)
			{
				if (lstring2 != null)
				{
					if (lstring2.Count > 0)
					{
						for (int i = 0; i < lstring2.Count; i++)
						{
							if (!RetVal.Contains(lstring2[i]))
							{
								if (i % 2 == 0)
								{
									RetVal.Insert(0, lstring2[i]);
								}
								else
								{
									RetVal.Add(lstring2[i]);
								}
							}
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static List<string> Minus(List<string> lstring1, List<string> lstring2)
		{
			List<string> RetVal = lstring1;
			if (lstring1 != null)
			{
				if (lstring2 != null)
				{
					if (lstring2.Count > 0)
					{
						foreach (string mstring in lstring2)
						{
							if (RetVal.Contains(mstring))
							{
								RetVal.Remove(mstring);
							}
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static List<string> CopyRandom(List<string> lstring)
		{
			List<string> RetVal = new List<string>();
			if (lstring.Count > 0)
			{
				List<string> l_string = Copy(lstring);
				while (l_string.Count > 0)
				{
					int RandNum = NumberUtil.RandNumber(0, l_string.Count);
					RetVal.Add(l_string[RandNum]);
					l_string.Remove(l_string[RandNum]);
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static List<string> GetRandom(List<string> lstring, int NumberOfElements)
		{
			List<string> RetVal = new List<string>();
			if (NumberOfElements > 0)
			{
				if (lstring.Count > 0)
				{
					if (NumberOfElements >= lstring.Count)
					{
						RetVal = lstring;
					}
					else
					{
						for (int i = 0; i < NumberOfElements; i++)
						{
							int RandNum = NumberUtil.RandNumber(0, lstring.Count);
							RetVal.Add(lstring[RandNum]);
							lstring.Remove(lstring[RandNum]);
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static char[] GetRandom(string Str, int NumberOfElements)
		{
			char[] RetVal = new char[NumberOfElements];
			if (NumberOfElements > 0)
			{
				if (!string.IsNullOrEmpty(Str))
				{
					if (NumberOfElements >= Str.Length)
					{
						RetVal = Str.ToCharArray();
					}
					else
					{
						for (int i = 0; i < NumberOfElements; i++)
						{
							int RandNum = NumberUtil.RandNumber(0, Str.Length);
							RetVal[i] = Str[RandNum];
							Str = Str.Remove(RandNum, 1);
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static string ReadSetting(string key)
		{
			return ((ConfigurationManager.AppSettings[key] == null) ? "" : ConfigurationManager.AppSettings[key].ToString().Trim());
		}
		//------------------------------------------------------
		public static string RandomOfDigists(byte len)
		{
			string RetVal = "";
			if ((len > 0) && (len < 10))
			{
				string InitStr = DIGISTS;
				for (byte i = 0; i < len; i++)
				{
					if (InitStr.Length == 1)
					{
						RetVal += InitStr;
						break;
					}
					else
					{
						int Index = NumberUtil.RandNumber(0, InitStr.Length - 1);
						string CharStr = InitStr[Index].ToString();
						InitStr = InitStr.Replace(CharStr, "");
						RetVal += CharStr;
					}
				}
			}
			else
			{
				RetVal = DIGISTS;
			}
			return RetVal;
		}
		public static string StringOfDigists(string InitString)
		{
			string RetVal = "";
			if (string.IsNullOrEmpty(InitString))
			{
				RetVal = DIGISTS;
			}
			else
			{
				if (InitString[0].ToString().ToUpper() == "R")
				{
					if (InitString.Length > 1)
					{
						byte len = 0;
						if (InitString[1].ToString().ToUpper() == "R")
						{
							len = (byte)(NumberUtil.RandNumber(255, 32767) % 8 + 3);
							RetVal = RandomOfDigists(len);
						}
						else
						{
							if (Byte.TryParse(InitString[1].ToString(), out len))
							{
								RetVal = RandomOfDigists(len);
							}
							else
							{
								RetVal = DIGISTS;
							}
						}
					}
					else
					{
						RetVal = InitString;
					}
				}
				else
				{
					RetVal = InitString;
				}
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static bool IsNeedMedia(string str, int len)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(str))
			{
				if (str.Length > len)
				{
					RetVal = true;
				}
				else
				{
					if (str.Contains("<img"))
					{
						RetVal = true;
					}
				}
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static bool IsNeedMedia(string str)
		{
			return IsNeedMedia(str, SystemConstants.ContentLength);
		}
		//----------------------------------------------------
		public static bool IsValidUserName(string Str)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Str))
			{
				if (Str == Str.Replace(" ", ""))
				{
					if ((Str.Length >= 4) && (Str.Length <= 50))
					{
						string ValidIdentifierChars = alphabet + alphabetUpper + DIGISTS + "_.@";
						RetVal = true;
						for (int i = 0; i < Str.Length; i++)
						{
							if (!ValidIdentifierChars.Contains(Str[i].ToString()))
							{
								RetVal = false;
								break;
							}
						}
					}
				}
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static bool IsValidEmail(string Str)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Str))
			{
				if (Str == Str.Replace(" ", ""))
				{
					string[] Arr = Str.Split('@');
					if (Arr.Length == 2)
					{
						Arr = Arr[1].Split('.');
						if (Arr.Length > 1)
						{
							RetVal = true;
						}
					}
				}
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static bool IsMember(string[] StrArray, string Str)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Str))
			{
				if (StrArray != null)
				{
					foreach (string item in StrArray)
					{
						if (item == Str)
						{
							RetVal = true;
							break;
						}
					}
				}
			}
			return RetVal;
		}

		public static bool BeginParagraph(string Str)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Str))
			{
				string[] StrArray = new string[] { "-", "+", "*" };
				RetVal = IsMember(StrArray, Str);
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Concat(string str1, string str2, string Join)
		{
			string RetVal = str1;
			if (string.IsNullOrEmpty(RetVal))
			{
				RetVal = str2;
			}
			else
			{
				if (!string.IsNullOrEmpty(str2))
				{
					RetVal += Join + str2;
				}
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string ReplaceAt(string input, int index, char newChar)
		{
			if (!string.IsNullOrEmpty(input))
			{
				StringBuilder sb = new StringBuilder(input);
				sb[index] = newChar;
				input = sb.ToString();
			}
			return input;
		}
		//---------------------------------------------------------------------
		public static string ReplaceQuot(string input)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(input))
			{
				RetVal = input.Replace("\"", "&quot;");
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Static_Caption(string Name)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(Name))
			{
				Name = Name.Trim();
				int Length = Name.Length;
				if (Length > 1)
				{
					RetVal = Name.Substring(0, 1).ToUpper() + Name.Substring(1, Length - 1).ToLower();
				}
				else
				{
					RetVal = Name.ToUpper();
				}
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string Static_SentenceCorrect(string str)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(str))
			{
				str = str.Trim();
				int Length = str.Length;
				if (Length > 1)
				{
					RetVal = str.Substring(0, 1).ToUpper() + str.Substring(1, Length - 1);
					if (!RetVal.EndsWith("."))
					{
						RetVal += ".";
					}
				}
				else
				{
					RetVal = str;
				}
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static bool Static_SplitFullName(string FullName, out string FirstName, out string MiddleName, out string LastName)
		{
			bool RetVal = false;
			FirstName = "";
			MiddleName = "";
			LastName = "";
			if (!string.IsNullOrEmpty(FullName))
			{
				FullName = FullName.Trim();
				if (!string.IsNullOrEmpty(FullName))
				{
					RetVal = true;
					string[] data = FullName.Split(' ');
					if (data.Length > 0)
					{
						if (data.Length == 1)
						{
							LastName = Static_Caption(data[0]);
						}
						else
						{
							if (data.Length == 2)
							{
								FirstName = Static_Caption(data[0]);
								LastName = Static_Caption(data[1]);
							}
							else
							{
								FirstName = Static_Caption(data[0]);
								LastName = Static_Caption(data[data.Length - 1]);
								for (int i = 1; i < data.Length - 1; i++)
								{
									MiddleName += " " + Static_Caption(data[i]);
								}
								MiddleName = MiddleName.Trim();
							}
						}
					}
					else
					{
						LastName = Static_Caption(FullName);
					}
				}
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static string InternationalMsIsdn(string MsIsdn)
		{
			if (!string.IsNullOrEmpty(MsIsdn))
			{
				if (!MsIsdn.StartsWith("84"))
				{
					if (MsIsdn.StartsWith("0"))
					{
						MsIsdn = "84" + MsIsdn.Substring(1);
					}
					else
					{
						MsIsdn = "84" + MsIsdn;
					}
				}
			}
			return MsIsdn;
		}
		//---------------------------------------------------------------------
		public static string Static_BuildXml(string TagName, string TagValue)
		{
			string RetVal = (string.IsNullOrEmpty(TagName)) ? "" : "<" + TagName + ">" + TagValue + "</" + TagName + ">";
			return RetVal;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_Decorate(byte Levels, string StrValue)
		{
			try
			{
				switch (Levels)
				{
					case 1:
						return "<font color='" + colorLv1 + "'>" + StrValue + "</font>";
					case 2:
						return "<font color='" + colorLv2 + "'><b>" + StrValue + "</b></font>";
					case 3:
						return "<font color='" + colorLv3 + "'>" + StrValue + "</font>";
					case 4:
						return "<font color='" + colorLv4 + "'>" + StrValue + "</font>";
					case 5:
						return "<font color='" + colorLv5 + "'>" + StrValue + "</font>";
					default:
						return "<font color='" + colorLvOrther + "'>" + StrValue + "</font>";
				}
			}
			catch
			{
				return "<font color='red'><u>Lỗi hiển thị</u></font>";
			}
		}


		public static string GetStandardizedString(string input)
		{
			try
			{
				if (string.IsNullOrEmpty(input))
				{
					return string.Empty;
				}
				input = input.Replace(",", ", ").Replace("; ", "; ").Replace(":", ": ").Replace("?", "? ");
				while (input.Contains("  "))
				{
					input = input.Replace("  ", " ");
				}
				input = input.Replace(" ,", ",").Replace(" ;", ";").Replace(" :", ":").Replace(" ?", "?");
				while (input.Contains("  "))
				{
					input = input.Replace("  ", " ");
				}
				string[] inputSplit = input.Split('.');
				input = string.Empty;
				for (int i = 0; i < inputSplit.Length; i++)
				{
					string tmp = inputSplit[i].Trim();
					tmp = tmp.Substring(0, 1).ToUpper() + tmp.Substring(1, tmp.Length - 1);
					if (!string.IsNullOrEmpty(input))
					{
						if (i - 1 >= 0)
						{
							if (isNumeric(tmp) && isNumeric(inputSplit[i - 1].Trim()))
							{
								input += ".";
							}
							else
							{
								input += ". ";
							}
						}
						else
						{
							input += ". ";
						}
					}
					input += tmp;
				}
				return input;
			}
			catch
			{
				return input;
			}
		}
		public static void getRequestParametersForTagCloud(string input, ref int tagId, ref int pageIndex)
		{
			try
			{
				tagId = 0; pageIndex = 0;
				if (!string.IsNullOrEmpty(input))
				{
					int paraStartIndex = input.LastIndexOf("-tag");
					if (paraStartIndex >= 0)
					{
						input = input.Substring(paraStartIndex + 1, input.Length - paraStartIndex - 1);
						string[] inputSplit = input.Split('-');
						for (int i = 0; i < inputSplit.Length; i++)
						{
							string tmp = inputSplit[i].ToString();
							if (tmp.StartsWith("tag"))
							{
								tagId = StrToInt32(tmp.Replace("tag", ""));
							}
							else if (tmp.StartsWith("p"))
							{
								pageIndex = StrToInt32(tmp.Replace("p", ""));
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static string RemoveSignature(string input)
		{
			if (input == null)
			{
				return null;
			}
			input = input.Replace("-", "");
			Regex rga = new Regex("[àÀảẢãÃáÁạẠăĂằẰẳẲẵẴắẮặẶâÂầẦẩẨẫẪấẤậẬ]");
			Regex rgd = new Regex("[đĐ]");
			Regex rge = new Regex("[èÈẻẺẽẼéÉẹẸêÊềỀểỂễỄếẾệỆ]");
			Regex rgi = new Regex("[ìÌỉỈĩĨíÍịỊ]");
			Regex rgo = new Regex("[òÒỏỎõÕóÓọỌôÔồỒổỔỗỖốỐộỘơƠờỜởỞỡỠớỚợỢ]");
			Regex rgu = new Regex("[ùÙủỦũŨúÚụỤưƯừỪửỬữỮứỨựỰ]");
			Regex rgy = new Regex("[ỳỲỷỶỹỸýÝỵỴ]");
			input = rga.Replace(input, "a");
			input = rgd.Replace(input, "d");
			input = rge.Replace(input, "e");
			input = rgi.Replace(input, "i");
			input = rgo.Replace(input, "o");
			input = rgu.Replace(input, "u");
			input = rgy.Replace(input, "y");
			return input;
		}

		public static void getRequestParametersForCatalog(string input, ref short cateId, ref int provinceId, ref int districtId, ref int vip)
		{
			try
			{
				cateId = 0; provinceId = -1; districtId = 0; vip = 0;
				if (!string.IsNullOrEmpty(input))
				{
					string[] inputSplit = input.Split('-');
					for (int i = 0; i < inputSplit.Length; i++)
					{
						string tmp = inputSplit[i].ToString();
						if (tmp.StartsWith("c"))
						{
							cateId = Convert.ToInt16(tmp.Replace("c", ""));
						}
						else if (tmp.StartsWith("d"))
						{
							districtId = Convert.ToInt32(tmp.Replace("d", ""));
						}
						else if (tmp.StartsWith("pro"))
						{
							provinceId = Convert.ToInt32(tmp.Replace("pro", ""));
						}
						else if (tmp.StartsWith("v"))
						{
							vip = Convert.ToInt32(tmp.Replace("v", ""));
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void getRequestParametersForClassifiedAdvs(string input, ref int itemId)
		{
			try
			{
				itemId = 0;
				if (string.IsNullOrEmpty(input))
					return;
				int paraStartIndex = input.LastIndexOf("-");
				if (paraStartIndex < 0)
					return;
				input = input.Substring(paraStartIndex + 1, input.Length - paraStartIndex - 1);
				itemId = Convert.ToInt32(input);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static string Static_AddingUnique(string curentStr, string newStr, string delimiter)
		{
			string tmp = "";
			string RetVal = string.IsNullOrEmpty(curentStr) ? "" : curentStr;
			if (string.IsNullOrEmpty(delimiter))
			{
				delimiter = Delimiter.ToString();
			}
			if (RetVal.Length > 0)
			{
				tmp = delimiter + newStr + delimiter;
				if (tmp.IndexOf(newStr, 0) < 0)
				{
					RetVal += delimiter + newStr;
					tmp = delimiter + RetVal + delimiter;
				}
			}
			else
			{
				RetVal = newStr;
				tmp = delimiter + RetVal + delimiter;
			}
			return RetVal;
		}
		public static string Static_TitleUnMark(string title)
		{
			string s = "";
			int i;
			for (i = 0; i < title.Length; i++)
			{
				if (Char.IsWhiteSpace(title[i]) || Char.IsLetterOrDigit(title[i]))
				{
					s += title[i];
				}

			}
			s = s.Trim();
			s = s.Replace("  ", " ");
			s = s.Replace("/", "");
			s = s.Replace("?", "");
			s = s.ToLower();
			s = UCS2Convert(s);
			if (s.Length > 160)
			{
				s = s.Substring(0, 159);
				s = s.Substring(0, s.LastIndexOf(" "));
			}
			return s;
		}



		public static string FromHex(string hex)
		{
			hex = hex.Replace("-", "");
			byte[] raw = new byte[(hex.Length - 2) / 2];
			for (int i = 1; i <= raw.Length; i++)
			{
				raw[i - 1] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
			}
			if (hex.Substring(0, 2).Equals("00"))
			{
				return Encoding.ASCII.GetString(raw);
			}
			if (hex.Substring(0, 2).Equals("08"))
			{
				return Encoding.BigEndianUnicode.GetString(raw);
			}
			return hex + "";//truong hop khong phai HEX
		}
		public static String UCS2Convert(String sContent)
		{
			String sUTF8Lower = "a|á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ|đ|e|é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ|i|í|ì|ỉ|ĩ|ị|o|ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ|u|ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự|y|ý|ỳ|ỷ|ỹ|ỵ";
			String sUTF8Upper = "A|Á|À|Ả|Ã|Ạ|Ă|Ắ|Ằ|Ẳ|Ẵ|Ặ|Â|Ấ|Ầ|Ẩ|Ẫ|Ậ|Đ|E|É|È|Ẻ|Ẽ|Ẹ|Ê|Ế|Ề|Ể|Ễ|Ệ|I|Í|Ì|Ỉ|Ĩ|Ị|O|Ó|Ò|Ỏ|Õ|Ọ|Ô|Ố|Ồ|Ổ|Ỗ|Ộ|Ơ|Ớ|Ờ|Ở|Ỡ|Ợ|U|Ú|Ù|Ủ|Ũ|Ụ|Ư|Ứ|Ừ|Ử|Ữ|Ự|Y|Ý|Ỳ|Ỷ|Ỹ|Ỵ";
			String sUCS2Lower = "a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|d|e|e|e|e|e|e|e|e|e|e|e|e|i|i|i|i|i|i|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|u|u|u|u|u|u|u|u|u|u|u|u|y|y|y|y|y|y";
			String sUCS2Upper = "A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|D|E|E|E|E|E|E|E|E|E|E|E|E|I|I|I|I|I|I|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|U|U|U|U|U|U|U|U|U|U|U|U|Y|Y|Y|Y|Y|Y";
			//String sUCS2Lower = "a|&#225;|&#224;|&#7843;|&#227;|&#7841;|&#259;|&#7855;|&#7857;|&#7859;|&#7861;|&#7863;|&#226;|&#7845;|&#7847;|&#7849;|&#7851;|&#7853;|&#273;|e|&#233;|&#232;|&#7867;|&#7869;|&#7865;|&#234;|&#7871;|&#7873;|&#7875;|&#7877;|&#7879;|i|&#237;|&#236;|&#7881;|&#297;|&#7883;|o|&#243;|&#242;|&#7887;|&#245;|&#7885;|&#244;|&#7889;|&#7891;|&#7893;|&#7895;|&#7897;|&#417;|&#7899;|&#7901;|&#7903;|&#7905;|&#7907;|u|&#250;|&#249;|&#7911;|&#361;|&#7909;|&#432;|&#7913;|&#7915;|&#7917;|&#7919;|&#7921;|y|&#253;|&#7923;|&#7927;|&#7929;|&#7925;";
			//String sUCS2Upper = "A|&#193;|&#192;|&#7842;|&#195;|&#7840;|&#258;|&#7854;|&#7856;|&#7858;|&#7860;|&#7862;|&#194;|&#7844;|&#7846;|&#7848;|&#7850;|&#7852;|&#272;|E|&#201;|&#200;|&#7866;|&#7868;|&#7864;|&#202;|&#7870;|&#7872;|&#7874;|&#7876;|&#7878;|I|&#205;|&#204;|&#7880;|&#296;|&#7882;|O|&#211;|&#210;|&#7886;|&#213;|&#7884;|&#212;|&#7888;|&#7890;|&#7892;|&#7894;|&#7896;|&#416;|&#7898;|&#7900;|&#7902;|&#7904;|&#7906;|U|&#218;|&#217;|&#7910;|&#360;|&#7908;|&#431;|&#7912;|&#7914;|&#7916;|&#7918;|&#7920;|Y|&#221;|&#7922;|&#7926;|&#7928;|&#7924;";
			String[] aUTF8Lower = sUTF8Lower.Split(new Char[] { '|' });
			String[] aUTF8Upper = sUTF8Upper.Split(new Char[] { '|' });
			String[] aUCS2Lower = sUCS2Lower.Split(new Char[] { '|' });
			String[] aUCS2Upper = sUCS2Upper.Split(new Char[] { '|' });
			Int32 nLimitChar;
			nLimitChar = aUTF8Lower.GetUpperBound(0);
			for (int i = 1; i <= nLimitChar; i++)
			{
				sContent = sContent.Replace(aUTF8Lower[i], aUCS2Lower[i]);
				sContent = sContent.Replace(aUTF8Upper[i], aUCS2Upper[i]);
			}
			return sContent;
		}
		public static string DivideHeadTail(string Str, string Delimiter, ref string Head, ref string Tail)
		{
			string RetVal = "";
			Head = "";
			Tail = "";
			try
			{
				if (!string.IsNullOrEmpty(Str))
				{
					if ((Delimiter.Length > 0) || (Delimiter == " "))
					{
						int Pos = Str.IndexOf(Delimiter);
						if (Pos >= 0)
						{
							if (Pos > 0)
							{
								Head = Str.Substring(0, Pos);
							}
							if (Pos + 1 < Str.Length)
							{
								Tail = Str.Substring(Pos + 1);
							}
						}
						else
						{
							Head = Str;
						}
					}
					else
					{
						Head = Str;
					}
					RetVal = Head;
				}
			}
			catch (Exception ex)
			{
				RetVal = ex.Message;
			}
			return RetVal;
		}
		//------------------------------------------------------------------------------------
		public static string Left(string Str, int Pos)
		{
			string RetVal = "";
			try
			{
				if (Str != null)
				{
					Str = Str.Trim();
					RetVal = (Pos > 0) ? ((Str.Length > Pos) ? (Str.Substring(0, Pos) + "...") : Str) : Str;
				}
			}
			catch { }
			return RetVal;
		}
		public static string DefaultDelimiter()
		{
			return Delimiter.ToString();
		}
		public static string GetQuote(string odString)
		{
			string[] codes = odString.Split(',');
			string result = "";
			for (int i = 0; i < codes.Length; i++)
			{
				result = result + "'" + codes[i] + "',";
			}
			result = result.Substring(0, result.Length - 1);
			return result;
		}
		//-------------------------------------------
		public static string Right(string str, int length)
		{
			string RetVal = str;
			if ((length > 0) && (str != null))
			{
				int len = str.Length;
				if (len > length)
				{
					RetVal = str.Substring(len - length, length);
				}
			}
			return RetVal;
		}

		public static string Separated(string str, byte num, string pad)
		{
			string RetVal = str;
			try
			{
				if ((str != null) && (num > 0) && (pad != null))
				{
					if ((str.Length > num) && (pad.Length > 0))
					{
						int padLength = str.Length % num;
						int pos = num;
						if (padLength > 0)
						{
							RetVal = str.Substring(0, padLength);
							pos = padLength;
						}
						else
						{
							RetVal = str.Substring(0, num);
						}

						while (pos < str.Length)
						{
							RetVal += pad + str.Substring(pos, num);
							pos += num;
						}
						RetVal = RetVal.Trim();
					}
				}
			}
			catch (Exception ex)
			{
				RetVal = ex.Message;
			}
			return RetVal;
		}
		public static string ExactValue(string str, string objName, char delimiter)
		{
			string[] codes = str.Split(delimiter);
			string result = "";
			int i = 0;
			while (i < codes.Length)
			{
				if (codes[i].Contains(objName))
				{
					result = codes[i].Replace(objName, "");
					result = result.Replace("=", "").Trim();
					i = codes.Length;
				}
				i++;
			}
			return result;
		}
		//------------------------------------------------------
		public static string GetChecked(CheckBoxList chkList)
		{
			return GetChecked(chkList, DefaultDelimiter());
		}
		//------------------------------------------------------
		public static string GetChecked(CheckBoxList chkList, string delimiter)
		{
			string RetVal = "";
			if (chkList != null)
			{
				delimiter = delimiter.Trim();
				if (delimiter.Length <= 0)
				{
					delimiter = DefaultDelimiter();
				}
				for (int i = 0; i < chkList.Items.Count; i++)
				{
					if (chkList.Items[i].Selected)
					{
						RetVal += chkList.Items[i].Value + delimiter;
					}
				}
				RetVal = RemoveLastString(RetVal, delimiter);
			}
			return RetVal;
		}
		//------------------------------------------------------
		public static void PutChecked(string strList, ref CheckBoxList chkList)
		{
			try
			{
				if (strList.Length > 0)
				{
					int Pos = 0;
					for (int i = 0; i < chkList.Items.Count; i++)
					{
						Pos = strList.IndexOf(chkList.Items[i].Value.ToString().Trim());
						if (Pos >= 0)
						{
							chkList.Items[i].Selected = true;
						}
						else
						{
							chkList.Items[i].Selected = false;
						}
					}
				}
			}
			catch
			{

			}
		}

		//---------------------------------------------------------------------
		public static string IntToStr(int IntValue, byte length)
		{
			return IntToStr(IntValue, length, "");
		}
		//---------------------------------------------------------------------
		public static string IntToStr(int IntValue, byte length, string Padding)
		{
			string RetVal = "";
			try
			{
				if (Padding.Length > 0)
				{
					if (IntValue >= 0)
					{
						RetVal = IntValue.ToString();
						while (RetVal.Length < length)
						{
							RetVal = "0" + RetVal;
						}
					}
					else
					{
						IntValue = -IntValue;
						RetVal = IntValue.ToString();
						while (RetVal.Length < length)
						{
							RetVal = "0" + RetVal;
						}
						RetVal = "-" + RetVal;
					}
				}
				else
				{
					RetVal = IntValue.ToString();
				}

			}
			catch
			{

			}
			return RetVal;
		}
		//----------------------------------------------------------------------
		public static string GetVisibleAndSelectedValue(DropDownList mDropDownList, ref bool Visible)
		{
			string RetVal = "";
			Visible = false;
			try
			{
				if (mDropDownList != null)
				{
					if (mDropDownList.Items.Count > 0)
					{
						RetVal = mDropDownList.SelectedValue.ToString();
						if (mDropDownList.Items.Count > 1)
						{
							Visible = true;
						}
						else
						{
							Visible = false;
						}
					}
				}
			}
			catch
			{
				RetVal = "";
			}
			return RetVal;
		}
		public static string[] SplitMessage(string s, int MsgLen)
		{
			int count = (int)s.Length / MsgLen;
			if (s.Length % MsgLen > 0) count += 1;
			string[] result = new string[count];
			try
			{
				for (int i = 0; i < count; i++)
				{
					if (i != count - 1)
					{
						result[i] = s.Substring(i * MsgLen, MsgLen);
					}
					else
					{
						result[i] = s.Substring(i * MsgLen, s.Length - i * MsgLen);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
		public static string Str2Hex(string input)
		{
			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
			byte[] fByte = enc.GetBytes(input);

			string hexString = "";
			for (int i = 0; i < fByte.Length; i++)
			{
				hexString += fByte[i].ToString("X2");

			}
			return hexString;
		}
		//---------------------------------------------
		public static bool IsNotInjection(string str)
		{
			return str == Static_InjectionString(str);
		}
		//----------------------------------------------------
		public static bool IsValidContent(string Str)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Str))
			{
				RetVal = IsNotInjection(Str);
			}
			return RetVal;
		}
		//----------------------------------------------------
		public static bool IsValidIds(string Str)
		{
			bool RetVal = false;
			if (!string.IsNullOrEmpty(Str))
			{
				Str = Str.Replace(" ", "");
				Str = Str.Replace(",", "");
				RetVal = IsDigists(Str);
			}
			return RetVal;
		}
		//---------------------------------------------
		public static string Static_InjectionString(string str)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(str))
			{
				string[] badChars = new string[] { "select", "drop", ";", "--", "insert", "delete", "xp_" };
				RetVal = Static_KillChar(str, badChars);
			}
			return RetVal;
		}
		//---------------------------------------------
		public static string Static_KillSpecialChar(string str)
		{
			string[] badChars = new string[] { "%", "/", ";", "\\", "?", ":", "!", "[", "]", "{", "}" };
			return Static_KillChar(str, badChars);
		}
		//---------------------------------------------
		private static string Static_KillChar(string str, string[] badChars)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(str))
			{
				RetVal = str.Trim();
				string LowerStr = RetVal.ToLower();
				for (int i = 0; i < badChars.Length; i++)
				{
					int Pos = LowerStr.IndexOf(badChars[i]);
					if (Pos >= 0)
					{
						string tmp = RetVal.Substring(Pos, badChars[i].Length);
						RetVal = RetVal.Replace(badChars[i], "");
						RetVal = RetVal.Replace(tmp, "");
						RetVal = RetVal.Replace(badChars[i].ToUpper(), "");
						LowerStr = RetVal.ToLower();
					}
				}
			}
			return RetVal;
		}
		//------------------------------Standarlized-------------------------------------------------------
		public static bool isNumeric(string intput)
		{
			bool result = true;
			string signArr = "+-";
			string numArr = "0123456789";
			try
			{
				for (int i = 0; i < intput.Length; i++)
				{
					if (!numArr.Contains(intput[i].ToString()))
					{
						if (i == 0)
						{
							if (!signArr.Contains(intput[i].ToString()))
							{
								result = false;
								break;
							}
						}
						else
						{
							result = false;
							break;
						}
					}
				}
			}
			catch 
			{				
			}
			return result;
		}
		//---------------------------------------------------------------------
		public static float StrToFloat(string s)
		{
			float RetVal = 0;
			try
			{
				if (!string.IsNullOrEmpty(s))
				{
					float.TryParse(s, out RetVal);
				}
			}
			catch
			{
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static byte StrToByte(string s)
		{
			byte RetVal = 0;
			try
			{
				if (!string.IsNullOrEmpty(s))
				{
					byte.TryParse(s, out RetVal);
				}
			}
			catch
			{
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static short StrToInt16(string s)
		{
			short RetVal = 0;
			try
			{
				if (!string.IsNullOrEmpty(s))
				{
					RetVal = Convert.ToInt16(s);
				}
			}
			catch
			{

			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static int StrToInt32(string s)
		{
			int RetVal = 0;
			try
			{
				if (!string.IsNullOrEmpty(s))
				{
					int.TryParse(s, out RetVal);
				}
			}
			catch
			{
			}
			return RetVal;
		}
		//---------------------------------------------------------------------
		public static long StrToInt64(string s)
		{
			long RetVal = 0;
			try
			{
				if (isNumeric(s))
				{
					RetVal = Convert.ToInt64(s);
				}
			}
			catch
			{

			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------------------
		public static int StrToInt32(string input, string EndChar)
		{
			int RetVal = 0;
			try
			{
				if (!string.IsNullOrEmpty(input))
				{
					int paraStartIndex = input.LastIndexOf(EndChar);
					if (paraStartIndex >= 0)
					{
						input = input.Substring(paraStartIndex + 1, input.Length - paraStartIndex - 1);
						RetVal = StrToInt32(input);
					}
				}
			}
			catch
			{

			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------------------
		public static string IdentifierNameCorrect(string Str)
		{
			Str = Static_KillSpecialChar(Str);
			Str = Str.Replace(" ", "");
			Str = Str.Replace("(", "");
			Str = Str.Replace(")", "");
			Str = Str.Replace("+", "");
			Str = Str.Replace("-", "");
			Str = Str.Replace("*", "");
			return VietnameseNoMark(Str);
		}
		//-----------------------------------------------------------------------------------------------
		public static string VietnameseNoMark(string Str)
		{
			if (!string.IsNullOrEmpty(Str))
			{
				Str = Str.Trim();
				Str = Str.Replace("á", "a");
				Str = Str.Replace("à", "a");
				Str = Str.Replace("ạ", "a");
				Str = Str.Replace("ả", "a");
				Str = Str.Replace("ã", "a");

				Str = Str.Replace("Á", "A");
				Str = Str.Replace("À", "A");
				Str = Str.Replace("Ạ", "A");
				Str = Str.Replace("Ả", "A");
				Str = Str.Replace("Ã", "A");

				Str = Str.Replace("ă", "a");
				Str = Str.Replace("ắ", "a");
				Str = Str.Replace("ằ", "a");
				Str = Str.Replace("ặ", "a");
				Str = Str.Replace("ẳ", "a");
				Str = Str.Replace("ẵ", "a");

				Str = Str.Replace("Ă", "A");
				Str = Str.Replace("Ắ", "A");
				Str = Str.Replace("Ằ", "A");
				Str = Str.Replace("Ặ", "A");
				Str = Str.Replace("Ẳ", "A");
				Str = Str.Replace("Ẵ", "A");

				Str = Str.Replace("â", "a");
				Str = Str.Replace("ấ", "a");
				Str = Str.Replace("ầ", "a");
				Str = Str.Replace("ậ", "a");
				Str = Str.Replace("ẩ", "a");
				Str = Str.Replace("ẫ", "a");

				Str = Str.Replace("Â", "A");
				Str = Str.Replace("Ấ", "A");
				Str = Str.Replace("Ầ", "A");
				Str = Str.Replace("Ậ", "A");
				Str = Str.Replace("Ẩ", "A");
				Str = Str.Replace("Ẫ", "A");

				Str = Str.Replace("đ", "d");
				Str = Str.Replace("Đ", "D");

				Str = Str.Replace("é", "e");
				Str = Str.Replace("è", "e");
				Str = Str.Replace("ẻ", "e");
				Str = Str.Replace("ẹ", "e");
				Str = Str.Replace("ẽ", "e");

				Str = Str.Replace("É", "E");
				Str = Str.Replace("È", "E");
				Str = Str.Replace("Ẻ", "E");
				Str = Str.Replace("Ẹ", "E");
				Str = Str.Replace("Ẽ", "E");

				Str = Str.Replace("ê", "e");
				Str = Str.Replace("ế", "e");
				Str = Str.Replace("ề", "e");
				Str = Str.Replace("ể", "e");
				Str = Str.Replace("ệ", "e");
				Str = Str.Replace("ễ", "e");

				Str = Str.Replace("Ê", "E");
				Str = Str.Replace("Ế", "E");
				Str = Str.Replace("Ề", "E");
				Str = Str.Replace("Ể", "E");
				Str = Str.Replace("Ệ", "E");
				Str = Str.Replace("Ễ", "E");

				Str = Str.Replace("í", "i");
				Str = Str.Replace("ì", "i");
				Str = Str.Replace("ỉ", "i");
				Str = Str.Replace("ị", "i");
				Str = Str.Replace("ĩ", "i");

				Str = Str.Replace("Í", "I");
				Str = Str.Replace("Ì", "I");
				Str = Str.Replace("Ỉ", "I");
				Str = Str.Replace("Ị", "I");
				Str = Str.Replace("Ĩ", "I");

				Str = Str.Replace("ó", "o");
				Str = Str.Replace("ò", "o");
				Str = Str.Replace("ỏ", "o");
				Str = Str.Replace("ọ", "o");
				Str = Str.Replace("õ", "o");

				Str = Str.Replace("Ó", "O");
				Str = Str.Replace("Ò", "O");
				Str = Str.Replace("Ỏ", "O");
				Str = Str.Replace("Ọ", "O");
				Str = Str.Replace("Õ", "O");

				Str = Str.Replace("ô", "o");
				Str = Str.Replace("ố", "o");
				Str = Str.Replace("ồ", "o");
				Str = Str.Replace("ổ", "o");
				Str = Str.Replace("ộ", "o");
				Str = Str.Replace("ỗ", "o");

				Str = Str.Replace("Ô", "O");
				Str = Str.Replace("Ố", "O");
				Str = Str.Replace("Ồ", "O");
				Str = Str.Replace("Ổ", "O");
				Str = Str.Replace("Ộ", "O");
				Str = Str.Replace("Ỗ", "O");

				Str = Str.Replace("ơ", "o");
				Str = Str.Replace("ớ", "o");
				Str = Str.Replace("ờ", "o");
				Str = Str.Replace("ở", "o");
				Str = Str.Replace("ợ", "o");
				Str = Str.Replace("ỡ", "o");

				Str = Str.Replace("Ơ", "O");
				Str = Str.Replace("Ớ", "O");
				Str = Str.Replace("Ờ", "O");
				Str = Str.Replace("Ở", "O");
				Str = Str.Replace("Ọ", "O");
				Str = Str.Replace("Ỡ", "O");

				Str = Str.Replace("ú", "u");
				Str = Str.Replace("ù", "u");
				Str = Str.Replace("ủ", "u");
				Str = Str.Replace("ụ", "u");
				Str = Str.Replace("ũ", "u");

				Str = Str.Replace("Ú", "U");
				Str = Str.Replace("Ù", "U");
				Str = Str.Replace("Ủ", "U");
				Str = Str.Replace("Ụ", "U");
				Str = Str.Replace("Ũ", "U");

				Str = Str.Replace("ư", "u");
				Str = Str.Replace("ứ", "u");
				Str = Str.Replace("ừ", "u");
				Str = Str.Replace("ử", "u");
				Str = Str.Replace("ự", "u");
				Str = Str.Replace("ữ", "u");

				Str = Str.Replace("Ư", "U");
				Str = Str.Replace("Ứ", "U");
				Str = Str.Replace("Ừ", "U");
				Str = Str.Replace("Ử", "U");
				Str = Str.Replace("Ự", "U");
				Str = Str.Replace("Ữ", "U");

				Str = Str.Replace("ý", "y");
				Str = Str.Replace("ỳ", "y");
				Str = Str.Replace("ỷ", "y");
				Str = Str.Replace("ỵ", "y");
				Str = Str.Replace("ỹ", "y");

				Str = Str.Replace("Ý", "Y");
				Str = Str.Replace("Ỳ", "Y");
				Str = Str.Replace("Ỷ", "Y");
				Str = Str.Replace("Ỵ", "Y");
				Str = Str.Replace("Ỹ", "Y");
			}
			return Str;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_GetLead(string str, int len)
		{
			try
			{
				if (!string.IsNullOrEmpty(str))
				{
					str = str.Replace("\"", "'");
					str = str.Replace("\n", "");
					str = str.Replace("\r", "");
					str = str.Replace("&nbsp;", " ");
					str = HtmlParser.RemoveHTML(str);
					if (str.Length > len)
					{
						str = str.Substring(0, len);
						str = str.Substring(0, str.LastIndexOf(" ")) + "...";
					}
				}
			}
			catch
			{
			}
			return str;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_GetLead(string str)
		{
			try
			{
				if (!string.IsNullOrEmpty(str))
				{
					str = str.Replace("'", "");
					str = HtmlParser.RemoveHTML(str);
					if (str.Length > 250)
					{
						str = str.Substring(0, 247);
						str = str.Substring(0, str.LastIndexOf(" ")) + "...";
					}
				}
			}
			catch
			{
			}
			return str;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_HighlightKeywords(string input, string keywords)
		{
			try
			{
				if (!string.IsNullOrEmpty(keywords))
				{
					if (!string.IsNullOrEmpty(input))
					{
						string[] sKeywords = keywords.Split(' ');
						foreach (string sKeyword in sKeywords)
						{
							input = Regex.Replace(input, sKeyword, string.Format("#shiglight{0}#ehiglight", "$0"), RegexOptions.IgnoreCase);
						}
						input = input.Replace("#shiglight", "<span class=\"higlight\">");
						input = input.Replace("#ehiglight", "</span>");
					}
				}
			}
			catch
			{
			}
			return input;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_LeadHighLight(string str, string keyword)
		{
			return Static_HighlightKeywords(Static_GetLead(str), keyword);
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_GetTitle(string str, int len)
		{
			string RetVal = "";
			if (len > 0)
			{
				if (!string.IsNullOrEmpty(str))
				{
					RetVal = str.Replace("'", "");
					if (RetVal.Length > len)
					{
						RetVal = RetVal.Substring(0, len);
						if (RetVal.LastIndexOf(" ") > 0)
						{
							RetVal = RetVal.Substring(0, RetVal.LastIndexOf(" ")) + "...";
						}
					}
				}
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_GetTitle(string str, int len, string Title)
		{
			string RetVal = Static_GetTitle(str, len).Trim();
			if (string.IsNullOrEmpty(RetVal))
			{
				RetVal = Title;
			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_GetIcon(string str)
		{
			try
			{
				if (!string.IsNullOrEmpty(str))
				{
					str = str.Replace(" ", "");
					str = str.Replace("-", "");
					str = str.Replace(",", "");
					str = str.Replace("&", "");
					str = VietnameseNoMark(str);
				}
			}
			catch
			{
			}
			return str;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_GetTitleForBackLink(string str)
		{
			try
			{
				if (!string.IsNullOrEmpty(str))
				{
					str = str.Replace("'", "");
					if (str.Length > 25)
					{
						str = str.Substring(0, 25);
						if (str.Contains("."))
						{
							str = str.Substring(0, str.LastIndexOf("."));
						}
						else
						{
							if (str.Contains(","))
							{
								str = str.Substring(0, str.LastIndexOf(","));
							}
							else
							{
								if (str.Contains("-"))
								{
									str = str.Substring(0, str.LastIndexOf("-"));
								}
								else
								{
									if (str.Contains(" "))
									{
										str = str.Substring(0, str.LastIndexOf(" "));
									}
								}
							}
						}
					}
				}
			}
			catch
			{

			}
			return str;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_RemovePattern(string str, string Pattern)
		{
			try
			{
				if (!string.IsNullOrEmpty(str))
				{
					if (!string.IsNullOrEmpty(Pattern))
					{
						while (str.Contains(Pattern))
						{
							str = str.Replace(Pattern, "");
						}
					}
				}
			}
			catch
			{
			}
			return str;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_RemoveDublicated(string input, string Pattern)
		{
			try
			{
				if (!string.IsNullOrEmpty(input))
				{
					if (!string.IsNullOrEmpty(Pattern))
					{
						string Pattern2 = Pattern + Pattern;
						while (input.Contains(Pattern2))
						{
							input = input.Replace(Pattern2, Pattern);
						}
					}
				}
			}
			catch
			{

			}
			return input;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_GetStandardString(string input)
		{
			try
			{
				if (!string.IsNullOrEmpty(input))
				{
					input = Static_RemoveDublicated(input, " ");
					input = Static_RemoveDublicated(input, "&nbsp;");
					input = input.Replace("\"", "").Replace("'", "").Replace(Environment.NewLine, "");
				}
			}
			catch
			{

			}
			return input;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_RandomString(byte Len)
		{
			string RetVal = "";
			try
			{
				if (Len > 0)
				{
					Random rd = new Random((int)DateTime.Now.Ticks); ;
					StringBuilder sb = new StringBuilder();
					for (byte i = 0; i < Len; i++)
					{
						sb.Append(ALPHABET.Substring(rd.Next(0, ALPHABET.Length - 1), 1));
					}
					RetVal = sb.ToString();
				}
			}
			catch
			{

			}
			return RetVal;
		}
		//-----------------------------------------------------------------------------------------------
		public static string Static_RandomString()
		{
			return Static_RandomString(5);
		}
		//-------------------------------------------------------------------------------------------------------------------------------
		public static string Static_RandomPass(byte Len)
		{
			string RetVal = "";
			try
			{
				if (Len <= 6)
				{
					Len = 6;
				}
				Random rd = new Random((int)DateTime.Now.Ticks); ;
				string key = ALPHABET + DIGISTS + SPECIAL_CHAR;
				StringBuilder sb = new StringBuilder();
				for (byte i = 0; i < Len; i++)
				{
					sb.Append(key.Substring(rd.Next(0, key.Length - 1), 1));
				}
				RetVal = sb.ToString().Trim();
			}
			catch
			{

			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------------------------------------------------
		public static string Static_RandomPass()
		{
			return Static_RandomPass(10);
		}
		//-------------------------------------------------------------------------------------------------------------------------------
		public static string Static_SpellDayLeft(short days, string Desc)
		{
			string RetVal = "";
			try
			{
				RetVal = (days > 0) ? "Tin này còn " + days + " ngày" : " Tin này đã hết hạn";
				if (!string.IsNullOrEmpty(Desc))
				{
					RetVal += " trên mục " + Desc.ToUpper() + ".";
				}
			}
			catch
			{

			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------------------------------------------------
		public static string Static_Left(string str, int len)
		{
			try
			{
				if (!string.IsNullOrEmpty(str))
				{
					if (str.Length > len)
					{
						str = str.Substring(0, len);
						str = str.Substring(0, str.LastIndexOf(" ")) + "...";
					}
				}
			}
			catch
			{
			}
			return str;
		}
		//-------------------------------------------------------------------------------------------------------------------------------
		public static string TitleFormat(string title)
		{
			string s = "";
			try
			{
				if (!string.IsNullOrEmpty(title))
				{
					int i;
					for (i = 0; i < title.Length; i++)
					{
						if (Char.IsWhiteSpace(title[i]) || Char.IsLetterOrDigit(title[i]))
						{
							s += title[i];
						}
					}
					s = s.Trim();
					s = s.Replace("  ", " ");
					s = s.Replace(" ", "-");
					s = s.Replace("/", "");
					s = s.Replace("?", "");
					s = s.Replace(",", "-");
					s = s.Replace("&", "-");
					s = s.ToLower();
					s = UCS2Convert(s);
				}
			}
			catch
			{
			}
			return s;
		}
		public static int ExactInt32(string IdString, string FILE_EXTEND)
		{
			int RetVal = 0;
			try
			{
				if (!string.IsNullOrEmpty(IdString))
				{
					IdString = IdString.Substring(IdString.LastIndexOf("-") + 1);
					IdString = IdString.Replace(FILE_EXTEND, "");
					IdString = IdString.Replace(".", "");
					RetVal = StrToInt32(IdString);
				}
			}
			catch
			{
			}
			return RetVal;
		}
		//-------------------------------------------------------------------------------------------------------------------------------
		public static string Static_BuildUrl(string ROOT_PATH, string FILE_EXTEND, string HouseName, int ItemId, string ItemName)
		{
			string RetVal = "";
			try
			{
				if (ItemId > 0)
				{
					if (!string.IsNullOrEmpty(HouseName))
					{
						if (!string.IsNullOrEmpty(ItemName))
						{
							RetVal = ROOT_PATH + TitleFormat(HouseName) + "/" + TitleFormat(ItemName) + "-" + ItemId.ToString() + "." + FILE_EXTEND;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return RetVal;
		}
		//------------------------------------------------------------------------------------------
		public static void StrToByteInt32(string Str, string Delimiter, ref byte Val1, ref int Val2)
		{
			Val1 = 0;
			Val2 = 0;
			try
			{
				if (!string.IsNullOrEmpty(Str))
				{
					string Head = "";
					string Tail = "";
					DivideHeadTail(Str, Delimiter, ref Head, ref Tail);
					Val1 = StrToByte(Head);
					Val2 = StrToInt32(Tail);
				}
			}
			catch
			{
			}
		}
		//------------------------------------------------------------------------------------------
		public static string NumberToStr(long i, string Delimiter)
		{
			string RetVal = "";
			try
			{
				RetVal = i.ToString(Delimiter);
			}
			catch
			{
			}
			return RetVal;
		}
		//------------------------------------------------------------------------------------------
		public static string Static_ErrorBuild(string StringType, string RequestId, string ErrorId, string ErrorDesc)
		{
			string RetVal = "";
			if (StringType == "JSON")
			{
				RetVal = "{\"RequestId\":" + RequestId + ",\"ErrorId\"" + ErrorId + ",\"ErrorDesc\"" + ErrorDesc + "}";
			}
			else
			{
				//RetVal = "<RequestId>" + RequestId + "</RequestId><ErrorId>" + ErrorId + "</ErrorId><ErrorDesc>ErrorDesc</ErrorDesc>";
				RetVal += Static_BuildXml("RequestId", RequestId);
				RetVal += Static_BuildXml("ErrorId", ErrorId);
				RetVal += Static_BuildXml("ErrorDesc", ErrorDesc);
			}
			return RetVal;
		}
		public static string Static_InternationalPhoneNumber(string PhoneNumber)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(PhoneNumber))
			{
				if (PhoneNumber.StartsWith("84"))
				{
					RetVal = PhoneNumber;
				}
				else
				{
					if (PhoneNumber.StartsWith("0"))
					{
						RetVal = "84" + PhoneNumber.Substring(1);
					}
					else
					{
						RetVal = RetVal = "84" + PhoneNumber;
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------------------
		public static string RemoveLastString(string str, string LastString)
		{
			string result = "";
			if (!string.IsNullOrEmpty(str))
			{
				str = str.Trim();
				if (string.IsNullOrEmpty(LastString))
				{
					result = str;
				}
				else
				{
					if (str.EndsWith(LastString))
					{
						result = str.Substring(0, str.Length - LastString.Length).Trim();
					}
					else
					{
						result = str;
					}
				}
			}
			return result;
		}
		//------------------------------------------------------------------
		public static string RemoveSpaceBeforEndMark(string str)
		{
			string RetVal = str;
			if (!string.IsNullOrEmpty(RetVal))
			{
				RetVal = RetVal.Trim();
				if (RetVal.Length > 1)
				{
					string[] LastStringArr = new string[] { ".", ";", ":", "!" };
					foreach (string LastString in LastStringArr)
					{
						if (RetVal.EndsWith(LastString))
						{
							RetVal = RetVal.Substring(0, RetVal.Length - 1).Trim() + LastString;
							break;
						}
					}
					RetVal = str;
				}
			}
			return RetVal;
		}
		//------------------------------------------------------------------
		public static string[] SplitToParagraph(string Str)
		{
			string[] StrArr = Str.Split(Environment.NewLine.ToCharArray());
			string[] RetVal = new string[StrArr.Length];
			int j = 0;
			foreach (string item in StrArr)
			{
				string e = RemoveSpaceBeforEndMark(item);
				if (!string.IsNullOrEmpty(e))
				{
					if (j == 0)
					{
						RetVal[j] = e;
						j++;
					}
					else
					{
						if (BeginParagraph(e.Substring(0, 1)))
						{
							RetVal[j - 1] += Environment.NewLine + e;
						}
						else
						{
							RetVal[j] = e;
							j++;
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------------------
		public static List<string> SplitToSentence(string Str)
		{
			List<string> RetVal = new List<string>();
			if (!string.IsNullOrEmpty(Str))
			{
				Str = Str.Trim();
				if (!string.IsNullOrEmpty(Str))
				{
					int Num = 3;
					char[] OLdChar = new char[] { '.', '!', '?' };
					char[] NewChar = new char[] { '#', '_', '^' };
					bool[] Flag = new bool[Num];
					string[] OldStr = new string[Num];
					string[] NewStr = new string[Num];
					for (int i = 0; i < Num; i++)
					{
						OldStr[i] = "";
						OldStr[i] = OldStr[i].PadLeft(Num, OLdChar[i]);
						NewStr[i] = "";
						NewStr[i] = "<" + NewStr[i].PadLeft(Num, NewChar[i]) + ">";
						Flag[i] = (Str.IndexOf(OldStr[i]) >= 0);
						if (Flag[i])
						{
							Str = Str.Replace(OldStr[i], NewStr[i]);
						}
					}
					foreach (string item in Str.Split(OLdChar))
					{
						string s = item.Trim();
						if (s.Length > 1)
						{
							for (int i = 0; i < Num; i++)
							{
								if (Flag[i])
								{
									s = s.Replace(NewStr[i], OldStr[i]);
								}
							}
							RetVal.Add(s);
						}
					}
				}
			}
			return RetVal;
		}
		//------------------------------------------------------------------
		public static string RemoveDublicated(string Str, string RemoveStr)
		{
			string Result = "";
			if (!string.IsNullOrEmpty(Str))
			{
				Str = Str.Trim();
				string DubRemoveStr = "";
				if (RemoveStr == " ")
				{
					DubRemoveStr = RemoveStr + RemoveStr;
					while (Str.Contains(DubRemoveStr))
					{
						Str = Str.Replace(DubRemoveStr, RemoveStr);
					}
					Result = RemoveLastString(Str, RemoveStr);
				}
				else
				{
					if (!string.IsNullOrEmpty(RemoveStr))
					{
						DubRemoveStr = RemoveStr + RemoveStr;
						while (Str.Contains(DubRemoveStr))
						{
							Str = Str.Replace(DubRemoveStr, RemoveStr);
						}
						Result = RemoveLastString(Str, RemoveStr);
					}
				}
			}
			return Result;
		}

		//------------------------------------------------------------------
		public static string RemoveString(string str, string beginDivTag)
		{
			string result = "";
			if (!string.IsNullOrEmpty(str))
			{
				string endDivTag = "</DIV>";
				if (str.IndexOf(beginDivTag) != -1)
				{
					int bPost = str.IndexOf(beginDivTag);
					result = str.Substring(0, bPost);
				}
				int leadLength = result.Length;
				str = str.Substring(leadLength, str.Length - leadLength);
				if (str.IndexOf(endDivTag) != -1)
				{
					int ePost = str.IndexOf(endDivTag);
					result += str.Substring(ePost + endDivTag.Length, str.Length - (ePost + endDivTag.Length));
				}
			}
			return result;
		}
		//------------------------------------------------
		public static string RemoveString(string str)
		{
			string beginDivTag = "<DIV style=\"DISPLAY: none\" align=left>"; //begin Div Tag
			return RemoveString(str, beginDivTag);
		}
		//------------------------------------------------
		public static bool IsSame(string[] str1, string[] str2)
		{
			bool RetVal = false;
			if (str1 != null)
			{
				if (str2 != null)
				{
					if (str1.Length > 0)
					{
						if (str1.Length == str2.Length)
						{
							string string1 = "";
							string string2 = "";
							for (int i = 0; i < str1.Length; i++)
							{
								string1 += str1[i];
								string2 += str2[i];
							}
							if (string1 == string2)
							{
								RetVal = true;
							}
						}
					}
				}
			}
			return RetVal;
		}
	}
}