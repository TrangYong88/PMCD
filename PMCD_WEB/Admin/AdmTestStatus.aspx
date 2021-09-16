<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmTestStatus.aspx.cs" Inherits="admin_pages_admin_elearn_AdmTestStatus" Title="Danh sách dạng trạng thái bài kiểm tra" %>
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
  <asp:GridView ID="m_grid" DataKeyNames="TestStatusId" runat="server" CellPadding="4"
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
			<asp:TemplateField HeaderText="Tên dạng trạng thái bài kiểm tra">
				<ItemTemplate>
					<%#Eval("TestStatusName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtTestStatusName" runat="server" Width="100" Text='<%# Bind("TestStatusName") %>' />
					<asp:RequiredFieldValidator ID="RequiredTestStatusName" runat="server" ControlToValidate="txtTestStatusName" Display="Dynamic" ErrorMessage="Nhập tên dạng trạng thái bài kiểm tra" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertTestStatusName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertTestStatusName" runat="server" ControlToValidate="txtInsertTestStatusName" Display="Dynamic" ErrorMessage="Nhập tên dạng trạng thái bài kiểm tra" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Mô tả trạng thái bài kiểm tra">
				<ItemTemplate>
					<%#Eval("TestStatusDesc")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtTestStatusDesc" runat="server" Width="100" Text='<%# Bind("TestStatusDesc") %>' />
					<asp:RequiredFieldValidator ID="RequiredTestStatusDesc" runat="server" ControlToValidate="txtTestStatusDesc" Display="Dynamic" ErrorMessage="Nhập Mô tả trạng thái bài kiểm tra" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertTestStatusDesc" runat="server" Width="100"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertTestStatusDesc" runat="server" ControlToValidate="txtInsertTestStatusDesc"		Display="Dynamic" ErrorMessage="Nhập Mô tả trạng thái bài kiểm tra" SetFocusOnError="True"></asp:RequiredFieldValidator>
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

