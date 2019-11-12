<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="lehrersupplierung.aspx.cs" Inherits="Infoscreen_Verwaltung.conf.supplierungen.lehrersupplierung" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Supplierungen bearbeiten</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(function () {
            $('.autocompLehrer').autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        url: "lehrersupplierung.aspx/GetLehrer",
                        data: "{ 'pre':'" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return { value: item }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                }
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
    <h1>Supplierungen hinzufügen</h1>
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
            <asp:TableCell>Lehrer: </asp:TableCell>
            <asp:TableCell>
                <div class="selectWrap">
                    <asp:DropDownList ID="DropDownListLehrer" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListLehrer_SelectedIndexChanged"></asp:DropDownList>
                 </div>   
            </asp:TableCell>
        
            <asp:TableCell>Grund: </asp:TableCell>
            <asp:TableCell><asp:TextBox ID="tbGrund" runat="server"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <hr />
    <h3><asp:Label ID="SupplierungKlasse" runat="server"></asp:Label></h3>
    <div style="float:left">
        <asp:Calendar ID="Calendar1" runat="server" Height="99px" Width="241px" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
    </div> 
    <div style="margin-left:30px; float:left">
        <asp:Table ID="tbLehrerStundenplan" runat="server" CssClass="tableSupplierung"></asp:Table>
    </div>
    <div class="clear"></div>     
    <asp:Label runat="server" ID="lbHinweis" Visible ="false" ForeColor="Green" >Supplierung wurde erfolgreich eingetragen!</asp:Label>
    <br /><br /><hr />
     <div style="float:left">
        <asp:Button ID="Zurueck" CssClass="DeleteButton" runat="server" Text="Zurück" Width="90px" OnClick="Zurueck_Click" />
    </div>
    <div style="margin-left:125px">
        <asp:Button ID="Bestaetigen" CssClass="SaveButton" runat="server" Text="Bestätigen" OnClick="Bestaetigen_Click" Width="90px" />
    </div>  
</asp:Content>