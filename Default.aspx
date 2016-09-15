<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WFFiles.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
   <%-- <asp:Image runat="server" ID="imgDisplayImage" />--%>
    <%--    <asp:GridView ID="GridView1" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
    RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000"
    AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="FileName" HeaderText="File Name"/>
        <asp:BoundField DataField="Date Created" HeaderText="Date Created" />
        <asp:TemplateField ItemStyle-HorizontalAlign = "Center">
            
            <ItemTemplate>
                
                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"
                    CommandArgument='<%# Eval("No_") %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>--%>

    </div>
    </form>
</body>
</html>
