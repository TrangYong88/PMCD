using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Reflection;
using Lib.Elearn;
using Lib.Utils;

public partial class Admin_MainForm : System.Web.UI.Page
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    protected Actions m_Actions;
    private int ActUserId = 0;
    protected string IpAddress = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string Redirect = "";
        try
        {
            IpAddress = Request.UserHostAddress;
            m_Actions = new Actions(ELEARN_CONSTR);
            if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
            {
                if (ActUserId > 0)
                {
                    if (!IsPostBack)
                    {
                        List<Actions> l_Actions = m_Actions.GetListActionByUserId(LogFilePath, LogFileName, ActUserId);
                        for (int i = 0; i < l_Actions.Count; i++)
                        {
                            if (l_Actions[i].ActionName.Equals("Login"))
                            {
                                l_Actions.RemoveAt(i);
                            }
                        }
                        m_grid.DataSource = l_Actions;
                        m_grid.DataBind();
                    }
                }
                else
                {
                    Redirect = "/Code1/Admin/Default.aspx";
                }
            }
            else
            {
                Redirect = "/Code1/Admin/Default.aspx";
            }
        }
        catch (Exception ex)
        {
            LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
        }
        if (!string.IsNullOrEmpty(Redirect))
        {
            Response.Redirect(Redirect);
        }
    }
    //---------------------------------------------------------------------------------------
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session.Remove("ActUserId");
        Response.Redirect("/Code1/Admin/Default.aspx");
    }
    //---------------------------------------------------------------------------------------
    protected void btnManageForm_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Code1/Admin/MainForm.aspx");
    }
}
