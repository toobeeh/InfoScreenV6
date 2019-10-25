<%@ Page Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="logout.aspx.cs" Inherits="Infoscreen_Verwaltung.logout" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Willkommen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    Sie haben Sich erfolgreich ausgeloggt!
</asp:Content>
