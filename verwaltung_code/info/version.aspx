<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="version.aspx.cs" Inherits="Infoscreen_Verwaltung.info.version" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Versionsinfo</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Versionsinfo</h1>
    &copy; Adrian Schnetz &amp; Kevin Schönegger - 2016/17<br /><br />
    <asp:Table ID="Table4" runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell>Server Betriebssystem:</asp:TableHeaderCell>
            <asp:TableCell>Microsoft Windows Server 2012</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Webserver:</asp:TableHeaderCell>
            <asp:TableCell>Microsoft Internet Information Server 7</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Datenbank:</asp:TableHeaderCell>
            <asp:TableCell>Microsoft SQL Server 2012</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Verwaltungswebseite Version:</asp:TableHeaderCell>
            <asp:TableCell>4.0.0.0</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Bildschirmanzeige Version:</asp:TableHeaderCell>
            <asp:TableCell>4.0.0.0</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br /><hr /><br />
    &copy; Gregor Saibl, Johannes Taschler und Harold Tupan - 2015/16<br /><br />
    <asp:Table ID="Table1" runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell>Server Betriebssystem:</asp:TableHeaderCell>
            <asp:TableCell>Microsoft Windows Server 2012</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Webserver:</asp:TableHeaderCell>
            <asp:TableCell>Microsoft Internet Information Server 7</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Datenbank:</asp:TableHeaderCell>
            <asp:TableCell>Microsoft SQL Server 2012</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Verwaltungswebseite Version:</asp:TableHeaderCell>
            <asp:TableCell>3.0.0.0</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Bildschirmanzeige Version:</asp:TableHeaderCell>
            <asp:TableCell>3.0.0.0</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br /><hr /><br />
    &copy; Kevin Besenlehner, Stefan Buschbeck und Stefan Nitsche - 2013/14<br /><br />
    <asp:Table ID="Table2" runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell>Verwaltungswebseite Version:</asp:TableHeaderCell>
            <asp:TableCell>2.0.0.0</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Bildschirmanzeige Version:</asp:TableHeaderCell>
            <asp:TableCell>2.0.0.0</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br /><hr /><br />
    &copy; Andreas Fehlmann, Christian Gottsnahm, Tobias Printz und Stefan Radlwimmer - 2012/13<br /><br />
    <asp:Table ID="Table3" runat="server">
        <asp:TableRow>
            <asp:TableHeaderCell>Verwaltungswebseite Version:</asp:TableHeaderCell>
            <asp:TableCell>1.0.0.0</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell>Bildschirmanzeige Version:</asp:TableHeaderCell>
            <asp:TableCell>1.0.0.0</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
