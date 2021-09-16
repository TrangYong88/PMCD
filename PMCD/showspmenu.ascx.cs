using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using Lib.Obecna;
using Lib.Utils;
public partial class admin_showspmenu : System.Web.UI.UserControl
{
    protected string OBECNA_CONNECTION_STRING = MyConstants.OBECNA_CONNECTION_STRING;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    protected StringBuilder strMenu =new StringBuilder();
	private string AdminFolder = MyConstants.AdminFolder;
    protected string banner = "";
    protected string Fullname = "";
    private string UserName = "";
    private string UserPass = "";
    protected string IpAddress = "";
    private int ActUserId = 0;
	protected void Page_Load(object sender, EventArgs e)
	{
		string redirect = "";
		try
		{
			UserName = (Session["UserName"] == null) ? "" : Session["UserName"].ToString();
			UserPass = (Session["UserPass"] == null) ? "" : Session["UserPass"].ToString();
			IpAddress = Request.UserHostAddress.ToString();
			ActUserId = (Session["ActUserId"] == null) ? 0 : Int32.Parse(Session["ActUserId"].ToString());
			if (ActUserId > 0)
			{
				string Url = Request.Url.ToString();
				string RelativeUrl = HtmlUtils.Static_GetRelativeUrl(AdminFolder + "/", Url);
				Actions m_Actions = new Actions(OBECNA_CONNECTION_STRING);
				m_Actions = m_Actions.GetByUrl(LogFilePath, LogFileName, ActUserId, RelativeUrl);
				if (m_Actions.ActionId>0)
				{
					Fullname = (Session["FullName"] == null) ? "" : Session["FullName"].ToString().Trim();
					strMenu = Actions.GenMenuNew(LogFilePath, LogFileName, OBECNA_CONNECTION_STRING, ActUserId, MyConstants.PRJ_ROOT, MyConstants.ROOT_PATH, Fullname);
				}
				else
				{
					redirect = MyConstants.PRJ_ROOT + "/errMsg.aspx";
				}
			}
			else
			{
				redirect = MyConstants.PRJ_ROOT + "/Login.aspx";
			}
		}
		catch (Exception ex)
		{
			LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
		}
		if (!string.IsNullOrEmpty(redirect))
		{
			Response.Redirect(redirect);
		}
	}
}