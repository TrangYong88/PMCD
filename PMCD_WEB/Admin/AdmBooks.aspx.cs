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

public partial class admin_pages_admin_elearn_AdmBooks : System.Web.UI.Page
{
	protected string ELEARN_CONSTR = MyConstants.ELEARN_CONSTR;
	protected string LogFilePath = MyConstants.LogFilePath;
	protected string LogFileName = MyConstants.LogFileName;
	protected List<BookKinds> cboBookKinds = new List<BookKinds>();
	private Books m_Books;
	protected BookKinds m_BookKinds;
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
			m_Books = new Books(ELEARN_CONSTR);
			m_BookKinds = new BookKinds(ELEARN_CONSTR);
			
            if (Int32.TryParse((Session["ActUserId"] == null) ? "0" : ((string.IsNullOrEmpty(Session["ActUserId"].ToString().Trim())) ? "0" : Session["ActUserId"].ToString().Trim()), out ActUserId))
			{
				if (ActUserId >= 0)
				{
					if (!IsPostBack)
					{
						cboBookKinds = m_BookKinds.GetList(LogFilePath, LogFileName);
						cboSearchBookKinds.DataSource = m_BookKinds.CopyAll(cboBookKinds);
						cboSearchBookKinds.DataBind();
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
			if (cboBookKinds.Count <= 0)
			{
				cboBookKinds = m_BookKinds.GetList(LogFilePath, LogFileName);
			}
						
			byte BookKindId = 0;
			Byte.TryParse(cboSearchBookKinds.SelectedValue,out BookKindId);
            string SeachKeyword = txtSeachKeyword.Text.ToString();
            List<Books> l_Books = m_Books.GetList(LogFilePath, LogFileName, BookKindId, SeachKeyword);
			m_grid.EditIndex = index;
			bool NoRecord = (l_Books.Count <= 0);
			if (NoRecord)
			{
				l_Books.Add(new Books(ELEARN_CONSTR));
			}
			m_grid.DataSource = l_Books;
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
						m_Books = m_Books.Get(l_Books,Id);
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

	protected void m_grid_RowEditing(object sender, GridViewEditEventArgs e)
	{
		EditIndex = e.NewEditIndex;
		bindData(EditIndex);
	}

	protected void m_grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		bindData(-1);
	}

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
				m_Books = m_Books.Get(LogFilePath, LogFileName, updateId);
				if (m_Books.BookId > 0)
				{
					m_Books.BookName = ((TextBox)row.FindControl("txtBookName")).Text;
					m_Books.BookDesc = ((TextBox)row.FindControl("txtBookDesc")).Text;
					m_Books.BookKindId = Convert.ToByte(((DropDownList)row.FindControl("ddlBookKinds")).SelectedValue);
					m_Books.CrUserId = ActUserId;
					m_Books.CrDateTime = System.DateTime.Now;
					if (m_Books.Update(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
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
				m_Books.BookName = ((TextBox)row.FindControl("txtInsertBookName")).Text;
				m_Books.BookDesc = ((TextBox)row.FindControl("txtInsertBookDesc")).Text;
				m_Books.BookKindId = Convert.ToByte(((DropDownList)row.FindControl("ddlInsertBookKinds")).Text);
				m_Books.CrUserId = ActUserId;
				m_Books.CrDateTime = System.DateTime.Now;
				if (m_Books.Insert(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId))
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
                    if (m_Books.Delete(LogFilePath, LogFileName, MyConstants.DISTRIBUTED_PROCESS, IpAddress, ActUserId, delId))
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
    //
    protected void m_grid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
