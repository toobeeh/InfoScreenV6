<%@ Page Language="C#" Theme="Standarddesign" AutoEventWireup="true" CodeBehind="Sprechstunden.aspx.cs" Inherits="Infoscreen_Bildschirme.Sprechstunden" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sprechstunden</title>
</head>
<body style="margin:0px;">
    <form id="form_Sprechstunden" runat="server" style="position:fixed; top:0px; left:0px; right:0px; bottom:0px;">
        <div id="div_Oberfläche" runat="server" style="position:absolute; top:0px; left:0px; right:0px; bottom:50px;">
            <asp:Table ID="table_Sprechstunden" SkinID="Standardtabelle" runat="server"></asp:Table>
        </div>
        <%-- Bereich der Fußzeile --%>
        <div id="div_foother" runat="server" style="height; padding-right:10px; text-align:right; background:#405768; position:absolute; left:0px; right:0px; bottom:0px;">
            <asp:Label ID="lb_Fußzeile" SkinID="Fußzeilenlabel" runat="server" Text="Seite"></asp:Label>
        </div>
    </form>
</body>
</html>
