<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneratePassword.aspx.cs" Inherits="UserLoginHistory.GeneratePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Generate Password</title>
</head>
<body>

<form id="form1" runat="server">

    <asp:Button
        ID="btnGenerate"
        runat="server"
        Text="Generate"
        OnClick="btnGenerate_Click" />

    <br /><br />

    <asp:Label
        ID="lblResult"
        runat="server">
    </asp:Label>

</form>

</body>
</html>
