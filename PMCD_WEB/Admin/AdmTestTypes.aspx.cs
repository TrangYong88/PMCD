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

public partial class admin_pages_admin_elearn_AdmTestTypes : System.Web.UI.Page
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    protected List<TestTypes> cboTestTypes = new List<TestTypes>();
    private TestTypes m_TestTypes;
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
            m_TestTypes = new TestTypes(ELEARN_CONSTR);
            if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
            {
                if (ActUserId >= 0)
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
            if (cboTestTypes.Count <= 0)
            {
                cboTestTypes = m_TestTypes.GetList(LogFilePath, LogFileName);
            }
            string SeachKeyword = txtSeachKeyword.Text.ToString();
            List<TestTypes> l_TestTypes = m_TestTypes.GetList(LogFilePath, LogFileName, SeachKeyword);
            m_grid.EditIndex = index;
            bool NoRecord = (l_TestTypes.Count <= 0);
            if (NoRecord)
            {
                l_TestTypes.Add(new TestTypes(ELEARN_CONSTR));
            }
            m_grid.DataSource = l_TestTypes;
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
                        m_TestTypes = m_TestTypes.Get(l_TestTypes, Id);
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
    //-------------------------------------------------------------------------------------------------
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
                m_TestTypes = m_TestTypes.Get(LogFilePath, LogFileName, updateId);
                if (m_TestTypes.TestTypeId > 0)
                {
                    m_TestTypes.TestTypeName = ((TextBox)row.FindControl("txtTestTypeName")).Text;
                    m_TestTypes.TestTypeQuatityTime = Convert.ToInt32(((TextBox)row.FindControl("txtTestTypeQuatityTime")).Text);
                    if (m_TestTypes.Update(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
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
                    SysMessageDesc = "Không tìm thấy sách";
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
                m_TestTypes.TestTypeName = ((TextBox)row.FindControl("txtInsertTestTypeName")).Text;
                m_TestTypes.TestTypeQuatityTime = Convert.ToInt32(((TextBox)row.FindControl("txtInsertTestTypeQuatityTime")).Text);
                if (m_TestTypes.Insert(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
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
                    if (m_TestTypes.Delete(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId, delId))
                    {
                        SysMessageDesc = "Đã xóa thành công";
                    }
                    else
                    {
                        SysMessageDesc = "Lỗi xoá ABC";
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
    //---------------------------------------------------------------------------------------
    protected void btnManageForm_Click(object sender, EventArgs e)
    {
        Response.Redirect("/PMCD_WEB/Admin");
    }
    //---------------------------------------------------------------------------------------
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session.Remove("ActUserId");
        Response.Redirect("/PMCD_WEB/Admin/Default.aspx");
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
