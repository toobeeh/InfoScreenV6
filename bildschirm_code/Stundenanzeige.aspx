<%@ Page Language="C#" Theme="Standarddesign" AutoEventWireup="true" CodeBehind="Stundenanzeige.aspx.cs" Inherits="Infoscreen_Bildschirme.Stundenanzeige" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="expires" content="-1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Stundenanzeige</title>
</head>
<body style="margin: 0px;">
    <form id="form_Stundenanzeige" runat="server" style="position: fixed; top: 0px; left: 0px; right: 0px; bottom: 0px;">
        <%-- div, der die Bildschirmauflösung simulieren soll --%>
        <div id="div_Oberfläche" runat="server" style="width: 100%; height: 100%;">
            <%-- Der Header der Stundenanzeige  --%>
            <div id="div_Header" runat="server" style="width: 100%; height: 10%; background-color: white">
                <%-- Dieses Image zeigt das Logo der Abteilung an --%>
                <asp:Image ID="image_AbteilungsLogo" runat="server" Style="width: auto; height: 100%; float:left;" ImageUrl="~/Abteilungslogo/Abteilungslogo.png" />
                <%-- --%><div id="div_Klassentietel" runat="server" style="width: auto; margin-right:30%; height: 100%; float: right; margin-top:0px;"><%-- --%>
                            <asp:Label ID="lb_Klassentietel" runat="server" Text="Klasse" Font-Names="Segoe UI" Font-Size="70px" Font-Bold="true"></asp:Label>
                <%-- --%></div><%-- --%>
            </div>
            <%-- Die linke Leiste, welche Informationen ausgeben soll --%>
            <div id="div_Informationswand" runat="server" style="width: 20%; height: 90%; float: left; background-color: #2f4348">
                <%-- Gibt Informationen über den aktuellen Raum aus --%>
                <div id="div_Rauminformationen" style="width: 100%; height: 25%;">
                    <asp:Label ID="lb_Raum" SkinID="Überschrift_Informationslabel" runat="server" Text="RAUM:"></asp:Label><br />
                    <asp:Label ID="lb_Raumnummer" SkinID="Informationslabel" runat="server" Text="Raumnummer"></asp:Label><br />
                    <asp:Label ID="lb_Klasse" SkinID="Informationslabel" runat="server" Text="Klasse"></asp:Label><br />
                    <asp:Label ID="lb_Klassenvorstand" SkinID="Informationslabel" runat="server" Text="Klassenvorstand"></asp:Label>
                </div>
                <%-- Gibt informationen aus, welche für die Klassenmitglieder selbst interresant sind (z.B Supplierungen) --%>
                <div id="div_Zusatzinformationen" style="width: 100%; height: 75%;">
                    <asp:Label ID="lb_Info" SkinID="Überschrift_Informationslabel" runat="server" Text="INFO:"></asp:Label>
                    <%-- Informationsausgabe 1 von 2 --%>
                    <div id="div_Klasseninformation_1" runat="server" style="width: 100%;">
                        <asp:Label ID="lb_Klasse_1" SkinID="Unterüberschrift_Informationslabel" runat="server" Text="Überschrifft 1"></asp:Label>
                        <asp:Label ID="lb_Klasse_1_Informationen" SkinID="Informationslabel" runat="server" Text="Hier werden die gewünschten Informationen stehen!"></asp:Label>
                    </div>
                    <%-- Informationsausgabe 2 von 2 --%>
                    <div id="div_Klasseninformation_2" runat="server" style="width: 100%;">
                        <asp:Label ID="lb_Klasse_2" SkinID="Unterüberschrift_Informationslabel" runat="server" Text="Überschrifft 2"></asp:Label>
                        <asp:Label ID="lb_Klasse_2_Informationen" SkinID="Informationslabel" runat="server" Text="Hier werden die gewünschten Informationen stehen!"></asp:Label>
                    </div>
                </div>
            </div>
            <%-- Gibt den Stundenplan dieses Raumes aus --%>
            <div id="div_Stundenanzeige" runat="server" style="width: 80%; height: 90%; float: left;">
                <asp:Table ID="table_Stundenanzeige" SkinID="Standardtabelle" runat="server"></asp:Table>
            </div>
        </div>
    </form>
</body>
</html>