<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="tests.aspx.cs" Inherits="Infoscreen_Verwaltung.show.tests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Schularbeiten / Tests anzeigen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <asp:Label ID="lb_AnzeigeSA" runat="server" Text="lb_Anzeige"></asp:Label>
    Klasse: 
    <div class="selectWrap">
        <asp:DropDownList ID="DropDownListKlasse" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListKlasse_SelectedIndexChanged"></asp:DropDownList>
    </div>
    <br /><hr /><br />
    <asp:Table ID="TestListe" CssClass="table" runat="server"></asp:Table>
</asp:Content>
