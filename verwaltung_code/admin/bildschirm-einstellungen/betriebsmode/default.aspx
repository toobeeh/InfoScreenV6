<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.bildschirm_einstellungen.betriebsmode._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Betriebsmode verwalten</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1 runat="server" id="überschrift">Betriebsmode verwalten</h1>
    <hr />
    <br />
   
    <asp:Table ID="tbBildschirme" runat="server" CssClass="table"></asp:Table>
    <br />
    <asp:Button ID="btÜbernehmen" CssClass ="SaveButton" runat="server" Text="Übernehmen" OnClick="btÜbernehmen_Click" />
    <asp:Button ID="btAbbrechen" CssClass="DeleteButton" runat="server" Text="Abbrechen" OnClick="btAbbrechen_Click" />
        
    <br />
    <hr />

    <div>
        <h2>Dateien</h2>
        <asp:Button ID="btNeueDatei" CssClass="ActionButton" runat="server" Text="Neue Datei" OnClick="btNeueDatei_Click" />
        <br /><br />
        <asp:Table ID="tbDateien" runat="server" CssClass="table"></asp:Table>

    </div>
    <br /><hr /><br />
    <asp:Button ID="btFertig" CssClass="ActionButton" runat="server" Text="Fertig" OnClick="btFertig_Click" />
    </asp:Content>