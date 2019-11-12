
<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.raeume._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Raumeigenschaften</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <div id="picker_container" runat="server"/>  
    <h1>Raumeigenschaften</h1>
     Abteilung: 
    <div class="selectWrap">
        <asp:DropDownList ID="dropDownAbteilung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownAbteilung_SelectedIndexChanged"></asp:DropDownList>
    </div>
    <hr />

    <h3>Um Räume / Bildschirme hinzuzufügen muss per SSMS manuell ein Datenbank-Eintrag in "Räume" und "Bildschirme" gemacht werden.</h3>
    <asp:Table ID="tableRaeume" runat="server" CssClass="table"/>
    
</asp:Content>

