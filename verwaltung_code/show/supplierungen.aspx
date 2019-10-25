<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="supplierungen.aspx.cs" Inherits="Infoscreen_Verwaltung.show.supplierungen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Supplierungen anzeigen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Supplierungen anzeigen</h1>
    Abteilung: 
    <div class="selectWrap">
        <asp:DropDownList ID="dropDownAbteilung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownAbteilung_SelectedIndexChanged"></asp:DropDownList>
    </div>
    <br /><hr />
    <asp:Panel ID="cont" runat="server"></asp:Panel>
</asp:Content>
