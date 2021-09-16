<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmQuestionLevels.aspx.cs" Inherits="admin_pages_admin_elearn_AdmQuestionLevels" Title="Danh sách dạng câu hỏi" %>
<%@ Import Namespace="Lib.Utils" %>                                                                                             
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font>
  <table width="100%" align = "center">
    <tr>
      <td> 
        Từ khóa: <asp:TextBox  ID="txtSeachKeyword" runat="server" Text="" Columns="50" MaxLength="255" ></asp:TextBox> 
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"  CausesValidation="false" /> 
      </td>
    </tr>
  </table>  
  <asp:GridView ID="m_grid" DataKeyNames="QuestionLevelId" runat="server" CellPadding="4"
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
			<asp:TemplateField HeaderText="Tên dạng câu hỏi">
				<ItemTemplate>
					<%#Eval("QuestionLevelName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtQuestionLevelName" runat="server" Width="100" Text='<%# Bind("QuestionLevelName") %>' />
					<asp:RequiredFieldValidator ID="RequiredQuestionLevelName" runat="server" ControlToValidate="txtQuestionLevelName" Display="Dynamic" ErrorMessage="Nhập tên dạng câu hỏi" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertQuestionLevelName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertQuestionLevelName" runat="server" ControlToValidate="txtInsertQuestionLevelName" Display="Dynamic" ErrorMessage="Nhập tên dạng câu hỏi" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Mô tả dạng câu hỏi">
				<ItemTemplate>
					<%#Eval("QuestionLevelDesc")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtQuestionLevelDesc" runat="server" Width="100" Text='<%# Bind("QuestionLevelDesc") %>' />
					<asp:RequiredFieldValidator ID="RequiredQuestionLevelDesc" runat="server" ControlToValidate="txtQuestionLevelDesc" Display="Dynamic" ErrorMessage="Nhập mô tả dạng câu hỏi" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertQuestionLevelDesc" runat="server" Width="100"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertQuestionLevelDesc" runat="server" ControlToValidate="txtInsertQuestionLevelDesc"		Display="Dynamic" ErrorMessage="Nhập mô tả dạng câu hỏi" SetFocusOnError="True"></asp:RequiredFieldValidator>
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
      <td width = "80%">
        Số lượng:<%=m_grid.Rows.Count %> 
      </td>
    </tr>
  </table>
</asp:Content>

