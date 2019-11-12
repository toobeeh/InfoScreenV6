<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="impressum.aspx.cs" Inherits="Infoscreen_Verwaltung.info.impressum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Impressum</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Impressum</h1>
    © Adrian Schnetz &amp; Kevin Schönegger <% =(DateTime.Now.Year) %><br /><br />
    Diese Webseite entstand im Rahmen einer Diplomarbeit mit dem Titel &quot;Infoscreen 4.0&quot; der Abteilung Elektronik im Jahr 2016/17.<br /><br />
    <h2>Rechtliche Informationen:</h2>
    Wir &uuml;bernehmen keine Haftung oder Garantie f&uuml;r die Aktualit&auml;t, Richtigkeit und Vollst&auml;ndigkeit der zur Verf&uuml;gung gestellten Informationen. Die Texte und bildlichen Darstellungen auf dieser Website sind Eigentum der HTL M&ouml;dling. Vervielf&auml;ltigung von Informationen oder Daten, insbesondere von Texten, Textabschnitten oder Bildmaterial bedarf der Zustimmung der HTL M&ouml;dling.
</asp:Content>