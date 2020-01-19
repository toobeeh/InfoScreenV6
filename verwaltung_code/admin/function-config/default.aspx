<%@ Page Language="C#" EnableEventValidation="false"  MasterPageFile="~/style/MasterPage.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infoscreen_Verwaltung.admin.FeatureConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titel" runat="server">Infoscreen - Funktionseinstellungen</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopBar" runat="server">
    <asp:Label ID="TopMenu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBar" runat="server">
    <asp:Label ID="Menu" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">

    <script src="../../scripts/jQuery-1.11.1.js"></script>
    <script src="../../scripts/jquery-ui-1.11.1/jquery-ui.js"></script>
    <script src="../../scripts/featureInput.js"></script>

    <asp:HiddenField  runat="server" ID="switchTimeMs"/>
    <asp:HiddenField  runat="server" ID="animateUpcomingExamLessons"/>
    <asp:HiddenField  runat="server" ID="showExamTimeMs"/>
    <asp:HiddenField  runat="server" ID="examTimesShown"/>
    <asp:HiddenField  runat="server" ID="examAnimationDuration"/>
    <asp:HiddenField  runat="server" ID="displayClockTile"/>
    <asp:HiddenField  runat="server" ID="displayClassDetailsTile"/>
    <asp:HiddenField  runat="server" ID="displayUpcomingExamsTile"/>
    <asp:HiddenField  runat="server" ID="markActiveLesson"/>

    <h1>Funktionen anpassen</h1>
    <h3>Achtung: IE11 unterstützt viele Funktionen nicht. Es wird empfohlen einen aktuellen Browser zu verwenden.</h3>
    <hr />
    <br />
    <div id="feature_settings">
        
        <asp:Table ID="SettingsTable" runat="server" CssClass="table">
            <asp:TableRow CssClass="head">

                <asp:TableCell CssClass="head" Width="20%" >
                    Einstellungen
                </asp:TableCell>

                <asp:TableCell CssClass="head" Width="35%"  HorizontalAlign="Left" >                   
                </asp:TableCell>

                <asp:TableCell CssClass="head" Width="45%" HorizontalAlign="Left">    
                   <asp:Button ID="BtSave" OnClick="BtSave_Click" style="float:right; margin-right: 20px" CssClass="SaveButton"  runat="server" AutoPostBack="True" Text="Speichern"/>
                   <asp:Button ID="BtCancel" OnClick="BtCancel_Click" style="float:right; margin-right: 20px" Cssclass="DeleteButton" runat="server" AutoPostBack="True" Text="Abbrechen"/>
                </asp:TableCell>
                
            </asp:TableRow>    
           
           
        </asp:Table>
    </div>
    
    <br />
    


    
  
</asp:Content>


