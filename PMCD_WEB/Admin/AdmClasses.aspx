<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmClasses.aspx.cs" Inherits="admin_pages_admin_elearn_AdmClasses" Title="Danh sách các lớp học" %>
<%@ Import Namespace="Lib.Utils" %>
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font>
  <table width="100%" align = "center">
    <tr>
      <td> 
        Mã lớp:<asp:TextBox  ID="txtSeachKeyword" runat="server" Text="" Columns="50" MaxLength="255" ></asp:TextBox> 
        Chương trình:<asp:DropDownList ID="cboSearchCurriculums" runat="server" DataTextField="CurriculumName"  DataValueField="CurriculumId"/>
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"  CausesValidation="false" /> 
      </td>
    </tr>
  </table>  
  <asp:GridView ID="m_grid" DataKeyNames="ClassId" runat="server" CellPadding="4"
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
			<asp:TemplateField HeaderText="Curriculums">
				<ItemTemplate>
					<%# m_Curriculums.Get(cboCurriculums, Convert.ToByte(Eval("CurriculumId"))).CurriculumName%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlCurriculums" runat="server" DataSource='<%#cboCurriculums %>' DataTextField="CurriculumName"   DataValueField="CurriculumId" SelectedValue='<%#Bind("CurriculumId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertCurriculums" runat="server" DataSource='<%#cboCurriculums %>' DataTextField="CurriculumName" DataValueField="CurriculumId" SelectedValue='<%#Bind("CurriculumId") %>' />
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="ClassCode">
				<ItemTemplate>
					<%#Eval("ClassCode")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtClassCode" runat="server" Width="100" Text='<%# Bind("ClassCode") %>' />
					<asp:RequiredFieldValidator ID="RequiredClassCode" runat="server" ControlToValidate="txtClassCode" Display="Dynamic" ErrorMessage="Nhập tên lớp học" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertClassCode" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertClassCode" runat="server" ControlToValidate="txtInsertClassCode" Display="Dynamic" ErrorMessage="Nhập tên lớp học" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="ClassName">
				<ItemTemplate>
					<%#Eval("ClassName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtClassName" runat="server" Width="100" Text='<%# Bind("ClassName") %>' />
					<asp:RequiredFieldValidator ID="RequiredClassName" runat="server" ControlToValidate="txtClassName" Display="Dynamic" ErrorMessage="Nhập tên lớp học" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertClassName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertClassName" runat="server" ControlToValidate="txtInsertClassName" Display="Dynamic" ErrorMessage="Nhập tên lớp học" SetFocusOnError="True"></asp:RequiredFieldValidator>
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
			
			<asp:TemplateField HeaderText="Các bài kiểm tra">
				<ItemTemplate>
					<a href="AdmClassTests.aspx?ClassId=<%#Eval("ClassId")%>" title="Danh sách bài kiểm tra">Bài kiểm tra</a>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Danh sách sinh viên">
				<ItemTemplate>
					<a href="AdmUserClasses.aspx?ClassId=<%#Eval("ClassId")%>" title="Danh sách sinh viên">Sinh viên</a>
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

