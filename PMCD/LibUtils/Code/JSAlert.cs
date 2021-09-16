using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public class JSAlert
{
	public JSAlert()
	{
	}
	//--------------------------------------------------------------------------------------------
	public static void Alert(string msg, Page m_Page)
	{
		if (!string.IsNullOrEmpty(msg))
		{
			string script = @"<script language='JavaScript'>" + "alert('" + msg + "');" + "</script>";
			m_Page.RegisterClientScriptBlock("alert", script);
		}
	}
	//--------------------------------------------------------------------------------------------
	public static void Alert(string msg, bool display, Page m_Page)
	{
		if (display)
		{
			Alert(msg, m_Page);
		}
	}
	//--------------------------------------------------------------------------------------------
	public static void close(Page m_Page)
	{
		string script = @"<script language='JavaScript'>" + "window.close();" + "</script>";
		m_Page.RegisterClientScriptBlock("CloseScript", script);
	}
	//--------------------------------------------------------------------------------------------
	public static void Confirm(string msg, bool display, Page m_Page)
	{
		string script = @"<script language='JavaScript'>" +
								" function confirmSubmit() {" +
								" var msg = '" + msg + "';" +
								" return confirm(msg);" +
								" }" +
								" </script>";
		if (display)
		{
			m_Page.RegisterClientScriptBlock("Confirm", script);
			m_Page.Form.Attributes.Add("onSubmit", "return confirmSubmit();");
		}
	}
}
