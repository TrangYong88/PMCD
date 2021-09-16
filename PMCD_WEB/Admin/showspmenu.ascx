<%@ Control Language="C#" AutoEventWireup="true" CodeFile="showspmenu.ascx.cs" Inherits="admin_showspmenu" %>

<%@ Import Namespace="Lib.Elearn" %>
<style type="text/css">
    .welcometext { font-family: Arial,Tahoma ,Times New Roman; font-size: 10pt; }
    .usernametext { font-family: Arial,Tahoma ,Times New Roman; font-size: 10pt; color: Blue; }
    .logouttext { font-family: Arial,Tahoma ,Times New Roman; font-size: 10pt; color: Red; text-decoration: none; }
</style>
<table style="border-collapse: collapse" cellspacing="0" cellpadding="0" width="100%"  border="0">
  <tr>
    <td align="left" style="height: 23px">
      <div id="myMenuID"></div>
    </td>
  </tr>
</table>
<script type="text/javascript" language="JavaScript">
  var myMenu = <%=strMenu.ToString() %>
</script>
<script type="text/javascript" language="JavaScript">
  cmDraw('myMenuID', myMenu, 'hbr', cmThemeDefault, 'ThemeDefault');
</script>
<div style="margin:3px; text-indent:5px;">
  <font class="welcometext"><%=Fullname%></font>
  <asp:Label ID="lblUser" CssClass="usernametext" runat="server"></asp:Label>&nbsp;
  <asp:HyperLink ID="HyperLink1" CssClass="logouttext" runat="server" NavigateUrl="/Code1/Admin/Default.aspx">Thoát</asp:HyperLink>&nbsp;
</div>