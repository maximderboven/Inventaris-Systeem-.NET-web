<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inventaris.aspx.cs" Inherits="presentationWeb.Inventaris" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventaris Rekencentra 2020</title>
    <!-- CSS -->
    <style type="text/css">
        .auto-style3 {
            width: 103px;
            height: 103px;
        }

        body, html, * {
            margin: 0px;
        }

        .header {
            background-color: #1C9F75;
            margin: 0px;
            padding: 12px;
        }

        .content {
            margin: 20px;
        }

        .headertekst {
            color: white;
            font-family: Verdana, Arial;
        }

        .button1 {
            background-color: #44c767;
            border: 1px solid #18ab29;
            display: inline-block;
            cursor: pointer;
            color: #ffffff;
            font-family: Verdana, Arial;
            font-size: 12px;
            padding: 3px 3px;
            text-shadow: 0px 1px 0px #2f6627;
            font-weight: bold;
        }

            .button1:hover {
                background-color: #5cbf2a;
            }

            .button1:active {
                position: relative;
                top: 1px;
            }

        .newStyle1 {
            font-family: Verdana, Geneva, Tahoma, sans-serif;
            font-size: x-large;
            color: #C0C0C0;
        }

        .footer {
            text-align: center;
            color: grey;
            margin-top: 12px;
            bottom: 0;
            position: fixed;
            width: 100%;
            left: 0;
        }

        .auto-style4 {
            width: 103px;
        }

        .auto-style5 {
            width: 741px;
        }

        .auto-style6 {
            margin-bottom: 19px;
        }

        .auto-style7 {
            height: 87px;
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
                    <!-- Panel controlls -->

                    <span class="newStyle1"><strong>Apparaat</strong></span>
                    <table border="0" id="apparaatTabel">
                        <tr>
                            <td class="auto-style22">
                                <asp:DropDownList ID="DDLType" runat="server" Width="103px" DataTextField="omschrijving" DataValueField="omschrijving">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="auto-style8">
                                <asp:DropDownList ID="DDLMerk" runat="server" Height="23px" Width="104px" DataTextField="omschrijving" DataValueField="omschrijving">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="auto-style22">
                                <asp:DropDownList ID="DDLModel" runat="server" DataTextField="omschrijving" DataValueField="omschrijving">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="auto-style11">
                                <asp:TextBox ID="TxtSerienummer" runat="server" placeholder="Serie nummer" Width="140" />
                            </td>
                            <td>
                                <script type="text/javascript">

                                    function ShowHideDiv(CBStock) {
                                        var dvGebruiker = document.getElementById("dvGebruiker");
                                        dvGebruiker.style.display = CBStock.checked ? "none" : "block";
                                    }
                                </script>
                                <label for="CBStock">
                                    <asp:CheckBox ID="CBStock" Checked="true" runat="server" onclick="ShowHideDiv(this)" Text="In Stock?" />
                                </label>
                            </td>
                            <td>
                                <div id="dvGebruiker" style="display: none">
                                    <asp:DropDownList ID="DDLGebruiker" runat="server" DataTextField="naam" DataValueField="naam">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLLeverancier" runat="server" DataTextField="naam" DataValueField="naam">
                                </asp:DropDownList>
                                <asp:TextBox ID="TxtFactuurnummer" runat="server" placeholder="factuur nummer..."></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnApparaatNew" runat="server" class="button1" Height="32px" OnClick="Insert_Apparaat" Text="Toevoegen" Width="92px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">
                                <asp:TextBox ID="txtTypeNew" runat="server" placeholder="Type" Width="180px"></asp:TextBox>
                                <asp:Button ID="btnTypeNew" runat="server" Text="Type toevoegen" OnClick="btnTypeNew_Click" Width="180px" />
                            </td>
                            <td class="auto-style7">
                                <asp:TextBox ID="txtMerkNew" runat="server" placeholder="Merk" Width="180px"></asp:TextBox>
                                <asp:Button ID="btnMerkNew" runat="server" Text="Merk toevoegen" OnClick="btnMerkNew_Click" Width="180px" />
                            </td>
                            <td class="auto-style7">
                                <asp:TextBox ID="TxtModelNew" runat="server" placeholder="Model" Width="180px"></asp:TextBox>
                                <asp:Button ID="btnModelNew" runat="server" Text="Model toevoegen" OnClick="btnModelNew_Click" Width="180px" />
                            </td>
                            <td class="auto-style7">
                                <asp:TextBox ID="txtLeverancierNaamNew" runat="server" placeholder="Naam leverancier"></asp:TextBox>
                                <asp:TextBox ID="txtLeverancierContactNew" runat="server" placeholder="Contact leverancier"></asp:TextBox>
                                <asp:TextBox ID="txtLeverancierEmailNew" runat="server" placeholder="Email leverancier"></asp:TextBox>
                                <asp:TextBox ID="txtLeverancierTeleNew" runat="server" placeholder="Telefoon leverancier"></asp:TextBox>
                                <asp:Button ID="btnLeverancierNew" runat="server" Height="31px" Text="Leverancier toevoegen" Width="174px" OnClick="btnLeverancierNew_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>

                                <asp:Label ID="lblTypeNewInfo" runat="server" Visible="False"></asp:Label>

                            </td>
                            <td>

                                <asp:Label ID="lblMerkNewInfo" runat="server" Visible="False"></asp:Label>

                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblLeverancierNewInfo" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <h6>
                        <br />
                        <span class="newStyle1"><strong>Werknemer</strong></span></h6>
                    <table id="werknemerTabel" border="0" class="auto-style14">
                        <tr>
                            <td class="auto-style25">
                                <asp:TextBox ID="txtWerknemerNaam" runat="server" placeholder="Naam"></asp:TextBox>
                            </td>
                            <td class="auto-style24"></td>
                            <td class="auto-style17">
                                <asp:DropDownList ID="DDLLocatie" runat="server" DataTextField="omschrijving" DataValueField="omschrijving"></asp:DropDownList>
                            </td>
                            <td class="auto-style23">
                                <asp:Button ID="btnWerknemerNew" class="button1" runat="server" OnClick="Insert_Werknemer" Style="text-align: justify" Text="Toevoegen" Width="92px" Height="32px" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <div>
                        <span class="newStyle1"><strong>Filter</strong></span>
                    </div>
                    <table id="filterTabel" border="0" class="auto-style14">
                        <tr>
                            <td class="auto-style20">
                                <asp:Button ID="btnBekijkApparaten" runat="server" Height="32px" OnClick="ButtonBekijkApparaten_Click" Text="Apparaten" Width="92px" />
                            </td>
                            <td class="auto-style21">
                                <asp:Button ID="btnBekijkWerknemers" runat="server" Height="32px" OnClick="ButtonBekijkWerknemers_Click" Text="Werkenmers" Width="92px" />
                            </td>
                            <td class="auto-style17">
                                <asp:Button ID="btnBekijkHistoriek" runat="server" Height="32px" OnClick="ButtonBekijkHistoriek_Click" Text="Historiek" Width="92px" />
                            </td>
                            <td class="auto-style20">
                                <asp:Button ID="btnZoeken" runat="server" OnClick="ButtonZoeken_Click" Text="Zoeken" />
                            </td>
                            <td class="auto-style21">
                                <asp:TextBox ID="txtZoeken" runat="server" Height="23px" placeholder="Zoeken naar .." Width="183px"></asp:TextBox>
                            </td>
                            <td class="auto-style17">
                                <asp:Button ID="btnCheckAll" runat="server" OnClick="CheckAll_Click" Text="Check All" />
                            </td>
                            <td class="auto-style16">
                                <asp:Button ID="btnUncheckAll" runat="server" OnClick="UncheckAll_Click" Text="Uncheck All" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" OnClick="DeleteButton_Click" Text="Delete" />
                            </td>
                        </tr>

                    </table>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>

                </asp:Panel>
                <!-- close panel controlls -->
            </div>
            <script type="text/javascript">

                function ShowHideGebruiker(lblCheckBoxStock) {
                    var dvGebruikerEdit = document.getElementById("dvGebruikerEdit");
                    dvGebruikerEdit.style.display = lblCheckBoxStock.checked ? "none" : "block";
                }
            </script>
            <div id="Gridviews">
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
                    CellPadding="3" BackColor="White" BorderColor="#CCCCCC"
                    BorderStyle="None" BorderWidth="1px" AllowSorting="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" PageSize="25" AllowPaging="True">
                    <Columns>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CBApparaten" runat="server" />
                            </ItemTemplate>
                            <ControlStyle Width="10px" />
                        </asp:TemplateField>

                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="cmdHistoriek" runat="server" CausesValidation="false" CommandName="CommandHistoriek" Text="Historiek"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:CommandField ShowEditButton="True" ItemStyle-Width="100">
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:CommandField>

                        <asp:BoundField DataField="id" HeaderText="ID" InsertVisible="False" ReadOnly="True" Visible="False" />
                        <asp:TemplateField HeaderText="Type" SortExpression="Type">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLTypeEdit" runat="server" DataTextField="omschrijving" DataValueField="omschrijving">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("type") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Merk">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLMerkEdit" runat="server" DataTextField="omschrijving" DataValueField="omschrijving">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblMerk" runat="server" Text='<%# Eval("merk") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Model">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLModelEdit" runat="server" DataTextField="omschrijving" DataValueField="omschrijving">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblModel" runat="server" Text='<%# Bind("model") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="serie nummer" HeaderText="Serie nummer">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Leverancier">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLLeverancierEdit" runat="server" DataTextField="naam" DataValueField="naam">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLeverancier" runat="server" Text='<%# Bind("leverancier") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="Factuurnummer" DataField="factuurnummer">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="EOL" HeaderText="EOL" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOL" HeaderText="SOL">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Stock">
                            <EditItemTemplate>
                                <label for="lblCheckBoxStock">
                                    <asp:CheckBox ID="cbStockEdit" onclick="ShowHideGebruiker(this)" Checked='True' runat="server" Enabled="True" />
                                </label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbStock" Checked='<%# Eval("stock").ToString() == "1" ? true : false %>' runat="server" Enabled="False" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gebruiker">
                            <EditItemTemplate>
                                <div id="dvGebruikerEdit" style="display: none">
                                    <asp:DropDownList ID="DDLGebruikerEdit" runat="server" DataTextField="naam" DataValueField="naam">
                                    </asp:DropDownList>
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblGebruiker" runat="server" Text='<%# Bind("gebruiker") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                    </Columns>

                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#1C9F75" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" />

                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView>
                <!-- Close gridview aparaten -->

                <!-- Gridview werknemers -->
                <asp:GridView ID="WerknemersGridView" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="id" Width="100%" AllowPaging="True" AutoGenerateColumns="False" Visible="False"
                    OnRowDeleting="WerknemersGridView_RowDeleting"
                    EmptyDataText="Geen data"
                    OnPageIndexChanging="WerknemersGridView_OnPaging"
                    OnRowEditing="WerknemersGridView_RowEditing"
                    OnSorting="WerknemersGridView_OnSorting"
                    OnRowUpdating="WerknemersGridView_RowUpdating"
                    AllowSorting="True" ShowHeaderWhenEmpty="True"
                    OnRowCancelingEdit="WerknemersGridView_RowCancelingEdit" CssClass="auto-style6">

                    <Columns>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbWerknemers" runat="server" />
                            </ItemTemplate>
                            <ControlStyle Width="10px" />
                            <ItemStyle Width="45px" />
                        </asp:TemplateField>

                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="cmdHistoriek" runat="server" CausesValidation="false" CommandName="" Text="Historiek"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="120px" />
                        </asp:TemplateField>

                        <asp:CommandField ShowEditButton="True">

                            <ItemStyle Width="100px" />
                        </asp:CommandField>

                        <asp:BoundField DataField="id" HeaderText="ID" Visible="False" />

                        <asp:BoundField DataField="naam" HeaderText="Naam">

                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Locatie">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DDLLocatieEdit" runat="server" DataSourceID="Locatie" Width="123px" DataTextField="omschrijving" DataValueField="omschrijving">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLocatie" runat="server" Text='<%# Bind("locatie") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="status" HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="commentaar" HeaderText="Commentaar" />

                    </Columns>

                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#1C9F75" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" />

                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />

                </asp:GridView>
                <!-- close gridview werknemers -->

                <!-- Gridview Historiek -->
                <asp:GridView ID="HistoriekGridView" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Width="100%" AllowPaging="True" Visible="False" EmptyDataText="Geen data" AutoGenerateColumns="False" OnRowEditing="HistoriekGridView_RowEditing" OnRowCancelingEdit="HistoriekGridView_RowCancelingEdit" DataKeyNames="id" OnPageIndexChanging="HistoriekGridView_OnPaging" OnRowUpdating="HistoriekGridView_RowUpdating" ShowHeaderWhenEmpty="True" HorizontalAlign="Left">

                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbHistoriek" runat="server" />
                            </ItemTemplate>
                            <ControlStyle Width="30px" />
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="id" HeaderText="ID" Visible="False" />
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
                                <asp:DropDownList ID="DDLWerknemer" runat="server" Width="109px">
                                </asp:DropDownList>
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
                        <asp:TemplateField HeaderText="Model">
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

                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#1C9F75" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" />

                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />

                </asp:GridView>


            </div>
        </form>
    </div>
    <div class="footer">
        <hr />
        <footer style="margin-top: 12px; margin-bottom: 10px">&copy; Copyright 2020 Maxim Derboven & Alexie Chaerler></footer>
    </div>
    <!-- close form -->
</body>
<!-- close body -->
</html>
<!-- close html -->
