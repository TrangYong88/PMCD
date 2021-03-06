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

public partial class admin_pages_admin_elearn_AdmUsers : System.Web.UI.Page
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    protected List<UserStatus> cboUserStatus = new List<UserStatus>();
    protected List<Genders> cboGenders = new List<Genders>();
    private Users m_Users;
    protected UserStatus m_UserStatus;
    protected Genders m_Genders;
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
            m_Users = new Users(ELEARN_CONSTR);
            m_UserStatus = new UserStatus(ELEARN_CONSTR);
            m_Genders = new Genders(ELEARN_CONSTR);

            if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
            {
                if (ActUserId > 0)
                {
                    if (!IsPostBack)
                    {
                        cboGenders = m_Genders.GetList(LogFilePath, LogFileName);
                        cboSearchGenders.DataSource = m_Genders.CopyAll(cboGenders);
                        cboSearchGenders.DataBind();
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
            if (cboGenders.Count <= 0)
            {
                cboGenders = m_Genders.GetList(LogFilePath, LogFileName);
            }
            if (cboUserStatus.Count <= 0)
            {
                cboUserStatus = m_UserStatus.GetList(LogFilePath, LogFileName);
            }
            byte GenderId = 0;
            Byte.TryParse(cboSearchGenders.SelectedValue, out GenderId);
            string SeachKeyword = txtSeachKeyword.Text.ToString();
            List<Users> l_Users = m_Users.GetList(LogFilePath, LogFileName, GenderId, SeachKeyword);
            m_grid.EditIndex = index;
            bool NoRecord = (l_Users.Count <= 0);
            if (NoRecord)
            {
                l_Users.Add(new Users(ELEARN_CONSTR));
            }
            m_grid.DataSource = l_Users;
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
                        m_Users = m_Users.Get(l_Users, Id);
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
    //-----------------------------------------------------------------------------------------
    protected void m_grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        EditIndex = e.NewEditIndex;
        bindData(EditIndex);
    }
    //-----------------------------------------------------------------------------------------
    protected void m_grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        bindData(-1);
    }
    //-----------------------------------------------------------------------------------------
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
                m_Users = m_Users.Get(LogFilePath, LogFileName, updateId);
                if (m_Users.UserStatusId > 0)
                {
                    m_Users.UserStatusId = Convert.ToByte(((DropDownList)row.FindControl("ddlUserStatus")).SelectedValue);
                    m_Users.UserName = ((TextBox)row.FindControl("txtUserName")).Text;
                    m_Users.UserPass = ((TextBox)row.FindControl("txtUserPass")).Text;
                    m_Users.FirstName = ((TextBox)row.FindControl("txtFirstName")).Text;
                    m_Users.MiddleName = ((TextBox)row.FindControl("txtMiddleName")).Text;
                    m_Users.LastName = ((TextBox)row.FindControl("txtLastName")).Text;
                    m_Users.Birthday = Convert.ToDateTime(((TextBox)row.FindControl("txtBirthday")).Text);
                    m_Users.Address = ((TextBox)row.FindControl("txtAddress")).Text;
                    m_Users.GenderId = Convert.ToByte(((DropDownList)row.FindControl("ddlGenders")).SelectedValue);
                    m_Users.CrUserId = ActUserId;
                    m_Users.CrDateTime = System.DateTime.Now;
                    m_Users.Update(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId);
                    
                }
                else
                {
                    SysMessageDesc = "Không tìm thấy người dùng";
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
                m_Users.UserStatusId = Convert.ToByte(((DropDownList)row.FindControl("ddlInsertUserStatus")).Text);
                m_Users.UserName = ((TextBox)row.FindControl("txtInsertUserName")).Text;
                m_Users.UserPass = ((TextBox)row.FindControl("txtInsertUserPass")).Text;
                m_Users.FirstName = ((TextBox)row.FindControl("txtInsertFirstName")).Text;
                m_Users.MiddleName = ((TextBox)row.FindControl("txtInsertMiddleName")).Text;
                m_Users.LastName = ((TextBox)row.FindControl("txtInsertLastName")).Text;
                m_Users.Birthday = Convert.ToDateTime(((TextBox)row.FindControl("txtInsertBirthday")).Text);
                m_Users.Address = ((TextBox)row.FindControl("txtInsertAddress")).Text;
                m_Users.GenderId = Convert.ToByte(((DropDownList)row.FindControl("ddlInsertGenders")).Text);
                m_Users.CrUserId = ActUserId;
                m_Users.CrDateTime = System.DateTime.Now;
                if (m_Users.Insert(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
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
                    if (m_Users.Delete(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId, delId))
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
    //-----------------------------------------------------------------------------------------
    protected void m_grid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
