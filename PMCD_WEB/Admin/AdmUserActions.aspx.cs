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

public partial class admin_pages_admin_elearn_AdmUserActions : System.Web.UI.Page
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    private UserActions m_UserActions;
    protected List<Actions> cboActions = new List<Actions>();
    protected List<UserActions> cboUserActions = new List<UserActions>();
    protected Actions m_Actions;
    protected Users m_Users;
    private int ActUserId = 0;
    private int UserId = 0;
    protected string IpAddress = "";
    private byte DistributedProcess = 0;
    string SysMessageDesc = "";
    private IFormatProvider culture = new CultureInfo("fr-FR", true);
    protected void Page_Load(object sender, EventArgs e)
    {
        string Redirect = "";
        try
        {
            IpAddress = Request.UserHostAddress;
            m_UserActions = new UserActions(ELEARN_CONSTR);
            m_Actions = new Actions(ELEARN_CONSTR);
            m_Users = new Users(ELEARN_CONSTR);
            if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
            {
                if (ActUserId > 0)
                {
                    if (int.TryParse((Request["UserId"] == null) ? "0" : Request["UserId"].ToString().Trim(), out UserId))
                    {
                        if (!IsPostBack)
                        {
                            m_Users = m_Users.Get(LogFilePath, LogFileName, UserId);
                            if (m_Users.UserId > 0)
                            {
                                lblInfo.Text = m_Users.UserNameFullName;
                                bindData(-1);
                            }
                            else
                            {
                                JSAlert.Alert("Không tìm thấy người dùng", this);
                            }
                            
                        }
                    }
                    else
                    {
                        JSAlert.Alert("Chưa có định danh người dùng", this);
                    }
                }
                else
                {
                    Redirect = "/PMCD_WEB/Admin/Default.aspx";
                }
            }
            else
            {
                Redirect = "/PMCD_WEB/Admin/Default.aspx";
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
    //-------------------------------------------------------------------------------------------------
    private void bindData(int index)
    {
        try
        {
            if (int.TryParse((Request["UserId"] == null) ? "0" : Request["UserId"].ToString().Trim(), out UserId))
            {
                int checkId = UserId;
                cboUserActions = m_UserActions.GetList(LogFileName, LogFilePath, UserId);
            }

            if (cboActions.Count <= 0)
            {
                cboActions = m_Actions.GetList(LogFilePath, LogFileName);
            }
            m_grid.DataSource = cboActions;
            m_grid.DataBind();
            if (m_grid.Rows.Count > 0)
            {
                for (int i = 0; i < m_grid.Rows.Count; i++)
                {
                    int Id = Int32.Parse(m_grid.DataKeys[i].Value.ToString());
                    m_Actions = m_Actions.Get(cboActions, Id);
                    CheckBox cb = (CheckBox)m_grid.Rows[i].Cells[0].FindControl("chkStatus");
                    for (int j = 0; j < cboUserActions.Count; j++)
                    {
                        if (m_Actions.ActionId == cboUserActions[j].ActionId)
                        {
                            cb.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
        }
    }
    //---------------------------------------------------------------------------------------
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (UserId > 0)
        {
            GridViewRow row;
            List<UserActions> l_UserActions = m_UserActions.GetListByUserId(LogFilePath, LogFileName, UserId);
            for (int i = 0; i < m_grid.Rows.Count; i++)
            {
                row = m_grid.Rows[i];
                short ActionId = Convert.ToInt16(m_grid.DataKeys[i].Value.ToString());
                bool IsChecked = HtmlParser.CheckBoxIsChecked(row, "chkStatus");
                m_UserActions = m_UserActions.GetUnique(l_UserActions, UserId, ActionId);
                if (m_UserActions.UserActionId > 0)
                {
                    if (!IsChecked)
                    {
                        m_UserActions.Delete(LogFilePath, LogFileName, DistributedProcess, IpAddress, ActUserId, m_UserActions.UserActionId);
                    }
                }
                else
                {
                    if (IsChecked)
                    {
                        m_UserActions.UserId = UserId;
                        m_UserActions.ActionId = ActionId;
                        m_UserActions.CrUserId = ActUserId;
                        m_UserActions.CrDateTime = System.DateTime.Now;
                        m_UserActions.Insert(LogFilePath, LogFileName, DistributedProcess, IpAddress, ActUserId);
                    }
                }
            }
            SysMessageDesc = "Cập nhật thành công";
            JSAlert.Alert(SysMessageDesc, this);
        } 
 }        
}
