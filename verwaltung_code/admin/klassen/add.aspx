<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="add.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.klassen.add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Klasse ändern</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(function ()
        {
            $('#<%=tbGebäude.ClientID%>').autocomplete(
            {
                source: function (request, response)
                {
                    $.ajax(
                    {
                        url: "add.aspx/GetGebaeude",
                        data: "{ 'pre':'" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data)
                        {
                            response($.map(data.d, function (item)
                            {
                                return { value: item }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown)
                        {
                            alert(textStatus);
                        }
                    });
                }
            });

            <%--$('#<%=tbKlasse.ClientID%>').autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        url: "add.aspx/GetKlasse",
                        data: "{ 'pre':'" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return { value: item }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(XMLHttpRequest);
                        }
                    });
                }
            });
            
            InfoScreen V6: USELESS! NEW Classes dont't have to exist already!!

            --%>
        });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1 runat="server" id="überschrift"></h1>
    <br /><hr /><br />
    <asp:Label ID="lbTbKlasse" runat="server" Text="Klasse:"></asp:Label>
    <asp:TextBox ID="tbKlasse" runat="server" MaxLength="10"></asp:TextBox>
    <asp:Label ID="lbErrorTbKlasseLen" runat="server" Text="Die länge der Klasse muss zwischen 2 und 10 Zeichen liegen." ForeColor="Red" Visible="false"></asp:Label>
    <asp:Label ID="lbErrorTbKlasseDou" runat="server" Text="Die angegebene Klasse ist bereits vorhanden!" ForeColor="Red" Visible="false"></asp:Label>
    <br /><br />

    <asp:Label ID="lbTbGebäude" runat="server" Text="Gebäude:"></asp:Label>
    <asp:TextBox ID="tbGebäude" runat="server" MaxLength="40"></asp:TextBox>
    <asp:Label ID="lbErrorTbGebäudeLen" runat="server" Text="Die länge des Gebäudes darf 40 Zeichen nicht übersteigen!" ForeColor="Red" Visible="false"></asp:Label>
    <asp:Label ID="lbErrorTbGebäudeNot" runat="server" Text="Das angegebene Gebäude existiert nicht! Haben Sie eventuell den Raum noch nicht erstellt?" ForeColor="Red" Visible="false"></asp:Label>
    <br /><br />

    <asp:Label ID="lbTbRaum" runat="server" Text="Raum:"></asp:Label>
    <asp:TextBox ID="tbRaum" runat="server" MaxLength="6"></asp:TextBox>
    <asp:Label ID="lbErrorTbRaumLen" runat="server" Text="Als Raum sind nur Zahlen erlaubt, welche nicht mehr als 6 Stellen haben dürfen!" ForeColor="Red" Visible="false"></asp:Label>
    <asp:Label ID="lbErrorTbRaumNot" runat="server" Text="Der angegebene Raum existiert nicht! Haben Sie eventuell den Raum noch nicht erstellt?" ForeColor="Red" Visible="false"></asp:Label>
    <br /><hr /><br />

    <asp:Button ID="btÜbernehmen" runat="server" Text="Übernehmen" OnClick="btÜbernehmen_Click" />
    <asp:Button ID="btAbbrechen" runat="server" Text="Abbrechen" OnClick="btAbbrechen_Click" />
</asp:Content>