<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MobileOtpLogin.aspx.cs" Inherits="UserLoginHistory.MobileOtpLogin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Mobile OTP Login - My Application</title>
    <link href="login.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">
    <div class="page-container">
        <div class="login-card">
            <div class="login-header">
                <div class="logo">&#128241;</div>
                <h1>Mobile login</h1>
                <p>Use a one-time password to sign in securely</p>
            </div>

            <div class="login-form">
                <asp:Panel ID="pnlMessage" runat="server" CssClass="message-panel" Visible="false">
                    <asp:Label ID="lblMessage" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlMobile" runat="server">
                    <div class="form-group">
                        <asp:Label ID="lblMobileNumber" runat="server" AssociatedControlID="txtMobileNumber" CssClass="form-label" Text="Mobile number" />
                        <div class="input-wrapper">
                            <span class="input-icon">&#128222;</span>
                            <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-control" MaxLength="16" placeholder="+919876543210" autocomplete="tel" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvMobileNumber" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="Please enter your mobile number." CssClass="validation-error" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revMobileNumber" runat="server" ControlToValidate="txtMobileNumber" ValidationExpression="^\+?[0-9]{10,15}$" ErrorMessage="Enter 10 to 15 digits, with an optional + prefix." CssClass="validation-error" Display="Dynamic" />
                    </div>
                    <asp:Button ID="btnSendOtp" runat="server" Text="Send OTP" CssClass="login-button" OnClick="btnSendOtp_Click" CausesValidation="true" />
                </asp:Panel>

                <asp:Panel ID="pnlOtp" runat="server" Visible="false">
                    <div class="otp-notice">
                        <asp:Label ID="lblOtpDestination" runat="server" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblOtp" runat="server" AssociatedControlID="txtOtp" CssClass="form-label" Text="One-time password" />
                        <div class="input-wrapper">
                            <span class="input-icon">&#128272;</span>
                            <asp:TextBox ID="txtOtp" runat="server" CssClass="form-control" TextMode="Number" MaxLength="6" placeholder="Enter 6-digit OTP" autocomplete="one-time-code" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvOtp" runat="server" ControlToValidate="txtOtp" ErrorMessage="Please enter the OTP." CssClass="validation-error" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revOtp" runat="server" ControlToValidate="txtOtp" ValidationExpression="^[0-9]{6}$" ErrorMessage="OTP must contain 6 digits." CssClass="validation-error" Display="Dynamic" />
                    </div>
                    <asp:Button ID="btnVerifyOtp" runat="server" Text="Verify and sign in" CssClass="login-button" OnClick="btnVerifyOtp_Click" CausesValidation="true" />
                    <asp:Button ID="btnResendOtp" runat="server" Text="Send a new OTP" CssClass="secondary-button" OnClick="btnResendOtp_Click" CausesValidation="false" />
                </asp:Panel>
            </div>

            <div class="login-footer">
                <a href="Login.aspx">Use username and password instead</a>
            </div>
        </div>
    </div>
</form>
</body>
</html>
