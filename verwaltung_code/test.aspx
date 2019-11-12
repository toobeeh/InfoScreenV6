<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Infoscreen_Verwaltung.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="ID:"></asp:Label>
        <asp:TextBox ID="tb_id" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="abfrage" runat="server" Text="Abfragen" OnClick="abfrage_Click" />
        <br /><br /><hr />
        <h2>Aktuelle Stunde:</h2>
        <asp:Label ID="lb_stunde" runat="server" Text="Label"></asp:Label>
        <br /><br /><hr />
        <h2>Anzuzeigende Elemente:</h2>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" Enabled="False">
            <asp:ListItem Text="Stundenplan"></asp:ListItem>
            <asp:ListItem Text="Sprechstunden"></asp:ListItem>
            <asp:ListItem Text="Raumaufteilung"></asp:ListItem>
            <asp:ListItem Text="Supplierplan"></asp:ListItem>
            <asp:ListItem Text="Abteilungsinfo"></asp:ListItem>
            <asp:ListItem Text="Aktuelle Supplierungen"></asp:ListItem>
            <asp:ListItem Text="PowerPoint"></asp:ListItem>
        </asp:CheckBoxList>
        <br /><br /><hr />
        <h2>Klasseninfo:</h2>
        <asp:Label ID="lb_klasseninfo" runat="server" Text=""></asp:Label>
        <br /><br /><hr />
        <h2>Rauminfo:</h2>
        <asp:BulletedList ID="BulletedList1" runat="server">
            <asp:ListItem Text="Stammklasse: "></asp:ListItem>
            <asp:ListItem Text="Abteilung: "></asp:ListItem>
            <asp:ListItem Text="Raumnummer: "></asp:ListItem>
            <asp:ListItem Text="Gebäude: "></asp:ListItem>
            <asp:ListItem Text="Aktuelle Klasse: "></asp:ListItem>
            <asp:ListItem Text="Klassenvorstand: "></asp:ListItem>
            <asp:ListItem Text="Nicht Stören: "></asp:ListItem>
        </asp:BulletedList>
        <br /><br /><hr />
        <h2>Abteilungsinfo:</h2>
        <asp:Label ID="lb_abteilungsinfo" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
