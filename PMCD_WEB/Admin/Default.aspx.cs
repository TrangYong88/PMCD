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

public partial class _Default : System.Web.UI.Page 
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    protected void Page_Load(object sender, EventArgs e)
    {  
    }
    //-----------------------------------------------------------------------------------------
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Users m_Users = new Users(ELEARN_CONSTR);
        Actions m_Actions = new Actions(ELEARN_CONSTR);
        string UserName = Convert.ToString(ipUserName.Value);
        string UserPass = Convert.ToString(ipUserPass.Value);
        m_Users = m_Users.GetByUserName(LogFilePath, LogFileName, UserName);
         if (m_Users.UserId > 0)
            {
                if (m_Users.UserPass == UserPass)
                {
                    Session["ActUserId"] = m_Users.UserId.ToString();
                    Session["UserName"] = m_Users.UserName;
                    Session["FullName"] = m_Users.FullName;
                    if ((m_Actions.GetList(LogFilePath, LogFileName, m_Users.UserId)).Count > 0)
                    {
                        Response.Redirect("/Code1/Admin/AdmActions.aspx");
                    }
                    else
                    {
                        Response.Redirect("/Code1/Admin/PrivatePage.aspx");
                    }
                }
                else
                {
                    JSAlert.Alert("UserName or UserPass not true", this);
                }
            }
            else
            {
                JSAlert.Alert("UserName or UserPass not true", this);
            } 
    }
}
