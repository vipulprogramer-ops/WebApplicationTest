<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Login</title>

    <style type="text/css">
        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            padding: 0;
            font-family: Arial, Helvetica, sans-serif;
            background: linear-gradient(135deg, #667eea, #764ba2);
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .login-container {
            width: 380px;
            background: #ffffff;
            padding: 35px;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.25);
        }

        .login-title {
            text-align: center;
            font-size: 28px;
            font-weight: bold;
            color: #333;
            margin-bottom: 30px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            margin-bottom: 7px;
            font-weight: bold;
            color: #444;
        }

        .textbox {
            width: 100%;
            height: 45px;
            padding: 10px 12px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 15px;
            outline: none;
        }

        .textbox:focus {
            border-color: #667eea;
            box-shadow: 0 0 4px rgba(102,126,234,0.4);
        }

        .login-button {
            width: 100%;
            height: 45px;
            border: none;
            border-radius: 6px;
            background: #667eea;
            color: white;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
        }

        .login-button:hover {
            background: #5568d8;
        }

        .message {
            display: block;
            text-align: center;
            margin-top: 15px;
            color: #d9534f;
            font-size: 14px;
        }

        .validation {
            color: #d9534f;
            font-size: 12px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="login-container">

        <div class="login-title">
            Login
        </div>

        <div class="form-group">
            <asp:Label ID="lblUsername"
                       runat="server"
                       Text="Username">
            </asp:Label>

            <asp:TextBox ID="txtUsername"
                         runat="server"
                         CssClass="textbox"
                         MaxLength="50">
            </asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvUsername"
                runat="server"
                ControlToValidate="txtUsername"
                ErrorMessage="Username is required"
                CssClass="validation"
                Display="Dynamic">
            </asp:RequiredFieldValidator>
        </div>

        <div class="form-group">
            <asp:Label ID="lblPassword"
                       runat="server"
                       Text="Password">
            </asp:Label>

            <asp:TextBox ID="txtPassword"
                         runat="server"
                         CssClass="textbox"
                         TextMode="Password"
                         MaxLength="100">
            </asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvPassword"
                runat="server"
                ControlToValidate="txtPassword"
                ErrorMessage="Password is required"
                CssClass="validation"
                Display="Dynamic">
            </asp:RequiredFieldValidator>
        </div>

        <asp:Button ID="btnLogin"
                    runat="server"
                    Text="Login"
                    CssClass="login-button"
                    OnClick="btnLogin_Click" />

        <asp:Label ID="lblMessage"
                   runat="server"
                   CssClass="message">
        </asp:Label>

    </div>
    </form>
</body>
</html>
