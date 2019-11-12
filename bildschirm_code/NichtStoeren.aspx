<%@ Page Language="C#" Theme="Standarddesign" AutoEventWireup="true" CodeBehind="NichtStoeren.aspx.cs" Inherits="Infoscreen_Bildschirme.NichtStoeren" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="expires" content="-1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Bitte nicht Stören!</title>
</head>
<body style="margin:0px;">
    <form id="form_NichtStoeren" runat="server">
        <asp:Table ID="table_NichtStoeren" SkinID="Standardtabelle" runat="server" style="position:fixed; top:0px; left:0px; right:0px; bottom:0px;">
            <asp:TableRow runat="server">
                <asp:TableCell SkinID="Abteilungsscreen_ZentrierCell" runat="server" Height="50%">
                    <asp:Label ID="lb_NichtStören1" SkinID="lb_NichtStören" runat="server" Text="<h1>Bitte nicht stören!</h1>"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell SkinID="Abteilungsscreen_ZentrierCell" runat="server" Height="50%">
                    <asp:Label ID="lb_NichtStören4" SkinID="lb_NichtStören" runat="server" Width="80%" Height="80%"></asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </form>
</body>
</html>