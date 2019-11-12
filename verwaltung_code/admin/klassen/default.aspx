<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.klassen._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Klasseneigenschaften</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
   <script language="javascript" type="text/javascript">
        $(function () {
            $('.autocompKlassensprecherID').autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        url: "default.aspx/GetKlassensprecherID",
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
        $(function () {
            $('.autocompKlassensprecherName').autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        url: "default.aspx/GetKlassensprecherName",
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
    <div id="picker_container" runat="server"/>
    <h1>Klasseneigenschaften</h1>
    Abteilung: 
    <div class="selectWrap">
        <asp:DropDownList ID="dropDownAbteilung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropDownAbteilung_SelectedIndexChanged">
        </asp:DropDownList>
    </div>
    <br /><hr /><br />
    <div style="float:left">     
        <asp:Button ID="btNeueKlasse" CssClass="ActionButton" runat="server" Text="Neue Klasse" Enabled="false" OnClick="btNeueKlasse_Click" ToolTip="Neue Klasse hinzufügen" />
        <asp:Button ID="btNeuesSchuljahr" CssClass="ActionButton" runat="server" Enabled="false" OnClick="btNeuesSchuljahr_Click" Text="Neues Schuljahr" ToolTip="KV wird den entsprechenden Klassen zugewiesen" />
        <asp:Button ID="btAlleStundenplaeneLeeren" CssClass="DeleteButton" runat="server" Enable="false" OnClick="btAlleStundenplaeneLeeren_Click" Text="Alle Stundenpläne leeren" />      
    </div>   
    <div style="float:right">
        <asp:Button ID="btUebernehmen" CssClass="SaveButton" runat="server" Text="Klasseneigenschaften Übernehmen" Enabled="false" OnClick="btUebernehmen_Click" />
        
    </div>
    <div class="clear"></div>
    <br />
    <asp:Label ID="lbHinweis1" runat="server"></asp:Label>
    <asp:Table ID="tbKlassen" runat="server" CssClass="table"></asp:Table>
    <asp:Label ID="lbHinweis2" runat="server"></asp:Label>
    <br /><hr /><br />
    
 </asp:Content>