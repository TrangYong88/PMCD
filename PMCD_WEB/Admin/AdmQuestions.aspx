<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmQuestions.aspx.cs" Inherits="admin_pages_admin_elearn_AdmQuestions" Title="Danh sách các câu hỏi" %>
<%@ Import Namespace="Lib.Utils" %>
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font>
  <table width="100%" align = "center">
    <tr>
      <td> 
        Từ khóa: <asp:TextBox  ID="txtSeachKeyword" runat="server" Text="" Columns="50" MaxLength="255" ></asp:TextBox> 
        Mức:<asp:DropDownList ID="cboSearchQuestionLevels" runat="server" DataTextField="QuestionLevelName"  DataValueField="QuestionLevelId"/>
        Loại:<asp:DropDownList ID="cboSearchQuestionTypes" runat="server" DataTextField="QuestionTypeName"  DataValueField="QuestionTypeId"/>
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"  CausesValidation="false" /> 
      </td>
    </tr>
  </table>  
  <asp:GridView ID="m_grid" DataKeyNames="QuestionId" runat="server" CellPadding="4"
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
		    
			
			<asp:TemplateField HeaderText="QuestionLevels">
				<ItemTemplate>
					<%# m_QuestionLevels.Get(cboQuestionLevels, Convert.ToByte(Eval("QuestionLevelId"))).QuestionLevelDesc%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlQuestionLevels" runat="server" DataSource='<%#cboQuestionLevels %>' DataTextField="QuestionLevelDesc"   DataValueField="QuestionLevelId" SelectedValue='<%#Bind("QuestionLevelId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertQuestionLevels" runat="server" DataSource='<%#cboQuestionLevels %>' DataTextField="QuestionLevelDesc" DataValueField="QuestionLevelId" SelectedValue='<%#Bind("QuestionLevelId") %>' />
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="QuestionTypes">
				<ItemTemplate>
					<%# m_QuestionTypes.Get(cboQuestionTypes, Convert.ToByte(Eval("QuestionTypeId"))).QuestionTypeDesc%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:DropDownList ID="ddlQuestionTypes" runat="server" DataSource='<%#cboQuestionTypes %>' DataTextField="QuestionTypeDesc"   DataValueField="QuestionTypeId" SelectedValue='<%#Bind("QuestionTypeId") %>' />
				</EditItemTemplate>
				<FooterTemplate>
					<asp:DropDownList ID="ddlInsertQuestionTypes" runat="server" DataSource='<%#cboQuestionTypes %>' DataTextField="QuestionTypeDesc" DataValueField="QuestionTypeId" SelectedValue='<%#Bind("QuestionTypeId") %>' />
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			 
			<asp:TemplateField HeaderText="QuestionName">
				<ItemTemplate>
					<%#Eval("QuestionName")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtQuestionName" runat="server" Width="100" Text='<%# Bind("QuestionName") %>' />
					<asp:RequiredFieldValidator ID="RequiredQuestionName" runat="server" ControlToValidate="txtQuestionName" Display="Dynamic" ErrorMessage="Nhập tên lớp học" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertQuestionName" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertQuestionName" runat="server" ControlToValidate="txtInsertQuestionName" Display="Dynamic" ErrorMessage="Nhập tên lớp học" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="QuestionDesc">
				<ItemTemplate>
					<%#Eval("QuestionDesc")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtQuestionDesc" runat="server" Width="100" Text='<%# Bind("QuestionDesc") %>' />
					<asp:RequiredFieldValidator ID="RequiredQuestionDesc" runat="server" ControlToValidate="txtQuestionDesc" Display="Dynamic" ErrorMessage="Nhập tên lớp học" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertQuestionDesc" runat="server"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertQuestionDesc" runat="server" ControlToValidate="txtInsertQuestionDesc" Display="Dynamic" ErrorMessage="Nhập tên lớp học" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</FooterTemplate>
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>

			<asp:TemplateField HeaderText="QuestionMaxTime">
				<ItemTemplate>
					<%#Eval("QuestionMaxTime")%>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="txtQuestionMaxTime" runat="server" Width="100" Text='<%# Bind("QuestionMaxTime") %>' />
					<asp:RequiredFieldValidator ID="RequiredQuestionMaxTime" runat="server" ControlToValidate="txtQuestionMaxTime" Display="Dynamic" ErrorMessage="Nhập mật khẩu" SetFocusOnError="True"></asp:RequiredFieldValidator>
				</EditItemTemplate>
				<FooterTemplate>
					<asp:TextBox ID="txtInsertQuestionMaxTime" runat="server" Width="100"></asp:TextBox><br />
					<asp:RequiredFieldValidator ID="RequiredInsertQuestionMaxTime" runat="server" ControlToValidate="txtInsertQuestionMaxTime"		Display="Dynamic" ErrorMessage="Nhập mật khẩu" SetFocusOnError="True"></asp:RequiredFieldValidator>
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
			
			<asp:TemplateField HeaderText="Các câu trả lời">
				<ItemTemplate>
					<a href="AdmAnswers.aspx?QuestionId=<%#Eval("QuestionId")%>" title="Danh sách các câu trả lời">Câu trả lời</a>
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

