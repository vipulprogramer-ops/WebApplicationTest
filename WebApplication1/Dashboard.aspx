<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebApplication1.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard</title>
</head>
<body>

<form id="form1" runat="server">

    <h1>Welcome to Dashboard</h1>

    <asp:Label ID="lblWelcome"
               runat="server">
    </asp:Label>
    <asp:HyperLink ID="lnkLogout"
        runat="server"
        NavigateUrl="Logout.aspx"
        Text="Logout">
    </asp:HyperLink>

</form>

</body>
</html>
