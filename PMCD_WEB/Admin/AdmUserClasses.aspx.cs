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

public partial class admin_pages_admin_elearn_AdmUserClasses : System.Web.UI.Page
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    protected List<Users> cboUsers = new List<Users>();
    protected List<Classes> cboClasses = new List<Classes>();
    protected List<Ranks> cboRanks = new List<Ranks>();
    protected List<UserClasses> cboUserClasses = new List<UserClasses>();
    private UserClasses m_UserClasses;
    protected Users m_Users;
    protected Classes m_Classes;
    protected Ranks m_Ranks;
    private int EditIndex;
    private int ActUserId = 0;
    private int ClassId = 0;
    private byte DistributedProcess = 0;
    protected string IpAddress = "";
    string SysMessageDesc = "";
    private IFormatProvider culture = new CultureInfo("fr-FR", true);
    //------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        string Redirect = "";
        try
        {
            IpAddress = Request.UserHostAddress;
            m_UserClasses = new UserClasses(ELEARN_CONSTR);
            m_Classes = new Classes(ELEARN_CONSTR);
            m_Users = new Users(ELEARN_CONSTR);
            m_Ranks = new Ranks(ELEARN_CONSTR);

            if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
            {
                if (ActUserId > 0)
                {
                    if (int.TryParse((Request["ClassId"] == null) ? "0" : Request["ClassId"].ToString().Trim(), out ClassId))
                    {
                        m_Classes = m_Classes.Get(LogFilePath, LogFileName, ClassId);
                        if (!IsPostBack)
                        {
                            if (m_Classes.ClassId > 0)
                            {
                                bindData(-1);
                            }
                            else
                            {
                                JSAlert.Alert("Không tìm thấy lớp học", this);
                            }

                        }
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
            if (int.TryParse((Request["ClassId"] == null) ? "0" : Request["ClassId"].ToString().Trim(), out ClassId))
            {
                int checkId = ClassId;
                cboUserClasses = m_UserClasses.GetList(LogFileName, LogFilePath, ClassId);
            }

            if (cboUsers.Count <= 0)
            {
                cboUsers = m_Users.GetList(LogFilePath, LogFileName);
            }
            if (cboClasses.Count <= 0)
            {
                cboClasses = m_Classes.GetList(LogFilePath, LogFileName);
            }
            if (cboRanks.Count <= 0)
            {
                cboRanks = m_Ranks.GetList(LogFilePath, LogFileName);
            }
            string SeachKeyword = txtSeachKeyword.Text.ToString();
            List<UserClasses> l_UserClasses = m_UserClasses.GetList(LogFilePath, LogFileName, ClassId, SeachKeyword);
            m_grid.EditIndex = index;
            bool NoRecord = (l_UserClasses.Count <= 0);
            if (NoRecord)
            {
                l_UserClasses.Add(new UserClasses(ELEARN_CONSTR));
            }
            m_grid.DataSource = l_UserClasses;
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
                        m_UserClasses = m_UserClasses.Get(l_UserClasses, Id);
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
    //------------------------------------------------------------------------
    protected void m_grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        EditIndex = e.NewEditIndex;
        bindData(EditIndex);
    }
    //------------------------------------------------------------------------
    protected void m_grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        bindData(-1);
    }
    //------------------------------------------------------------------------
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
                m_UserClasses = m_UserClasses.Get(LogFilePath, LogFileName, updateId);
                if (m_UserClasses.UserClassId > 0)
                {
                    m_UserClasses.UserId = Convert.ToByte(((DropDownList)row.FindControl("ddlUsers")).SelectedValue);
                    m_UserClasses.ClassId = Convert.ToInt32(((DropDownList)row.FindControl("ddlClasses")).SelectedValue);
                    m_UserClasses.UserClassRow = Convert.ToByte(((TextBox)row.FindControl("txtUserClassRow")).Text);
                    m_UserClasses.UserClassColumn = Convert.ToByte(((TextBox)row.FindControl("txtUserClassColumn")).Text);
                    m_UserClasses.RankId = Convert.ToByte(((DropDownList)row.FindControl("ddlRanks")).SelectedValue);
                    m_UserClasses.CrUserId = ActUserId;
                    m_UserClasses.CrDateTime = System.DateTime.Now;
                    if (m_UserClasses.Update(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
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
                    SysMessageDesc = "Không tìm thấy lớp học";
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
                m_UserClasses.UserId = Convert.ToByte(((DropDownList)row.FindControl("ddlInsertUsers")).SelectedValue);
                m_UserClasses.ClassId = ClassId;
                m_UserClasses.UserClassRow = Convert.ToByte(((TextBox)row.FindControl("txtInsertUserClassRow")).Text);
                m_UserClasses.UserClassColumn = Convert.ToByte(((TextBox)row.FindControl("txtInsertUserClassColumn")).Text);
                m_UserClasses.RankId = Convert.ToByte(((DropDownList)row.FindControl("ddlInsertRanks")).SelectedValue);
                m_UserClasses.CrUserId = ActUserId;
                m_UserClasses.CrDateTime = System.DateTime.Now;
                if (m_UserClasses.Insert(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
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
                    if (m_UserClasses.Delete(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId, delId))
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
