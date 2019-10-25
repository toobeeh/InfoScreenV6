<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="stundenplan.aspx.cs" Inherits="Infoscreen_Verwaltung.conf.stundenplan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Stundenplan bearbeiten</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(function () {
            $('.autocompFach').autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        url: "stundenplan.aspx/GetFach",
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
                            alert(textStatus);
                        }
                    });
                }
            });

            $('.autocompLehrer').autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        url: "stundenplan.aspx/GetLehrer",
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
                            alert(textStatus);
                        }
                    });
                }
            });
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
    <h1>Stundenplan bearbeiten</h1>
    Klasse: 
    <div class="selectWrap">
        <asp:DropDownList ID="DropDownListKlasse" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListKlasse_SelectedIndexChanged"></asp:DropDownList>
    </div>
       
    <asp:Button ID="btÜbernehmen" CssClass="SaveButton" runat="server" Text="Übernehmen" OnClick="btÜbernehmen_Click" /> 
    <asp:Button ID="btReset" CssClass="DeleteButton" runat="server" Text="Zurücksetzen" OnClick="btReset_Click" />
    <hr /> <br />
    <asp:Label ID="LabelFehler" runat="server" ForeColor="Red" Text=""></asp:Label>
    <asp:Table ID="Stundenplan" CssClass="table" runat="server"></asp:Table>
    
    
</asp:Content>
