<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="MainForm.aspx.cs" Inherits="Admin_MainForm" Title="Danh sách chức năng" %>
<%@ Import Namespace="Lib.Utils" %>                                                                                             
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font> 
  <table width="100%" >
    <tr>
      <td> 
        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click"  CausesValidation="false"/>
      </td>
    </tr>
  </table>
  <asp:GridView ID="m_grid" DataKeyNames="ActionId" runat="server" CellPadding="4"
		ForeColor="#333333"  ShowHeader="true" AutoGenerateColumns="False" Width="100%"
		ShowFooter="true" >
		<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
		<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
		<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
		<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
		<Columns>
			<asp:TemplateField HeaderText="Tên chức năng">
				<ItemTemplate>
					<%#Eval("ActionName")%>
				</ItemTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Thực hiện chức năng">
				<ItemTemplate>
					<a id="FunctionName" href="<%#Eval("ActionUrl")%>" runat="server" ForeColor="#5D7B9D" style="text-decoration: none"><%#Eval("ActionName")%></a> 
				</ItemTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>	
		</Columns>
  </asp:GridView>
</asp:Content>

