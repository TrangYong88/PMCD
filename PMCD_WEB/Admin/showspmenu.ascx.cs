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
using Lib.Elearn;
using Lib.Utils;
public partial class admin_showspmenu : System.Web.UI.UserControl
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    protected StringBuilder strMenu = new StringBuilder();
	private string AdminFolder = "Admin";
    protected string banner = "";
    protected string Fullname = "";
    private string UserPass = "";
    protected string IpAddress = "";
    private int ActUserId = 0;
	protected void Page_Load(object sender, EventArgs e)
	{
		string redirect = "";
		try
		{
			IpAddress = Request.UserHostAddress.ToString();
			ActUserId = (Session["ActUserId"] == null) ? 0 : Int32.Parse(Session["ActUserId"].ToString());
			if (ActUserId > 0)
			{
				string Url = Request.Url.ToString();
				string RelativeUrl = HtmlParser.Static_GetRelativeUrl(AdminFolder + "/", Url);
                Actions m_Actions = new Actions(ELEARN_CONSTR);
				m_Actions = m_Actions.GetByUrl(LogFilePath, LogFileName, ActUserId, RelativeUrl);
				if (m_Actions.ActionId > 0)
				{
					Fullname = (Session["FullName"] == null) ? "" : Session["FullName"].ToString().Trim();
                    strMenu = m_Actions.GenMenuNew(LogFilePath, LogFileName, ELEARN_CONSTR, ActUserId, MyConstants.PRJ_ROOT, MyConstants.ROOT_PATH, Fullname);
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