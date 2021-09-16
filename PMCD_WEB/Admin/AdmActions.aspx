<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmActions.aspx.cs" Inherits="admin_pages_admin_elearn_AdmActions" Title="Danh sách hành động" %>
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
  <asp:GridView ID="m_grid" DataKeyNames="ActionId" runat="server" CellPadding="4"
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
			<asp:TemplateField HeaderText=" Tên Hành động">
				<ItemTemplate>
					<%#Eval("ActionName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtActionName" runat="server" Width="100" Text='<%# Bind("ActionName") %>' />
					<asp:RequiredFieldValidator ID="RequiredActionName" runat="server" ControlToValidate="txtActionName" Display="Dynamic" ErrorMessage="Nhập tên Hành động" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertActionName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertActionName" runat="server" ControlToValidate="txtInsertActionName" Display="Dynamic" ErrorMessage="Nhập tên Hành động" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Mô tả hành động">
				<ItemTemplate>
					<%#Eval("ActionDesc")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtActionDesc" runat="server" Width="100" Text='<%# Bind("ActionDesc") %>' />
					<asp:RequiredFieldValidator ID="RequiredActionDesc" runat="server" ControlToValidate="txtActionDesc" Display="Dynamic" ErrorMessage="Nhập mô tả hành động" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertActionDesc" runat="server" Width="100"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertActionDesc" runat="server" ControlToValidate="txtInsertActionDesc"		Display="Dynamic" ErrorMessage="Nhập mô tả hành động" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Đường dẫn hành động">
				<ItemTemplate>
					<%#Eval("ActionUrl")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtActionUrl" runat="server" Width="100" Text='<%# Bind("ActionUrl") %>' />
					<asp:RequiredFieldValidator ID="RequiredActionUrl" runat="server" ControlToValidate="txtActionUrl" Display="Dynamic" ErrorMessage="Nhập đường dẫn hành động" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertActionUrl" runat="server" Width="100"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertActionUrl" runat="server" ControlToValidate="txtInsertActionUrl"		Display="Dynamic" ErrorMessage="Nhập đường dẫn hành động" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Mức độ hành động">
				<ItemTemplate>
					<%#Eval("ActionLevel")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtActionLevel" runat="server" Width="100" Text='<%# Bind("ActionLevel") %>' />
					<asp:RequiredFieldValidator ID="RequiredActionLevel" runat="server" ControlToValidate="txtActionLevel" Display="Dynamic" ErrorMessage="Nhập mức độ hành động" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertActionLevel" runat="server" Width="100"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertActionLevel" runat="server" ControlToValidate="txtInsertActionLevel"		Display="Dynamic" ErrorMessage="Nhập mức độ hành động" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Hành động trước">
				<ItemTemplate>
					<%# m_Actions.Get(cboActions, Convert.ToInt32(Eval("ParentActionId"))).ActionName%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlActions" runat="server" DataSource='<%#cboActions %>' DataTextField="ActionName"   DataValueField="ActionId" SelectedValue='<%#Bind("ActionId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertActions" runat="server" DataSource='<%#cboActions %>' DataTextField="ActionName" DataValueField="ActionId" SelectedValue='<%#Bind("ActionId") %>' />
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

