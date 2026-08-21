<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRDALL.aspx.cs" Inherits="WebApplication1.GRDALL" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hello WELCOME TO THE WORLD</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="ProductGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID"
    ShowFooter="True"
    OnRowEditing="ProductGrid_RowEditing" 
    OnRowCancelingEdit="ProductGrid_RowCancelingEdit" 
    OnRowUpdating="ProductGrid_ProductGrid_RowUpdating" 
    OnRowDeleting="ProductGrid_RowDeleting"
    OnRowCommand="ProductGrid_RowCommand" EnableModelValidation="True" OnSelectedIndexChanged="ProductGrid_SelectedIndexChanged">
    
    <Columns>
        
        <%-- 1. Product ID Column (Read-Only) --%>
        <asp:TemplateField HeaderText="ID">
            <ItemTemplate>
                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ProductID") %>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <strong>New:</strong>
            </FooterTemplate>
        </asp:TemplateField>

        <%-- 2. Product Name Column --%>
        <asp:TemplateField HeaderText="Product Name">
            <ItemTemplate>
                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditName" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="txtNewName" runat="server" placeholder="Enter name"></asp:TextBox>
            </FooterTemplate>
        </asp:TemplateField>

        <%-- 3. Product Number Column --%>
        <asp:TemplateField HeaderText="Product Number">
            <ItemTemplate>
                <asp:Label ID="lblNum" runat="server" Text='<%# Eval("ProductNumber") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditNumber" runat="server" Text='<%# Bind("ProductNumber") %>'></asp:TextBox>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="txtNewNumber" runat="server" placeholder="Enter number"></asp:TextBox>
            </FooterTemplate>
        </asp:TemplateField>

        <%-- 4. Price Column --%>
        <asp:TemplateField HeaderText="Price">
            <ItemTemplate>
                <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("ListPrice", "{0:C}") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditPrice" runat="server" Text='<%# Bind("ListPrice") %>'></asp:TextBox>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="txtNewPrice" runat="server" placeholder="0.00"></asp:TextBox>
            </FooterTemplate>
        </asp:TemplateField>

        <%-- 5. Control / Action Buttons Column --%>
        <asp:TemplateField HeaderText="Actions">
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
                <asp:Button ID="btnInsert" runat="server" CommandName="AddNew" Text="Add New" CssClass="btn-success" />
            </FooterTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>
        </div>
    </form>
</body>
</html>
