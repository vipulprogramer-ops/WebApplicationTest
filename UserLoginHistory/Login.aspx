<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UserLoginHistory.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="X-UA-Compatible"
          content="IE=edge" />

    <meta name="viewport"
          content="width=device-width, initial-scale=1.0" />

    <title>Login - My Application</title>

    <link href="login.css"
          rel="stylesheet"
          type="text/css" />

</head>
<body>

<form id="form1" runat="server">

    <div class="page-container">

        <div class="login-card">

            <!-- =========================================
                 Header
                 ========================================= -->

            <div class="login-header">

                <%--<div class="logo">
                    M
                </div>--%>

                <h1>
                    Welcome Back
                </h1>

                <p>
                    Sign in to continue to your account
                </p>

            </div>


            <!-- =========================================
                 Login Form
                 ========================================= -->

            <div class="login-form">

                <!-- Username -->

                <div class="form-group">

                    <asp:Label
                        ID="lblUsername"
                        runat="server"
                        AssociatedControlID="txtUsername"
                        CssClass="form-label"
                        Text="Username">
                    </asp:Label>

                    <div class="input-wrapper">

                        <span class="input-icon">
                            &#128100;
                        </span>

                        <asp:TextBox
                            ID="txtUsername"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="50"
                            autocomplete="username">
                        </asp:TextBox>

                    </div>

                    <asp:RequiredFieldValidator
                        ID="rfvUsername"
                        runat="server"
                        ControlToValidate="txtUsername"
                        ErrorMessage="Please enter your username."
                        CssClass="validation-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                </div>


                <!-- Password -->

                <div class="form-group">

                    <asp:Label
                        ID="lblPassword"
                        runat="server"
                        AssociatedControlID="txtPassword"
                        CssClass="form-label"
                        Text="Password">
                    </asp:Label>

                    <div class="input-wrapper">

                        <span class="input-icon">
                            &#128274;
                        </span>

                        <asp:TextBox
                            ID="txtPassword"
                            runat="server"
                            CssClass="form-control password-control"
                            TextMode="Password"
                            MaxLength="100"
                            autocomplete="current-password">
                        </asp:TextBox>

                        <button
                            type="button"
                            id="btnShowPassword"
                            class="password-toggle"
                            onclick="togglePassword();"
                            aria-label="Show password">

                            Show

                        </button>

                    </div>

                    <asp:RequiredFieldValidator
                        ID="rfvPassword"
                        runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="Please enter your password."
                        CssClass="validation-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                </div>


                <!-- Server Message -->

                


                <!-- Login Button -->

                <asp:Button
                    ID="btnLogin"
                    runat="server"
                    Text="Sign In"
                    CssClass="login-button"
                    OnClick="btnLogin_Click1" />
                

            </div>
            

            <!-- =========================================
                 Footer
                 ========================================= -->

            <div class="login-footer">

                <%--<span>
                    Secure Login
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Test Connection" Visible="False" />
                </span>--%>

                <asp:Panel
                    ID="pnlMessage"
                    runat="server"
                    CssClass="message-panel"
                    Visible="false">

                    <span
                        id="messageIcon"
                        class="message-icon">
                    <asp:Label ID="lblMessage" runat="server">
                    </asp:Label>
                    </span>

                </asp:Panel>

            </div>

        </div>

    </div>

</form>


<script type="text/javascript">

    // ================================================
    // Show / Hide password
    // ================================================

    function togglePassword() {

        var password =
            document.getElementById('<%= txtPassword.ClientID %>');

        var button =
            document.getElementById('btnShowPassword');

        if (password.type === 'password') {

            password.type = 'text';

            button.innerHTML = 'Hide';

            button.setAttribute(
                'aria-label',
                'Hide password'
            );

        }
        else {

            password.type = 'password';

            button.innerHTML = 'Show';

            button.setAttribute(
                'aria-label',
                'Show password'
            );
        }
    }


    // ================================================
    // Prevent empty submission
    // ================================================

    <%-- function validateBeforeSubmit(){

        if (typeof (Page_ClientValidate) === 'function') {

            if (!Page_ClientValidate()) {
                return false;
            }
        }

        var button =
            document.getElementById('<%= btnLogin.ClientID %>');

        if (button) {

            button.value = 'Signing in...';

            button.disabled = true;

            button.className =
                'login-button login-button-disabled';
        }

        return true;
    }--%>


    // ================================================
    // Focus username when page loads
    // ================================================

    window.onload = function () {

        var username =
            document.getElementById(
                '<%= txtUsername.ClientID %>'
            );

        if (username) {
            username.focus();
        }
    };

</script>

</body>

</html>
