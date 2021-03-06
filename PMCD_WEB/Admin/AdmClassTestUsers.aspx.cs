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

public partial class admin_pages_admin_elearn_AdmClassTestUsers : System.Web.UI.Page
{
    protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
    protected string LogFilePath = MyConstants.LogFilePath;
    protected string LogFileName = MyConstants.LogFileName;
    private ClassTestUsers m_ClassTestUsers;
    protected List<UserClasses> cboUserClasses = new List<UserClasses>();
    protected List<ClassTestUsers> cboClassTestUsers = new List<ClassTestUsers>();
    protected List<Users> cboUsers = new List<Users>();
    protected UserClasses m_UserClasses;
    protected ClassTests m_ClassTests;
    protected Users m_Users;
    private int ActUserId = 0;
    private int ClassTestId = 0;
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
            m_ClassTestUsers = new ClassTestUsers(ELEARN_CONSTR);
            m_UserClasses = new UserClasses(ELEARN_CONSTR);
            m_ClassTests = new ClassTests(ELEARN_CONSTR);
            m_Users = new Users(ELEARN_CONSTR);
            if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
            {
                if (ActUserId > 0)
                {
                    if (int.TryParse((Request["ClassTestId"] == null) ? "0" : Request["ClassTestId"].ToString().Trim(), out ClassTestId))
                    {
                        if (!IsPostBack)
                        {
                            m_ClassTests = m_ClassTests.Get(LogFilePath, LogFileName, ClassTestId);
                            if (m_ClassTests.ClassTestId > 0)
                            {
                                bindData(-1);
                            }
                            else
                            {
                                JSAlert.Alert("Không tìm thấy lớp học", this);
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
            if (int.TryParse((Request["ClassTestId"] == null) ? "0" : Request["ClassTestId"].ToString().Trim(), out ClassTestId))
            {
                int checkId = ClassTestId;
                cboClassTestUsers = m_ClassTestUsers.GetList(LogFileName, LogFilePath, ClassTestId);
            }
            if (cboUsers.Count <= 0)
            {
                cboUsers = m_Users.GetList(LogFilePath, LogFileName);
            }
            string TextUserClassRow = txtUserClassRow.Text.ToString();
            string TextUserClassColumn = txtUserClassColumn.Text.ToString();
            byte UserClassRow = 0;
            byte UserClassColumn = 0;
            if (!string.IsNullOrEmpty(TextUserClassRow))
            {
                UserClassRow = Convert.ToByte(txtUserClassRow.Text.ToString());
            }
            if (!string.IsNullOrEmpty(TextUserClassColumn))
            {
                UserClassColumn = Convert.ToByte(txtUserClassColumn.Text.ToString());
            }
            List<UserClasses> l_UserClasses = m_UserClasses.GetList(LogFilePath, LogFileName, ClassTestId, UserClassRow, UserClassColumn);
            bool NoRecord = (l_UserClasses.Count <= 0);
            if (NoRecord)
            {
                l_UserClasses.Add(new UserClasses(ELEARN_CONSTR));
            }
            m_grid.DataSource = l_UserClasses;
            m_grid.DataBind();
            if (m_grid.Rows.Count > 0)
            {
                for (int i = 0; i < m_grid.Rows.Count; i++)
                {
                    int Id = Int32.Parse(m_grid.DataKeys[i].Value.ToString());
                    m_UserClasses = m_UserClasses.Get(l_UserClasses, Id);
                    CheckBox cb = (CheckBox)m_grid.Rows[i].Cells[0].FindControl("chkStatus");
                    for (int j = 0; j < cboClassTestUsers.Count; j++)
                    {
                        if (m_UserClasses.UserClassId == cboClassTestUsers[j].UserClassId)
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

        if (ClassTestId > 0)
        {
            GridViewRow row;
            List<ClassTestUsers> l_ClassTestUsers = m_ClassTestUsers.GetListByClassTestId(LogFilePath, LogFileName, ClassTestId);
            for (int i = 0; i < m_grid.Rows.Count; i++)
            {
                row = m_grid.Rows[i];
                int UserClassId = Convert.ToInt32(m_grid.DataKeys[i].Value.ToString());
                bool IsChecked = HtmlParser.CheckBoxIsChecked(row, "chkStatus");
                m_ClassTestUsers = m_ClassTestUsers.GetUnique(l_ClassTestUsers, ClassTestId, UserClassId);
                if (m_ClassTestUsers.ClassTestUserId > 0)
                {
                    if (!IsChecked)
                    {
                        m_ClassTestUsers.Delete(LogFilePath, LogFileName, DistributedProcess, IpAddress, ActUserId, m_ClassTestUsers.ClassTestUserId);
                    }
                }
                else
                {
                    if (IsChecked)
                    {
                        m_ClassTestUsers.ClassTestId = ClassTestId;
                        m_ClassTestUsers.UserClassId = UserClassId;
                        m_ClassTestUsers.CrUserId = ActUserId;
                        m_ClassTestUsers.CrDateTime = System.DateTime.Now;
                        m_ClassTestUsers.Insert(LogFilePath, LogFileName, DistributedProcess, IpAddress, ActUserId);
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
