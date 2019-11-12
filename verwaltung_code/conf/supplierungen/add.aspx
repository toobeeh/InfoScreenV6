<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="add.aspx.cs" Inherits="Infoscreen_Verwaltung.conf.supplierungen.add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Supplierung erstellen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Supplierung hinzufügen</h1>
    <asp:Label ID="lbError" runat="server" Text="<ul><li>Das angegebene Lehrer Kürzel ist leider kein gültiges Kürzel</li></ul>" ForeColor="Red" Visible ="false"></asp:Label>
    Lehrer Kürzel: <asp:TextBox ID="tbLehrer" runat="server" OnTextChanged="tbLehrer_TextChanged"></asp:TextBox>
    <br />
    <asp:Label ID="lbName" runat="server"></asp:Label>
    <br />
    <asp:Table ID="tbDaten" CssClass="table" runat="server">
        <asp:TableRow>
            <asp:TableCell CssClass="head" ColumnSpan="2" HorizontalAlign="Center">von</asp:TableCell>
            <asp:TableCell CssClass="head" ColumnSpan="2" HorizontalAlign="Center">bis</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell CssClass="head" ColumnSpan="2" HorizontalAlign="Center">
                <asp:Calendar ID="calendar" runat="server" BackColor="White" OnSelectionChanged="calendar_SelectionChanged"></asp:Calendar>
            </asp:TableCell>
            <asp:TableCell CssClass="head" ColumnSpan="2" HorizontalAlign="Center">
                <asp:Calendar ID="calendar1" runat="server" BackColor="White" ></asp:Calendar>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell CssClass="head" >Grund:</asp:TableCell>
            <asp:TableCell CssClass="head" ColumnSpan="3"><asp:TextBox ID="tbGrund" runat="server" ></asp:TextBox></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    <br /><hr /><br />
    <asp:Button ID="btÜbernehmen" runat="server" Text="Übernehmen" OnClick="btÜbernehmen_Click" /> <asp:Button ID="btAbbruch" runat="server" Text="Abbrechen" OnClick="btAbbruch_Click" />
</asp:Content>