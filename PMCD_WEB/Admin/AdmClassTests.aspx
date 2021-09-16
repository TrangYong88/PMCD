<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmClassTests.aspx.cs" Inherits="admin_pages_admin_elearn_AdmClassTests" Title="Danh sách bài kiểm tra theo lớp" %>
<%@ Import Namespace="Lib.Utils" %>
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font>
  <table width="100%" align = "center">
    <tr>
      <td> 
        Trạng thái:<asp:DropDownList ID="cboSearchTestStatus" runat="server" DataTextField="TestStatusName"  DataValueField="TestStatusId"/>
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"  CausesValidation="false" /> 
      </td>
    </tr>
  </table>  
  <asp:GridView ID="m_grid" DataKeyNames="ClassTestId" runat="server" CellPadding="4"
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
			<asp:TemplateField HeaderText="TestTypes">
				<ItemTemplate>
					<%# m_TestTypes.Get(cboTestTypes, Convert.ToByte(Eval("TestTypeId"))).TestTypeName%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlTestTypes" runat="server" DataSource='<%#cboTestTypes %>' DataTextField="TestTypeName"   DataValueField="TestTypeId" SelectedValue='<%#Bind("TestTypeId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertTestTypes" runat="server" DataSource='<%#cboTestTypes %>' DataTextField="TestTypeName" DataValueField="TestTypeId" SelectedValue='<%#Bind("TestTypeId") %>' />
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
			 
			<asp:TemplateField HeaderText="BeginDateTime">
				<ItemTemplate>
					<%#Eval("BeginDateTime")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtBeginDateTime" runat="server" Width="100" Text='<%# Bind("BeginDateTime") %>' />
					<asp:RequiredFieldValidator ID="RequiredBeginDateTime" runat="server" ControlToValidate="txtBeginDateTime" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertBeginDateTime" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertBeginDateTime" runat="server" ControlToValidate="txtInsertBeginDateTime" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="EndDateTime">
				<ItemTemplate>
					<%#Eval("EndDateTime")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtEndDateTime" runat="server" Width="100" Text='<%# Bind("EndDateTime") %>' />
					<asp:RequiredFieldValidator ID="RequiredEndDateTime" runat="server" ControlToValidate="txtEndDateTime" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertEndDateTime" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertEndDateTime" runat="server" ControlToValidate="txtInsertEndDateTime" Display="Dynamic" ErrorMessage="Nhập tên tài khoản" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="TestStatus">
				<ItemTemplate>
					<%# m_TestStatus.Get(cboTestStatus, Convert.ToByte(Eval("TestStatusId"))).TestStatusDesc%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlTestStatus" runat="server" DataSource='<%#cboTestStatus %>' DataTextField="TestStatusDesc"   DataValueField="TestStatusId" SelectedValue='<%#Bind("TestStatusId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertTestStatus" runat="server" DataSource='<%#cboTestStatus %>' DataTextField="TestStatusDesc" DataValueField="TestStatusId" SelectedValue='<%#Bind("TestStatusId") %>' />
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>

			<asp:TemplateField HeaderText="MaxTime">
				<ItemTemplate>
					<%#Eval("MaxTime")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtMaxTime" runat="server" Width="100" Text='<%# Bind("MaxTime") %>' />
					<asp:RequiredFieldValidator ID="RequiredMaxTime" runat="server" ControlToValidate="txtMaxTime" Display="Dynamic" ErrorMessage="Nhập mật khẩu" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertMaxTime" runat="server" Width="100"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertMaxTime" runat="server" ControlToValidate="txtInsertMaxTime"		Display="Dynamic" ErrorMessage="Nhập mật khẩu" SetFocusOnError="True"></asp:RequiredFieldValidator>
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
			
			<asp:TemplateField HeaderText="Danh sách sinh viên">
				<ItemTemplate>
					<a href="AdmClassTestUsers.aspx?ClassTestId=<%#Eval("ClassTestId")%>" title="Danh sách sinh viên">Sinh viên</a>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Danh sách câu hỏi">
				<ItemTemplate>
					<a href="AdmClassTestQuestions.aspx?ClassTestId=<%#Eval("ClassTestId")%>" title="Danh sách câu hỏi">Câu hỏi</a>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Kiểm tra">
				<ItemTemplate>
					<a href="AdmClassTestQuestions.aspx?ClassTestId=<%#Eval("ClassTestId")%>" title="Kích hoạt kiểm tra">Kiểm tra</a>
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

