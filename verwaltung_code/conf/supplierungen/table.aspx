<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="table.aspx.cs" Inherits="Infoscreen_Verwaltung.conf.supplierungen.table" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Supplierung erstellen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Supplierungen bearbeiten</h1>
    Abteilung: 
     <div class="selectWrap">
          <asp:DropDownList ID="dropDownAbteilung" runat="server" AutoPostBack="True"></asp:DropDownList>
     </div>
    <hr />
    <table style="width:100%">
        <tr>
            <td style="width:13%; text-align:right">Sortieren nach:</td>
            <td style="width:29%">
                    <asp:RadioButtonList ID="Sortieren" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="Sortieren_SelectedIndexChanged">
                        <asp:ListItem Text="Lehrer" Selected="True">Lehrer</asp:ListItem>
                        <asp:ListItem Text="Ersatzlehrer">Ersatzlehrer</asp:ListItem>
                        <asp:ListItem Text="Datum">Datum</asp:ListItem>
                        <asp:ListItem Text="Klasse">Klasse</asp:ListItem>                      
                    </asp:RadioButtonList>
            </td>
            <td style="width:28%; text-align:right">
                <asp:Button ID="btLehrerSupp" CssClass="ActionButton" runat="server" AutoPostBack="True" Text="Lehrer Supplierung" OnClick="btLehrerSupp_Click" />
                <asp:Button ID="btHinzufuegen" CssClass="ActionButton" runat="server" AutoPostBack="True" Text="Verschiebung" OnClick="btHinzufuegen_Click" />
                <asp:Button ID="btGlobalEntfall" CssClass="ActionButton"  Width="130px" runat="server" AutoPostBack="True" Text="Lehrerkonferenz" OnClick="btGlobalEntfall_Click"  />
            </td>
        </tr>
    </table>
    
    <hr />
    <asp:Panel ID="cont" runat="server"></asp:Panel>
    <br />
    <hr />
    <br />
    <asp:Button ID="ExcelButton" CssClass="ActionButton" runat="server" Text="Excel" ToolTip="Excel Tabelle erstellen" OnClick="ExcelButton_Click"/>
    <br />
    <asp:Label ID="LabelNachricht" runat="server"></asp:Label>
</asp:Content>