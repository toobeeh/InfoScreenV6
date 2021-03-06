﻿<%@ Page Language="C#" EnableEventValidation="false"  MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.theme.Theme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Farbschema</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">

    <script type="text/javascript" src="/scripts/farbtastic/farbtastic.js"></script>
    <link rel="stylesheet" href="/scripts/farbtastic/farbtastic.css" type="text/css" />
    <script>
        $(document).ready(function () {
            jQuery.farbtastic('#col_picker').linkTo('#Content_prev');
        });
    </script>

    <div id="picker_container" runat="server"></div> 

    <h1>Farbschema anpassen</h1>
    Die Darstellung der dynamischen Seiten auf den InfoScreen-Bildschirmen kann via Farbthemen angepasst werden. <br />
    Die Themes "Light" und "Dark" sind voreingestellt, zusätzlich können auch eigene Themes definiert werden.
    Um ein einheitliches Design zu erlangen, sollten die verwendeten PowerPoints bestmöglich zu dem verwendeten Theme passen. <br />
    <h3>Achtung: Auf IE11 werden CSS-Variablen, die für die Themes notwendig sind, nicht unterstützt. Es wird als Fallback-Design das Dark-Theme verwendet.</h3>
    <hr />
    <br />
    <div>
        <asp:Table ID="ThemeBuilder" runat="server" CssClass="table">
            <asp:TableRow CssClass="head">
                <asp:TableCell CssClass="head" ID="TitleCell">
                    Neues Theme
                </asp:TableCell>
                <asp:TableCell>
                    Preset:
                </asp:TableCell>
                <asp:TableCell>
                    <div class="selectWrap">
                        <asp:DropDownList runat="server" ID="DropdownPreset" AutoPostBack="True" OnSelectedIndexChanged="PresetChanged" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>            
        </asp:Table>
        <asp:Table ID="Themes" runat="server" CssClass="table" />
    </div>
    


    
  
</asp:Content>


