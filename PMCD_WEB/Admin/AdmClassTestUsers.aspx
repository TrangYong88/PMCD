<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmClassTestUsers.aspx.cs" Inherits="admin_pages_admin_elearn_AdmClassTestUsers" Title="Danh sách sinh viên" %>
<%@ Import Namespace="Lib.Utils" %>                                                                                             
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font>
  <font face="Arial" size="2" color="red"><b><asp:Label ID="lblInfo" runat="server"/></b></font> 
  <table width="100%" align = "center">
    <tr>
      <td> 
        Hàng: <asp:TextBox  ID="txtUserClassRow" runat="server" Text="" Columns="50" MaxLength="2" Width="35px" ></asp:TextBox>
        Cột: <asp:TextBox  ID="txtUserClassColumn" runat="server" Text="" Columns="50" MaxLength="2" Width="35px" ></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"  CausesValidation="false" />
        <asp:Button ID="Button1" runat="server" Text="Xác nhận" OnClick="btnSubmit_Click"  CausesValidation="false" /> 
      </td>
    </tr>
  </table>
  <asp:GridView ID="m_grid" DataKeyNames="UserClassId" runat="server" CellPadding="4"
		ForeColor="#333333"  ShowHeader="true" AutoGenerateColumns="False" Width="100%"
		ShowFooter="true" >
		<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
		<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
		<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
		<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
		<EditRowStyle BackColor="#999999" />
		<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
		<Columns>
		
		    <asp:TemplateField HeaderText="Chọn">
				<ItemTemplate>
					<asp:CheckBox ID="chkStatus" runat="server"   />
				</ItemTemplate>
			</asp:TemplateField>
			
		    <asp:TemplateField HeaderText="Hàng">
				<ItemTemplate>
					<%#Eval("UserClassRow")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtUserClassRow" runat="server" Width="100" Text='<%# Bind("UserClassRow") %>' />
					<asp:RequiredFieldValidator ID="RequiredUserClassRow" runat="server" ControlToValidate="txtUserClassRow" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Cột">
				<ItemTemplate>
					<%#Eval("UserClassColumn")%>
				</ItemTemplate>
			</asp:TemplateField>
			
		    <asp:TemplateField HeaderText="Họ và tên">
				<ItemTemplate>
					<%# m_Users.Get(cboUsers, Convert.ToInt32(Eval("UserId"))).FullName%>
				</ItemTemplate>
			</asp:TemplateField>
			
		</Columns>
  </asp:GridView>
  <table width="100%" align = "center">
    <tr>
      <td>
        Số lượng:<%=m_grid.Rows.Count %> 
      </td>
    </tr>
  </table>
</asp:Content>


