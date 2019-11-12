<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="modify.aspx.cs" Inherits="Infoscreen_Verwaltung.conf.tests.modify" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Test / Schularbeit erstellen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">

    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <script type ="text/javascript">
        $(function () {
            $('.autocompFach').autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        url: "modify.aspx/GetFach",
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

            $('.autocompLehrer').autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        url: "modify.aspx/GetLehrer",
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
    <h1>
        <asp:Label ID="lb_SA" runat="server" Text="Label"></asp:Label>
    </h1>
    <br /><hr /><br />
    <asp:Label ID="LabelFehler" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div>

        <asp:Calendar ID="datum" runat="server" CssStyle="float:left"></asp:Calendar>

        <br />

         <asp:Table runat="server" style="margin-right: 2px;" Width="398px">      
            <asp:TableRow>
                <asp:TableCell>Stunde:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="tbStunde" runat="server" MaxLength="2"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Fach:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="tbFach" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Dauer (h):</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="tbDauer" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ToolTip="Mehrere Lehrer durch '/' trennen.">
                <asp:TableCell>Lehrerkürzel:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="tbLehrer" runat="server"></asp:TextBox>
                
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow></asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Art:</asp:TableCell>
                <asp:TableCell>
                    <div class="selectWrap" style="margin-left:0">
                        <asp:DropDownList ID="dropdlTestart" runat="server"></asp:DropDownList>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell> Raum: </asp:TableCell>
                <asp:TableCell>
                    <div class="selectWrap" style="margin-left:0">
                        <asp:DropDownList ID="DropListRaum" OnSelectedIndexChanged="DropListRaum_SelectionChanged" runat ="server" AutoPostBack ="true" UpdateMode="Conditional"></asp:DropDownList>
                    </div>    
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow></asp:TableRow>
             <asp:TableRow>
                  <asp:TableCell> Ersatzraum: </asp:TableCell>
                 <asp:TableCell> <asp:TextBox Enabled="false" ID ="tbErsatzraum" runat ="Server"></asp:TextBox> </asp:TableCell>
             </asp:TableRow>
        </asp:Table>

    </div>
  
    <br /><hr /><br />
    <asp:Button ID="btÜbernehmen" CssClass="SaveButton" runat="server" Text="Übernehmen" OnClick="btÜbernehmen_Click" /> 
</asp:Content>
