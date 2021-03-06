﻿<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Willkommen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Herzlich Willkommen!</h1>
    Sie haben sich mit ihren Zugangsdaten erfolgreich eingeloggt.<br />
    <hr />
    <br />
    <h2>NEU bei InfoScreen v6:</h2>
    <ul>
        <li>
            Modernes Design der Verwaltungswebsite sowie der Bildschirmdarstellung
        </li>
        <li>
            Benutzerdefinierte Themes für die Bildschirmdarstellung
        </li>
        <li>
            Schnellere Übergänge zwischen Inhalten (besonders Bildern) durch angepasste Algorithmen
        </li>
        <li>
            Verbesserte Performance durch neue Server-Software und .net Core Webframework 
        </li>
    </ul>
  
    <br />
    <br />
    <img src="https://media.giphy.com/media/3ohuP8gmeoBeA0xvgI/giphy.gif" alt="gif image" style="height:30%;" />
   
</asp:Content>
