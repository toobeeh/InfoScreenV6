<%@ Page Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.converter.Converter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Converter</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Powerpoint-to-PNG Converter Status</h1>
    Um hochgeladene Powerpoint-Präsentationen für Betriebsmodi automatisch in die Bilder, die auf den Infoscreens angezeigt werden, zu konvertieren muss am Server das Programm PPT2PNG laufen.
    <br />
    Dazu muss ein Benutzer mit ausreichend Rechten auf dem Server angemeldet sein.
    <br />
    <hr />
    <br />
    <h2>Status</h2>
    <asp:Label ID="Status" runat="server" Text=""> </asp:Label>
    <hr />
    <br />
    <h2>Funktionsweise</h2>
    Der Verwaltungs-Client speichert ausstehende Konvertierungen in eine Textdatei.<br />
    Am Server wird alle 5 Sekunden, sofern ein Benutzer mit gültigen Rechten angemeldet ist, die Textdatei geprüft und <br />
    die Konvertierungen durchgeführt. <br />
    <br />
    Bei einer Anmeldung wird der Converter automatisch gestartet, wenn nicht bereits auf einem anderen Account das Service läuft.<br />
    Notwendige Berechtigungen sind ein aktives MS-Office-Abonnement und Schreibrechte auf das Infoscreen-Lauferk D:/

   
</asp:Content>
