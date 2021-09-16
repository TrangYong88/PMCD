<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmUsers.aspx.cs" Inherits="admin_pages_admin_elearn_AdmUsers" Title="Danh sách người dùng" %>
<%@ Import Namespace="Lib.Utils" %>
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font>
  <table width="100%" align = "center">
    <tr>
      <td> 
        Từ khóa: <asp:TextBox  ID="txtSeachKeyword" runat="server" Text="" Columns="50" MaxLength="255" ></asp:TextBox> 
        Loại:<asp:DropDownList ID="cboSearchGenders" runat="server" DataTextField="GenderName"  DataValueField="GenderId"/>
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"  CausesValidation="false" /> 
      </td>
    </tr>
  </table>  
  <asp:GridView ID="m_grid" DataKeyNames="UserId" runat="server" CellPadding="4"
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
		    <asp:TemplateField HeaderText="Trạng thái">
				<ItemTemplate>
					<%# m_UserStatus.Get(cboUserStatus, Convert.ToByte(Eval("UserStatusId"))).UserStatusDesc%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlUserStatus" runat="server" DataSource='<%#cboUserStatus %>' DataTextField="UserStatusDesc"   DataValueField="UserStatusId" SelectedValue='<%#Bind("UserStatusId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertUserStatus" runat="server" DataSource='<%#cboUserStatus %>' DataTextField="UserStatusDesc" DataValueField="UserStatusId" SelectedValue='<%#Bind("UserStatusId") %>' />
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			 
			<asp:TemplateField HeaderText="Tên tài khoản">
				<ItemTemplate>
					<%#Eval("UserName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtUserName" runat="server" Width="100" Text='<%# Bind("UserName") %>' />
					<asp:RequiredFieldValidator ID="RequiredUserName" runat="server" ControlToValidate="txtUserName" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertUserName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertUserName" runat="server" ControlToValidate="txtInsertUserName" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Mật khẩu">
				<ItemTemplate>
					<%#Eval("UserPass")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtUserPass" runat="server" Width="100" Text='<%# Bind("UserPass") %>' />
					<asp:RequiredFieldValidator ID="RequiredUserPass" runat="server" ControlToValidate="txtUserPass" Display="Dynamic" ErrorMessage="Nhập mật khẩu" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertUserPass" runat="server" Width="100"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertUserPass" runat="server" ControlToValidate="txtInsertUserPass"		Display="Dynamic" ErrorMessage="Nhập mật khẩu" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Họ">
				<ItemTemplate>
					<%#Eval("FirstName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtFirstName" runat="server" Width="100" Text='<%# Bind("FirstName") %>' />
					<asp:RequiredFieldValidator ID="RequiredFirstName" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="Nhập họ người dùng" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertFirstName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertFirstName" runat="server" ControlToValidate="txtInsertFirstName" Display="Dynamic" ErrorMessage="Nhập họ người dùng" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Tên đệm">
				<ItemTemplate>
					<%#Eval("MiddleName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtMiddleName" runat="server" Width="100" Text='<%# Bind("MiddleName") %>' />
					<asp:RequiredFieldValidator ID="RequiredMiddleName" runat="server" ControlToValidate="txtMiddleName" Display="Dynamic" ErrorMessage="Nhập tên đệm người dùng" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertMiddleName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertMiddleName" runat="server" ControlToValidate="txtInsertMiddleName" Display="Dynamic" ErrorMessage="Nhập tên đệm người dùng" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Tên">
				<ItemTemplate>
					<%#Eval("LastName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtLastName" runat="server" Width="100" Text='<%# Bind("LastName") %>' />
					<asp:RequiredFieldValidator ID="RequiredLastName" runat="server" ControlToValidate="txtLastName" Display="Dynamic" ErrorMessage="Nhập tên người dùng" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertLastName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertLastName" runat="server" ControlToValidate="txtInsertLastName" Display="Dynamic" ErrorMessage="Nhập tên người dùng" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Ngày sinh">
				<ItemTemplate>
					<%#Eval("Birthday")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtBirthday" runat="server" Width="100" Text='<%# Bind("Birthday") %>' />
					<asp:RequiredFieldValidator ID="RequiredBirthday" runat="server" ControlToValidate="txtBirthday" Display="Dynamic" ErrorMessage="Nhập ngày sinh" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertBirthday" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertBirthday" runat="server" ControlToValidate="txtInsertBirthday" Display="Dynamic" ErrorMessage="Nhập ngày sinh" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Địa chỉ">
				<ItemTemplate>
					<%#Eval("Address")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtAddress" runat="server" Width="100" Text='<%# Bind("Address") %>' />
					<asp:RequiredFieldValidator ID="RequiredAddress" runat="server" ControlToValidate="txtAddress" Display="Dynamic" ErrorMessage="Nhập địa chỉ" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertAddress" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertAddress" runat="server" ControlToValidate="txtInsertAddress" Display="Dynamic" ErrorMessage="Nhập địa chỉ" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Giới tính">
				<ItemTemplate>
					<%# m_Genders.Get(cboGenders, Convert.ToByte(Eval("GenderId"))).GenderDesc%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlGenders" runat="server" DataSource='<%#cboGenders %>' DataTextField="GenderDesc"   DataValueField="GenderId" SelectedValue='<%#Bind("GenderId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertGenders" runat="server" DataSource='<%#cboGenders %>' DataTextField="GenderDesc" DataValueField="GenderId" SelectedValue='<%#Bind("GenderId") %>' />
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Thao tác">
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
			<asp:TemplateField HeaderText="Phân quyền">
				<ItemTemplate>
					<a href="AdmUserActions.aspx?UserId=<%#Eval("UserId")%>" title="Phân quyền thao tác">Phân quyền</a>
				</ItemTemplate>
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

