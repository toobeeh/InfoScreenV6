<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.lehrer._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Bildschirmanzeige Einstellungen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Lehrereigenschaften</h1>
    Abteilung: 
    <div class="selectWrap">
        <asp:DropDownList ID="dropDownAbteilung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownAbteilung_SelectedIndexChanged"></asp:DropDownList>
    </div>
    <asp:Button ID="btnSave" CssClass="SaveButton" runat="server" Text="Speichern" OnClick="btnSave_Click" Visible="false" Style="float:right" ToolTip="Änderungen abspeichern" />
    <br/><hr/>
    <asp:Label ID="lbError" runat="server" Text="Error" ForeColor="Red" Visible="false" /><br/>
    <asp:Button ID="btnAdd" CssClass="ActionButton" runat="server" Text="Hinzufügen" OnClick="btnAdd_Click" style="float:left" ToolTip="neuen Lehrer hinzufügen" />
    <asp:Button ID="btnChange" CssClass="ActionButton" runat="server" Text="Bearbeiten" OnClick="btnChange_Click" style="float:left; margin-left: 5px;" ToolTip="bestehende Lehrer bearbeiten" />
    <asp:Button ID="SprechstundenUpdate" CssClass="ActionButton" runat="server" Text ="Sprechstunden Update" ToolTip="Lehrereigenschaften inkl. Sprechstunden aus aktuellster Datei updaten" style="float:right" OnClick="SprechstundenUpdate_Click" />
        <br /> <br />
    
    <asp:Table ID="TischLE" runat="server" CssClass="table" />
</asp:Content>
