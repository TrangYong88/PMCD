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

public partial class admin_pages_admin_elearn_AdmQuestions : System.Web.UI.Page
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName; 
    protected List<QuestionLevels> cboQuestionLevels = new List<QuestionLevels>();
    protected List<QuestionTypes> cboQuestionTypes = new List<QuestionTypes>();
    private Questions m_Questions;  
    protected QuestionLevels m_QuestionLevels;
    protected QuestionTypes m_QuestionTypes;
    private int EditIndex;
    private int ActUserId = 0;
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
            m_Questions = new Questions(ELEARN_CONSTR);
            m_QuestionTypes = new QuestionTypes(ELEARN_CONSTR);
            m_QuestionLevels = new QuestionLevels(ELEARN_CONSTR);

            if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
            {
                if (ActUserId > 0)
                {
                    if (!IsPostBack)
                    {   
                        cboQuestionLevels = m_QuestionLevels.GetList(LogFilePath, LogFileName);
                        cboSearchQuestionLevels.DataSource = m_QuestionLevels.CopyAll(cboQuestionLevels);
                        cboSearchQuestionLevels.DataBind();
                        cboQuestionTypes = m_QuestionTypes.GetList(LogFilePath, LogFileName);
                        cboSearchQuestionTypes.DataSource = m_QuestionTypes.CopyAll(cboQuestionTypes);
                        cboSearchQuestionTypes.DataBind();
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
            if (cboQuestionLevels.Count <= 0)
            {
                cboQuestionLevels = m_QuestionLevels.GetList(LogFilePath, LogFileName);
            }
            if (cboQuestionTypes.Count <= 0)
            {
                cboQuestionTypes = m_QuestionTypes.GetList(LogFilePath, LogFileName);
            }
            byte QuestionLevelId = 0;
            byte QuestionTypeId = 0;
            Byte.TryParse(cboSearchQuestionLevels.SelectedValue, out QuestionLevelId);
            Byte.TryParse(cboSearchQuestionTypes.SelectedValue, out QuestionTypeId);
            string SeachKeyword = txtSeachKeyword.Text.ToString();
            List<Questions> l_Questions = m_Questions.GetList(LogFilePath, LogFileName,QuestionTypeId, QuestionLevelId, SeachKeyword);
            m_grid.EditIndex = index;
            bool NoRecord = (l_Questions.Count <= 0);
            if (NoRecord)
            {
                l_Questions.Add(new Questions(ELEARN_CONSTR));
            }
            m_grid.DataSource = l_Questions;
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
                        m_Questions = m_Questions.Get(l_Questions, Id);
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
                m_Questions = m_Questions.Get(LogFilePath, LogFileName, updateId);
                if (m_Questions.QuestionId > 0)
                {
                    m_Questions.QuestionTypeId = Convert.ToByte(((DropDownList)row.FindControl("ddlQuestionTypes")).SelectedValue);
                    m_Questions.QuestionLevelId = Convert.ToByte(((DropDownList)row.FindControl("ddlQuestionLevels")).SelectedValue);
                    m_Questions.QuestionName = ((TextBox)row.FindControl("txtQuestionName")).Text;
                    m_Questions.QuestionDesc = ((TextBox)row.FindControl("txtQuestionDesc")).Text;
                    m_Questions.QuestionMaxTime = Convert.ToByte(((TextBox)row.FindControl("txtQuestionMaxTime")).Text);
                    m_Questions.CrUserId = ActUserId;
                    m_Questions.CrDateTime = System.DateTime.Now;
                    if (m_Questions.Update(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
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
                    SysMessageDesc = "Không tìm thấy câu hỏi";
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
                m_Questions.QuestionTypeId = Convert.ToByte(((DropDownList)row.FindControl("ddlInsertQuestionTypes")).Text);
                m_Questions.QuestionLevelId = Convert.ToByte(((DropDownList)row.FindControl("ddlInsertQuestionLevels")).Text);
                m_Questions.QuestionName = ((TextBox)row.FindControl("txtInsertQuestionName")).Text;
                m_Questions.QuestionDesc = ((TextBox)row.FindControl("txtInsertQuestionDesc")).Text;
                m_Questions.QuestionMaxTime = Convert.ToByte(((TextBox)row.FindControl("txtInsertQuestionMaxTime")).Text);
                m_Questions.CrUserId = ActUserId;
                m_Questions.CrDateTime = System.DateTime.Now;
                if (m_Questions.Insert(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
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
                    if (m_Questions.Delete(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId, delId))
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
    //------------------------------------------------------------------------
    protected void m_grid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
