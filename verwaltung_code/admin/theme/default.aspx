<%@ Page Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.theme.Theme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Willkommen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Farbschema anpassen</h1>
    Die Darstellung der dynamischen Seiten auf den InfoScreen-Bildschirmen kann via Farbthemen angepasst werden. <br />
    Die Schema "Light" und "Dark" sind voreingestellt, zusätzlich können auch eigene Schema definiert werden.
    <br />
    <hr />
    <br />
    <asp:Table ID="Themes" runat="server" CssClass="table" />
    <asp:Label ID="lb" runat="server"></asp:Label>
  
</asp:Content>
