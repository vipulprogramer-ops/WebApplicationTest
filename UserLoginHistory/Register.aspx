<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="UserLoginHistory.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Create account - My Application</title>
    <link href="login.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">
    <div class="page-container">
        <div class="login-card">
            <div class="login-header">
                <h1>Create account</h1>
                <p>Choose a username and password to get started</p>
            </div>

            <div class="login-form">
                <div class="form-group">
                    <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" CssClass="form-label" Text="Username" />
                    <div class="input-wrapper">
                        <span class="input-icon">&#128100;</span>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="50" autocomplete="username" />
                    </div>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Please enter a username." CssClass="validation-error" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="form-label" Text="Password" />
                    <div class="input-wrapper">
                        <span class="input-icon">&#128274;</span>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100" autocomplete="new-password" />
                    </div>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Please enter a password." CssClass="validation-error" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" ValidationExpression="^.{8,}$" ErrorMessage="Password must be at least 8 characters." CssClass="validation-error" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" CssClass="form-label" Text="Confirm password" />
                    <div class="input-wrapper">
                        <span class="input-icon">&#128274;</span>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100" autocomplete="new-password" />
                    </div>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Please confirm your password." CssClass="validation-error" Display="Dynamic" />
                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ErrorMessage="Passwords do not match." CssClass="validation-error" Display="Dynamic" />
                </div>

                <asp:Panel ID="pnlMessage" runat="server" CssClass="message-panel" Visible="false">
                    <asp:Label ID="lblMessage" runat="server" />
                </asp:Panel>

                <asp:Button ID="btnRegister" runat="server" Text="Create account" CssClass="login-button" OnClick="btnRegister_Click" />
            </div>

            <div class="login-footer">
                Already have an account?
                <a href="Login.aspx">Sign in</a>
            </div>
        </div>
    </div>
</form>
</body>
</html>
