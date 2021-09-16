using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Reflection;
namespace Lib.Utils
{
	public class HtmlParser
	{
		public HtmlParser()
		{
		}
		public static bool Upload(string LogFilePath, string LogFileName, HttpRequest mHttpRequest, string ObjName, string FilePath,ref string FileName)
		{
			bool RetVal = false;
			try
			{
				if (!string.IsNullOrEmpty(FilePath))
				{
					FilePath = FilePath.Replace("/", "\\");
					HttpPostedFile postedFile = mHttpRequest.Files[ObjName];
					if (postedFile != null)
					{
						if ((postedFile.ContentLength > 0)
							&&(postedFile.ContentLength <= 5*1024*1024)
						)
						{
							if (DirectoryUtils.CreateDirectory(LogFilePath, LogFileName, FilePath))
							{
								string FileNameTmp = Path.GetFileName(postedFile.FileName);
								FileName = (string.IsNullOrEmpty(FileName)) ? FileNameTmp : FileName+"_"+FileNameTmp;
								if (FileUtils.CanUpload(FileName))
								{
									string SaveFullName = FilePath + "\\" + FileName;
									if (File.Exists(SaveFullName))
									{
										string BakFilePath = FilePath + "\\Bak";
										if (DirectoryUtils.CreateDirectory(LogFilePath, LogFileName, BakFilePath))
										{
											string BkFullName = BakFilePath + "\\" + FileUtils.GenerateBackupFileName(FileName);
											File.Move(SaveFullName, BkFullName);
										}
									}
									postedFile.SaveAs(SaveFullName);
									RetVal = true;
								}
								else
								{
									LogFiles.WriteLog("Deny upload FilePath=" + FilePath + ", FileName=" + FileName, LogFilePath + "\\" + SystemConstants.LogFilePath_Deny, LogFileName + ".HtmlParser." + MethodBase.GetCurrentMethod().Name);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message + " FilePath=" + FilePath + ", ObjName=" + ObjName, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".HtmlParser." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
        //-----------------------------------------------------------------
        public static string Static_GetRelativeUrl(string BeginPatern, string Url)
        {
            string RetVal = "";
            if (!string.IsNullOrEmpty(Url))
            {
                if (string.IsNullOrEmpty(BeginPatern))
                {
                    RetVal = Url;
                }
                else
                {
                    int PosFirstSlash = Url.IndexOf(BeginPatern);
                    if (PosFirstSlash > 0)
                    {
                        PosFirstSlash += BeginPatern.Length;
                        int PosQuestionMark = Url.LastIndexOf('?');
                        if (PosQuestionMark > PosFirstSlash)
                        {
                            RetVal = StringUtils.SubString(Url, PosFirstSlash, PosQuestionMark - PosFirstSlash).Trim();
                        }
                        else
                        {
                            RetVal = StringUtils.SubString(Url, PosFirstSlash, 0).Trim();
                        }
                    }
                    else
                    {
                        RetVal = Url;
                    }
                }
            }
            return RetVal;
        }
		//---------------------------------------------------------------------------
		public static bool GetValue(HttpRequest mHttpRequest, string ObjName, out string ObjValue)
		{
			bool RetVal = false;
			ObjValue = "";
			try
			{
				if (mHttpRequest != null)
				{
					if (mHttpRequest.Form[ObjName] != null)
					{
						string[] ArrValue = mHttpRequest.Form.GetValues(ObjName);
						if (ArrValue.Length > 0)
						{
							ObjValue = ArrValue[0];
						}
						RetVal = true;
					}
				}
			}
			catch 
			{
				
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------
		public static bool GetValueInt32(HttpRequest mHttpRequest, string ObjName, out int ObjValue)
		{
			string tmp = "";
			ObjValue = 0;
			bool RetVal = GetValue(mHttpRequest, ObjName, out tmp);
			if (RetVal)
			{
				RetVal = int.TryParse(tmp,out  ObjValue);
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------
		public static bool GetValueInt16(HttpRequest mHttpRequest, string ObjName, out short ObjValue)
		{
			string tmp = "";
			ObjValue = 0;
			bool RetVal = GetValue(mHttpRequest, ObjName, out tmp);
			if (RetVal)
			{
				RetVal = short.TryParse(tmp, out  ObjValue);
			}
			return RetVal;
		}
		//-------------------------------------------
		public static bool GetTextBox(GridViewRow row, string ElementName, out TextBox mTextBox)
		{
			mTextBox=GetTextBox(row, ElementName);
			return (mTextBox!=null);
		}
		//-------------------------------------------
		public static TextBox GetTextBox(GridViewRow row, string ElementName)
		{
			TextBox RetVal = null;
			if (row!=null)
			{
				RetVal = ((TextBox)row.FindControl(ElementName));
			}
			return RetVal;
		}
		//-------------------------------------------
		public static Label GetLabel(GridViewRow row, string ElementName)
		{
			Label RetVal = null;
			if (row != null)
			{
				RetVal = ((Label)row.FindControl(ElementName));
			}
			return RetVal;
		}
		//-------------------------------------------
		public static LinkButton GetLinkButton(GridViewRow row, string ElementName)
		{
			LinkButton RetVal = null;
			if (row != null)
			{
				RetVal = ((LinkButton)row.FindControl(ElementName));
			}
			return RetVal;
		}
		//-------------------------------------------
		public static HyperLink GetHyperLink(GridViewRow row, string ElementName)
		{
			HyperLink RetVal = null;
			if (row != null)
			{
				RetVal = ((HyperLink)row.FindControl(ElementName));
			}
			return RetVal;
		}
		//-------------------------------------------
		public static DropDownList GetDropDownList(GridViewRow row, string ElementName)
		{
			DropDownList RetVal = null;
			if (row != null)
			{
				RetVal = ((DropDownList)row.FindControl(ElementName));
			}
			return RetVal;
		}
		//-------------------------------------------
		public static CheckBox GetCheckBox(GridViewRow row, string ElementName)
		{
			CheckBox RetVal = null;
			if (row != null)
			{
				RetVal = ((CheckBox)row.FindControl(ElementName));
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string GetText(GridViewRow row, string ElementName)
		{
			string RetVal = "";
			TextBox m_TextBox = GetTextBox(row, ElementName);
			if (m_TextBox != null)
			{
				RetVal = m_TextBox.Text.Trim();
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string GetSelectedValue(DropDownList mDropDownList)
		{
			string RetVal = "";
			if (mDropDownList != null)
			{
				RetVal = mDropDownList.SelectedValue.Trim();
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string GetSelectedValue(GridViewRow row, string ElementName)
		{
			string RetVal = "";
			DropDownList m_DropDownList = GetDropDownList(row, ElementName);
			RetVal = GetSelectedValue(m_DropDownList);
			return RetVal;
		}
		//-------------------------------------------
		public static int GetTextInt32(GridViewRow row, string ElementName)
		{
			return StringUtils.StrToInt32(GetText(row, ElementName)); 
		}
		//-------------------------------------------
		public static short GetTextInt16(GridViewRow row, string ElementName)
		{
			return StringUtils.StrToInt16(GetText(row, ElementName));
		}
		//-------------------------------------------
		public static byte GetTextByte(GridViewRow row, string ElementName)
		{
			return StringUtils.StrToByte(GetText(row, ElementName));
		}
		//-------------------------------------------
		public static float GetTextFloat(GridViewRow row, string ElementName)
		{
			return StringUtils.StrToFloat(GetText(row, ElementName));
		}
		//-------------------------------------------
		public static DateTime GetTextDateTime(GridViewRow row, string ElementName)
		{
			return DateTimeUtils.ParseDateTime(GetText(row, ElementName));
		}
		//-------------------------------------------
		public static bool GetTextDateTime(GridViewRow row, string ElementName, ref DateTime mDateTime)
		{
			return DateTimeUtils.TryParseDateTime(GetText(row, ElementName), ref mDateTime);
		}
		//-------------------------------------------
		public static byte GetSelectedValueByte(GridViewRow row, string ElementName)
		{
			return StringUtils.StrToByte(GetSelectedValue(row, ElementName));
		}
		//-------------------------------------------
		public static int GetSelectedValueInt32(DropDownList mDropDownList)
		{
			return StringUtils.StrToInt32(GetSelectedValue(mDropDownList));
		}
		//-------------------------------------------
		public static short GetSelectedValueInt16(DropDownList mDropDownList)
		{
			return StringUtils.StrToInt16(GetSelectedValue(mDropDownList));
		}
		//-------------------------------------------
		public static byte GetSelectedValueByte(DropDownList mDropDownList)
		{
			return StringUtils.StrToByte(GetSelectedValue(mDropDownList));
		}
		//-------------------------------------------
		public static short GetSelectedValueInt16(GridViewRow row, string ElementName)
		{
			return StringUtils.StrToInt16(GetSelectedValue(row, ElementName));
		}
		//-------------------------------------------
		public static int GetSelectedValueInt32(GridViewRow row, string ElementName)
		{
			return StringUtils.StrToInt32(GetSelectedValue(row, ElementName));
		}
		//-------------------------------------------
		public static void PutTextBoxText(ref GridViewRow row, string ElementName, string TextValue, string ToolTipValue, bool Higlight, bool EnabledValue)
		{
			TextBox m_TextBox = GetTextBox(row, ElementName);
			if (m_TextBox != null)
			{
				m_TextBox.Text = TextValue;
				m_TextBox.ToolTip = ToolTipValue;
				if (Higlight)
				{
					m_TextBox.ForeColor = System.Drawing.Color.Red;
				}
				m_TextBox.Enabled = EnabledValue;
			}
		}
		//-------------------------------------------
		public static void PutTextBoxText(ref GridViewRow row, string ElementName, string TextValue, string ToolTipValue, bool Higlight)
		{
			PutTextBoxText(ref  row, ElementName, TextValue, ToolTipValue, Higlight, true);
		}
		//-------------------------------------------
		public static void PutTextBoxText(ref GridViewRow row, string ElementName, string TextValue, string ToolTipValue)
		{
			PutTextBoxText(ref row, ElementName, TextValue, TextValue, false);
		}
		//-------------------------------------------
		public static void PutTextBoxText(ref GridViewRow row, string ElementName, string TextValue)
		{
			PutTextBoxText(ref row, ElementName, TextValue, "",false);
		}
		//-------------------------------------------
		public static void PutLabelText(ref GridViewRow row, string ElementName, string TextValue, string ToolTipValue, bool Higlight)
		{
			Label m_Label = GetLabel(row, ElementName);
			if (m_Label != null)
			{
				m_Label.Text = TextValue;
				m_Label.ToolTip = ToolTipValue;
				if (Higlight)
				{
					m_Label.ForeColor = System.Drawing.Color.Red;
				}
			}
		}
		//-------------------------------------------
		public static void PutLabelText(ref GridViewRow row, string ElementName, string TextValue, string ToolTipValue)
		{
			PutLabelText(ref row, ElementName, TextValue, ToolTipValue, false);
		}
		//-------------------------------------------
		public static void PutLabelText(ref GridViewRow row, string ElementName, string TextValue)
		{
			PutLabelText(ref row, ElementName, TextValue, "",false);
		}
		//-------------------------------------------
		public static void PutHyperLinkText(ref GridViewRow row, string ElementName, string NavigateUrlValue
			, string TextValue, string ToolTipValue, bool EnabledValue, bool HighLight)
		{
			HyperLink m_HyperLink = GetHyperLink(row, ElementName);
			if (m_HyperLink != null)
			{
				m_HyperLink.NavigateUrl = NavigateUrlValue;
				m_HyperLink.Text = TextValue;
				m_HyperLink.ToolTip = ToolTipValue;
				m_HyperLink.Enabled = EnabledValue;
				if (HighLight)
				{
					m_HyperLink.ForeColor = System.Drawing.Color.Red;
				}
			}
		}
		//-------------------------------------------
		public static void PutHyperLinkText(ref GridViewRow row, string ElementName, string NavigateUrlValue
			,string TextValue, string ToolTipValue, bool EnabledValue)
		{
			HyperLink m_HyperLink = GetHyperLink(row, ElementName);
			if (m_HyperLink != null)
			{
				m_HyperLink.NavigateUrl = NavigateUrlValue;
				m_HyperLink.Text = TextValue;
				m_HyperLink.ToolTip = ToolTipValue;
				m_HyperLink.Enabled = EnabledValue;
			}
		}
		//-------------------------------------------
		public static void PutLinkButtonText(ref GridViewRow row, string ElementName, string TextValue
			, string ToolTipValue, bool VisibleValue, bool EnabledValue, string AttKey, string AttValue)
		{
			LinkButton m_LinkButton = GetLinkButton(row, ElementName);
			if (m_LinkButton != null)
			{
				m_LinkButton.Text = TextValue;
				m_LinkButton.ToolTip = ToolTipValue;
				m_LinkButton.Visible = VisibleValue;
				m_LinkButton.Enabled = EnabledValue;
				if (!string.IsNullOrEmpty(AttValue))
				{
					m_LinkButton.Attributes.Add(AttKey, AttValue);
				}
			}

		}
		//-------------------------------------------
		public static void PutLinkButtonText(ref GridViewRow row, string ElementName
			, bool VisibleValue, bool EnabledValue, string TextValue, string ToolTipValue)
		{
			PutLinkButtonText(ref  row, ElementName, TextValue, ToolTipValue, VisibleValue, EnabledValue, "", "");
		}
		//-------------------------------------------
		public static void PutLinkButtonText(ref GridViewRow row, string ElementName, string TextValue, string ToolTipValue)
		{
			PutLinkButtonText(ref  row, ElementName, TextValue, ToolTipValue, true,true, "", "");
		}
		//-------------------------------------------
		public static bool CheckBoxIsChecked(GridViewRow row, string ElementName)
		{
			bool RetVal = false;
			CheckBox m_CheckBox = GetCheckBox(row, ElementName);
			if (m_CheckBox != null)
			{
				RetVal = m_CheckBox.Checked;
			}
			return RetVal;
		}
		//-------------------------------------------
		public static void PutCheckBox(ref GridViewRow row, string ElementName, bool CheckedValue, bool EnabledValue)
		{
			CheckBox m_CheckBox = GetCheckBox(row, ElementName);
			if (m_CheckBox != null)
			{
				m_CheckBox.Checked = CheckedValue;
				m_CheckBox.Enabled = EnabledValue;
			}
		}
		//-------------------------------------------
		public static void PutSelectedValue(ref GridViewRow row, string ElementName, string SelectedValue)
		{
			try
			{
				DropDownList m_DropDownList = GetDropDownList(row, ElementName);
				if (m_DropDownList != null)
				{
					m_DropDownList.SelectedValue = SelectedValue;
				}
			}
			catch(Exception ex)
			{
				
			}
		}
		//-------------------------------------------
		public static string Static_BuildOl(string ValueOl)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(ValueOl))
			{
				RetVal = "<ol>" + ValueOl + "</ol>";
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildLi(string ValueLi)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(ValueLi))
			{
				RetVal = "<li>" + ValueLi + "</li>";
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildOption(string style,string Value,bool selected, string optionDesc)
		{
			string RetVal = "";
			//<option value="<%=mAbsentTypes.AbsentTypeId.ToString()%>" selected="selected"><%=mAbsentTypes.AbsentTypeDesc%></option>  
			RetVal = "<option value=\"" + Value + "\"";
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			if (selected)
			{
				RetVal += " selected=\"selected\"";
			}
			RetVal += ">"+optionDesc+"</option>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildOption(string Value, bool selected, string optionDesc)
		{
			string style = "";
			return Static_BuildOption( style, Value, selected,  optionDesc);
		}
		//-------------------------------------------
		public static string Static_BuildSelect(string style, string name, string Options, bool IsDisabled)
		{
			string RetVal = "";
			RetVal = "<select style=\"" + style + "\" name=\"" + name + "\"";
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			if (IsDisabled)
			{
				RetVal += " disabled=\"disabled\"";
			}
			RetVal += ">" + Options + "</select>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildSelect(string style, string name, string Options )
		{
			bool IsDisabled=false;
			return Static_BuildSelect(style, name, Options, IsDisabled);
		}
		//-------------------------------------------
		public static string Static_BuildUl(string ValueUl)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(ValueUl))
			{
				RetVal = "<ul>" + ValueUl + "</ul>";
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTr(string style ,string bgcolor, string Value)
		{
			string RetVal = "<tr";
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			if (!string.IsNullOrEmpty(bgcolor))
			{
				RetVal += " bgcolor=\"" + bgcolor + "\"";
			}
			RetVal += ">" + Value + "</tr>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTr(string bgcolor,string Value)
		{
			string style="";
			return Static_BuildTr(style ,bgcolor,Value);
		}
		//-------------------------------------------
		public static string Static_BuildTr(string ValueTr)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(ValueTr))
			{
				RetVal = "<tr>" + ValueTr + "</tr>";
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTable(string style, int cellpadding, int widthPercent, int border, string Value)
		{
			// <table border="1" cellpadding="4" cellspacing="0" 
			//style="border-collapse: collapse" width="100%">      
			string RetVal = "";
			if (!string.IsNullOrEmpty(Value))
			{
				RetVal = "<table border=\"" + border.ToString() + "\"";
				if (cellpadding > 0)
				{
					RetVal += " cellpadding=\"" + cellpadding.ToString() + "\"";
				}
				if (!string.IsNullOrEmpty(style))
				{
					RetVal += " style=\"" + style + "\"";
				}
				if (widthPercent > 0)
				{
					if (widthPercent >100)
					{
						RetVal += " width=\"" + widthPercent.ToString() + "px\"";
					}
					else
					{
						RetVal += " width=\"" + widthPercent.ToString() + "%\"";
					}
				}
				RetVal+=">" +Value + "</table>";
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTable(int  border, string Value)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(Value))
			{
				RetVal = "<table style=\"border-collapse: collapse;\" border=\"" + border.ToString() + "\">" + Value + "</table>";
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTable(string Value)
		{
			return Static_BuildTable(0, Value);
		}
		//-------------------------------------------
		public static string Static_BuildA01(string Value, string title, string href, string style)
		{
			string RetVal = "<a";
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			if (!string.IsNullOrEmpty(title))
			{
				RetVal += " title=\"" + title + "\"";
			}
			if (!string.IsNullOrEmpty(href))
			{
				RetVal += " href=\"" + href + "\"";
			}
			RetVal += ">" + Value + "</a>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildA01(string Value, string title, string href)
		{
			string style="";
			return Static_BuildA01(Value, title, href, style);
		}
		//-------------------------------------------
		public static string Static_BuildA01(string Value, string title)
		{
			string href = "";
			string style = "";
			return Static_BuildA01(Value, title, href, style);
		}
		//-------------------------------------------
		public static string Static_BuildA(string style, string title, string href, string Value)
		{
			string RetVal = "<a";
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			if (!string.IsNullOrEmpty(title))
			{
				RetVal += " title=\"" + title + "\"";
			}
			if (!string.IsNullOrEmpty(href))
			{
				RetVal += " href=\"" + href + "\"";
			}
			RetVal += ">" + Value + "</a>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildA(string title, string href, string Value)
		{
			string style = "";
			return Static_BuildA(style, title, href, Value);
		}
		//-------------------------------------------
		public static string Static_BuildImg(string title,string alt, int border, string src, int width, int height)
		{
			string RetVal = "<img alt=\"" + alt + "\" border=\"" + border.ToString() + "\" src=\"" + src+"\"";
			if (width>0)
			{
				RetVal += " width=\"" + width.ToString() + "px\"";
				RetVal += " style=\"width:" + width.ToString() + "px\"";
			}
			if (height>0)
			{
				RetVal += " height=\"" + height.ToString() + "px\"";
			}
			if (!string.IsNullOrEmpty(title))
			{
				RetVal += " title=\"" + title + "\"";
			}
			RetVal +="/>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTdText(string style, string Align, string Value)
		{
			string RetVal = "<td";
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			if (!string.IsNullOrEmpty(Align))
			{
				RetVal += " align=\"" + Align + "\"";
			}
			RetVal += ">" + Value + "</td>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTd(string Value, string Style, string Align, string Valign, string Title, int Colspan)
		{
			string RetVal = "";
			RetVal = "<td";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (!string.IsNullOrEmpty(Align))
			{
				RetVal += " align=\"" + Align + "\"";
			}
			if (!string.IsNullOrEmpty(Valign))
			{
				RetVal += " valign=\"" + Valign + "\"";
			}
			if (!string.IsNullOrEmpty(Title))
			{
				RetVal += " title=\"" + Title + "\"";
			}
			if (Colspan > 0)
			{
				RetVal += " colspan=\"" + Colspan.ToString() + "\"";
			}
			RetVal += ">";
			RetVal += Value;
			RetVal += "</td>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTd01(string Value, string Style, string Align, string Valign, string Title)
		{
			string RetVal = "";
			RetVal = "<td";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (!string.IsNullOrEmpty(Align))
			{
				RetVal += " align=\"" + Align + "\"";
			}
			if (!string.IsNullOrEmpty(Valign))
			{
				RetVal += " valign=\"" + Valign + "\"";
			}
			if (!string.IsNullOrEmpty(Title))
			{
				RetVal += " title=\"" + Title + "\"";
			}
			RetVal += ">";
			if (!string.IsNullOrEmpty(Value))
			{
				RetVal += Value;
			}
			RetVal += "</td>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTd01(string Value, string Style, string Align, string Valign)
		{
			string Title="";
			return Static_BuildTd01(Value, Style, Align, Valign, Title);
		}
		//-------------------------------------------
		public static string Static_BuildTd01(string Value, string Style, string Align)
		{
			string Valign = "top";
			string Title = "";
			return Static_BuildTd01(Value, Style, Align, Valign, Title);
		}
		//-------------------------------------------
		public static string Static_BuildTd01(string Value, string Style)
		{
			string Align = "left";
			string Valign = "top";
			string Title = "";
			return Static_BuildTd01(Value, Style, Align, Valign, Title);
		}
		//-------------------------------------------
		public static string Static_BuildTd(string Value)
		{
			string Style = "";
			string Align = "left";
			string Valign = "top";
			string Title = "";
			return Static_BuildTd01(Value, Style, Align, Valign, Title);
		}
		//-------------------------------------------
		public static string Static_BuildTd(string Style, string Align, string Value, string Title,string Valign)
		{
			string RetVal = "";
			RetVal = "<td";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (!string.IsNullOrEmpty(Align))
			{
				RetVal += " align=\"" + Align + "\"";
			}
			if (!string.IsNullOrEmpty(Valign))
			{
				RetVal += " valign=\"" + Valign + "\"";
			}
			if (!string.IsNullOrEmpty(Title))
			{
				RetVal += " title=\"" + Title + "\"";
			}
			RetVal += ">";
			if (!string.IsNullOrEmpty(Value))
			{
				RetVal += Value;
			}
			RetVal += "</td>";
			return RetVal;
		}
		
		//-------------------------------------------
		public static string Static_BuildTd(string Style, string Align, string Value, string Title)
		{
			string Valign = "top";
			return Static_BuildTd(Style, Align, Value, Title, Valign);
		}
		//-------------------------------------------
		public static string Static_BuildTd(string Style, string Align, string Value)
		{
			string Valign = "top";
			string Title = "";
			return Static_BuildTd(Style, Align, Value, Title, Valign);
		}
		//-------------------------------------------
		public static string Static_BuildHidden(string Style, string Id, string Name, string Value)
		{
			string RetVal = "<input type=\"hidden\" id=\"" + Id + "\" name=\""+Name+"\" value=\""+Value+"\"" ;
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			RetVal +="/>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildText(string Id, string Name, string Value,string title, string Style, string onclick)
		{
			string RetVal = "<input type=\"text\" id=\"" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"";
			bool Disabled = false;
			bool Readonly = true;
			if (!string.IsNullOrEmpty(title))
			{
				RetVal += " title=\"" + title + "\"";
			}
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (!string.IsNullOrEmpty(onclick))
			{
				RetVal += " onclick=\"" + onclick + "\"";
			}
			if (Disabled)
			{
				RetVal += " disabled=\"disabled\"";
			}
			if (Readonly)
			{
				RetVal += " readonly=\"readonly\"";
			}
			RetVal += "/>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildText(string Style, bool Disabled, bool Readonly, string Id, string Name, string Value, int MaxLength)
		{
			string RetVal = "<input type=\"text\" id=\"" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (Disabled)
			{
				RetVal += " disabled=\"disabled\"";
			}
			if (Readonly)
			{
				RetVal += " readonly=\"readonly\"";
			}
			if (MaxLength>0)
			{
				RetVal += " maxlength=\"" + MaxLength.ToString() + "\"";
			}
			RetVal += "/>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildText(string Style, bool Disabled, bool Readonly, string Id, string Name, string Value, int MaxLength, string onfocus, string onchange, string onkeyup)
		{
			string RetVal = "<input type=\"text\" id=\"" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (Disabled)
			{
				RetVal += " disabled=\"disabled\"";
			}
			if (Readonly)
			{
				RetVal += " readonly=\"readonly\"";
			}
			if (MaxLength > 0)
			{
				RetVal += " maxlength=\"" + MaxLength.ToString() + "\"";
			}
			if (!string.IsNullOrEmpty(onfocus))
			{
				RetVal += " onfocus=\"" + onfocus + "\"";
			}
			if (!string.IsNullOrEmpty(onchange))
			{
				RetVal += " onchange=\"" + onchange + "\"";
			}
			if (!string.IsNullOrEmpty(onkeyup))
			{
				RetVal += " onkeyup=\"" + onkeyup + "\"";
			}
			RetVal += "/>";
			return RetVal;
		}

		//-------------------------------------------
		public static string Static_BuildText(string Style, bool Disabled, bool Readonly, string Id, string Name, string Value, int MaxLength, int size)
		{
			string RetVal = "<input type=\"text\" id=\"" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (Disabled)
			{
				RetVal += " disabled=\"disabled\"";
			}
			if (Readonly)
			{
				RetVal += " readonly=\"readonly\"";
			}
			if (MaxLength > 0)
			{
				RetVal += " maxlength=\"" + MaxLength.ToString() + "\"";
			}
			if (size > 0)
			{
				RetVal += " size=\"" + size.ToString() + "\"";
			}
			RetVal += "/>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildFCKeditor(string Id, string Value)
		{
			string RetVal = "<FCKeditorV2:FCKeditor ID=\"Id"+Id+"\" runat=\"server\" BasePath=\"~/fckeditor/\" Value=\""+Value+"\"></FCKeditorV2:FCKeditor>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTextArea(string Style, bool Disabled, bool Readonly, string Id, string Name, string Value, int rows, int cols, string onfocus, string onchange, string onkeyup)
		{
			string RetVal = "<textarea id=\"Id" + Id + "\" name=\"" + Name + "\" cols=\"" + cols.ToString() + "\" rows=\"" + rows.ToString() + "\"";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (Disabled)
			{
				RetVal += " disabled=\"disabled\"";
			}
			if (Readonly)
			{
				RetVal += " readonly=\"readonly\"";
			}
			if (!string.IsNullOrEmpty(onfocus))
			{
				RetVal += " onfocus=\"" + onfocus + "\"";
			}
			if (!string.IsNullOrEmpty(onchange))
			{
				RetVal += " onchange=\"" + onchange + "\"";
			}
			if (!string.IsNullOrEmpty(onkeyup))
			{
				RetVal += " onkeyup=\"" + onkeyup + "\"";
			}
			RetVal += ">";
			if (!string.IsNullOrEmpty(Value))
			{
				RetVal += Value;
			}
			RetVal += "</textarea>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTextArea(string Style, bool Disabled, bool Readonly, string Id, string Name, string Value, int rows, int cols)
		{
			string RetVal = "<textarea id=\"Id" + Id + "\" name=\"" + Name + "\" cols=\"" + cols.ToString() + "\" rows=\""+rows.ToString()+"\"";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (Disabled)
			{
				RetVal += " disabled=\"disabled\"";
			}
			if (Readonly)
			{
				RetVal += " readonly=\"readonly\"";
			}
			RetVal += ">";
			if (!string.IsNullOrEmpty(Value))
			{
				RetVal += Value;
			}
			RetVal += "</textarea>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildButton(string Id, string Name, string Value,string title, string Style, string Onclick)
		{
			string RetVal = "<input type=\"button\" id=\"Id" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"";
			if (!string.IsNullOrEmpty(title))
			{
				RetVal += " title=\"" + Style + "\"";
			}
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (!string.IsNullOrEmpty(Onclick))
			{
				RetVal += " onclick=\"" + Onclick + "\"";
			}
			RetVal += "/>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildButton(string Style, string Id, string Name, string Value, string Onclick)
		{
			string RetVal = "<input type=\"button\" id=\"Id" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (!string.IsNullOrEmpty(Onclick))
			{
				RetVal += " onclick=\"" + Onclick + "\"";
			}
			RetVal += "/>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTdInputWithName(string styleTd,string styleInput, string alignInput, string disabledInput, string IdInput, string ValueInput, string ValueTd)
		{
			string RetVal = "<td style=\"" + styleTd + "\">";
			if (!string.IsNullOrEmpty(ValueTd))
			{
				RetVal += ValueTd;
			}
			RetVal += "<input type=\"hidden\" style=\"width:0px;\" id=\"Id" + IdInput + "\" name=\"Id" + IdInput + "\" value=\"" + IdInput + "\"/>";
			RetVal += "<input type=\"text\"";
			if (disabledInput == "disabled")
			{
				RetVal += " disabled=\"disabled\"";
			}
			if (!string.IsNullOrEmpty(styleInput))
			{
				RetVal += "style=\"" + styleInput+"\"";
			}
			RetVal +="id=\"Value" + IdInput + "\" name=\"Value" + IdInput + "\" value=\"" + ValueInput + "\"/>";
			RetVal+="</td>";
			return RetVal;
		}
		//-------------------------------------------
		//public static string Static_BuildTdInput(string styleInput, string alignInput, string disabledInput, string IdInput, string ValueInput)
		//{
		//  string RetVal = "<td style=\"" + styleInput + "\">";
		//  RetVal += "<input type=\"hidden\" style=\"width:0px;\" id=\"Id" + IdInput + "\" name=\"Id" + IdInput + "\" value=\"" + IdInput + "\"/>";
		//  RetVal += "<input type=\"text\"";
		//  if (disabledInput == "disabled")
		//  {
		//    RetVal += " disabled=\"disabled\"";
		//  }
		//  if (!string.IsNullOrEmpty(styleInput))
		//  {
		//    RetVal += " style=\"" + styleInput + "\"";
		//  }
		//  RetVal += "id=\"Value" + IdInput + "\" name=\"Value" + IdInput + "\" value=\"" + ValueInput + "\"/>";
		//  RetVal += "</td>";
		//  return RetVal;
		//}
		public static string Static_BuildTdInput(string styleInput, string alignInput, string disabledInput, string readonlyInput, string Id, string Name, string Value)
		{
			string RetVal = "<td style=\"" + styleInput + "\">";
			//RetVal += "<input type=\"hidden\" style=\"width:0px;\" id=\"" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"/>";
			RetVal += "<input type=\"text\"";
			if (readonlyInput == "readonly")
			{
				RetVal += " readonly=\"readonly\"";
			}
			if (disabledInput == "disabled")
			{
				RetVal += " disabled=\"disabled\"";
			}
			if (!string.IsNullOrEmpty(styleInput))
			{
				RetVal += " style=\"" + styleInput + "\"";
			}
			RetVal += "id=\"" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"/>";
			RetVal += "</td>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildTdInput(string styleInput, string alignInput, string disabledInput, string readonlyInput, string Id, string Value)
		{
			string RetVal = "<td> <input style=\"" + styleInput + "\" type=\"text\" id=\"" + Id + "\"";
			if (readonlyInput == "readonly")
			{
				RetVal += " readonly=\"readonly\"";
			}
			if (disabledInput == "disabled")
			{
				RetVal += " disabled=\"disabled\"";
			}
			RetVal += " name=\"" + Id + "\" value=\"" + Value + "\"/> </td>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildUploadFileBox(string Id, string Name, string Style)
		{
			string RetVal = "<input style=\"" + Style + "\" type=\"file\" id=\"" + Id + "\"";
			RetVal += " name=\"" + Name + "\" />";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildInput(string styleTd, string alignTd, string ValueTd, string styleInput, string alignInput, string disabledInput, string IdInput, string ValueInput)
		{
			string RetVal = Static_BuildTdText(styleTd, alignTd, ValueTd);
			string readonlyInput="";
			RetVal += Static_BuildTdInput(styleInput, alignInput, disabledInput, readonlyInput, IdInput, ValueInput);
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildInput(string styleTd, string alignTd, string ValueTd, string styleInput, string alignInput, string IdInput, string ValueInput)
		{
			string RetVal = Static_BuildTdText(styleTd, alignTd, ValueTd);
			RetVal += Static_BuildTdInput(styleInput, alignInput, "", "",IdInput, ValueInput);
			return RetVal;
		}
		
		//-------------------------------------------
		public static string Static_BuildInputRadio(string Id, string Name, string Value, bool Ischecked)
		{
			string RetVal = "<input type=\"radio\" id=\"" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"";
			if (Ischecked)
			{
				RetVal += " checked=\"checked\"";
			}
			RetVal += "/>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildLabel(string Id, string Name, string Value, string Title
			, string style, string runat, string onclick)
		{
			string RetVal = "<label id=\"" + Id + "\" name=\"" + Name + "\"";
			if (!string.IsNullOrEmpty(Title))
			{
				RetVal += " title=\"" + Title + "\"";
			}
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			if (!string.IsNullOrEmpty(runat))
			{
				RetVal += " runat=\"" + runat + "\"";
			}
			if (!string.IsNullOrEmpty(onclick))
			{
				RetVal += " onclick=\"" + onclick + "\"";
			}
			RetVal += ">" + Value + "</label>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildLabel(string style, string Id,string Name, string Value)
		{
			string RetVal = "<label id=\"" + Id + "\" name=\"" + Name + "\"";
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			RetVal += ">" + Value + "</label>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildLabelFor(string style, string Id, string Value)
		{
			string RetVal = "<label for=\"" + Id + "\"";
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			RetVal += ">" + Value + "</label>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildLabel(string style,string Id, string Value)
		{
			string RetVal = "<label Id=\"" + Id + "\"";
			if (!string.IsNullOrEmpty(style))
			{
				RetVal += " style=\"" + style + "\"";
			}
			RetVal +=">" + Value + "</label>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildStyleForLabel(bool Ischecked, string BasicStyle, int Num, bool IsRecomended, ref string label)
		{
			string RetVal = BasicStyle;
			if (Ischecked)
			{
				RetVal += (IsRecomended)? ";color:Red" : ";color:Blue";
				if (Num > 0)
				{
					label += "/" + Num.ToString();
				}
			}
			else
			{
				if (Num > 0)
				{
					label += "/" + Num.ToString();
					RetVal += ";color:Green";
				}
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildStyleForLabel(bool Ischecked, string BasicStyle, int Num, ref string label)
		{
			bool IsRecomended = false;
			return Static_BuildStyleForLabel(Ischecked, BasicStyle, Num, IsRecomended, ref label);
		}
		//-------------------------------------------
		public static string Static_BuildInputRadio(string Style,string Id, string Name, string Value,string Title, bool Ischecked,bool IsDisabled, string label)
		{
			string RetVal = "<input type=\"radio\" id=\"" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (!string.IsNullOrEmpty(Title))
			{
				RetVal += " title=\"" + Title + "\"";
			}
			if (Ischecked)
			{
				RetVal += " checked=\"checked\"";
			}
			if (IsDisabled)
			{
				RetVal += " disabled=\"disabled\"";
			}
			RetVal += "/>";
			if (!string.IsNullOrEmpty(label))
			{
				RetVal += " <label for=\"" + Id + "\"";
				if (!string.IsNullOrEmpty(Title))
				{
					RetVal += " title=\"" + Title + "\"";
				}
				if (!string.IsNullOrEmpty(Style))
				{
					RetVal += " style=\"" + Style + "\"";
				}
				RetVal += ">" + label + "</label>";
			}
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildInputRadio(string Id, string Name, string Value, string Title, bool Ischecked, bool IsDisabled, string label)
		{
			string Style = "";
			return Static_BuildInputRadio( Style, Id,  Name,  Value, Title,  Ischecked, IsDisabled,  label);
		}
		//-------------------------------------------
		public static string Static_BuildInputRadio(string Id, string Name, string Value, string Title, bool Ischecked, string label)
		{
			bool IsDisabled=false;
			return Static_BuildInputRadio( Id,  Name,  Value,  Title,  Ischecked,  IsDisabled,  label);
		}
		//-------------------------------------------
		public static string Static_BuildInputCheckbox(string Style, string Id, string Name, string Value, bool Ischecked,bool IsDisabled)
		{
			string RetVal = "<input type=\"checkbox\" id=\"" + Id + "\" name=\"" + Name + "\" value=\"" + Value + "\"";
			if (!string.IsNullOrEmpty(Style))
			{
				RetVal += " style=\"" + Style + "\"";
			}
			if (Ischecked)
			{
				RetVal += " checked=\"checked\"";
			}
			if (IsDisabled)
			{
				RetVal += " disabled=\"disabled\"";
			}
			RetVal += "/>";
			return RetVal;
		}
		//-------------------------------------------
		public static string Static_BuildInputCheckbox(string Id, string Name, string Value, bool Ischecked)
		{
			string Style="";
			bool IsDisabled = false;
			return Static_BuildInputCheckbox( Style,  Id,  Name,  Value,  Ischecked, IsDisabled);
		}
		//-------------------------------------------
		public static string RemoveHTML(string text)
		{
			string RetVal = "";
			if (!string.IsNullOrEmpty(text))
			{
				RetVal = text.Replace("&nbsp;", "");
				RetVal = StripTags(RetVal);
				RetVal = StripTagsRegex(RetVal);
				RetVal = StripTagsRegex2(RetVal);
				RetVal = RetVal.Trim();
			}
			return RetVal;
		}
		//-------------------------------------------
		public string getTextContent(string url)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse response = (HttpWebResponse)req.GetResponse();
			string StatusDescription = response.StatusDescription;
			string contentType = response.ContentType;
			long contentLength = response.ContentLength;
			string endcode = response.ContentEncoding;
			Encoding enc = System.Text.Encoding.GetEncoding(1252);
			StreamReader loResponseStream = new StreamReader(response.GetResponseStream());
			return loResponseStream.ReadToEnd();
		}
		//--------------------------------------------
		private static string StripTags(string text)
		{
			StringBuilder b = new StringBuilder(text.Length);
			bool inside = false;
			for (int i = 0; i < text.Length; i++)
			{
				char let = text[i];
				if (let == '<')
				{
					inside = true;
					continue;
				}
				if (let == '>')
				{
					inside = false;
					continue;
				}
				if (inside == false)
				{
					b.Append(let);
				}
			}
			return b.ToString();
		}

		/// <summary>
		/// 2.
		/// Common method used to remove all HTML tags from string with Regex.
		/// </summary>
		private static string StripTagsRegex(string text)
		{
			return Regex.Replace(text, "<.*?>", string.Empty);
		}

		/// <summary>
		/// Required for method 3.
		/// </summary>
		private static Regex _reg = new Regex("<.*?>", RegexOptions.Compiled);

		/// <summary>
		/// 3.
		/// Optimized Regex method to remove all HTML tags from string with Regex.
		/// </summary>
		private static string StripTagsRegex2(string text)
		{
			return _reg.Replace(text, string.Empty);
		}

		public static string convertHTML2XHTML(string sHTML)
		{
			try
			{
				HtmlReader reader = new HtmlReader(sHTML);
				StringBuilder sb = new StringBuilder();
				HtmlWriter writer = new HtmlWriter(sb);
				reader.Read();
				while (!reader.EOF)
				{
					writer.WriteNode(reader, true);
				}
				string s = sb.ToString();
				s = s.Replace("<html>", "");
				//s = s.Replace("<em>", "");
				s = s.Replace("&nbsp;", " ");
				s = s.Replace("&quote;", "\"");
				s = s.Replace("&", "&amp;");
				s = s.Replace("<div align=\"left\">", "<div align=\"center\">");
				s = s.Replace("<div align=\"right\">", "<div align=\"center\">");
				//s = s.Replace("<font face=\"arial, helvetica, sans-serif\">", "");
				//s = s.Replace("</font>", "");
				return s;
			}
			catch
			{
				return sHTML;
			}

		}
		//--------------------------------------------------------------------
		public static bool ObjExists(Page mPage, string ObjName)
		{
			bool retVal = false;
			try
			{				
				if (mPage.Request.Form[ObjName] != null)
				{
					retVal = true;
				}
			}
			catch 
			{
			}
			return retVal;
		}
		//--------------------------------------------------------------------
		public static bool Static_GetValue(string LogFilePath, string LogFileName, Page mPage, string ObjName, out string ObjValue)
		{
			bool retVal = false;
			ObjValue = "";
			try
			{
				if (mPage.Request.Form[ObjName] != null)
				{
					string[] ArrValue = mPage.Request.Form.GetValues(ObjName);
					if (ArrValue.Length > 0)
					{
						ObjValue = ArrValue[0].Trim();
					}
					retVal = true;
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + ".HtmlParser." + MethodBase.GetCurrentMethod().Name);
			}
			return retVal;
		}
	}
}
