using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using MySql.Data.MySqlClient;
using MySql.Data;

/*=============================================================================
 |
 |       Author:  Maxim Derboven & Alexie Chaerle
 |
 |      Company:  Rekencentra
 |     Due Date:  6/1/2020-17/1/2020
 |
 |  Description:  Inventarisering systeem met medewerker beheer.
 |       Needed:  mySQL dbinventaris
 |
 |     Language:  ASP.NET C# Framework v4.6.1 / mysql
 |      Version:  V1.0
 |                
 *===========================================================================*/


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                                                      //
//  string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;                                            //
//  using (MySqlConnection con = new MySqlConnection(constr))                                                                                           //
//  {                                                                                                                                                   //
//      using (MySqlCommand cmd = new MySqlCommand("STRING SQL"))                                                                                       //
//     {                                                                                                                                                //
//         cmd.Parameters.AddWithValue("@id", apparaatid);                                                                                              //
//         cmd.Connection = con;                                                                                                                        //
//         con.Open();                                                                                                                                  //
//         cmd.ExecuteNonQuery();                                                                                                                       //
//         con.Close();                                                                                                                                 //
//     }                                                                                                                                                //
//  }                                                                                                                                                   //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace presentationWeb
{
    public partial class Inventaris : System.Web.UI.Page
    {
        /*--------------------------------------------------------------------------------------------------*/
        //Gedeelde methode
        /*--------------------------------------------------------------------------------------------------*/
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindGridApparaten();
                this.BindGridWerknemer();
                this.BindGridHistoriek();
                this.BindGridControls();
            }
            else
            {
                this.BindGridControls();
            }

        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        public void SearchData(string item)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                //Zoeken in grid apparaten
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat left outer join (SELECT apparaatID, MIN(ingebruik),werknemerID FROM tblhistoriek GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id left outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID left outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id left outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id left outer JOIN tbltype ON tblmodel.typeID = tbltype.id left outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE tblapparaat.id IS NOT NULL AND tblmodel.omschrijving LIKE CONCAT(@item) OR tblapparaat.serienummer LIKE CONCAT(@item) OR tblapparaat.uitgebruik LIKE CONCAT(@item) OR tblleveranciers.naam LIKE CONCAT(@item) OR tblmodel.omschrijving LIKE CONCAT(@item) OR tblapparaat.factuurnummer LIKE CONCAT(@item) OR tblapparaat.aankoopdatum LIKE CONCAT(@item) OR tblapparaat.commentaar LIKE CONCAT(@item) OR tblapparaat.stock LIKE CONCAT(@item) OR tbltype.omschrijving LIKE CONCAT(@item) OR tblmerk.omschrijving LIKE CONCAT(@item)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@item", '%' + item + '%');
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            ApparatenGridView.DataSource = dt;
                        }
                        ApparatenGridView.DataBind();
                    }
                }

                //Zoeken in grid werknemers
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblwerknemer.id as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 0) as 'locatie', COALESCE(tblwerknemer.status, 'onbekend') as 'status', COALESCE(tblwerknemer.commentaar, 'onbekend') as 'commentaar' FROM tblwerknemer left outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE naam LIKE CONCAT(@item) OR tbllocatie.omschrijving LIKE CONCAT(@item) OR tblwerknemer.status LIKE CONCAT(@item) OR tblwerknemer.commentaar LIKE CONCAT(@item) AND tblwerknemer.id IS NOT NULL UNION SELECT tblwerknemer.id as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 0) as 'locatie', COALESCE(tblwerknemer.status, 'onbekend') as 'status', COALESCE(tblwerknemer.commentaar, 'onbekend') as 'commentaar' FROM tblwerknemer right outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE naam LIKE CONCAT(@item) OR tbllocatie.omschrijving LIKE CONCAT(@item) OR tblwerknemer.status LIKE CONCAT(@item) OR tblwerknemer.commentaar LIKE CONCAT(@item) AND tblwerknemer.id IS NOT NULL"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@item", '%' + item + '%');
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            WerknemersGridView.DataSource = dt;
                        }
                        WerknemersGridView.DataBind();
                    }
                }
            }
        }

        protected void UncheckAll_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
        }

        protected void CheckAll_Click(object sender, EventArgs e)
        {
            ToggleCheckState(true);
        }

        private void ToggleCheckState(bool checkState)
        {
            //apparaten
            if (ApparatenGridView.Visible == true)
            {
                foreach (GridViewRow row in ApparatenGridView.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("CBApparaten");
                    if (cb != null)
                        cb.Checked = checkState;
                }
            }

            //Werknemer
            if (WerknemersGridView.Visible == true)
            {
                foreach (GridViewRow row in WerknemersGridView.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("cbWerknemers");
                    if (cb != null)
                        cb.Checked = checkState;
                }
            }

            //Historiek
            if (HistoriekGridView.Visible == true)
            {
                foreach (GridViewRow row in HistoriekGridView.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("cbHistoriek");
                    if (cb != null)
                        cb.Checked = checkState;
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            //Apparaten
            if (ApparatenGridView.Visible == true)
            {
                int teller = 0;
                // Iterate through the Products.Rows property
                foreach (GridViewRow row in ApparatenGridView.Rows)
                {
                    // Access the CheckBox
                    CheckBox cb = (CheckBox)row.FindControl("CBApparaten");
                    if (cb != null && cb.Checked)
                    {
                        teller++;
                        int apparaatid = Convert.ToInt32(ApparatenGridView.DataKeys[row.RowIndex].Value);
                        string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
                        using (MySqlConnection con = new MySqlConnection(constr))
                        {
                            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM tblhistoriek WHERE (apparaatID = @ID);DELETE FROM tblapparaat WHERE (id = @ID);"))
                            {
                                cmd.Parameters.AddWithValue("@id", apparaatid);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }

                }
                this.BindGridApparaten();
            }

            //Werknemer
            if (WerknemersGridView.Visible == true)
            {
                int teller = 0;
                // Iterate through the Products.Rows property
                foreach (GridViewRow row in WerknemersGridView.Rows)
                {
                    // Access the CheckBox
                    CheckBox cb = (CheckBox)row.FindControl("cbWerknemers");
                    if (cb != null && cb.Checked)
                    {
                        teller++;
                        int werknemerid = Convert.ToInt32(WerknemersGridView.DataKeys[row.RowIndex].Value);
                        string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
                        using (MySqlConnection con = new MySqlConnection(constr))
                        {
                            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM tblwerknemer WHERE (ID = @ID);"))
                            {
                                cmd.Parameters.AddWithValue("@id", werknemerid);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }

                }
                this.BindGridWerknemer();
            }

            //Historiek
            if (HistoriekGridView.Visible == true)
            {
                int teller = 0;
                // Iterate through the Products.Rows property
                foreach (GridViewRow row in HistoriekGridView.Rows)
                {
                    // Access the CheckBox
                    CheckBox cb = (CheckBox)row.FindControl("cbHistoriek");
                    if (cb != null && cb.Checked)
                    {
                        teller++;
                        int historiekid = Convert.ToInt32(HistoriekGridView.DataKeys[row.RowIndex].Value);
                        string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
                        using (MySqlConnection con = new MySqlConnection(constr))
                        {
                            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM tblhistoriek WHERE (ID = @ID);"))
                            {
                                cmd.Parameters.AddWithValue("@id", historiekid);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }

                }
                this.BindGridHistoriek();
            }

        }

        /*--------------------------------------------------------------------------------------------------*/
        //Methode Controlpanel / aanmaken
        /*--------------------------------------------------------------------------------------------------*/
        protected void ButtonBekijkApparaten_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
            ApparatenGridView.Visible = true;
            WerknemersGridView.Visible = false;
            HistoriekGridView.Visible = false;
            this.BindGridApparaten();
        }

        protected void ButtonBekijkWerknemers_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
            ApparatenGridView.Visible = false;
            WerknemersGridView.Visible = true;
            HistoriekGridView.Visible = false;
            this.BindGridWerknemer();
        }

        protected void ButtonBekijkHistoriek_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
            ApparatenGridView.Visible = false;
            WerknemersGridView.Visible = false;
            HistoriekGridView.Visible = true;
            this.BindGridHistoriek();
        }

        protected void Insert_Apparaat(object sender, System.EventArgs e)
        {
            //TYPE EDIT
            string type = DDLType.SelectedValue.ToString();
            //MERK EDIT
            string merk = DDLMerk.SelectedItem.ToString();
            //MODEL EDIT
            string model = DDLModel.SelectedItem.ToString();
            //SERIENUMMER EDIT
            string serienummer = TxtSerienummer.Text;
            //LEVERANCIER EDIT
            string leverancier = DDLLeverancier.SelectedItem.ToString();
            //FACTUURNUMMER EDIT
            string factuurnummer = TxtFactuurnummer.Text;
            //Stock EDIT
            bool instock = CBStock.Checked;
            //GEBRUIKER EDIT
            string gebruiker;
            if (instock == true)
            {
                gebruiker = "stock";
            }
            else
            {
                gebruiker = DDLGebruiker.SelectedItem.ToString();
            }


            int typeID;
            int merkID;
            int modelID;
            int leverancierID;
            int werknemerID;
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                // TYPEID ophalen
                MySqlCommand cmd = new MySqlCommand("select tbltype.id from tbltype where tbltype.omschrijving like @type;");
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Connection = con;
                con.Open();
                typeID = Convert.ToInt16(cmd.ExecuteScalar());


                // MERKID ophalen
                cmd = new MySqlCommand("select tblmerk.id from tblmerk where tblmerk.omschrijving like @merk;");
                cmd.Parameters.AddWithValue("@merk", merk);
                cmd.Connection = con;
                merkID = Convert.ToInt16(cmd.ExecuteScalar());

                // DE LINK TUSSEN TYPEID en MERKID 
                cmd = new MySqlCommand("SELECT tblmodel.id FROM tblmodel WHERE tblmodel.typeID = @typeID AND tblmodel.merkID = @merkID;");
                cmd.Parameters.AddWithValue("@typeID", typeID);
                cmd.Parameters.AddWithValue("@merkID", merkID);
                cmd.Connection = con;
                if (cmd.ExecuteScalar() == null)
                {
                    cmd = new MySqlCommand("INSERT INTO tblmodel (id, omschrijving, typeID, merkID) VALUES(null, @model, @typeID, @merkID);SELECT LAST_INSERT_ID();");
                    {
                        cmd.Parameters.AddWithValue("@model", model);
                        cmd.Parameters.AddWithValue("@typeID", typeID);
                        cmd.Parameters.AddWithValue("@merkID", merkID);
                        cmd.Connection = con;
                        modelID = Convert.ToInt16(cmd.ExecuteScalar());
                    }
                }
                else
                {
                    modelID = Convert.ToInt16(cmd.ExecuteScalar());
                }

                cmd = new MySqlCommand("select tblleveranciers.id from tblleveranciers where tblleveranciers.naam like @leverancier;");
                cmd.Parameters.AddWithValue("@leverancier", leverancier);
                cmd.Connection = con;
                leverancierID = Convert.ToInt16(cmd.ExecuteScalar());

                //het apparaat updaten
                cmd = new MySqlCommand("Insert INTO tblapparaat(id,modelID,serienummer,uitgebruik,leverancierID,factuurnummer,aankoopdatum,commentaar,stock)VALUES(null,@modelID,@serienummer,null,@leverancierID,@factuurnummer,null,null,@stock);SELECT LAST_INSERT_ID();");
                cmd.Parameters.AddWithValue("@modelID", modelID);
                cmd.Parameters.AddWithValue("@serienummer", serienummer);
                cmd.Parameters.AddWithValue("@leverancierID", leverancierID);
                cmd.Parameters.AddWithValue("@stock", instock);
                cmd.Parameters.AddWithValue("@factuurnummer", factuurnummer);
                cmd.Connection = con;
                int apparaatID = Convert.ToInt16(cmd.ExecuteScalar());

                if (gebruiker != "stock")
                {
                    // werknemer ophalen
                    cmd = new MySqlCommand("select tblwerknemer.id from tblwerknemer where tblwerknemer.naam like @naam;");
                    cmd.Parameters.AddWithValue("@naam", gebruiker);
                    cmd.Connection = con;
                    werknemerID = Convert.ToInt16(cmd.ExecuteScalar());

                    //het apparaat toevoegen
                    cmd = new MySqlCommand("INSERT INTO tblhistoriek(id, ingebruik, uitgebruik, werknemerID, apparaatID) VALUES(null, CURDATE(), null, @werknemer, @apparaat);");
                    cmd.Parameters.AddWithValue("@apparaat", apparaatID);
                    cmd.Parameters.AddWithValue("@werknemer", werknemerID);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            this.BindGridApparaten();
        }

        protected void BindGridControls()
        {
            //Alle datasources
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                //Datasource: DDLMerk
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT(omschrijving) FROM dbinventaris.tblMerk WHERE omschrijving IS NOT NULL AND omschrijving not like '' ORDER BY omschrijving asc;");
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DDLMerk.DataSource = dt;
                        DDLMerk.DataBind();
                    }
                }

                //Datasource: DDLType
                cmd = new MySqlCommand("SELECT DISTINCT(omschrijving) FROM dbinventaris.tbltype WHERE omschrijving IS NOT NULL AND omschrijving not like '' ORDER BY omschrijving asc");
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DDLType.DataSource = dt;

                        DDLType.DataBind();
                    }
                }

                //Datasource: DDLModel
                cmd = new MySqlCommand("SELECT DISTINCT omschrijving FROM dbinventaris.tblmodel WHERE omschrijving IS NOT NULL AND omschrijving not like '' @typefilter @merkfilter ORDER BY omschrijving asc;");
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Parameters.AddWithValue("@typefilter", "AND tblmodel.typeID = (SELECT tbltype.id FROM tbltype WHERE tbltype.omschrijving like '" + DDLType.SelectedValue.ToString() + "')");
                    cmd.Parameters.AddWithValue("@merkfilter", "AND tblmodel.merkID = (SELECT tblmerk.id FROM tblmerk WHERE tblmerk.omschrijving like '" + DDLMerk.SelectedValue.ToString() + "')");
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DDLModel.DataSource = dt;
                        DDLModel.DataBind();
                    }
                }

                //Datasource: DDLLeverancier
                cmd = new MySqlCommand("SELECT DISTINCT(naam) FROM dbinventaris.tblleveranciers WHERE naam IS NOT NULL AND naam not like '' ORDER BY naam asc;");
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DDLLeverancier.DataSource = dt;
                        DDLLeverancier.DataBind();
                    }
                }

                //Datasource: DDLLeverancier
                cmd = new MySqlCommand("SELECT DISTINCT(naam) FROM dbinventaris.tblleveranciers WHERE naam IS NOT NULL AND naam not like '' ORDER BY naam asc;");
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DDLLeverancier.DataSource = dt;
                        DDLLeverancier.DataBind();
                    }
                }

                //Datasource: DDLLocatie
                cmd = new MySqlCommand("SELECT DISTINCT omschrijving FROM dbinventaris.tbllocatie WHERE omschrijving IS NOT NULL AND omschrijving not like '' ORDER BY omschrijving asc;");
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DDLLocatie.DataSource = dt;
                        DDLLocatie.DataBind();
                    }
                }

                //Datasource: DDLGebruiker
                cmd = new MySqlCommand("SELECT DISTINCT naam FROM dbinventaris.tblwerknemer WHERE naam IS NOT NULL AND naam not like '' ORDER BY naam asc;");
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DDLGebruiker.DataSource = dt;
                        DDLGebruiker.DataBind();
                    }
                }
            }

        } //Datasources

        protected void ButtonZoeken_Click(object sender, EventArgs e)
        {
            string item = txtZoeken.Text.ToString();
            SearchData(item);
            txtZoeken.Text = "";
        }

        protected void Insert_Werknemer(object sender, EventArgs e)
        {
            string naam = txtWerknemerNaam.Text;
            string locatie = DDLLocatie.Text;

            DDLLocatie.ClearSelection();
            txtWerknemerNaam.Text = "";

            string _conn;
            MySqlConnection _mySqlConnection;
            _conn = "server=localhost;user id = root; Password=Test123); database=dbinventaris";
            _mySqlConnection = new MySqlConnection(_conn);
            int locatieID;
            string sql = "";
            sql = "select tbllocatie.id from tbllocatie where tbllocatie.omschrijving like @locatie;";
            MySqlCommand comm = new MySqlCommand(sql, _mySqlConnection);
            comm.Parameters.AddWithValue("@naam", naam);
            comm.Parameters.AddWithValue("@locatie", locatie);

            _mySqlConnection.Open();

            //Als niet bestaat, locatie aanmaken:
            if (comm.ExecuteScalar() != null)
            {
                locatieID = Convert.ToInt16(comm.ExecuteScalar());
            }
            else
            {
                sql = "INSERT INTO tbllocatie(id,omschrijving,commentaar) VALUES(null,@locatie,'','');SELECT LAST_INSERT_ID();";
                comm = new MySqlCommand(sql, _mySqlConnection);
                comm.Parameters.AddWithValue("@naam", naam);
                comm.Parameters.AddWithValue("@locatie", locatie);
                locatieID = Convert.ToInt16(comm.ExecuteScalar());
            }

            //Werknemer toevoegen
            sql = "INSERT INTO tblwerknemer(id, naam, locatieID, status, commentaar) VALUES(null, @naam, @locatieID, '','');";

            comm = new MySqlCommand(sql, _mySqlConnection);
            comm.Parameters.AddWithValue("@naam", naam);
            comm.Parameters.AddWithValue("@locatieID", locatieID);

            comm.ExecuteNonQuery();
            _mySqlConnection.Close();
            this.BindGridWerknemer();
        }

        protected void btnTypeNew_Click(object sender, EventArgs e)
        {
            //Type toevoegen (button)
            string type = txtTypeNew.Text;
            txtTypeNew.Text = "";

            string _conn;
            MySqlConnection _mySqlConnection;
            _conn = "server=localhost;user id = root; Password=Test123); database=dbinventaris";
            _mySqlConnection = new MySqlConnection(_conn);

            int typeID;
            string sql = "select tbltype.id from tbltype where tbltype.omschrijving like @type;";
            MySqlCommand comm = new MySqlCommand(sql, _mySqlConnection);
            comm.Parameters.AddWithValue("@type", type);
            _mySqlConnection.Open();

            //Als niet bestaat, type aanmaken:
            if (comm.ExecuteScalar() != null)
            {
                typeID = Convert.ToInt16(comm.ExecuteScalar());
                lblTypeNewInfo.Visible = true;
                lblTypeNewInfo.Text = "Type is reeds aangemaakt";
            }
            else
            {
                sql = "INSERT INTO tbltype(id,omschrijving) VALUES(null,@type);SELECT LAST_INSERT_ID();";
                comm = new MySqlCommand(sql, _mySqlConnection);
                comm.Parameters.AddWithValue("@type", type);
                typeID = Convert.ToInt16(comm.ExecuteScalar());
                lblTypeNewInfo.Visible = true;
                lblTypeNewInfo.Text = "Type " + type + " successvol aangemaakt";
            }
        }

        protected void btnMerkNew_Click(object sender, EventArgs e)
        {
            //Merk toevoegen (button)
            string merk = txtMerkNew.Text;
            txtMerkNew.Text = "";

            string _conn;
            MySqlConnection _mySqlConnection;
            _conn = "server=localhost;user id = root; Password=Test123); database=dbinventaris";
            _mySqlConnection = new MySqlConnection(_conn);

            int merkID;
            string sql = "select tblmerk.id from tblmerk where tblmerk.omschrijving like @merk;";
            MySqlCommand comm = new MySqlCommand(sql, _mySqlConnection);
            comm.Parameters.AddWithValue("@merk", merk);
            _mySqlConnection.Open();

            //Als niet bestaat, merk aanmaken:
            if (comm.ExecuteScalar() != null)
            {
                merkID = Convert.ToInt16(comm.ExecuteScalar());
                lblMerkNewInfo.Visible = true;
                lblMerkNewInfo.Text = "Merk is reeds aangemaakt";
            }
            else
            {
                sql = "INSERT INTO tblmerk(id,omschrijving) VALUES(null,@merk);SELECT LAST_INSERT_ID();";
                comm = new MySqlCommand(sql, _mySqlConnection);
                comm.Parameters.AddWithValue("@merk", merk);
                merkID = Convert.ToInt16(comm.ExecuteScalar());
                lblMerkNewInfo.Visible = true;
                lblMerkNewInfo.Text = "Merk " + merk + " successvol aangemaakt";
            }

        }

        protected void btnModelNew_Click(object sender, EventArgs e)
        {
            //Model volgt
        }

        protected void btnLeverancierNew_Click(object sender, EventArgs e)
        {
            //Leverancier toevoegen (button)
            string leverancierNaam = txtLeverancierNaamNew.Text;
            txtLeverancierNaamNew.Text = ""; //Naam
            string leverancierContact = txtLeverancierContactNew.Text;
            txtLeverancierContactNew.Text = ""; //Contact
            string leverancierEmail = txtLeverancierEmailNew.Text;
            txtLeverancierEmailNew.Text = ""; //Email
            string leverancierTelefoon = txtLeverancierTeleNew.Text;
            txtLeverancierTeleNew.Text = ""; //Telefoonnummer

            //Connectie
            string _conn;
            MySqlConnection _mySqlConnection;
            _conn = "server=localhost;user id = root; Password=Test123); database=dbinventaris";
            _mySqlConnection = new MySqlConnection(_conn);

            int leverancierID;
            //Kijken naar naam van leverancier
            string sql = "select tblleveranciers.id from tblleveranciers where tblleveranciers.naam like @naam;";
            MySqlCommand comm = new MySqlCommand(sql, _mySqlConnection);
            comm.Parameters.AddWithValue("@naam", leverancierNaam);
            _mySqlConnection.Open();

            //Als niet bestaat, leverancier aanmaken:
            if (comm.ExecuteScalar() != null)
            {
                leverancierID = Convert.ToInt16(comm.ExecuteScalar());
                lblLeverancierNewInfo.Visible = true;
                lblLeverancierNewInfo.Text = "Leverancier reeds aangemaakt";
            }
            else
            {
                sql = "INSERT INTO tblleveranciers(id,naam, contactpersoon, email, telefoonnummer) VALUES(null,@leverancierNaam,@leverancierContact,@leverancierEmail,@leverancierTelefoon);SELECT LAST_INSERT_ID();";
                comm = new MySqlCommand(sql, _mySqlConnection);
                comm.Parameters.AddWithValue("@leverancierNaam", leverancierNaam);
                comm.Parameters.AddWithValue("@leverancierContact", leverancierContact);
                comm.Parameters.AddWithValue("@leverancierEmail", leverancierEmail);
                comm.Parameters.AddWithValue("@leverancierTelefoon", leverancierTelefoon);
                leverancierID = Convert.ToInt16(comm.ExecuteScalar());
                lblLeverancierNewInfo.Visible = true;
                lblLeverancierNewInfo.Text = "Leverancier " + leverancierNaam + " successvol aangemaakt";
            }
        }


        /*--------------------------------------------------------------------------------------------------*/
        //Aparaten CODE
        /*--------------------------------------------------------------------------------------------------*/

        private void BindGridApparaten(string sortExpression = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat left outer join (SELECT apparaatID, MIN(ingebruik),werknemerID FROM tblhistoriek GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id left outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID left outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id left outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id left outer JOIN tbltype ON tblmodel.typeID = tbltype.id left outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE tblapparaat.id IS NOT NULL UNION SELECT tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat right outer join (SELECT apparaatID, MIN(ingebruik),werknemerID FROM tblhistoriek GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id right outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID right outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id right outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id right outer JOIN tbltype ON tblmodel.typeID = tbltype.id right outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE tblapparaat.id IS NOT NULL;"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (sortExpression != null)
                            {
                                DataView dv = dt.AsDataView();
                                this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";

                                dv.Sort = sortExpression + " " + this.SortDirection;
                                ApparatenGridView.DataSource = dv;
                            }
                            else
                            {
                                ApparatenGridView.DataSource = dt;
                            }
                            ApparatenGridView.DataBind();
                        }
                    }
                }
            }
        }

        protected void ApparatenGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int apparaatid = Convert.ToInt32(ApparatenGridView.DataKeys[e.RowIndex].Values[0]);
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM tblhistoriek WHERE (apparaatID = @ID);DELETE FROM tblapparaat WHERE (id = @ID);"))
                {
                    cmd.Parameters.AddWithValue("@id", apparaatid);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGridApparaten();
        }

        protected void ApparatenGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != ApparatenGridView.EditIndex)
            {
                (e.Row.Cells[0].Controls[1] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }

            if (e.Row.RowType == DataControlRowType.DataRow && ApparatenGridView.EditIndex == e.Row.RowIndex)
            {
                DropDownList DropDownListMerk = (DropDownList)e.Row.FindControl("DropDownListMerk");
                string sql = "";
                string conString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(sql, con))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            DropDownListMerk.DataSource = dt;
                            DropDownListMerk.DataTextField = "Merk";
                            DropDownListMerk.DataValueField = "Merk";
                            DropDownListMerk.DataBind();
                            string selectedMerk = DataBinder.Eval(e.Row.DataItem, "Merk").ToString();
                            DropDownListMerk.Items.FindByValue(selectedMerk).Selected = true;
                        }
                    }
                }
            }
        }

        protected void ApparatenGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ApparatenGridView.EditIndex = e.NewEditIndex;
            this.BindGridApparaten();
        }

        protected void ApparatenGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //ROW
            GridViewRow row = ApparatenGridView.Rows[e.RowIndex];
            //ID
            int apparaatID = Convert.ToInt32(ApparatenGridView.DataKeys[e.RowIndex].Values[0]);
            //TYPE EDIT
            DropDownList typeCB = ((DropDownList)row.FindControl("DDLTypeEdit"));
            string type = typeCB.SelectedItem.ToString();
            //MERK EDIT
            DropDownList merkCB = ((DropDownList)row.FindControl("DDLMerkEdit"));
            string merk = merkCB.SelectedItem.ToString();
            //MODEL EDIT
            DropDownList modelCB = ((DropDownList)row.FindControl("DDLModelEdit"));
            string model = modelCB.SelectedItem.ToString();
            //SERIENUMMER EDIT
            string serienummer = (row.Cells[7].Controls[0] as TextBox).Text;
            //LEVERANCIER EDIT
            DropDownList LeverancierDL = ((DropDownList)row.FindControl("DDLLeverancierEdit"));
            string leverancier = LeverancierDL.SelectedItem.ToString();
            //FACTUURNUMMER EDIT
            string factuurnummer = (row.Cells[9].Controls[0] as TextBox).Text;
            //Stock EDIT
            CheckBox stockCB = ((CheckBox)row.FindControl("CheckBoxStockGVEdit"));
            int instock = Convert.ToInt16((stockCB.Checked));
            //GEBRUIKER EDIT
            DropDownList gebruikerDL = ((DropDownList)row.FindControl("DDLGebruikerEdit"));
            string gebruiker = gebruikerDL.SelectedItem.ToString();

            int typeID;
            int merkID;
            int modelID;
            int leverancierID;
            int werknemerID;
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                // TYPEID ophalen
                MySqlCommand cmd = new MySqlCommand("select tbltype.id from tbltype where tbltype.omschrijving like @type;");
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Connection = con;
                con.Open();
                typeID = Convert.ToInt16(cmd.ExecuteScalar());


                // MERKID ophalen
                cmd = new MySqlCommand("select tblmerk.id from tblmerk where tblmerk.omschrijving like @merk;");
                cmd.Parameters.AddWithValue("@merk", merk);
                cmd.Connection = con;
                merkID = Convert.ToInt16(cmd.ExecuteScalar());

                // DE LINK TUSSEN TYPEID en MERKID 
                cmd = new MySqlCommand("SELECT tblmodel.id FROM tblmodel WHERE tblmodel.typeID = @typeID AND tblmodel.merkID = @merkID;");
                cmd.Parameters.AddWithValue("@typeID", typeID);
                cmd.Parameters.AddWithValue("@merkID", merkID);
                cmd.Connection = con;
                if (cmd.ExecuteScalar() == null)
                {
                    cmd = new MySqlCommand("INSERT INTO tblmodel (id, omschrijving, typeID, merkID) VALUES(null, @model, @typeID, @merkID);SELECT LAST_INSERT_ID();");
                    {
                        cmd.Parameters.AddWithValue("@model", model);
                        cmd.Parameters.AddWithValue("@typeID", typeID);
                        cmd.Parameters.AddWithValue("@merkID", merkID);
                        cmd.Connection = con;
                        modelID = Convert.ToInt16(cmd.ExecuteScalar());
                    }
                }
                else
                {
                    modelID = Convert.ToInt16(cmd.ExecuteScalar());
                }

                cmd = new MySqlCommand("select tblleveranciers.id from tblleveranciers where tblleveranciers.naam like @leverancier;");
                cmd.Parameters.AddWithValue("@leverancier", leverancier);
                cmd.Connection = con;
                leverancierID = Convert.ToInt16(cmd.ExecuteScalar());

                //het apparaat updaten
                cmd = new MySqlCommand("UPDATE tblapparaat SET modelID = @modelID, serienummer = @serienummer, leverancierID = @leverancier, stock = @stock, factuurnummer = @factuurnummer WHERE(id = @ID);");
                cmd.Parameters.AddWithValue("@modelID", modelID);
                cmd.Parameters.AddWithValue("@serienummer", serienummer);
                cmd.Parameters.AddWithValue("@leverancier", leverancierID);
                cmd.Parameters.AddWithValue("@stock", instock);
                cmd.Parameters.AddWithValue("@factuurnummer", factuurnummer);
                cmd.Parameters.AddWithValue("@ID", apparaatID);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();

                if (stockCB.Checked == false)
                {
                    if ((row.Cells[11]).Text == gebruiker)
                    {
                        // werknemer ophalen
                        cmd = new MySqlCommand("select tblwerknemer.id from tblwerknemer where tblwerknemer.naam like @naam;");
                        cmd.Parameters.AddWithValue("@naam", gebruiker);
                        cmd.Connection = con;
                        werknemerID = Convert.ToInt16(cmd.ExecuteScalar());

                        //Historiek toevoegen
                        cmd = new MySqlCommand("INSERT INTO tblhistoriek(id, ingebruik, uitgebruik, werknemerID, apparaatID) VALUES(null, CURDATE(), null, @werknemer, @apparaat);");
                        cmd.Parameters.AddWithValue("@apparaat", apparaatID);
                        cmd.Parameters.AddWithValue("@werknemer", werknemerID);
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();

                    }
                }
                con.Close();
            }
            ApparatenGridView.EditIndex = -1;
            this.BindGridApparaten();
        }

        protected void ApparatenGridView_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.BindGridApparaten(e.SortExpression);
        }

        protected void ApparatenGridView_OnPaging(object sender, GridViewPageEventArgs e)
        {
            ApparatenGridView.PageIndex = e.NewPageIndex;
            this.BindGridApparaten();
        }

        protected void ApparatenGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ApparatenGridView.EditIndex = -1;
            this.BindGridApparaten();
        }

        protected void ApparatenGridView_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CommandHistoriek")
            {
                GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                string serienummer = row.Cells[7].Text;
                string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT COALESCE(tblhistoriek.id,0) as 'id', COALESCE(tblhistoriek.ingebruikname, 'onbekend') as 'ingebruikname', COALESCE(tblapparaat.serienummer, 0) as 'serienummer', COALESCE(tbltype.omschrijving, 'onbekend') as 'type', COALESCE(tblmodel.omschrijving, 'onbekend') as 'model', COALESCE(tblmerk.omschrijving, 'onbekend') as 'merk',COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 'onbekend') as 'locatie', COALESCE(tbllocatie.verdieping, 'onbekend') as 'verdieping' FROM tblhistoriek left outer JOIN tblapparaat ON tblapparaat.id = tblhistoriek.apparaatID left outer JOIN tblmodel ON tblmodel.id = tblapparaat.modelID left outer JOIN tbltype ON tblmodel.typeID = tbltype.id left outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id left outer JOIN tblwerknemer ON tblwerknemer.id = tblhistoriek.werknemerID left outer JOIN tbllocatie ON tbllocatie.id = tblwerknemer.locatieID WHERE (serienummer = @serienummer) AND tblhistoriek.id IS NOT NULL order by serienummer;"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@serienummer", serienummer);
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                HistoriekGridView.DataSource = dt;
                            }
                            HistoriekGridView.DataBind();
                        }
                    }
                }
                ApparatenGridView.Visible = false;
                HistoriekGridView.Visible = true;
            }
        }

        /*--------------------------------------------------------------------------------------------------*/
        //WERKNEMER CODE
        /*--------------------------------------------------------------------------------------------------*/

        protected void BindGridWerknemer(string sortExpression = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT COALESCE(tblwerknemer.id,0) as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 'onbekend') as 'locatie',COALESCE(tblwerknemer.status, 0) as 'status', COALESCE(tblwerknemer.commentaar, 0) as 'commentaar' FROM tblwerknemer left outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE tblwerknemer.id IS NOT NULL UNION SELECT  COALESCE(tblwerknemer.id, 0) as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 'onbekend') as 'locatie', COALESCE(tblwerknemer.status, 0) as 'status', COALESCE(tblwerknemer.commentaar, 0) as 'commentaar' FROM tblwerknemer right outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE tblwerknemer.id IS NOT NULL;"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (sortExpression != null)
                            {
                                DataView dv = dt.AsDataView();
                                this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";

                                dv.Sort = sortExpression + " " + this.SortDirection;
                                WerknemersGridView.DataSource = dv;
                            }
                            else
                            {
                                WerknemersGridView.DataSource = dt;
                            }
                            WerknemersGridView.DataBind();
                        }
                    }
                }
            }
        }

        protected void WerknemersGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int werknemerID = Convert.ToInt32(WerknemersGridView.DataKeys[e.RowIndex].Values[0]);
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM tblwerknemer WHERE (id = @ID);"))
                {
                    cmd.Parameters.AddWithValue("@id", werknemerID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGridWerknemer();
        }

        protected void WerknemersGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            WerknemersGridView.EditIndex = e.NewEditIndex;
            this.BindGridWerknemer();
        }

        protected void WerknemersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = WerknemersGridView.Rows[e.RowIndex];
            int idWerknemer = Convert.ToInt32(WerknemersGridView.DataKeys[e.RowIndex].Values[0]);
            string naam = (row.Cells[4].Controls[0] as TextBox).Text;
            string status = (row.Cells[6].Controls[0] as TextBox).Text;
            string commentaar = (row.Cells[7].Controls[0] as TextBox).Text;
            AjaxControlToolkit.ComboBox locatieCB = ((AjaxControlToolkit.ComboBox)row.FindControl("ComboBoxLocatieEdit"));
            DropDownList locatieDD = ((DropDownList)row.FindControl("LocatieEditDropDownList"));
            string locatie = locatieDD.SelectedItem.ToString();

            int locatieID;
            int id = Convert.ToInt32(WerknemersGridView.DataKeys[e.RowIndex].Values[0]);
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                // checken of het ingegeven locatie al bestaat en anders aanmaken:
                MySqlCommand cmd = new MySqlCommand("select tbllocatie.id from tbllocatie where tbllocatie.omschrijving like @locatie;");
                cmd.Parameters.AddWithValue("@locatie", locatie);
                cmd.Connection = con;
                con.Open();
                if (cmd.ExecuteScalar() == null)
                {
                    cmd = new MySqlCommand("INSERT INTO tbllocatie(id,omschrijving,commentaar) VALUES(null,@locatie,@commentaar);SELECT LAST_INSERT_ID();");
                    {
                        cmd.Parameters.AddWithValue("@locatie", locatie);
                        cmd.Connection = con;
                        locatieID = Convert.ToInt16(cmd.ExecuteScalar());
                    }
                }
                else
                {
                    locatieID = Convert.ToInt16(cmd.ExecuteScalar());
                }

                //Werknemer toevoegen
                cmd = new MySqlCommand("UPDATE tblwerknemer SET locatieID = @locatieID, naam = @naam, status = @status, commentaar = @commentaar WHERE(id = @ID);");
                cmd.Parameters.AddWithValue("@locatieID", locatieID);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@naam", naam);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@commentaar", commentaar);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            WerknemersGridView.EditIndex = -1;
            this.BindGridWerknemer();
        }

        protected void WerknemersGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WerknemersGridView.EditIndex = -1;
            this.BindGridWerknemer();
        }

        protected void WerknemersGridView_OnPaging(object sender, GridViewPageEventArgs e)
        {
            WerknemersGridView.PageIndex = e.NewPageIndex;
            this.DataBind();
        }

        protected void WerknemersGridView_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.BindGridWerknemer(e.SortExpression);
        }

        /*--------------------------------------------------------------------------------------------------*/
        //Historiek CODE
        /*--------------------------------------------------------------------------------------------------*/

        protected void BindGridHistoriek(string sortExpression = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblhistoriek.id as 'id', COALESCE(tblhistoriek.ingebruik, 'onbekend') as 'Ingebruik', COALESCE(tblhistoriek.uitgebruik, 'onbekend') as 'Uitgebruik', COALESCE(tblwerknemer.naam, 'onbekend') as 'Werknemer', COALESCE(tbltype.omschrijving, 'onbekend') as 'Type', COALESCE(tblmerk.omschrijving, 'onbekend') as 'Merk', COALESCE(tblmodel.omschrijving, 'onbekend') as 'Model', COALESCE(tblapparaat.serienummer, 'onbekend') as 'Serienummer' FROM tblhistoriek left outer JOIN tblwerknemer ON tblwerknemer.id = tblhistoriek.werknemerID left outer JOIN tblapparaat ON tblapparaat.id = tblhistoriek.apparaatID left outer JOIN tblmodel ON tblmodel.id = tblapparaat.modelID left outer JOIN tbltype ON tbltype.id = tblmodel.typeID left outer JOIN tblmerk ON tblmerk.id = tblmodel.merkID WHERE tblhistoriek.id IS NOT NULL order by serienummer;"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (sortExpression != null)
                            {
                                DataView dv = dt.AsDataView();
                                this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";

                                dv.Sort = sortExpression + " " + this.SortDirection;
                                HistoriekGridView.DataSource = dv;
                            }
                            else
                            {
                                HistoriekGridView.DataSource = dt;
                            }
                            HistoriekGridView.DataBind();
                        }
                    }
                }
            }
        }

        protected void HistoriekGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            HistoriekGridView.EditIndex = e.NewEditIndex;
            this.BindGridHistoriek();
        }

        protected void HistoriekGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            HistoriekGridView.EditIndex = -1;
            this.BindGridHistoriek();
        }

        protected void HistoriekGridView_OnPaging(object sender, GridViewPageEventArgs e)
        {
            HistoriekGridView.PageIndex = e.NewPageIndex;
            this.DataBind();
        }

        protected void HistoriekGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {/*
            GridViewRow row = HistoriekGridView.Rows[e.RowIndex];
            int idWerknemer = Convert.ToInt32(WerknemersGridView.DataKeys[e.RowIndex].Values[0]);
            int idApparaat = Convert.ToInt32(ApparatenGridView.DataKeys[e.RowIndex].Values[0]);

            AjaxControlToolkit.ComboBox locatieCB = ((AjaxControlToolkit.ComboBox)row.FindControl("ComboBoxLocatieEdit"));
            string locatie = locatieCB.SelectedItem.ToString();

            DropDownList verdiepingCB = ((DropDownList)row.FindControl("DropDownListVerdiepingEdit"));
            string verdieping = verdiepingCB.SelectedItem.ToString();

            int locatieID;
            int id = Convert.ToInt32(WerknemersGridView.DataKeys[e.RowIndex].Values[0]);
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                // checken of het ingegeven locatie al bestaat en anders aanmaken:
                MySqlCommand cmd = new MySqlCommand("select tbllocatie.id from tbllocatie where tbllocatie.omschrijving like @locatie AND tbllocatie.verdieping LIKE @verdieping;");
                cmd.Parameters.AddWithValue("@locatie", locatie);
                cmd.Parameters.AddWithValue("@verdieping", verdieping);
                cmd.Connection = con;
                con.Open();
                if (cmd.ExecuteScalar() == null)
                {
                    cmd = new MySqlCommand("INSERT INTO tbllocatie(id,omschrijving,verdieping) VALUES(null,@locatie,@verdieping);SELECT LAST_INSERT_ID();");
                    {
                        cmd.Parameters.AddWithValue("@locatie", locatie);
                        cmd.Parameters.AddWithValue("@verdieping", verdieping);
                        cmd.Connection = con;
                        locatieID = Convert.ToInt16(cmd.ExecuteScalar());
                    }
                }
                else
                {
                    locatieID = Convert.ToInt16(cmd.ExecuteScalar());
                }

                //Werknemer toevoegen
                cmd = new MySqlCommand("UPDATE tblwerknemer SET locatieID = @locatieID, naam = @naam WHERE(id = @ID);");
                cmd.Parameters.AddWithValue("@locatieID", locatieID);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@naam", naam);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            WerknemersGridView.EditIndex = -1;
            this.BindGridWerknemer();*/
        }

        protected void btnTypeAanmaken_Click(object sender, EventArgs e)
        {
            string omschrijving = txtTypeNew.Text;
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                // checken of het ingegeven type al bestaat en anders aanmaken:
                MySqlCommand cmd = new MySqlCommand("INSERT INTO tbltype(id,omschrijving) VALUES(null,@omschrijving);");
                cmd.Parameters.AddWithValue("@omschrijving", omschrijving);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnMerkAanmaken_Click(object sender, EventArgs e)
        {
            string omschrijving = txtMerkNew.Text;
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                // checken of het ingegeven type al bestaat en anders aanmaken:
                MySqlCommand cmd = new MySqlCommand("INSERT INTO tblMerk(id,omschrijving) VALUES(null,@omschrijving);");
                cmd.Parameters.AddWithValue("@omschrijving", omschrijving);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnModelAanmaken_Click(object sender, EventArgs e)
        {
            string omschrijving = TxtModelNew.Text;
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO tblmodel (id, omschrijving, typeID, merkID) VALUES(null, @model, @typeID, @merkID);");
                //cmd.Parameters.AddWithValue("@typeID", typeID);
                //cmd.Parameters.AddWithValue("@merkID", merkID);
                cmd.Connection = con;
                if (cmd.ExecuteScalar() == null)
                {
                    cmd = new MySqlCommand("SELECT LAST_INSERT_ID();");
                    {
                        //cmd.Parameters.AddWithValue("@model", model);
                        //cmd.Parameters.AddWithValue("@typeID", typeID);
                        //cmd.Parameters.AddWithValue("@merkID", merkID);
                        //cmd.Connection = con;
                        //modelID = Convert.ToInt16(cmd.ExecuteScalar());
                    }
                }
            }
        }

        protected void btnLeverancierAanmaken_Click(object sender, EventArgs e)
        {

        }

    }
}