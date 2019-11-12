<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="klasseninfo.aspx.cs" Inherits="Infoscreen_Verwaltung.conf.klasseninfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Klasseninfo bearbeiten</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Klasseninfo bearbeiten</h1>
     Abteilung: 
    <div class="selectWrap">
        <asp:DropDownList ID="DropDownListAbteilung" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListAbteilung_SelectedIndexChanged"></asp:DropDownList>
     </div>
    <asp:Button ID="btÜbernehmen" CssClass="SaveButton" runat="server" Text="Übernehmen" OnClick="btÜbernehmen_Click" /> 
    <asp:Button ID="btReset" CssClass="DeleteButton"  runat="server" Text="Zurücksetzen" OnClick="btReset_Click" />
    <hr /><br />
    <asp:Table ID="tbKlassenInfo" runat="server" CssClass="table"></asp:Table>
    <br /><hr /><br />
    
</asp:Content>