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

public partial class admin_pages_admin_elearn_AdmActions : System.Web.UI.Page
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    protected List<Actions> cboActions = new List<Actions>();
    protected Actions m_Actions;
    private int EditIndex;
    private int ActUserId = 0;
    protected string IpAddress = "";
    string SysMessageDesc = "";
    private IFormatProvider culture = new CultureInfo("fr-FR", true);
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
                        bindData(-1);
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
            if (cboActions.Count <= 0)
            {
                cboActions = m_Actions.GetList(LogFilePath, LogFileName);
            }
            string SeachKeyword = txtSeachKeyword.Text.ToString();
            List<Actions> l_Actions = m_Actions.GetList(LogFilePath, LogFileName, SeachKeyword);
            m_grid.EditIndex = index;
            bool NoRecord = (l_Actions.Count <= 0);
            if (NoRecord)
            {
                l_Actions.Add(new Actions(ELEARN_CONSTR));
            }
            m_grid.DataSource = l_Actions;
            m_grid.DataBind();
            if (m_grid.Rows.Count > 0)
            {
                if (NoRecord)
                {
                    m_grid.Rows[0].Enabled = false;
                }
                else
                {
                    string confirm = "return confirm('Bạn thực sự muốn xóa?')";
                    GridViewRow row;
                    LinkButton lbutton;
                    for (int i = 0; i < m_grid.Rows.Count; i++)
                    {
                        row = m_grid.Rows[i];
                        int Id = Int32.Parse(m_grid.DataKeys[i].Value.ToString());
                        m_Actions = m_Actions.Get(l_Actions, Id);
                        lbutton = (LinkButton)row.FindControl("cmdDelete");
                        if (lbutton != null)
                        {
                            lbutton.Attributes.Add("onclick", confirm);
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
    //-------------------------------------------------------------------------------------------------
    protected void m_grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        EditIndex = e.NewEditIndex;
        bindData(EditIndex);
    }

    protected void m_grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        bindData(-1);
    }
    //-------------------------------------------------------------------------------------------------
    protected void m_grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int id = e.RowIndex;
            m_grid.EditIndex = id;
            GridViewRow row = m_grid.Rows[id];
            int updateId = Int32.Parse(m_grid.DataKeys[id].Value.ToString());
            if (updateId > 0)
            {
                m_Actions = m_Actions.Get(LogFilePath, LogFileName, updateId);
                if (m_Actions.ActionId > 0)
                {
                    m_Actions.ActionName = ((TextBox)row.FindControl("txtActionName")).Text;
                    m_Actions.ActionDesc = ((TextBox)row.FindControl("txtActionDesc")).Text;
                    m_Actions.ActionUrl = ((TextBox)row.FindControl("txtActionUrl")).Text;
                    m_Actions.ActionLevel = Convert.ToByte(((TextBox)row.FindControl("txtActionLevel")).Text);
                    m_Actions.ParentActionId = Convert.ToInt32(((DropDownList)row.FindControl("ddlActions")).SelectedValue);
                    m_Actions.CrUserId = ActUserId;
                    m_Actions.CrDateTime = System.DateTime.Now;
                    if (m_Actions.Update(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
                    {
                        SysMessageDesc = "Cập nhật thành công";
                    }
                    else
                    {
                        SysMessageDesc = "Lỗi cập nhật";
                    }
                }
                else
                {
                    SysMessageDesc = "Không tìm thấy thao tác";
                }
                JSAlert.Alert(SysMessageDesc, this);
                bindData(-1);
            }
        }
        catch (Exception ex)
        {
            LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
        }
    }
    //-----------------------------------------------------------------------------------------------------
    protected void m_grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string commandName = e.CommandName;
            GridViewRow row = m_grid.FooterRow;
            if (commandName == "Insert")
            {
                m_Actions.ActionName = ((TextBox)row.FindControl("txtInsertActionName")).Text;
                m_Actions.ActionDesc = ((TextBox)row.FindControl("txtInsertActionDesc")).Text;
                m_Actions.ActionUrl = ((TextBox)row.FindControl("txtInsertActionUrl")).Text;
                m_Actions.ActionLevel = Convert.ToByte(((TextBox)row.FindControl("txtInsertActionLevel")).Text);
                m_Actions.ParentActionId = Convert.ToByte(((DropDownList)row.FindControl("ddlInsertActions")).Text);
                m_Actions.CrUserId = ActUserId;
                m_Actions.CrDateTime = System.DateTime.Now;
                if (m_Actions.Insert(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
                {
                    SysMessageDesc = "Đã thêm thành công";
                }
                else
                {
                    SysMessageDesc = "Lỗi thêm mới";
                }
                JSAlert.Alert(SysMessageDesc, this);
                bindData(-1);
            }
        }
        catch (Exception ex)
        {
            LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
        }
    }
    //--------------------------------------------------------------------------------------------
    protected void m_grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int delId = 0;
            if (Int32.TryParse(m_grid.DataKeys[e.RowIndex].Value.ToString(), out delId))
            {
                if (delId > 0)
                {
                    if (m_Actions.Delete(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId, delId))
                    {
                        SysMessageDesc = "Đã xóa thành công";
                    }
                    else
                    {
                        SysMessageDesc = "Lỗi xoá";
                    }
                    JSAlert.Alert(SysMessageDesc, this);
                }
            }
            bindData(-1);
        }
        catch (Exception ex)
        {
            LogFiles.WriteLog(ex.Message, LogFilePath + "\\Exception", LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
        }
    }
    //---------------------------------------------------------------------------------------
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(-1);
    }
    //-----------------------------------------------------------------------------------------
    protected void m_grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        m_grid.PageIndex = e.NewPageIndex;
        bindData(-1);
    }
    protected void m_grid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
