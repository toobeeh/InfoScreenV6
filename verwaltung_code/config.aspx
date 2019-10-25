<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="config.aspx.cs" Inherits="Infoscreen_Verwaltung.config" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Benutzereinstellungen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">

    

    <h1>Benutzerprofil</h1>
    <b>Benutzername: </b> <% =(Infoscreen_Verwaltung.classes.Login.User) %><br />
    <b>Vollständiger Name: </b> <% =(Infoscreen_Verwaltung.classes.Login.Name) %><br />
    <b>Klasse: </b> <% =(Infoscreen_Verwaltung.classes.Login.StammKlasse) %><br />
    <b>Abteilung: </b> <% =(Infoscreen_Verwaltung.classes.Login.StammAbteilung) %><br />
    <% if (Infoscreen_Verwaltung.classes.Login.Rechte.Superadmin)
        { %> <br /><b>Angemeldet als Superadmin</b> <%} 
    %>
    <hr />
    Derzeit sind für Sie keine weiteren Einstellungen m&ouml;glich.
    <hr />
    
</asp:Content>