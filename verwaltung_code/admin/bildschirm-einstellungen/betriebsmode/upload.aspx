<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="upload.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.bildschirm_einstellungen.betriebsmode.upload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - PowerPoint hochladen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">



    <h1>PowerPoint Datei hochladen</h1>
    <hr /><br />
    <asp:FileUpload onchange="updateText()" ID="file" runat="server" CssClass="ActionButton" Accept="application/vnd.ms-powerpoint, application/vnd.openxmlformats-officedocument.presentationml.presentation"/>   
    <label for="Content_file" class="FileInput">PowerPoint auswählen </label>
    <br /><br />
    Dateiname: <asp:TextBox ID="tbName" runat="server" MaxLength="45"></asp:TextBox><br /><br />
    <asp:Label ID="lbFehler" runat="server" Text="" ForeColor="Red"></asp:Label>
    <br /><hr /><br />
    <asp:Button ID="btHochladen" CssClass="SaveButton" runat="server" Text="Hochladen" OnClick="btHochladen_Click" />
    <asp:Button ID="btAbbrechen" CssClass="DeleteButton" runat="server" Text="Abbrechen" OnClick="btAbbrechen_Click" />
    </asp:Content>