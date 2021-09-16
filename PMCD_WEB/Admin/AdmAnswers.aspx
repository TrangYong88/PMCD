<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmAnswers.aspx.cs" Inherits="admin_pages_admin_elearn_AdmAnswers" Title="Danh sách câu trả lời" %>
<%@ Import Namespace="Lib.Utils" %>
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font> 
  <asp:GridView ID="m_grid" DataKeyNames="AnswerId" runat="server" CellPadding="4"
		ForeColor="#333333"  ShowHeader="true" AutoGenerateColumns="False" Width="100%"
		OnRowDeleting="m_grid_RowDeleting" 
		OnRowEditing="m_grid_RowEditing"
		OnRowCancelingEdit="m_grid_RowCancelingEdit"
		OnRowUpdating="m_grid_RowUpdating" 
		ShowFooter="true" 
		OnRowCommand="m_grid_RowCommand" OnSelectedIndexChanged="m_grid_SelectedIndexChanged">
		<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
		<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
		<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
		<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
		<EditRowStyle BackColor="#999999" />
		<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
		<Columns>
			<asp:TemplateField HeaderText="Questions">
				<ItemTemplate>
					<%# m_Questions.Get(cboQuestions, Convert.ToByte(Eval("QuestionId"))).QuestionName%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlQuestions" runat="server" DataSource='<%#cboQuestions %>' DataTextField="QuestionName"   DataValueField="QuestionId" SelectedValue='<%#Bind("QuestionId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertQuestions" runat="server" DataSource='<%#cboQuestions %>' DataTextField="QuestionName" DataValueField="QuestionId" SelectedValue='<%#Bind("QuestionId") %>' />
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="AnswerPoint">
				<ItemTemplate>
					<%#Eval("AnswerPoint")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtAnswerPoint" runat="server" Width="100" Text='<%# Bind("AnswerPoint") %>' />
					<asp:RequiredFieldValidator ID="RequiredAnswerPoint" runat="server" ControlToValidate="txtAnswerPoint" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertAnswerPoint" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertAnswerPoint" runat="server" ControlToValidate="txtInsertAnswerPoint" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="AnswerContent">
				<ItemTemplate>
					<%#Eval("AnswerContent")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtAnswerContent" runat="server" Width="100" Text='<%# Bind("AnswerContent") %>' />
					<asp:RequiredFieldValidator ID="RequiredAnswerContent" runat="server" ControlToValidate="txtAnswerContent" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertAnswerContent" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertAnswerContent" runat="server" ControlToValidate="txtInsertAnswerContent" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField>
				<HeaderStyle Width="110px"></HeaderStyle>
				<ItemTemplate>
					<asp:LinkButton ID="cmdEdit" runat="server" CommandName="Edit" CausesValidation="false">Sửa</asp:LinkButton>&nbsp;
					<asp:LinkButton ID="cmdDelete" runat="server" CommandName="Delete" CausesValidation="false">Xoá</asp:LinkButton>
				</ItemTemplate>
				<FooterTemplate>
					<asp:LinkButton ID="cmdInsert" runat="server" CommandName="Insert">Thêm mới</asp:LinkButton>
				</FooterTemplate>
				<EditItemTemplate>
					<asp:LinkButton ID="cmdUpdate" runat="server" CommandName="Update" CausesValidation="false">Cập nhật</asp:LinkButton>&nbsp;
					<asp:LinkButton ID="cmdCancel" runat="server" CommandName="Cancel" CausesValidation="false">Thoát</asp:LinkButton>
				</EditItemTemplate>
			</asp:TemplateField>	
			
		</Columns>
  </asp:GridView>
  <table width="100%" >
    <tr>
      <td width = "70%">
        Số lượng:<%=m_grid.Rows.Count %> 
      </td>
    </tr>
  </table>
</asp:Content>

