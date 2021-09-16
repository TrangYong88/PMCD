<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmBooks.aspx.cs" Inherits="admin_pages_admin_elearn_AdmBooks" Title="Danh sách học liệu" %>
<%@ Import Namespace="Lib.Utils" %>
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font>
  <table width="100%" align = "center">
    <tr>
      <td> 
        Từ khóa: <asp:TextBox  ID="txtSeachKeyword" runat="server" Text="" Columns="50" MaxLength="255" ></asp:TextBox> 
        Loại:<asp:DropDownList ID="cboSearchBookKinds" runat="server" DataTextField="BookKindDesc"  DataValueField="BookKindId"/>
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"  CausesValidation="false" /> 
      </td>
    </tr>
  </table>  
  <asp:GridView ID="m_grid" DataKeyNames="BookId" runat="server" CellPadding="4"
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
			<asp:TemplateField HeaderText="Tên học liệu">
				<ItemTemplate>
					<%#Eval("BookName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtBookName" runat="server" Width="100" Text='<%# Bind("BookName") %>' />
					<asp:RequiredFieldValidator ID="RequiredBookName" runat="server" ControlToValidate="txtBookName" Display="Dynamic" ErrorMessage="Nhập tên học liệu" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertBookName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertBookName" runat="server" ControlToValidate="txtInsertBookName" Display="Dynamic" ErrorMessage="Nhập tên học liệu" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Mô tả">
				<ItemTemplate>
					<%#Eval("BookDesc")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtBookDesc" runat="server" Width="100" Text='<%# Bind("BookDesc") %>' />
					<asp:RequiredFieldValidator ID="RequiredBookDesc" runat="server" ControlToValidate="txtBookDesc" Display="Dynamic" ErrorMessage="Nhập mô tả học liệu" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertBookDesc" runat="server" Width="100"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertBookDesc" runat="server" ControlToValidate="txtInsertBookDesc"		Display="Dynamic" ErrorMessage="Nhập mô tả học liệu" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Loại tài liệu">
				<ItemTemplate>
					<%# m_BookKinds.Get(cboBookKinds,Convert.ToByte(Eval("BookKindId"))).BookKindDesc%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlBookKinds" runat="server" DataSource='<%#cboBookKinds %>' DataTextField="BookKindDesc"   DataValueField="BookKindId" SelectedValue='<%#Bind("BookKindId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertBookKinds" runat="server" DataSource='<%#cboBookKinds %>' DataTextField="BookKindDesc" DataValueField="BookKindId" SelectedValue='<%#Bind("BookKindId") %>' />
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
      <td width = "90%">
        Số lượng:<%=m_grid.Rows.Count %> 
      </td>
      <td> 
        <asp:Button ID="btnManageForm" runat="server" Text="ManageForm" OnClick="btnManageForm_Click"  CausesValidation="false"/> 
      </td>
    </tr>
  </table>
</asp:Content>

