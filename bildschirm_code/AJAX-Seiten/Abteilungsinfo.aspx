<%@ Page Language="C#" Theme="Standarddesign" AutoEventWireup="true" CodeBehind="Abteilungsinfo.aspx.cs" Inherits="Infoscreen_Bildschirme.Abteilungsinfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Abteilungsinfo</title>
</head>
<body style="margin:0px;">
    <form id="form_Abteilungsinfo" runat="server" style="position:fixed; top:0px; left:0px; right:0px; bottom:0px;">
        <%-- div, der die Bildschirmauflösung simulieren soll --%>
        <div id="div_Oberfläche" runat="server" style="width: 100%; height: 100%;">
            <asp:Table ID="table_Abteilungsinfo" runat="server" Width="100%" Height="100%">
                <asp:TableRow runat="server">
                    <asp:TableCell ID="tablecell_Abteilungsinfo" runat="server" SkinID="Abteilungsscreen_ZentrierCell"></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </form>
</body>
</html>