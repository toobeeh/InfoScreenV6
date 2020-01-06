<%@ Page Language="C#" EnableEventValidation="false"  MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.theme.Theme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Willkommen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">

    <div id="picker_container" runat="server"></div> 

    <h1>Farbschema anpassen</h1>
    Die Darstellung der dynamischen Seiten auf den InfoScreen-Bildschirmen kann via Farbthemen angepasst werden. <br />
    Die Themes "Light" und "Dark" sind voreingestellt, zusätzlich können auch eigene Themes definiert werden.
    Um ein einheitliches Design zu erlangen, sollten die verwendeten PowerPoints bestmöglich zu dem verwendeten Theme pasen. <br />
    <br />
    <hr />
    <br />
    <div>
        <asp:Table ID="ThemeBuilder" runat="server" CssClass="table">
            <asp:TableRow CssClass="head">
                <asp:TableCell CssClass="head">
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


