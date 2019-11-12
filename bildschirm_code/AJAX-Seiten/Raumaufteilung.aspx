<%@ Page Language="C#" Theme="Standarddesign" AutoEventWireup="true" CodeBehind="Raumaufteilung.aspx.cs" Inherits="Infoscreen_Bildschirme.Raumaufteilung" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Raumaufteilung</title>
</head>
<body style="margin:0px;">
    <form id="form_Raumaufteilung" runat="server" style="position:fixed; top:0px; left:0px; right:0px; bottom:0px;">
        <div id="div_Oberfläche" runat="server" style="width: 100%; height: 95%;">
            <asp:Table ID="table_Raumaufteilung" SkinID="Standardtabelle" runat="server"></asp:Table>
        </div>
        <%-- Bereich der Fußzeile --%>
        <div id="div_foother" runat="server" style="height:5%; padding-right:10px; text-align:right; background:#405768;">
            <asp:Label ID="lb_Fußzeile" SkinID="Fußzeilenlabel" runat="server" Text="Seite"></asp:Label>
        </div>
    </form>
</body>
</html>