<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.conf.supplierungen._default" %>
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
    <h1>Verschiebung hinzufügen</h1>
     <asp:Table ID="TableLoginForm" runat="server">
        <asp:TableRow>
            <asp:TableCell>Abteilung: </asp:TableCell>
            <asp:TableCell>
                <div class="selectWrap">
                    <asp:DropDownList ID="DropDownListAbteilung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListAbteilung_SelectedIndexChanged"></asp:DropDownList>
                </div>
             </asp:TableCell>

        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Klasse: </asp:TableCell>
            <asp:TableCell>
               <div class="selectWrap">
                   <asp:DropDownList ID="DropDownListKlasse" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListKlasse_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <hr/>
    <h3><asp:Label ID="SupplierungKlasse" runat="server"></asp:Label></h3>

    <div style="float:left">
        <asp:Label runat="server" Text="Verschiebt von:" style="margin-left:53px"></asp:Label>
        <asp:Calendar ID="Calendar1" runat="server" Height="99px" Width="241px" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
    </div> 
    <div style="float:left; margin-left:15px; margin-top:24px">
        <asp:Table ID="tbStundenplanVon" runat="server" CssClass="tableSupplierung"></asp:Table>
    </div>
    <div class="clear"></div>
    <br />
    <div style="float:left" >
        <asp:Label runat="server" Text="Verschiebt auf:" style="margin-left:53px"></asp:Label>
        <asp:Calendar ID="Calendar2" runat="server" Height="99px" Width="241px" OnSelectionChanged="Calendar2_SelectionChanged"></asp:Calendar>
    </div> 
    <div style="float:left; margin-left:15px; margin-top:24px">
        <asp:Table ID="tbStundenplanAuf" runat="server" CssClass="tableSupplierung"></asp:Table>
    </div>
    <div class="clear"></div>     
    <asp:Label runat="server" ID="lbHinweis" Visible ="false" ForeColor="Green" >Verschiebung wurde erfolgreich eingetragen!</asp:Label>
    <br /><br /><hr />
     <div style="float:left">
        <asp:Button ID="Zurueck" CssClass="DeleteButton" runat="server" Text="Zurück" Width="90px" OnClick="Zurueck_Click" />
    </div>
    <div style="margin-left:125px">
        <asp:Button ID="Bestaetigen" CssClass="SaveButton" runat="server" Text="Bestätigen" OnClick="Bestaetigen_Click" Width="90px" />
    </div>  
</asp:Content>