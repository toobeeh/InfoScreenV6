<%@ Page Language="C#" Theme="Standarddesign" AutoEventWireup="true" CodeBehind="Abteilungsscreen.aspx.cs" Inherits="Infoscreen_Bildschirme.Abteilungsscreen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="expires" content="-1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Abteilungsscreen</title>
</head>
<body style="margin:0px;">
    <form id="form_Abteilungsscreen" runat="server" style="position:fixed; top:0px; left:0px; right:0px; bottom:0px;">
        <%-- div, der die Bildschirmauflösung simulieren soll --%>
        <div id="div_Oberfläche" runat="server" style="width: 100%; height: 100%;">
            <%-- Bereich für die Tab-Übersicht --%>
            <div id="div_Tabs" runat="server" style="width: 100%; height: 5%;">
                <%-- Tabelle, welche die einzelen Tabs beinhaltet --%>
                <asp:Table ID="table_Tabs" SkinID="Standardtabelle" runat="server">
                    <asp:TableRow runat="server">
                        <asp:TableCell ID="tab3" runat="server" SkinID="InaktiverTab">Abteilungsinfo</asp:TableCell>
                        <asp:TableCell ID="tab4" runat="server" SkinID="InaktiverTab">Sprechstunden</asp:TableCell>
                        <asp:TableCell ID="tab5" runat="server" SkinID="InaktiverTab">Raumaufteilung</asp:TableCell>
                        <asp:TableCell ID="tab6" runat="server" SkinID="InaktiverTab">Supplierungen</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <%-- Bereich in den die Informationen für den Abteilungsscreen geladen werden --%>
            <section id="section_main" style="width: 100%; height: 95%;">
                <iframe runat="server" id="placeholder" style="border:none; width:100%; height:100%;" scrolling="no"></iframe>
            </section>
        </div>
    </form>
</body>
</html>