<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="Inventaris.aspx.cs" Inherits="presentationWeb.Inventaris" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventaris Rekencentra 2020</title>

    <!-- CSS -->
    <link rel="stylesheet" href="StyleSheet.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

    <style type="text/css">
        .auto-style9 {
            width: 538px;
            height: 58px;
        }

        .newStyle2 {
            font-family: bahnschrift;
            font-size: xx-large;
            font-weight: bold;
            font-style: normal;
            font-variant: normal;
            text-transform: capitalize;
            color: #1D9F75;
        }

        .auto-style10 {
            height: 30px;
        }

        .auto-style11 {
            height: 20px;
        }

        .auto-style12 {
            height: 20px;
            width: 538px;
        }
    </style>

</head>

<body>
    <header class="header">
        <table>
            <tr>
                <td class="auto-style4">
                    <img alt="Afbeelding" class="auto-style3" src="logo.png" />
                </td>
                <td class="auto-style5">
                    <h1 class="headertekst">&nbsp; Inventarissysteem Rekencentra </h1>
                </td>
            </tr>
        </table>
    </header>

    <div id="content" class="content">
        <form id="inventaris" runat="server">
            <div id="controlPanel">
                <!-- form start -->
                <asp:Panel ID="PanelControls" runat="server">
                    <p>
                        <span>
                            <!-- Panel controlls -->
                        </span>
                    </p>
                    <div>
                        <ajaxToolkit:TabContainer ID="TabControl" runat="server" CssClass="TabStyle" ActiveTabIndex="0" Width="75%">
                            <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                                <HeaderTemplate>
                                    Apparaat
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <span class="newStyle2"><strong>Apparaat toevoegen</strong></span>
                                    <br />
                                    <br />
                                    <div id="apptoevoegen1" style="float: left; width: 300px;">
                                        Type:&nbsp;<asp:DropDownList ID="DDLType" runat="server" AutoPostBack="True" DataTextField="omschrijving" DataValueField="omschrijving" Width="110px" CssClass="mydropdownlist">
                                        </asp:DropDownList>
                                        &nbsp;<asp:Label ID="lblTypeError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                        <br />
                                        <br />
                                        Merk:
                                    <asp:DropDownList ID="DDLMerk" runat="server" DataTextField="omschrijving" DataValueField="omschrijving" Width="110px" AutoPostBack="True" CssClass="mydropdownlist">
                                    </asp:DropDownList>
                                        &nbsp;<asp:Label ID="lblMerkError" runat="server" CssClass="auto-style10" Visible="False" Style="color: #FF0000"></asp:Label>
                                        <br />
                                        <br />
                                        Model:
                                    <asp:DropDownList ID="DDLModel" runat="server" DataTextField="omschrijving" DataValueField="omschrijving" Width="110px" CssClass="mydropdownlist">
                                    </asp:DropDownList>
                                        &nbsp;<asp:Label ID="lblModelError" runat="server" CssClass="auto-style10" Visible="False" Style="color: #FF0000"></asp:Label>
                                        <br />
                                        Serienummer:
                                    <asp:TextBox ID="TxtSerienummer" runat="server" placeholder="Serie nummer" Width="140px" />
                                        <br />
                                        <br />
                                        <asp:Button ID="btnApparaatNew" runat="server" class="btnStyle" OnClick="Insert_Apparaat" Text="Toevoegen" />
                                    </div>
                                    <div id="apptoevoegen2" style="float: left">
                                        <asp:CheckBox ID="CBStock" runat="server" Checked="True" AutoPostBack="True" OnCheckedChanged="Change_gebruiker_state" Text="In Stock?" />
                                        <br />
                                        <asp:DropDownList ID="DDLGebruiker" Enabled="False" runat="server" DataTextField="naam" DataValueField="naam" CssClass="mydropdownlist">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        Leverancier:
                                    <asp:DropDownList ID="DDLLeverancier" runat="server" DataTextField="naam" DataValueField="naam" Width="110px" CssClass="mydropdownlist">
                                        <asp:ListItem />
                                    </asp:DropDownList>
                                        &nbsp;<asp:Label ID="lblLeverancierError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                        <br />
                                        Factuurnummer:
                                    <asp:TextBox ID="TxtFactuurnummer" runat="server" placeholder="factuur nummer..."></asp:TextBox>
                                        <br />
                                        <br />
                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                                <HeaderTemplate>
                                    Werknemer
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <h6><span class="newStyle1"><strong><span class="newStyle2">Werknemer toevoegen</span></strong></span></h6>
                                    <p>
                                        &nbsp;
                                    </p>
                                    Naam:
                                    <asp:TextBox ID="txtWerknemerNaam" runat="server" placeholder="Naam"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblWerknemerNaamError" runat="server" CssClass="auto-style10" Visible="False" Style="color: #FF0000"></asp:Label>
                                    <br />
                                    Locatie:
                                    <asp:DropDownList ID="DDLLocatie" runat="server" DataTextField="omschrijving" DataValueField="omschrijving" CssClass="mydropdownlist">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Label ID="lblWerknemerLocatieError" runat="server" CssClass="auto-style10" Visible="False" Style="color: #FF0000"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnWerknemerNew" runat="server" class="btnStyle" OnClick="Insert_Werknemer" Style="text-align: justify" Text="Toevoegen" />
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                                <HeaderTemplate>
                                    Type
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <span class="newStyle1"><strong><span class="newStyle2">Type toevoegen</span><br />
                                    </strong></span>
                                    <br />
                                    Omschrijving:
                                    <asp:TextBox ID="txtTypeNew" runat="server" placeholder="Type" Width="180px"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblTypeOmschrijvingError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnTypeNew" runat="server" OnClick="btnTypeNew_Click" Text="Toevoegen" CssClass="btnStyle" />
                                    <br />
                                    <asp:Label ID="lblTypeNewInfo" runat="server" Visible="False"></asp:Label>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="TabPanel4">
                                <HeaderTemplate>
                                    Merk
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <span class="newStyle1"><strong><span class="newStyle2">Merk toevoegen</span><br />
                                    </strong></span>
                                    <br />
                                    Omschrijving:
                                    <asp:TextBox ID="txtMerkNew" runat="server" placeholder="Merk" Width="180px"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblMerkOmschrijvingError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnMerkNew" runat="server" OnClick="btnMerkNew_Click" Text="Toevoegen" CssClass="btnStyle" />
                                    <br />
                                    <asp:Label ID="lblMerkNewInfo" runat="server" Visible="False"></asp:Label>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="TabPanel5">
                                <HeaderTemplate>
                                    Model
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <span class="newStyle1"><strong><span class="newStyle2">Model toevoegen</span><br />
                                    </strong></span>
                                    <br />
                                    Omschrijving:
                                    <asp:TextBox ID="TxtModelNew" runat="server" placeholder="Model" Width="180px"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblModelOmschrijvingError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    Selecteer Type:
                                    <asp:DropDownList ID="DDLTypeConfirmation" runat="server" DataTextField="omschrijving" DataValueField="omschrijving" Width="110px" CssClass="mydropdownlist">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:Label ID="lblTypeErrorModel" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    Merk:
                                    <asp:DropDownList ID="DDLMerkConfirmation" runat="server" DataTextField="omschrijving" DataValueField="omschrijving" Width="110px" CssClass="mydropdownlist">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Label ID="lblMerkErrorModel" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnModelNew" runat="server" OnClick="btnModelNew_Click" Text="Toevoegen" CssClass="btnStyle" />
                                    <br />
                                    <asp:Label ID="lblModelConfirmation" runat="server" Visible="False"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblModelNewInfo" runat="server" Visible="False"></asp:Label>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="TabPanel6">
                                <HeaderTemplate>
                                    Leverancier
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <span class="newStyle1"><strong><span class="newStyle2">Leverancier toevoegen</span><br />
                                    </strong></span>
                                    <br />
                                    Naam:
                                    <asp:TextBox ID="txtLeverancierNaamNew" runat="server" placeholder="Naam leverancier"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblLeverancierNaamError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    Contact:
                                    <asp:TextBox ID="txtLeverancierContactNew" runat="server" placeholder="Contact leverancier"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblLeverancierContactError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    Email:
                                    <asp:TextBox ID="txtLeverancierEmailNew" runat="server" placeholder="Email leverancier"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblLeverancierEmailError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    Telefoonnummer:
                                    <asp:TextBox ID="txtLeverancierTeleNew" runat="server" placeholder="Telefoon leverancier"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblLeverancierTelefoonnummerError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnLeverancierNew" runat="server" OnClick="btnLeverancierNew_Click" Text="Toevoegen" CssClass="btnStyle" />
                                    <br />
                                    <asp:Label ID="lblLeverancierNewInfo" runat="server" Visible="False"></asp:Label>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel8" runat="server" HeaderText="TabPanel8">
                                <HeaderTemplate>
                                    Locatie
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <span class="newStyle1"><strong><span class="newStyle2">Locatie toevoegen</span><br />
                                    </strong></span>
                                    <br />
                                    Locatie:
                                    <asp:TextBox ID="txtLocatieWerknemerNew" runat="server" placeholder="locatie..."></asp:TextBox>
                                    <asp:Label ID="lblLocatieOmschrijvingError" runat="server" CssClass="auto-style10" Style="color: #FF0000" Visible="False"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnLocatieWerknemerNew" runat="server" OnClick="btnLocatieWerknemerNew_Click" Text="Toevoegen" CssClass="btnStyle" />
                                    <br />
                                    <asp:Label ID="lblLocatieNewInfo" runat="server"></asp:Label>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
                        <table class="controls">
                            <tr>
                                <td class="auto-style12">
                                    <!--Btn bekijk apparaten-->
                                    <asp:Button ID="btnBekijkApparaten" runat="server" CssClass="btnStyle" OnClick="ButtonBekijkApparaten_Click" Text="Apparaten" />

                                    <!--Btn bekijk Werknemers-->
                                    <asp:Button ID="btnBekijkWerknemers" runat="server" CssClass="btnStyle" OnClick="ButtonBekijkWerknemers_Click" Text="Werkenmers" />

                                    <!--Btn bekijk historiek-->
                                    <asp:Button ID="btnBekijkHistoriek" runat="server" CssClass="btnStyle" OnClick="ButtonBekijkHistoriek_Click" Text="Historiek" />

                                    <!--Btn bekijk Leveranciers-->
                                    <asp:Button ID="btnBekijkLeveranciers" runat="server" CssClass="btnStyle" OnClick="ButtonBekijkLeveranciers_Click" Text="Leveranciers" />
                                </td>
                                <td class="auto-style10">&nbsp;
                                <asp:Button ID="btnFilterAanUit" CssClass="btnStyle" runat="server" Text="Filters a/u" OnClick="Enable_Disable_FiltersH"></asp:Button>
                                    <asp:Button ID="btnFilterApparaten" runat="server" CssClass="btnStyle" OnClick="btnFilterApparaten_Click" Text="Filter" />
                                    &nbsp;
                                    <asp:Button ID="btnZoeken" runat="server" CssClass="btnStyle" OnClick="ButtonZoeken_Click" Text="Zoeken" />
                                    <asp:TextBox ID="txtZoeken" runat="server" CssClass="auto-style13" placeholder="Zoeken naar .."></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style9">
                                    <asp:Button ID="btnMaakActief" runat="server" CssClass="btnStyle" OnClick="btnMaakActief_Click" Text="Actief" />
                                    <asp:Button ID="btnMaakInactief" runat="server" CssClass="btnStyle" OnClick="btnMaakInactief_Click" Text="Inactief" />
                                    <asp:Button ID="btnLaatInactiefZien" runat="server" CssClass="btnStyle" OnClick="btnLaatInactiefZien_Click" Text="Inactief tonen" Width="120px" />
                                    <asp:Button ID="btnUncheckAll" runat="server" CssClass="btnStyle" OnClick="UncheckAll_Click" Text="Uncheck All" />
                                    <asp:Button ID="btnCheckAll" runat="server" CssClass="btnStyle" OnClick="CheckAll_Click" Text="Check All" />
                                </td>
                                <td class="auto-style11">
                                    <asp:Button ID="BtnExport" runat="server" OnClick="BtnExport_Click" Enabled="true" Text="Export"  CssClass="btnStyle"/>

                                    <asp:Button ID="btnDelete" runat="server" Enabled="False" OnClick="DeleteButton_Click" Text="Delete" Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </asp:Panel>
                <!-- close panel controlls -->

            </div>
            <div id="Gridviews" class="gridview">
                <!-- Gridview aparaten -->
                <asp:GridView ID="ApparatenGridView" runat="server" Width="100%" DataKeyNames="id"
                    OnRowCancelingEdit="ApparatenGridView_RowCancelingEdit"
                    OnRowDeleting="ApparatenGridView_RowDeleting"
                    OnRowEditing="ApparatenGridView_RowEditing"
                    OnRowUpdating="ApparatenGridView_RowUpdating"
                    OnPageIndexChanging="ApparatenGridView_OnPaging"
                    OnSorting="ApparatenGridView_OnSorting"
                    OnRowCommand="ApparatenGridView_OnRowCommand"
                    EmptyDataText="Geen data"
                    CellPadding="4" AllowSorting="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" PageSize="25" AllowPaging="True" ForeColor="#333333" GridLines="Horizontal" Font-Names="Arial">
                    <Columns>

                        <asp:TemplateField ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:CheckBox ID="CBApparaten" runat="server" />
                            </ItemTemplate>

                            <ItemStyle Width="30px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:LinkButton ID="cmdHistoriek" runat="server" CausesValidation="false" CommandName="CommandHistoriek" Style="color: darkgreen;" class="btn btn-mini" Text=""><i class="fa fa-history" aria-hidden="true"></i></asp:LinkButton>
                            </ItemTemplate>

                            <ItemStyle Width="30px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="" Style="color: darkgreen;" class="btn btn-mini">
                                    <i class="fa fa-edit" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="" Style="color: darkgreen;" class="btn btn-mini">
                                    <i class="fa fa-plus" aria-hidden="true"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text=" " Style="color: darkred;" class="btn btn-mini">
                                    <i class="fa fa-close" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </EditItemTemplate>

                            <ItemStyle Width="30px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="id" HeaderText="ID" InsertVisible="False" ReadOnly="True" Visible="False" />
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="<%# Container.DataItemIndex %>" CausesValidation="false" CommandName="Meer_Informatie" Text="" Style="color: cornflowerblue;" class="btn btn-mini">
                                    <i class="fa fa-info-circle" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle Width="10px" />

                            <ItemStyle Width="30px"></ItemStyle>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Type" SortExpression="Type">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLTypeEdit" runat="server" DataTextField="omschrijving" DataValueField="omschrijving" DataSourceID="SDSTypeEdit" SelectedValue='<%# Bind("type") %>'>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SDSTypeEdit" runat="server" ConnectionString="<%$ ConnectionStrings:dbinventarisConnectionString %>" ProviderName="<%$ ConnectionStrings:dbinventarisConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT(omschrijving) FROM dbinventaris.tblType WHERE omschrijving IS NOT NULL AND omschrijving not like '' ORDER BY omschrijving asc;"></asp:SqlDataSource>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Type
                                <br />
                                <asp:TextBox Visible="false" ID="txtTypeFilter" runat="server" Height="16px" Width="92px"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("type") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Merk" SortExpression="Merk">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLMerkEdit" runat="server" DataTextField="omschrijving" DataValueField="omschrijving" DataSourceID="SDSMerkEdit" SelectedValue='<%# Bind("merk") %>'>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SDSMerkEdit" runat="server" ConnectionString="<%$ ConnectionStrings:dbinventarisConnectionString %>" ProviderName="<%$ ConnectionStrings:dbinventarisConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT(omschrijving) FROM dbinventaris.tblmerk WHERE omschrijving IS NOT NULL AND omschrijving not like '' ORDER BY omschrijving asc;"></asp:SqlDataSource>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Merk
                                <br />
                                <asp:TextBox Visible="false" ID="txtMerkFilter" runat="server" Height="16px" Width="114px"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblMerk" runat="server" Text='<%# Eval("merk") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Model" SortExpression="model">

                            <EditItemTemplate>

                                <asp:DropDownList ID="DDLModelEdit" runat="server" DataTextField="omschrijving" DataValueField="omschrijving" AppendDataBoundItems="True" DataSourceID="SDSModelEdit" SelectedValue='<%# Bind("model") %>'>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SDSModelEdit" runat="server" ConnectionString="<%$ ConnectionStrings:dbinventarisConnectionString %>" ProviderName="<%$ ConnectionStrings:dbinventarisConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT(omschrijving) FROM dbinventaris.tblmodel WHERE omschrijving IS NOT NULL  ORDER BY omschrijving asc;"></asp:SqlDataSource>
                                <!-- AND omschrijving not like '' (INDIEN GEEN LEGE MODELLEN) -->
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Model
                                <br />
                                <asp:TextBox Visible="false" ID="txtModelFilter" runat="server"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblModel" runat="server" Text='<%# Bind("model") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="serie nummer" HeaderText="Serienummer " SortExpression="Merk">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Leverancier">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLLeverancierEdit" runat="server" DataTextField="naam" DataValueField="naam" DataSourceID="SDSLeverancierEdit" SelectedValue='<%# Bind("leverancier") %>'>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SDSLeverancierEdit" runat="server" ConnectionString="<%$ ConnectionStrings:dbinventarisConnectionString %>" ProviderName="<%$ ConnectionStrings:dbinventarisConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT(naam) FROM dbinventaris.tblleveranciers WHERE naam IS NOT NULL AND naam not like '' ORDER BY naam asc;"></asp:SqlDataSource>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Leverancier
                                <br />
                                <asp:TextBox Visible="false" ID="txtLeverancierFilter" runat="server" Height="16px" Width="107px"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLeverancier" runat="server" Text='<%# Bind("leverancier") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="Factuurnummer" DataField="factuurnummer">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="EOL" HeaderText="EOL" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" Width="10px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="SOL">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSOLEdit" runat="server" Text='<%# Bind("SOL") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                SOL
                                <br />
                                <asp:TextBox Visible="false" ID="TxtDatumFilter1" runat="server" Height="16px" Width="61px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender Format="dd/MM/yyyy" ID="TxtDatumFilter1_CalendarExtender" runat="server" TargetControlID="TxtDatumFilter1" />
                                <asp:TextBox Visible="false" ID="TxtDatumFilter2" runat="server" Height="16px" Width="72px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender Format="dd/MM/yyyy" ID="TxtDatumFilter2_CalendarExtender" runat="server" TargetControlID="TxtDatumFilter2" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SOL") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Stock">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbStockEdit" AutoPostBack="True" OnCheckedChanged="Change_gebruiker_state_gv" Checked='<%# Eval("stock").ToString() == "1" ? true : false %>' runat="server" Enabled="True" Visible="true" />
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Stock
                                <br />
                                <asp:CheckBox Visible="false" ID="cbStockFilter" runat="server" />
                            </HeaderTemplate>

                            <ItemTemplate>
                                <asp:CheckBox ID="cbStock" Checked='<%# Eval("stock").ToString() == "1" ? true : false %>' runat="server" Enabled="False" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gebruiker">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLGebruikerEdit" runat="server" Enabled='<%# Eval("stock").ToString() == "1" ? false : true %>' DataSourceID="SDSGebruikerEdit" DataTextField="naam" AppendDataBoundItems="True" DataValueField="naam" SelectedValue='<%# Bind("gebruiker") %>'>
                                    <asp:ListItem Selected="True" Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SDSGebruikerEdit" runat="server" ConnectionString="<%$ ConnectionStrings:dbinventarisConnectionString %>" ProviderName="<%$ ConnectionStrings:dbinventarisConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT(naam) FROM tblwerknemer WHERE naam IS NOT NULL AND naam not like '' AND status like 1 ORDER BY naam asc;"></asp:SqlDataSource>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Gebruiker
                                <br />
                                <asp:TextBox Visible="false" ID="txtGebruikerFilter" runat="server" Height="16px" Width="102px"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblGebruiker" runat="server" Text='<%# Bind("gebruiker") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField InsertVisible="False" Visible="True">
                            <ItemTemplate>
                                <asp:HiddenField ID="hfGebruiker" runat="server" Value='<%# Bind("gebruiker") %>'></asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <EditRowStyle BackColor="#D8D8D8" />

                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" Font-Names="Arial" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="False" ForeColor="White" Font-Names="Times New Roman" Font-Overline="False" Font-Underline="False" HorizontalAlign="Left" />
                    <PagerStyle BackColor="DarkGray" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" Font-Names="Times New Roman" Width="10px" />

                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
                <asp:LinkButton Text="" ID="lnkFake" runat="server" />
                <ajaxToolkit:ModalPopupExtender ID="mpInfoApparaten" runat="server" TargetControlID="lnkFake" PopupControlID="pnlPopupInfo" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                <!-- Close gridview aparaten -->
                <!-- Close gridview aparaten CancelControlID="btnClose"-->
                <asp:Panel ID="pnlPopupInfo" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        Details
                    </div>
                    <div class="body">
                        <table border="0">
                            <tr>
                                <td style="width: 60px">
                                    <asp:Label Visible="false" ID="lblidapparaatInfo" runat="server" Text="Label"></asp:Label>
                                    <b>Serienummer: </b>
                                </td>
                                <td>
                                    <asp:Label ID="lblserienummerApparaatInfo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Type/Merk/Model: </b>
                                </td>
                                <td>
                                    <asp:Label ID="lbltypeApparaatInfo" runat="server" />
                                    <asp:Label ID="lblmerkApparaatInfo" runat="server" />
                                    <asp:Label ID="lblmodelApparaatInfo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>SOL/EOL</b>
                                </td>
                                <td>
                                    <asp:Label ID="lblsolApparaatInfo" runat="server" />
                                    <asp:Label ID="lbleolApparaatInfo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Factuur/Leverancier: </b>
                                </td>
                                <td>
                                    <asp:Label ID="lblfactuurnmbrApparaatInfo" runat="server" />
                                    <asp:Label ID="lblleverancierApparaatInfo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Gebruiker ATM: </b>
                                </td>
                                <td>
                                    <asp:Label ID="lblgebruikerApparaatInfo" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <b>Commentaar: </b>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="Insert_ApparaatCommentaar"> EDIT </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:TextBox TextMode="MultiLine" ID="txtcommentaarApparaatInfo" ReadOnly="true" runat="server" Height="70px" Width="270px" MaxLength="450"></asp:TextBox>
                        <br />
                    </div>
                    <div class="footer">
                        <table border="0">
                            <tr>
                                <td>
                                    <asp:Button ID="btnClose" OnClick="modalpopup_close_submit" runat="server" Text="Sluiten en opslaan" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <!-- Gridview leveranciers -->
                <asp:GridView ID="LeveranciersGridView" runat="server" AutoGenerateColumns="False" Visible="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" HorizontalAlign="Justify" DataKeyNames="id" OnRowEditing="LeveranciersGridView_RowEditing" OnRowCancelingEdit="LeveranciersGridView_RowCancelingEdit" OnRowUpdating="LeveranciersGridView_RowUpdating" AllowSorting="True" CssClass="auto-style8" EmptyDataText="Geen data" ShowHeaderWhenEmpty="true" Font-Names="Times New Roman" OnPageIndexChanging="LeveranciersGridView_OnPaging" AllowPaging="True">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbLeveranciers" runat="server" />
                            </ItemTemplate>
                            <ControlStyle />
                        </asp:TemplateField>
                        <asp:BoundField DataField="id" HeaderText="id" Visible="False" />
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="" Style="color: darkgreen;" class="btn btn-mini">
                                    <i class="fa fa-edit" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="" Style="color: darkgreen;" class="btn btn-mini">
                                    <i class="fa fa-plus" aria-hidden="true"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text=" " Style="color: darkred;" class="btn btn-mini">
                                    <i class="fa fa-close" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Naam">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtNaamLeveranciersEdit" runat="server" Text='<%# Bind("naam") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("naam") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contactpersoon">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtContactpersoonLeveranciersEdit" runat="server" Text='<%# Bind("Contactpersoon") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Contactpersoon") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEmailLeveranciersEdit" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Telefoonnummer">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTelefoonnummerLeveranciersEdit" runat="server" Text='<%# Bind("Telefoonnummer") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Telefoonnummer") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#D8D8D8" />
                    <EmptyDataTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </EmptyDataTemplate>
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="DarkGray" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
                <!-- WERKNEMERS -->
                <asp:GridView ID="WerknemersGridView" runat="server" CellPadding="4" DataKeyNames="id" Width="100%" AllowPaging="True" AutoGenerateColumns="False" Visible="False"
                    OnRowDeleting="WerknemersGridView_RowDeleting"
                    EmptyDataText="Geen data"
                    OnPageIndexChanging="WerknemersGridView_OnPaging"
                    OnRowCommand="WerknemersGridView_OnRowCommand"
                    OnRowEditing="WerknemersGridView_RowEditing"
                    OnSorting="WerknemersGridView_OnSorting"
                    OnRowUpdating="WerknemersGridView_RowUpdating"
                    AllowSorting="True" ShowHeaderWhenEmpty="True"
                    OnRowCancelingEdit="WerknemersGridView_RowCancelingEdit" ForeColor="#333333" GridLines="Horizontal" Font-Names="Times New Roman">

                    <AlternatingRowStyle BackColor="White" />

                    <Columns>

                        <asp:TemplateField ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbWerknemers" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:LinkButton ID="cmdHistoriek" runat="server" CausesValidation="false" CommandName="CommandHistoriek" Style="color: darkgreen;" class="btn btn-mini" Text=""><i class="fa fa-history" aria-hidden="true"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="" Style="color: darkgreen;" class="btn btn-mini">
                                    <i class="fa fa-edit" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="" Style="color: darkgreen;" class="btn btn-mini">
                                    <i class="fa fa-plus" aria-hidden="true"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text=" " Style="color: darkred;" class="btn btn-mini">
                                    <i class="fa fa-close" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="id" HeaderText="ID" Visible="False" />

                        <asp:BoundField DataField="naam" HeaderText="Naam" SortExpression="naam">

                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Locatie" SortExpression="locatie">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLLocatieEdit" runat="server" DataSourceID="SDSLocatieEdit" Width="123px" DataTextField="omschrijving" DataValueField="omschrijving" SelectedValue='<%# Bind("locatie") %>'>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SDSLocatieEdit" runat="server" ConnectionString="<%$ ConnectionStrings:dbinventarisConnectionString %>" ProviderName="<%$ ConnectionStrings:dbinventarisConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT(omschrijving) FROM dbinventaris.tbllocatie WHERE omschrijving IS NOT NULL AND omschrijving not like '' ORDER BY omschrijving asc;"></asp:SqlDataSource>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLocatie" runat="server" Text='<%# Bind("locatie") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status" SortExpression="status">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("status").ToString() == "1" ? "actief" : "inactief" %>' Enabled="false"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("status").ToString() == "1" ? "actief" : "inactief" %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="commentaar" HeaderText="Commentaar" />

                    </Columns>

                    <EditRowStyle BackColor="#D8D8D8" />

                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="DarkGray" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />

                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />

                </asp:GridView>

                <!-- Gridview Historiek -->
                <asp:GridView ID="HistoriekGridView" runat="server" CellPadding="4" Width="100%" AllowPaging="True" Visible="False" EmptyDataText="Geen data" AutoGenerateColumns="False" OnRowEditing="HistoriekGridView_RowEditing" OnRowCancelingEdit="HistoriekGridView_RowCancelingEdit" DataKeyNames="id" OnPageIndexChanging="HistoriekGridView_OnPaging" OnRowUpdating="HistoriekGridView_RowUpdating" ShowHeaderWhenEmpty="True" HorizontalAlign="Left" ForeColor="#333333" GridLines="Horizontal" Font-Names="Times New Roman">

                    <AlternatingRowStyle BackColor="White" />

                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbHistoriek" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="" Style="color: darkgreen;" class="btn btn-mini">
                                    <i class="fa fa-edit" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="" Style="color: darkgreen;" class="btn btn-mini">
                                    <i class="fa fa-plus" aria-hidden="true"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text=" " Style="color: darkred;" class="btn btn-mini">
                                    <i class="fa fa-close" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="id" HeaderText="ID" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Ingebruik">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtingebruik" runat="server" Text='<%# Bind("Ingebruik") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblingebruik" runat="server" Text='<%# Bind("Ingebruik") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Uitgebruik">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtUitgebruik" runat="server" Text='<%# Bind("Uitgebruik") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUitgebruik" runat="server" Text='<%# Bind("Uitgebruik") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Werknemer">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLWerknemer" runat="server" Width="109px" DataSourceID="SDSWerknemerEdit" DataTextField="naam" DataValueField="naam">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SDSWerknemerEdit" runat="server" ConnectionString="<%$ ConnectionStrings:dbinventarisConnectionString %>" ProviderName="<%$ ConnectionStrings:dbinventarisConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT(naam) from tblwerknemer where naam NOT LIKE &quot;&quot;;"></asp:SqlDataSource>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblWerknemer" runat="server" Text='<%# Bind("Werknemer") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Merk">
                            <ItemTemplate>
                                <asp:Label ID="lblMerk" runat="server" Text='<%# Bind("Merk") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Model" SortExpression="model">
                            <ItemTemplate>
                                <asp:Label ID="lblModel" runat="server" Text='<%# Bind("Model") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Serienummer">
                            <ItemTemplate>
                                <asp:Label ID="lblSerienummer" runat="server" Text='<%# Bind("Serienummer") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>

                    <EditRowStyle BackColor="#D8D8D8" />

                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="DarkGray" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />

                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />

                </asp:GridView>
            </div>
        </form>
    </div>
    <!-- close form -->
    <div id="footer" class="footer">
        <footer class="footer">&copy; Copyright 2020 Maxim Derboven & Alexie Chaerle</footer>
    </div>

</body>
<!-- close body -->
</html>
<!-- close html -->
