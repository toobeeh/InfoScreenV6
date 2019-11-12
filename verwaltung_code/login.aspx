<%@ Page Title="" Language="C#" MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Infoscreen_Verwaltung.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Login</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <h1>Benutzer-Login</h1>
    Für den Zugriff ist ein gültiges Konto an der HTL-MD.schule - Domäne erforderlich. <br />
    Wenn sich das berechtigte Konto in einer anderen Domäne befindet, muss dies mit // oder @ im Benutzernamen angegeben werden. <br /><br />
    <hr /><br />
    
    <asp:Table ID="TableLoginForm" runat="server" Width="30%">
        <asp:TableRow>
            <asp:TableHeaderCell Style="text-align:left;"><asp:Label ID="LabelUser" runat="server" Text="Benutzername " ></asp:Label></asp:TableHeaderCell>
            <asp:TableCell><asp:TextBox ID="TextBoxUser"  style="width:100%" runat="server"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell Style="text-align:left;"><asp:Label ID="LabelPassword" runat="server" Text="Kennwort "></asp:Label></asp:TableHeaderCell>
            <asp:TableCell><asp:TextBox ID="TextBoxPassword" style="width:100%" TextMode="Password" runat="server"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell>
                <br />
                <asp:Button ID="ButtonLogin" CssClass="SaveButton" Style="width:100%" runat="server" Text="Einloggen" OnClick="ButtonLogin_Click" />
            </asp:TableCell>
        </asp:TableRow>
       
    </asp:Table>
     <br />
    <asp:Label ID="Error" runat="server" Text="Die Kombination aus Benutzername und Kennwort existiert nicht" ForeColor="Red" Visible="False"></asp:Label><br />
        
    
</asp:Content>