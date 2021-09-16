<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmUserClasses.aspx.cs" Inherits="admin_pages_admin_elearn_AdmUserClasses" Title="Danh sách sinh viên theo lớp" %>
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
  <asp:GridView ID="m_grid" DataKeyNames="UserClassId" runat="server" CellPadding="4"
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
			<asp:TemplateField HeaderText="Users">
				<ItemTemplate>
					<%# m_Users.Get(cboUsers, Convert.ToByte(Eval("UserId"))).FullName%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlUsers" runat="server" DataSource='<%#cboUsers %>' DataTextField="FullName"   DataValueField="UserId" SelectedValue='<%#Bind("UserId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertUsers" runat="server" DataSource='<%#cboUsers %>' DataTextField="FullName" DataValueField="UserId" SelectedValue='<%#Bind("UserId") %>' />
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Classes">
				<ItemTemplate>
					<%# m_Classes.Get(cboClasses, Convert.ToInt32(Eval("ClassId"))).ClassName%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlClasses" runat="server" DataSource='<%#cboClasses %>' DataTextField="ClassName"   DataValueField="ClassId" SelectedValue='<%#Bind("ClassId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertClasses" runat="server" DataSource='<%#cboClasses %>' DataTextField="ClassName" DataValueField="ClassId" SelectedValue='<%#Bind("ClassId") %>' />
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>	
			 
			<asp:TemplateField HeaderText="UserClassRow">
				<ItemTemplate>
					<%#Eval("UserClassRow")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtUserClassRow" runat="server" Width="100" Text='<%# Bind("UserClassRow") %>' />
					<asp:RequiredFieldValidator ID="RequiredUserClassRow" runat="server" ControlToValidate="txtUserClassRow" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertUserClassRow" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertUserClassRow" runat="server" ControlToValidate="txtInsertUserClassRow" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="UserClassColumn">
				<ItemTemplate>
					<%#Eval("UserClassColumn")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtUserClassColumn" runat="server" Width="100" Text='<%# Bind("UserClassColumn") %>' />
					<asp:RequiredFieldValidator ID="RequiredUserClassColumn" runat="server" ControlToValidate="txtUserClassColumn" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertUserClassColumn" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertUserClassColumn" runat="server" ControlToValidate="txtInsertUserClassColumn" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Ranks">
				<ItemTemplate>
					<%# m_Ranks.Get(cboRanks, Convert.ToByte(Eval("RankId"))).RankDesc%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlRanks" runat="server" DataSource='<%#cboRanks %>' DataTextField="RankDesc"   DataValueField="RankId" SelectedValue='<%#Bind("RankId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertRanks" runat="server" DataSource='<%#cboRanks %>' DataTextField="RankDesc" DataValueField="RankId" SelectedValue='<%#Bind("RankId") %>' />
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

