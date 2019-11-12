<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="add.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.bildschirm_einstellungen.add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Betriebsmode ändern</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1 runat="server" id="überschrift"></h1>
    <br /><hr /><br />
    <asp:Label ID="lbTbName" runat="server" Text="Name des neuen Betriebsmode:"></asp:Label>
    <asp:TextBox ID="TextBoxName" runat="server" MaxLength="45"></asp:TextBox>
    <br />
    <asp:Button ID="btÜbernehmen" CssClass="SaveButton" runat="server" Text="Übernehmen" OnClick="btÜbernehmen_Click" />
    <asp:Button ID="btAbbrechen" CssClass="DeleteButton" runat="server" Text="Abbrechen" OnClick="btAbbrechen_Click" />
    <asp:Label ID="lbError" runat="server" Text="Die Länge der Bezeichnung ist auf 45 Zeichen begrenzt!" ForeColor="Red" Visible="false"></asp:Label>
    <br /><br />
    
    <asp:Label ID="lbWarnung" runat="server" Text="WARNUNG!<br />Durch das Löschen dieses Betriebsmode gehen auch alle dazu festgelegten Dateien und Einstellungen verloren!<br />Möchten Sie wirklich fortfahren?" ForeColor="Red" Visible="false"></asp:Label>


    </asp:Content>