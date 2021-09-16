<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdmQuestionCurriculums.aspx.cs" Inherits="admin_pages_admin_elearn_AdmQuestionCurriculums" Title="Danh sách chương trình câu hỏi" %>
<%@ Import Namespace="Lib.Utils" %>                                                                                             
<%@ Import Namespace="Lib.Elearn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="m_contentBody" runat="Server">
  <font class="titleForm"><%=this.Title.ToUpper()%></font>
  <font face="Arial" size="2" color="red"><b><asp:Label ID="lblInfo" runat="server"/></b></font> 
  <table width="100%" align = "center">
    <tr>
      <td> 
        Từ khóa: <asp:TextBox  ID="txtSeachKeyword" runat="server" Text="" Columns="50" MaxLength="255" ></asp:TextBox> 
        Mức:<asp:DropDownList ID="cboSearchQuestionLevels" runat="server" DataTextField="QuestionLevelName"  DataValueField="QuestionLevelId"/>
        Loại:<asp:DropDownList ID="cboSearchQuestionTypes" runat="server" DataTextField="QuestionTypeName"  DataValueField="QuestionTypeId"/>
        <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click"  CausesValidation="false" /> 
        <asp:Button ID="Button1" runat="server" Text="Xác nhận" OnClick="btnSubmit_Click"  CausesValidation="false" />
      </td>
    </tr>
  </table>
        
     
  <asp:GridView ID="m_grid" DataKeyNames="QuestionId" runat="server" CellPadding="4"
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
			
			<asp:TemplateField HeaderText="Mức độ câu hỏi">
				<ItemTemplate>
					<%# m_QuestionLevels.Get(cboQuestionLevels, Convert.ToByte(Eval("QuestionLevelId"))).QuestionLevelDesc%>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Kiểu câu hỏi">
				<ItemTemplate>
					<%# m_QuestionTypes.Get(cboQuestionTypes, Convert.ToByte(Eval("QuestionTypeId"))).QuestionTypeDesc%>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Tên câu hỏi">
				<ItemTemplate>
					<%#Eval("QuestionName")%>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Mô tả câu hỏi">
				<ItemTemplate>
					<%#Eval("QuestionDesc")%>
				</ItemTemplate>
			</asp:TemplateField>
			
			<asp:TemplateField HeaderText="Thời gian tối đa">
				<ItemTemplate>
					<%#Eval("QuestionMaxTime")%>
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


