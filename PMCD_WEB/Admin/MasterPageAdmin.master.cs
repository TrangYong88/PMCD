using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Windows.Forms;
using Lib.Utils;
public partial class MasterPageAdmin : System.Web.UI.MasterPage
{
    protected string ROOT_PATH = MyConstants.ROOT_PATH;
    protected string MAIL_USER = "abc@gmail.com";
    protected string OFFICE_ADDRESS = "Hà Nội";
    protected string PHONE_USER = "";
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
	protected void Page_Load(object sender, EventArgs e)
	{
        string redirect = "";
        try
        {
            string UserName = (Session["UserName"] == null) ? "" : Session["UserName"].ToString().Trim();
            if (string.IsNullOrEmpty(UserName))
            {
                redirect = MyConstants.PRJ_ROOT + "Default.aspx";
            }
            else
            { 
                    int ActUserId = 0;
                    if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
                    {
                        if (ActUserId <= 0)
                        {

                            redirect = MyConstants.PRJ_ROOT + "Default.aspx";
                        }
                    }
                    else
                    {
                        redirect = MyConstants.PRJ_ROOT + "Default.aspx";
                    }
            }
        }
        catch (Exception ex)
        {
            LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
        }
        if (!string.IsNullOrEmpty(redirect))
        {
            Response.Redirect(redirect);
        }
	}
}
