<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRDAdd.aspx.cs" Inherits="WebApplication1.GRDAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
    ShowFooter="True" OnRowCommand="GridView1_RowCommand" CellPadding="4" EnableModelValidation="True" ForeColor="#333333" GridLines="None" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnSorting="GridView1_Sorting">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    <Columns>
        
        <%-- 1. ID Column (Read Only / Automatic) --%>
        <asp:TemplateField HeaderText="ID">
            <ItemTemplate>
                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <%-- Leave blank or add a descriptive label --%>
                <strong>New:</strong>
            </FooterTemplate>
        </asp:TemplateField>

        <%-- 2. Name Column --%>
        <asp:TemplateField HeaderText="Name" SortExpression="TestName">
            <ItemTemplate>
                <asp:Label ID="lblName" runat="server" Text='<%# Eval("TestName") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditName" runat="server" Text='<%# Bind("TestName") %>'></asp:TextBox>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="txtNewName" runat="server" placeholder="Enter name"></asp:TextBox>
               
            </FooterTemplate>
        </asp:TemplateField>
            
        <%-- 5. Actions Column --%>
        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" />
                |
                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" 
                    OnClientClick="return confirm('Are you sure you want to delete this record?');" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />
                |
                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" />
            </EditItemTemplate>

            <FooterTemplate>
                <%-- CommandName="AddNew" links this button to the OnRowCommand event --%>
                <asp:Button ID="btnInsert" runat="server" Text="Add Name" CommandName="AddNew" CssClass="btn-success" />
            </FooterTemplate>
        </asp:TemplateField>

    </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
</asp:GridView>
        </div>
    </form>
</body>
</html>
