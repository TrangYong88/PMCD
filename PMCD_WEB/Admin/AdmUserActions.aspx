<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmUserActions.aspx.cs" Inherits="admin_pages_admin_elearn_AdmUserActions" Title="Danh sách hành động" %>
<%@ Import Namespace="Lib.Utils" %>                                                                                             
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font>
  <font face="Arial" size="2" color="red"><b><asp:Label ID="lblInfo" runat="server"/></b></font> 
  <table width="100%" align = "center">
    <tr>
      <td> 
        <asp:Button ID="Button1" runat="server" Text="Xác nhận" OnClick="btnSubmit_Click"  CausesValidation="false" /> 
      </td>
    </tr>
  </table>
  <asp:GridView ID="m_grid" DataKeyNames="ActionId" runat="server" CellPadding="4"
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
			<asp:TemplateField HeaderText="Tên hành động người dùng">
				<ItemTemplate>
					<%# m_Actions.Get(cboActions, Convert.ToInt32(Eval("ActionId"))).ActionName%>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Mô tả hành động người dùng">
				<ItemTemplate>
					<%# m_Actions.Get(cboActions, Convert.ToInt32(Eval("ActionId"))).ActionDesc%>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Đường dẫn hành động người dùng">
				<ItemTemplate>
					<%# m_Actions.Get(cboActions, Convert.ToInt32(Eval("ActionId"))).ActionUrl%>
				</ItemTemplate>
			</asp:TemplateField>	
			
			<asp:TemplateField HeaderText="Mức độ hành động người dùng">
				<ItemTemplate>
					<%# m_Actions.Get(cboActions, Convert.ToInt32(Eval("ActionId"))).ActionLevel%>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Hành động trước">
				<ItemTemplate>
					<%# m_Actions.Get(cboActions, (m_Actions.Get(cboActions, Convert.ToInt32(Eval("ActionId"))).ParentActionId)).ActionName%>
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


