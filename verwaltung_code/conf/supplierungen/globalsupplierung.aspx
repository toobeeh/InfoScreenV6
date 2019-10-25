<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="globalsupplierung.aspx.cs" Inherits="Infoscreen_Verwaltung.conf.supplierungen.globalsupplierung" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Supplierungen bearbeiten</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Lehrerkonferenz hinzufügen</h1>
     <asp:Table ID="TableLoginForm" runat="server">
        <asp:TableRow>
            <asp:TableCell>Abteilung: </asp:TableCell>
            <asp:TableCell>
                <div class="selectWrap">
                    <asp:DropDownList ID="DropDownListAbteilung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListAbteilung_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <hr />
    <h3><asp:Label ID="GlobaleSupplierung" runat="server">Lehrerkonferenz eintragen</asp:Label></h3>
    <div> 
        <asp:Table ID="TableSupplierungEintragen" runat="server">
            <asp:TableRow ID ="row0">
                <asp:TableCell ID="cell0">Datum: </asp:TableCell>
                <asp:TableCell><asp:TextBox runat="server" ID ="tb0" Enabled="false"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID ="row1">
                <asp:TableCell>Von Stunde: </asp:TableCell>
                <asp:TableCell><asp:TextBox runat="server" ID ="tb1"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID ="row2">
                <asp:TableCell>Bis Stunde: </asp:TableCell>
                <asp:TableCell><asp:TextBox runat="server" ID ="tb2" CssClass="autocompLehrer"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
        </asp:Table>  
    </div>
    <br />
    <div  >
        <asp:Calendar ID="Calendar1" runat="server" Height="99px" Width="241px" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
    </div> 

    <br />   
    <asp:Label runat="server" ID="lbHinweis" Visible ="false" ForeColor="Green" >Konferenz wurde erfolgreich eingetragen!</asp:Label>
    <br /><hr />
     
    <asp:Button ID="Zurueck" CssClass="DeleteButton" runat="server" Text="Zurück" Width="90px" OnClick="Zurueck_Click" />
    <asp:Button ID="Bestaetigen" CssClass="SaveButton" runat="server" Text="Bestätigen" OnClick="Bestaetigen_Click" Width="90px" />
   
</asp:Content>