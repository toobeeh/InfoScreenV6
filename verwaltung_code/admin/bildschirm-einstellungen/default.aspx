<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.bildschirm_einstellungen._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Bildschirmanzeige Einstellungen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Bildschirmanzeige Einstellungen</h1>
    Abteilung: 
    <div class="selectWrap">
        <asp:DropDownList ID="dropDownAbteilung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownAbteilung_SelectedIndexChanged"></asp:DropDownList>
    </div>
        <br/><hr/><br/>
    Betriebsmode: 
    <div class="selectWrap">
        <asp:DropDownList ID="dropDownBetriebsmode" runat="server"></asp:DropDownList>
    </div>
    <asp:Button ID="btÜbernehmen" CssClass="SaveButton" runat="server" Text="Anzeigen" OnClick="btÜbernehmen_Click" ToolTip="ausgewählten Betriebsmode auf Monitoren anzeigen" />
    <asp:Button ID="btBetriebsmodeEinstellungen" CssClass="ActionButton" runat="server" Text="Einstellungen zum Betriebsmode" OnClick="btBetriebsmodeEinstellungen_Click" ToolTip="Einstellungen zum ausgewählten Betriebsmode ändern" />
    
    <br/><hr/><br/>
    <asp:Button ID="btLöschen" runat="server" CssClass="DeleteButton" Text="Betriebsmode Löschen" Visible="false" OnClick="btLöschen_Click" ToolTip="ausgewählten Betriebsmode löschen" />
    <asp:Button ID="btNeu" runat="server" CssClass="ActionButton" Text="Neuer Betriebsmode" Visible="false" OnClick="btNeu_Click" ToolTip="neuen Betriebsmode hinzufügen" />
    <asp:Button ID="btÄndern" runat="server" CssClass="ActionButton" Text="Betriebsmode Umbenennen" Visible="false" OnClick="btÄndern_Click" ToolTip="ausgewählten Betriebsmode umbenennen" />
    

    <br/><hr/>
    <h1>Zeitgesteuerte Anzeige</h1>
    <br />

    <div style="width:100%;" >
      
        <div id="Links" style="width:60%; float:left">
            <asp:Table ID="TischZGA" runat="server" CssClass="table" />
        </div>
        <div id="Rechts" style="width:30%; float:left; margin-left: 6%">
            <table>
                <tr>
                    <td><asp:Label ID="lbKalender" runat="server" Text="" /></td>
                </tr>

                <tr>
                    <td><asp:Calendar ID ="Kalender" runat="server" Width="225px" SelectionMode="Day" /></td>
                </tr>

                <tr>
                    <td>Uhrzeit: <asp:TextBox ID="tbZeit" runat="server" Width="35px" Style="text-align:center" Text="" /></td>
                </tr>

                <tr>
                    <td>Betriebsmodus: 
                        <div class="selectWrap">
                            <asp:DropDownList ID="dropDownBetriebsmode2" runat="server"/>
                        </div>
                    </td>
                       
                </tr>
                <tr>
                    <td><asp:Button ID="btnSave" CssClass="SaveButton" runat="server" Text="Speichern" OnClick="btnSave_Click" /></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbError" runat="server" Text="Error" ForeColor="Red" Visible="false" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
