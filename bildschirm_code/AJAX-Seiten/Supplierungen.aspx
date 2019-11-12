<%@ Page Language="C#" Theme="Standarddesign" AutoEventWireup="true" CodeBehind="Supplierungen.aspx.cs" Inherits="Infoscreen_Bildschirme.Supplierungen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Supplierungen</title>
</head>
<body style="margin:0px;">
    <form id="form_Supplierungen" runat="server" style="position:fixed; top:0px; left:0px; right:0px; bottom:0px;">
        <div id="div_Oberfläche" runat="server" style="width: 100%; height: 95%; background-color:#D2D3D5">
            <div id="div_Supplierung_1" runat="server" style="width: 50%; height: 50%; float: left; display:none; text-align: center;">
                <asp:Table runat="server" SkinID="Standardtabelle">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" SkinID="Abteilungsscreen_ZentrierCell" HorizontalAlign="Center" VerticalAlign="Middle">
                            <asp:Table ID="table_Supplierung_1" runat="server" SkinID="Abteilungsscreen_Supplierungstabelle"></asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="div_Supplierung_2" runat="server" style="width: 50%; height: 50%; float: right; display:none;">
                <asp:Table runat="server" SkinID="Standardtabelle">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" SkinID="Abteilungsscreen_ZentrierCell" HorizontalAlign="Center" VerticalAlign="Middle">
                            <asp:Table ID="table_Supplierung_2" runat="server" SkinID="Abteilungsscreen_Supplierungstabelle"></asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="div_Supplierung_3" runat="server" style="width: 50%; height: 50%; float: left; display:none;">
                <asp:Table runat="server" SkinID="Standardtabelle">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" SkinID="Abteilungsscreen_ZentrierCell" HorizontalAlign="Center" VerticalAlign="Middle">
                            <asp:Table ID="table_Supplierung_3" runat="server" SkinID="Abteilungsscreen_Supplierungstabelle"></asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="div_Supplierung_4" runat="server" style="width: 50%; height: 50%; float: right; display:none;">
                <asp:Table runat="server" SkinID="Standardtabelle">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" SkinID="Abteilungsscreen_ZentrierCell" HorizontalAlign="Center" VerticalAlign="Middle">
                            <asp:Table ID="table_Supplierung_4" runat="server" SkinID="Abteilungsscreen_Supplierungstabelle"></asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
        <%-- Bereich der Fußzeile --%>
        <div id="div_foother" runat="server" style="height:5%; padding-right:10px; text-align:right; background:#405768;">
            <asp:Label ID="lb_Fußzeile" SkinID="Fußzeilenlabel" runat="server" Text="Seite"></asp:Label>
        </div>
    </form>
</body>
</html>