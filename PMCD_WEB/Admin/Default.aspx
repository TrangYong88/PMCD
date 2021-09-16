<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Import Namespace="Lib.Utils" %>
<%@ Import Namespace="Lib.Elearn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<style type="text/css">
.title{
    color: green; 
    text-align:center;
    margin: auto; 
    width: 100%; 
    }
td{
    border-collapse:collapse;
    border : 1px solid black;
    height: 22px; 
    width: 170px; 
    color: #5D7B9D
}
table{
    border : 1px solid black;
    cellpadding : 4;
    width : 20%;
    margin: auto;
    align : center;
}
.tdleft{
    background-color: #5D7B9D;
    color: white;
}
.tdright{
    text-align: center;
}
#login{
    width:5%;
    align: center;
    margin-left: auto;
    margin-right: auto;
    margin-top: 15px;
    
}

</style>
<title></title>
</head>
<body>
    <p class="title">LOGIN</p>
    <form id="form1" runat="server">
    <table> 
        <tr>
            <td class="tdleft">
                Main Form
            </td> 
            <td class="tdright" style="width: 168px">
            <a href="MainForm.aspx" runat="server" style="text-decoration: none">MainFrom</a> 
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                UserName
            </td> 
            <td class="tdright" style="width: 168px">
                <input type="text" id="ipUserName" required runat="server" maxlength="255" style="width: 196px"/> 
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                UserPass
            </td> 
            <td class="tdright" style="width: 168px">
                <input type="password" id="ipUserPass" required  runat="server" maxlength="255" style="width: 196px"/> 
            </td>
        </tr>
    </table>
    <div id="login">
        <asp:Button ID="btnLogin" runat="server" Text="Login" Width="100%" OnClick="btnLogin_Click"  CausesValidation="false"/>
    </div>
    </form> 
</body>
</html>
