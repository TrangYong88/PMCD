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

public partial class admin_pages_admin_elearn_AdmQuestionCurriculums : System.Web.UI.Page
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    private QuestionCurriculums m_QuestionCurriculums;
    protected List<Questions> cboQuestions = new List<Questions>();
    protected List<QuestionCurriculums> cboQuestionCurriculums = new List<QuestionCurriculums>();
    protected List<QuestionLevels> cboQuestionLevels = new List<QuestionLevels>();
    protected List<QuestionTypes> cboQuestionTypes = new List<QuestionTypes>();
    protected Questions m_Questions;
    protected Curriculums m_Curriculums;
    protected QuestionLevels m_QuestionLevels;
    protected QuestionTypes m_QuestionTypes;
    private int ActUserId = 0;
    private int CurriculumId = 0;
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
            m_QuestionCurriculums = new QuestionCurriculums(ELEARN_CONSTR);
            m_Questions = new Questions(ELEARN_CONSTR);
            m_Curriculums = new Curriculums(ELEARN_CONSTR);
            m_QuestionLevels = new QuestionLevels(ELEARN_CONSTR);
            m_QuestionTypes = new QuestionTypes(ELEARN_CONSTR);
            if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
            {
                if (ActUserId > 0)
                {
                    if (int.TryParse((Request["CurriculumId"] == null) ? "0" : Request["CurriculumId"].ToString().Trim(), out CurriculumId))
                    {
                        if (!IsPostBack)
                        {
                            m_Curriculums = m_Curriculums.Get(LogFilePath, LogFileName, CurriculumId);
                            if (m_Curriculums.CurriculumId > 0)
                            {
                                cboQuestionLevels = m_QuestionLevels.GetList(LogFilePath, LogFileName);
                                cboSearchQuestionLevels.DataSource = m_QuestionLevels.CopyAll(cboQuestionLevels);
                                cboSearchQuestionLevels.DataBind();
                                cboQuestionTypes = m_QuestionTypes.GetList(LogFilePath, LogFileName);
                                cboSearchQuestionTypes.DataSource = m_QuestionTypes.CopyAll(cboQuestionTypes);
                                cboSearchQuestionTypes.DataBind();
                                bindData(-1);
                            }
                            else
                            {
                                JSAlert.Alert("Không tìm thấy chương trình", this);
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
            byte QuestionLevelId = 0;
            byte QuestionTypeId = 0;
            Byte.TryParse(cboSearchQuestionLevels.SelectedValue, out QuestionLevelId);
            Byte.TryParse(cboSearchQuestionTypes.SelectedValue, out QuestionTypeId);
            string SeachKeyword = txtSeachKeyword.Text.ToString();
            if (int.TryParse((Request["CurriculumId"] == null) ? "0" : Request["CurriculumId"].ToString().Trim(), out CurriculumId))
            {
                int checkId = CurriculumId;
                cboQuestionCurriculums = m_QuestionCurriculums.GetList(LogFileName, LogFilePath, CurriculumId, QuestionTypeId, QuestionLevelId, SeachKeyword);
            }
            if (cboQuestionLevels.Count <= 0)
            {
                cboQuestionLevels = m_QuestionLevels.GetList(LogFilePath, LogFileName);
            }
            if (cboQuestionTypes.Count <= 0)
            {
                cboQuestionTypes = m_QuestionTypes.GetList(LogFilePath, LogFileName);
            }
            List<Questions> l_Questions = m_Questions.GetList(LogFilePath, LogFileName, QuestionTypeId, QuestionLevelId, SeachKeyword);
            bool NoRecord = (l_Questions.Count <= 0);
            if (NoRecord)
            {
                l_Questions.Add(new Questions(ELEARN_CONSTR));
            }
            m_grid.DataSource = l_Questions;
            m_grid.DataBind();
            int a = CurriculumId;
            if (m_grid.Rows.Count > 0)
            {
                for (int i = 0; i < m_grid.Rows.Count; i++)
                {
                    int Id = Int32.Parse(m_grid.DataKeys[i].Value.ToString());
                    m_Questions = m_Questions.Get(l_Questions, Id);
                    CheckBox cb = (CheckBox)m_grid.Rows[i].Cells[0].FindControl("chkStatus");
                    for (int j = 0; j < cboQuestionCurriculums.Count; j++)
                    {
                        if (m_Questions.QuestionId == cboQuestionCurriculums[j].QuestionId)
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

        if (CurriculumId > 0)
        {
            GridViewRow row;
            List<QuestionCurriculums> l_QuestionCurriculums = m_QuestionCurriculums.GetListByCurriculumId(LogFilePath, LogFileName, CurriculumId);
            for (int i = 0; i < m_grid.Rows.Count; i++)
            {
                row = m_grid.Rows[i];
                byte QuestionId = Convert.ToByte(m_grid.DataKeys[i].Value.ToString());
                bool IsChecked = HtmlParser.CheckBoxIsChecked(row, "chkStatus");
                m_QuestionCurriculums = m_QuestionCurriculums.GetUnique(l_QuestionCurriculums, CurriculumId, QuestionId);
                if (m_QuestionCurriculums.QuestionCurriculumId > 0)
                {
                    if (!IsChecked)
                    {
                        m_QuestionCurriculums.Delete(LogFilePath, LogFileName, DistributedProcess, IpAddress, ActUserId, m_QuestionCurriculums.QuestionCurriculumId);
                    }
                }
                else
                {
                    if (IsChecked)
                    {
                        m_QuestionCurriculums.CurriculumId = CurriculumId;
                        m_QuestionCurriculums.QuestionId = QuestionId;
                        m_QuestionCurriculums.CrUserId = ActUserId;
                        m_QuestionCurriculums.CrDateTime = System.DateTime.Now;
                        m_QuestionCurriculums.Insert(LogFilePath, LogFileName, DistributedProcess, IpAddress, ActUserId);
                    }
                }
            }
            SysMessageDesc = "Cập nhật thành công";
            JSAlert.Alert(SysMessageDesc, this);
        }
    }
    //---------------------------------------------------------------------------------------
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(-1);
    }
}
