<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="abteilungsinfo.aspx.cs" Inherits="Infoscreen_Verwaltung.conf.abteilungsinfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Abteilungsinfo bearbeiten</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../scripts/bbc/wbbtheme.css" type="text/css" media="all" />
	<script src="../scripts/bbc/jquery.wysibb.min.js"></script>
	<script src="../scripts/bbc/de.js"></script>
    <script>
        $(function () {
            $("textarea").wysibb({
                buttons: "bold,italic,underline,|,fontcolor,fontsize,fontfamily",
                lang: "de"
            });
        });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Abteilungsinfo bearbeiten</h1>
    Abteilung: 
    <div class="selectWrap">
        <asp:DropDownList ID="dropDownAbteilung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownAbteilung_SelectedIndexChanged"></asp:DropDownList>
    </div>
    <br /><hr /><br />
    <textarea id="editor" runat="server" style="width:100%; height:400px;"></textarea>
    <br /><hr /><br />
    <asp:Button ID="btÜbernehmen" CssClass="SaveButton" runat="server" Text="Übernehmen" OnClick="btÜbernehmen_Click" /> <asp:Button ID="btReset" CssClass="DeleteButton" runat="server" Text="Zurücksetzen" OnClick="btReset_Click" />
</asp:Content>
