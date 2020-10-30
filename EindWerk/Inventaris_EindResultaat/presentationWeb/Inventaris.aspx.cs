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
using System.IO; //NODIG VOOR EXPORT

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

namespace presentationWeb
{
    public partial class Inventaris : System.Web.UI.Page
    {

        /*--------------------------------------------------------------------------------------------------*/
        //Gedeelde methodes
        /*--------------------------------------------------------------------------------------------------*/
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindGridApparaten();
                this.BindGridWerknemer();
                this.BindGridHistoriek();
                this.BindGridControls();
                this.BindGridLeveranciers();
                btnBekijkApparaten.Attributes.Add("style", "background:#C0C0C0");
                //DDLType.Items.Add("");
                //DDLType.SelectedValue = "";
            }
            if (Page.IsPostBack)
            {
                DDLModelPopulate(null, null);
            }
        } //postbacks events & fire voor databinds

        protected void Change_gebruiker_state(object sender, EventArgs e)
        {
            if (CBStock.Checked == false)
            {
                DDLGebruiker.Enabled = true;
            }
            else if (CBStock.Checked == true)
            {
                DDLGebruiker.Enabled = false;
            }
        } //apparaat aanmaken -> gebruiker & stock relatie onderdrukken

        protected void Change_gebruiker_state_gv(object sender, EventArgs e)
        {
            GridViewRow row = ((CheckBox)sender).Parent.Parent as GridViewRow;
            CheckBox stockCB = ((CheckBox)row.FindControl("cbStockEdit"));
            bool instock = stockCB.Checked;
            //GEBRUIKER EDIT
            DropDownList gebruikerDL = ((DropDownList)row.FindControl("DDLGebruikerEdit"));
            string gebruiker = gebruikerDL.SelectedItem.ToString();
            if (true)
            {
                if (instock == true)
                {
                    gebruikerDL.Enabled = false;
                    gebruikerDL.SelectedIndex = -1; ;
                }
                else if (instock == false)
                {
                    gebruikerDL.Enabled = true;
                }
            }
        } //apparaat bewerken -> gebruiker & stock relatie onderdrukken

        private void DDLModelPopulate(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                //Datasource: DDLModel
                if (DDLMerk.SelectedValue.ToString() != "Select" && DDLType.SelectedValue.ToString() != "Select")
                {
                    DDLModel.Enabled = true;
                    MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT omschrijving FROM dbinventaris.tblmodel WHERE omschrijving IS NOT NULL AND omschrijving not like '' AND tblmodel.typeID = (SELECT tbltype.id FROM tbltype WHERE tbltype.omschrijving like @type) AND tblmodel.merkID = (SELECT tblmerk.id FROM tblmerk WHERE tblmerk.omschrijving like @merk) ORDER BY omschrijving asc;");
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@type", DDLType.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@merk", DDLMerk.SelectedItem.ToString());
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            DDLModel.DataSource = dt;
                            DDLModel.DataBind();
                        }
                    }
                }
                else
                {
                    DDLModel.Enabled = false;
                }
            }
        } //apparaat aanmaken -> type, merk - model relatie onderdrukken

        /* protected void DDLModelPopulateGV(object sender, EventArgs e)
         {
             //< !--AutoPostback = "true" OnSelectedIndexChanged = "DDLModelPopulateGV"-- >
               //ROW
               GridViewRow row = ((DropDownList)sender).Parent.Parent as GridViewRow;
             //ID
                 //TYPE EDIT
                 DropDownList typeCB = ((DropDownList)row.FindControl("DDLTypeEdit"));
                 //MERK EDIT
                 DropDownList merkCB = ((DropDownList)row.FindControl("DDLMerkEdit"));
                 //MODEL EDIT
                 DropDownList modelCB = ((DropDownList)row.FindControl("DDLModelEdit"));

             string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
             {
                     MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT omschrijving FROM dbinventaris.tblmodel WHERE omschrijving IS NOT NULL AND omschrijving not like '' AND tblmodel.typeID = (SELECT tbltype.id FROM tbltype WHERE tbltype.omschrijving like @type) AND tblmodel.merkID = (SELECT tblmerk.id FROM tblmerk WHERE tblmerk.omschrijving like @merk) ORDER BY omschrijving asc;");
                     using (MySqlDataAdapter sda = new MySqlDataAdapter())
                     {
                         cmd.Parameters.AddWithValue("@type", typeCB.SelectedItem.ToString());
                         cmd.Parameters.AddWithValue("@merk", merkCB.SelectedItem.ToString());
                         cmd.Connection = con;
                         sda.SelectCommand = cmd;
                         using (DataTable dt = new DataTable())
                         {
                             sda.Fill(dt);
                         modelCB.DataSource = dt;
                         modelCB.DataBind();
                         }
                     }
                 }
             }*/ //apparaat bewerken -> type, merk - model relatie onderdrukken

        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        } //header klik gv sorting volgorde

        public void SearchData(string item)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                //Zoeken in grid apparaten
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat left outer join (SELECT apparaatID, MIN(ingebruik),werknemerID FROM tblhistoriek GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id left outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID left outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id left outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id left outer JOIN tbltype ON tblmodel.typeID = tbltype.id left outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE tblapparaat.id IS NOT NULL AND tblmodel.omschrijving LIKE CONCAT(@item) OR tblapparaat.serienummer LIKE CONCAT(@item) OR tblapparaat.uitgebruik LIKE CONCAT(@item) OR tblleveranciers.naam LIKE CONCAT(@item) OR tblmodel.omschrijving LIKE CONCAT(@item) OR tblapparaat.factuurnummer LIKE CONCAT(@item) OR tblapparaat.aankoopdatum LIKE CONCAT(@item) OR tbltype.omschrijving LIKE CONCAT(@item) OR tblmerk.omschrijving LIKE CONCAT(@item) OR tblwerknemer.naam like CONCAT(@item)"))
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
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblwerknemer.id as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 0) as 'locatie', COALESCE(tblwerknemer.status, 'onbekend') as 'status', COALESCE(tblwerknemer.commentaar, 'onbekend') as 'commentaar' FROM tblwerknemer left outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE naam LIKE CONCAT(@item) OR tbllocatie.omschrijving LIKE CONCAT(@item) OR tblwerknemer.status LIKE CONCAT(@item) OR tblwerknemer.commentaar LIKE CONCAT(@item) AND tblwerknemer.id IS NOT NULL UNION SELECT tblwerknemer.id as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 0) as 'locatie', COALESCE(tblwerknemer.status, 'onbekend') as 'status', COALESCE(tblwerknemer.commentaar, 'onbekend') as 'commentaar' FROM tblwerknemer right outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE naam LIKE CONCAT(@item) OR tbllocatie.omschrijving LIKE CONCAT(@item) OR tblwerknemer.status LIKE CONCAT(@item) AND tblwerknemer.id IS NOT NULL;"))
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
                //Zoeken in grid leveranciers
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblleveranciers.id as 'id', COALESCE(tblleveranciers.naam, 'onbekend') as 'naam', COALESCE(tblleveranciers.contactpersoon, 'onbekend') as 'contactpersoon', COALESCE(tblleveranciers.email, 'onbekend') as 'email', COALESCE(tblleveranciers.telefoonnummer, 'onbekend') as 'telefoonnummer' FROM tblleveranciers WHERE tblleveranciers.id IS NOT NULL AND naam LIKE CONCAT(@item) OR contactpersoon LIKE CONCAT(@item) OR email LIKE CONCAT(@item) OR telefoonnummer LIKE CONCAT(@item) order by naam;"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@item", '%' + item + '%');
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            LeveranciersGridView.DataSource = dt;
                        }
                        LeveranciersGridView.DataBind();
                    }
                }
            }
        } //gv opnieuw binden met filter data van de search

        protected void UncheckAll_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
        } //alle selectievakjes deselecteren

        protected void CheckAll_Click(object sender, EventArgs e)
        {
            ToggleCheckState(true);
        } //alle selectievakjes selecteren

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

            //Leveranciers
            if (LeveranciersGridView.Visible == true)
            {
                foreach (GridViewRow row in LeveranciersGridView.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("cbLeveranciers");
                    if (cb != null)
                        cb.Checked = checkState;
                }
            }
        } //de rijen selecteren

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
                            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM tblapparaat WHERE (id = @ID);"))
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

        } // de geselecteerde velden verwijderen

        /*--------------------------------------------------------------------------------------------------*/
        //Methode Controlpanel / aanmaken
        /*--------------------------------------------------------------------------------------------------*/
        public override void VerifyRenderingInServerForm(Control control)
        {

        } //Nodig om het form/control te controleren voor export

        protected void ButtonBekijkApparaten_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
            ApparatenGridView.Visible = true;
            WerknemersGridView.Visible = false;
            HistoriekGridView.Visible = false;
            LeveranciersGridView.Visible = false;
            if (ApparatenGridView.Visible == true)
            {
                btnBekijkApparaten.Attributes.Add("style", "background:#C0C0C0");
                btnBekijkWerknemers.Attributes.Add("style", "background:#FCFCFC");
                btnBekijkHistoriek.Attributes.Add("style", "background:#FCFCFC");
                btnBekijkLeveranciers.Attributes.Add("style", "background:#FCFCFC");
            }
            this.BindGridApparaten();
        } //laat alle actieve apparaten zien in de gv

        protected void ButtonBekijkWerknemers_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
            ApparatenGridView.Visible = false;
            WerknemersGridView.Visible = true;
            HistoriekGridView.Visible = false;
            LeveranciersGridView.Visible = false;

            if (WerknemersGridView.Visible == true)
            {
                btnBekijkApparaten.Attributes.Add("style", "background:#FCFCFC");
                btnBekijkWerknemers.Attributes.Add("style", "background:#C0C0C0");
                btnBekijkHistoriek.Attributes.Add("style", "background:#FCFCFC");
                btnBekijkLeveranciers.Attributes.Add("style", "background:#FCFCFC");
            }
            this.BindGridWerknemer();
        } //laat alle actieve werknemers zien in de gv

        protected void ButtonBekijkHistoriek_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
            ApparatenGridView.Visible = false;
            WerknemersGridView.Visible = false;
            LeveranciersGridView.Visible = false;
            HistoriekGridView.Visible = true;

            //Button feedback
            if (HistoriekGridView.Visible == true)
            {
                btnBekijkApparaten.Attributes.Add("style", "background:#FCFCFC");
                btnBekijkWerknemers.Attributes.Add("style", "background:#FCFCFC");
                btnBekijkLeveranciers.Attributes.Add("style", "background:#FCFCFC");
                btnBekijkHistoriek.Attributes.Add("style", "background:#C0C0C0");
            }

            this.BindGridHistoriek();
        } //laat alle historieken zien in de gv

        protected void ButtonBekijkLeveranciers_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
            ApparatenGridView.Visible = false;
            WerknemersGridView.Visible = false;
            HistoriekGridView.Visible = false;
            LeveranciersGridView.Visible = true;

            //Button feedback
            if (LeveranciersGridView.Visible == true)
            {
                btnBekijkApparaten.Attributes.Add("style", "background:#FCFCFC");
                btnBekijkWerknemers.Attributes.Add("style", "background:#FCFCFC");
                btnBekijkLeveranciers.Attributes.Add("style", "background:#C0C0C0");
                btnBekijkHistoriek.Attributes.Add("style", "background:#FCFCFC");
            }

            this.BindGridLeveranciers();
        } //laat alle leveranciers zien in de gv

        protected void Filter_Apparaat()
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {

                TextBox txtMerk = (ApparatenGridView.HeaderRow.FindControl("txtMerkFilter") as TextBox);
                string itemMerk = null;
                if (txtMerk.Text != "")
                { itemMerk = '%' + txtMerk.Text + '%'; }

                TextBox txtType = (ApparatenGridView.HeaderRow.FindControl("txtTypeFilter") as TextBox);
                string itemType = null;
                if (txtType.Text != "")
                { itemType = '%' + txtType.Text + '%'; }

                TextBox txtModel = (ApparatenGridView.HeaderRow.FindControl("txtModelFilter") as TextBox);
                string itemModel = null;
                if (txtModel.Text != "")
                { itemModel = '%' + txtModel.Text + '%'; }

                TextBox txtLeverancier = (ApparatenGridView.HeaderRow.FindControl("txtLeverancierFilter") as TextBox);
                string itemLeverancier = null;
                if (txtLeverancier.Text != "")
                { itemLeverancier = '%' + txtLeverancier.Text + '%'; }

                TextBox txtGebruiker = (ApparatenGridView.HeaderRow.FindControl("txtGebruikerFilter") as TextBox);
                string itemGebruiker = null;
                if (txtGebruiker.Text != "")
                { itemGebruiker = '%' + txtGebruiker.Text + '%'; }

                TextBox cditem1 = (ApparatenGridView.HeaderRow.FindControl("TxtDatumFilter1") as TextBox);
                TextBox cditem2 = (ApparatenGridView.HeaderRow.FindControl("TxtDatumFilter2") as TextBox);
                DateTime? itemSOL1 = null;
                if (cditem1.Text != "")
                { itemSOL1 = Convert.ToDateTime(cditem1.Text); ; }
                DateTime? itemSOL2 = null;
                if (cditem2.Text != "")
                { itemSOL2 = Convert.ToDateTime(cditem2.Text); }

                CheckBox cbStock = (ApparatenGridView.HeaderRow.FindControl("cbStockFilter") as CheckBox);
                bool itemstock = cbStock.Checked;

                //Zoeken in grid apparaten
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat left outer join (SELECT apparaatID, MIN(ingebruik),werknemerID FROM tblhistoriek GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id left outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID left outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id left outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id left outer JOIN tbltype ON tblmodel.typeID = tbltype.id left outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE (tblapparaat.id IS NOT NULL) AND(@type IS NULL OR tbltype.omschrijving LIKE @type) AND (@merk IS NULL OR tblmerk.omschrijving LIKE @merk) AND (@model IS NULL OR tblmodel.omschrijving LIKE @model) AND (@leverancier IS NULL or tblleveranciers.naam LIKE @leverancier) AND (@stock IS NULL OR tblapparaat.stock LIKE @stock) AND(@gebruiker IS NULL OR tblwerknemer.naam LIKE @gebruiker) AND ((@itemSOL1 IS NULL AND @itemSOL2 IS NULL) or (tblapparaat.aankoopdatum >= @itemSOL1 AND tblapparaat.aankoopdatum <= @itemSOL2))")) // AND (@itemSOL1 IS NULL AND @itemSOL2 IS NULL or tblapparaat.aankoopdatum >= @itemSOL1 AND tblapparaat.aankoopdatum =< @itemSOL2)
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@type", itemType);
                        cmd.Parameters.AddWithValue("@merk", itemMerk);
                        cmd.Parameters.AddWithValue("@model", itemModel);
                        cmd.Parameters.AddWithValue("@leverancier", itemLeverancier);
                        cmd.Parameters.AddWithValue("@stock", Convert.ToInt16(itemstock));
                        cmd.Parameters.AddWithValue("@gebruiker", itemGebruiker);
                        cmd.Parameters.AddWithValue("@itemSOL1", itemSOL1);
                        cmd.Parameters.AddWithValue("@itemSOL2", itemSOL2);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            ApparatenGridView.DataSource = dt;
                        }
                        ApparatenGridView.DataBind();
                        Enable_Disable_FiltersH(null, null);
                    }

                }
                txtMerk = (ApparatenGridView.HeaderRow.FindControl("txtMerkFilter") as TextBox);
                if (itemMerk != null)
                {
                    txtMerk.Text = itemMerk.Trim('%').ToString();
                }
                txtType = (ApparatenGridView.HeaderRow.FindControl("txtTypeFilter") as TextBox);
                if (itemType != null)
                {
                    txtType.Text = itemType.Trim('%').ToString();
                }
                txtModel = (ApparatenGridView.HeaderRow.FindControl("txtModelFilter") as TextBox);
                if (itemModel != null)
                {
                    txtModel.Text = itemModel.Trim('%').ToString();
                }
                txtLeverancier = (ApparatenGridView.HeaderRow.FindControl("txtLeverancierFilter") as TextBox);
                if (itemLeverancier != null)
                {
                    txtLeverancier.Text = itemLeverancier.Trim('%').ToString();
                }
                txtGebruiker = (ApparatenGridView.HeaderRow.FindControl("txtGebruikerFilter") as TextBox);
                if (itemGebruiker != null)
                {
                    txtGebruiker.Text = itemGebruiker.Trim('%').ToString();
                }
                cditem1 = (ApparatenGridView.HeaderRow.FindControl("TxtDatumFilter1") as TextBox);
                if (itemSOL1 != null)
                {
                    cditem1.Text = itemSOL1.ToString();
                }
                cditem2 = (ApparatenGridView.HeaderRow.FindControl("TxtDatumFilter2") as TextBox);
                if (itemSOL2 != null)
                {
                    cditem2.Text = itemSOL2.ToString();
                }
                cbStock = (ApparatenGridView.HeaderRow.FindControl("cbStockFilter") as CheckBox);
                cbStock.Checked = Convert.ToBoolean(itemstock);
            }
        } //vul gv apparaten adhv filter data (databind)

        protected void Enable_Disable_FiltersH(object sender, System.EventArgs e)
        {
            TextBox txtMerk = (ApparatenGridView.HeaderRow.FindControl("txtMerkFilter") as TextBox);
            TextBox txtType = (ApparatenGridView.HeaderRow.FindControl("txtTypeFilter") as TextBox);
            TextBox txtModel = (ApparatenGridView.HeaderRow.FindControl("txtModelFilter") as TextBox);
            TextBox txtLeverancier = (ApparatenGridView.HeaderRow.FindControl("txtLeverancierFilter") as TextBox);
            TextBox txtGebruiker = (ApparatenGridView.HeaderRow.FindControl("txtGebruikerFilter") as TextBox);
            TextBox cditem1 = (ApparatenGridView.HeaderRow.FindControl("TxtDatumFilter1") as TextBox);
            TextBox cditem2 = (ApparatenGridView.HeaderRow.FindControl("TxtDatumFilter2") as TextBox);
            CheckBox cbStock = (ApparatenGridView.HeaderRow.FindControl("cbStockFilter") as CheckBox);
            if (txtMerk.Visible == false)
            {
                txtMerk.Visible = true;
                txtType.Visible = true;
                txtModel.Visible = true;
                txtLeverancier.Visible = true;
                txtGebruiker.Visible = true;
                cditem1.Visible = true;
                cditem2.Visible = true;
                cbStock.Visible = true;
            }
            else
            {
                txtMerk.Visible = false;
                txtType.Visible = false;
                txtModel.Visible = false;
                txtLeverancier.Visible = false;
                txtGebruiker.Visible = false;
                cditem1.Visible = false;
                cditem2.Visible = false;
                cbStock.Visible = false;
            }

        } //toon of verberg de header voor filters

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
                DDLMerk.Items.Insert(0, new ListItem("--Selecteer--", "Select"));
                DDLModel.Enabled = false;
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
                DDLType.Items.Insert(0, new ListItem("--Selecteer--", "Select"));
                DDLModel.Items.Insert(0, new ListItem("--Selecteer--", "Select"));

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

                DDLLeverancier.Items.Insert(0, new ListItem("--Selecteer--", "Select"));

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

                DDLLocatie.Items.Insert(0, new ListItem("--Selecteer--", "Select"));

                //Datasource: DDLGebruiker
                cmd = new MySqlCommand("SELECT DISTINCT naam FROM dbinventaris.tblwerknemer WHERE naam IS NOT NULL AND naam not like '' and status like 1 ORDER BY naam asc;");
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

                //Datasource: DDLTypeConfirmation
                cmd = new MySqlCommand("SELECT DISTINCT(omschrijving) FROM dbinventaris.tbltype WHERE omschrijving IS NOT NULL AND omschrijving not like '' ORDER BY omschrijving asc;");
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DDLTypeConfirmation.DataSource = dt;
                        DDLTypeConfirmation.DataBind();
                    }
                }

                DDLTypeConfirmation.Items.Insert(0, new ListItem("--Selecteer--", "Select"));

                //Datasource: DDLMerkConfirmation
                cmd = new MySqlCommand("SELECT DISTINCT(omschrijving) FROM dbinventaris.tblMerk WHERE omschrijving IS NOT NULL AND omschrijving not like '' ORDER BY omschrijving asc;");
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DDLMerkConfirmation.DataSource = dt;
                        DDLMerkConfirmation.DataBind();
                    }
                }
            }

            DDLMerkConfirmation.Items.Insert(0, new ListItem("--Selecteer--", "Select"));

        } //Al de controls (aanmaken ddl) databinden

        protected void ButtonZoeken_Click(object sender, EventArgs e)
        {
            string item = txtZoeken.Text.ToString();
            SearchData(item);
            txtZoeken.Text = "";
        } //ingevulde zoekdata doorgeven > SearchData()

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

            //Error label voor ingevuld
            if (type == "Select")
            {
                lblTypeError.Visible = true;
                lblTypeError.Text = "Selecteer een type";
                return;
            }

            if (merk == "--Selecteer--")
            {

                lblMerkError.Visible = true;
                lblTypeError.Visible = false;
                lblMerkError.Text = "Selecteer een merk";
                return;
            }
            if (model == "--Selecteer--")
            {
                lblModelError.Visible = true;
                lblMerkError.Visible = false;
                lblMerkError.Visible = false;
                lblModelError.Text = "Selecteer een model";
                return;
            }
            lblModelError.Visible = false;

            if (leverancier == "--Selecteer--")
            {
                lblLeverancierError.Visible = true;
                lblLeverancierError.Text = "Selecteer een leverancier";
                return;
            }
            lblLeverancierError.Visible = false;

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

                //het apparaat toevoegen
                cmd = new MySqlCommand("Insert INTO tblapparaat(id,modelID,serienummer,uitgebruik,leverancierID,factuurnummer,aankoopdatum,commentaar,stock)VALUES(null,@modelID,@serienummer,null,@leverancierID,@factuurnummer,CURDATE(),null,@stock);SELECT LAST_INSERT_ID();");
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

                    //Historiek toevoegen
                    cmd = new MySqlCommand("INSERT INTO tblhistoriek(id, ingebruik, uitgebruik, werknemerID, apparaatID) VALUES(null, CURDATE(), null, @werknemer, @apparaat);");
                    cmd.Parameters.AddWithValue("@werknemer", werknemerID);
                    cmd.Parameters.AddWithValue("@apparaat", apparaatID);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            DDLType.SelectedIndex = 0;
            DDLMerk.SelectedIndex = 0;
            TxtSerienummer.Text = "";
            DDLLeverancier.SelectedIndex = 0;
            TxtFactuurnummer.Text = "";
            DDLGebruiker.SelectedIndex = 0;
            DDLModel.SelectedIndex = 0;
            DDLModelPopulate(null, null);
            this.BindGridApparaten();
        } //Voeg een apparaat toe

        protected void Insert_Werknemer(object sender, EventArgs e)
        {
            string naam = txtWerknemerNaam.Text;
            string locatie = DDLLocatie.Text;
            DDLGebruiker.Items.Add(naam);

            if (naam == "")
            {
                lblWerknemerNaamError.Visible = true;
                lblWerknemerNaamError.Text = "Gelieve een naam in te vullen!";
                return;
            }
            lblWerknemerNaamError.Visible = false;

            //Error label voor ingevuld DDLLocatie
            if (locatie == "Select")
            {
                lblWerknemerLocatieError.Visible = true;
                lblWerknemerLocatieError.Text = "Selecteer een locatie";
                return;
            }
            lblWerknemerLocatieError.Visible = false;

            DDLLocatie.ClearSelection();
            txtWerknemerNaam.Text = "";

            //Connectie
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                int locatieID;
                string sql = "";
                sql = "select tbllocatie.id from tbllocatie where tbllocatie.omschrijving like @locatie;";
                MySqlCommand comm = new MySqlCommand(sql, con);
                comm.Parameters.AddWithValue("@naam", naam);
                comm.Parameters.AddWithValue("@locatie", locatie);

                con.Open();

                //Als niet bestaat, locatie aanmaken:
                if (comm.ExecuteScalar() != null)
                {
                    locatieID = Convert.ToInt16(comm.ExecuteScalar());
                }
                else
                {
                    sql = "INSERT INTO tbllocatie(id,omschrijving,commentaar) VALUES(null,@locatie,'','');SELECT LAST_INSERT_ID();";
                    comm = new MySqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@naam", naam);
                    comm.Parameters.AddWithValue("@locatie", locatie);
                    locatieID = Convert.ToInt16(comm.ExecuteScalar());
                }

                //Werknemer toevoegen
                sql = "INSERT INTO tblwerknemer(id, naam, locatieID, status, commentaar) VALUES(null, @naam, @locatieID, 1,'');";

                comm = new MySqlCommand(sql, con);
                comm.Parameters.AddWithValue("@naam", naam);
                comm.Parameters.AddWithValue("@locatieID", locatieID);

                comm.ExecuteNonQuery();
                con.Close();
                this.BindGridWerknemer();
            }
        } //Voeg een werknemer toe

        protected void btnTypeNew_Click(object sender, EventArgs e)
        {
            //Type toevoegen (button)
            string type = txtTypeNew.Text;


            if (type == "")
            {
                lblTypeOmschrijvingError.Visible = true;
                lblTypeOmschrijvingError.Text = "Gelieve een type in te vullen!";
                return;
            }
            lblTypeOmschrijvingError.Visible = false;

            txtTypeNew.Text = "";

            //Connectie
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {

                int typeID;
                string sql = "select tbltype.id from tbltype where tbltype.omschrijving like @type;";
                MySqlCommand comm = new MySqlCommand(sql, con);
                comm.Parameters.AddWithValue("@type", type);
                con.Open();

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
                    comm = new MySqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@type", type);
                    typeID = Convert.ToInt16(comm.ExecuteScalar());
                    lblTypeNewInfo.Visible = true;
                    lblTypeNewInfo.Text = "Type " + type + " successvol aangemaakt";
                    if (type != "")
                    {
                        DDLType.Items.Add(type);
                        DDLTypeConfirmation.Items.Add(type);
                    }
                }
            }
        } //Voeg een type toe

        protected void btnMerkNew_Click(object sender, EventArgs e)
        {
            //Merk toevoegen (button)
            string merk = txtMerkNew.Text;


            if (merk == "")
            {
                lblMerkOmschrijvingError.Visible = true;
                lblMerkOmschrijvingError.Text = "Gelieve een merk in te vullen!";
                return;
            }
            lblMerkOmschrijvingError.Visible = false;

            txtMerkNew.Text = "";

            //Connectie
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {

                int merkID;
                string sql = "select tblmerk.id from tblmerk where tblmerk.omschrijving like @merk;";
                MySqlCommand comm = new MySqlCommand(sql, con);
                comm.Parameters.AddWithValue("@merk", merk);
                con.Open();

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
                    comm = new MySqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@merk", merk);
                    merkID = Convert.ToInt16(comm.ExecuteScalar());
                    lblMerkNewInfo.Visible = true;
                    lblMerkNewInfo.Text = "Merk " + merk + " successvol aangemaakt";
                    if (merk != "")
                    {
                        DDLMerk.Items.Add(merk);
                        DDLMerkConfirmation.Items.Add(merk);
                    }
                }
            }
        } //Voeg een merk toe

        protected void btnModelNew_Click(object sender, EventArgs e)
        {
            //Model toevoegen (button)
            string modelNaam = TxtModelNew.Text;
            string typeNaam = DDLTypeConfirmation.SelectedValue;
            string merkNaam = DDLMerkConfirmation.SelectedValue;

            if (modelNaam == "")
            {
                lblModelOmschrijvingError.Visible = true;
                lblModelOmschrijvingError.Text = "Gelieve een model in te vullen!";
                return;
            }
            lblModelOmschrijvingError.Visible = false;

            //Error label voor ingevuld
            if (typeNaam == "Select")
            {
                lblTypeErrorModel.Visible = true;
                lblTypeErrorModel.Text = "Selecteer een type";
                return;
            }

            if (merkNaam == "Select")
            {
                lblMerkErrorModel.Visible = true;
                lblTypeErrorModel.Visible = false;
                lblMerkErrorModel.Text = "Selecteer een merk";
                return;
            }
            lblMerkErrorModel.Visible = false;

            TxtModelNew.Text = "";

            //Connectie
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {

                int modelID;
                int typeID;
                int merkID;
                string sql = "SELECT tblmodel.id FROM tblmodel left outer join tbltype ON tbltype.id = tblmodel.typeID left outer join tblmerk ON tblmerk.id = tblmodel.merkID where tblmodel.omschrijving like @modelNaam AND tbltype.omschrijving like @typeNaam AND tblmerk.omschrijving like @merkNaam";
                MySqlCommand comm = new MySqlCommand(sql, con);
                comm.Parameters.AddWithValue("@modelNaam", modelNaam);
                comm.Parameters.AddWithValue("@typeNaam", typeNaam);
                comm.Parameters.AddWithValue("@merkNaam", merkNaam);
                con.Open();

                //Als alle waarde van model bestaan -> Foutmelding:
                if (comm.ExecuteScalar() != null)
                {
                    lblModelConfirmation.Text = "Model " + modelNaam + " is reeds aangemaakt.";
                    modelID = Convert.ToInt16(comm.ExecuteScalar());
                }
                else
                //Als model NIET bestaat -> Model aanmaken met nieuwe id's
                {
                    //typeID ophalen:
                    sql = "SELECT tbltype.id FROM tbltype WHERE tbltype.omschrijving LIKE @typeNaam";
                    comm = new MySqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@typeNaam", typeNaam);
                    typeID = Convert.ToInt16(comm.ExecuteScalar());

                    //merkID ophalen:
                    sql = "SELECT tblmerk.id FROM tblmerk WHERE tblmerk.omschrijving LIKE @merkNaam";
                    comm = new MySqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@merkNaam", merkNaam);
                    merkID = Convert.ToInt16(comm.ExecuteScalar());

                    //Model aanmaken met ID's
                    sql = "INSERT INTO tblmodel(id,omschrijving,typeID,merkID) VALUES(null,@modelNaam, @typeID, @merkID);";
                    comm = new MySqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@modelNaam", modelNaam);
                    comm.Parameters.AddWithValue("@typeID", typeID);
                    comm.Parameters.AddWithValue("@merkID", merkID);
                    modelID = Convert.ToInt16(comm.ExecuteScalar());

                    lblModelNewInfo.Visible = true;
                    if (modelNaam != "")
                    {
                        lblModelNewInfo.Text = "Model " + modelNaam + " successvol aangemaakt";
                        DDLModel.Items.Add(modelNaam);
                    }
                }
            }
        } //Voeg een apparmodelaat toe

        protected void btnLeverancierNew_Click(object sender, EventArgs e)
        {
            //Leverancier toevoegen (button)
            string leverancierNaam = txtLeverancierNaamNew.Text;
            string leverancierContact = txtLeverancierContactNew.Text;
            string leverancierEmail = txtLeverancierEmailNew.Text;
            string leverancierTelefoon = txtLeverancierTeleNew.Text;

            if (leverancierNaam == "")
            {
                lblLeverancierNaamError.Visible = true;
                lblLeverancierNaamError.Text = "Gelieve een naam in te vullen!";
                return;
            }
            lblLeverancierNaamError.Visible = false;

            if (leverancierContact == "")
            {
                lblLeverancierContactError.Visible = true;
                lblLeverancierContactError.Text = "Gelieve contact in te vullen!";
                return;
            }
            lblLeverancierContactError.Visible = false;

            if (leverancierEmail == "")
            {
                lblLeverancierEmailError.Visible = true;
                lblLeverancierEmailError.Text = "Gelieve een email adres in te vullen!";
                return;
            }
            lblLeverancierEmailError.Visible = false;

            if (leverancierTelefoon == "")
            {
                lblLeverancierTelefoonnummerError.Visible = true;
                lblLeverancierTelefoonnummerError.Text = "Gelieve een telefoonnummer in te vullen!";
                return;
            }
            lblLeverancierTelefoonnummerError.Visible = false;

            //Velden clearen
            txtLeverancierNaamNew.Text = ""; //Naam
            txtLeverancierContactNew.Text = ""; //Contact
            txtLeverancierEmailNew.Text = ""; //Email
            txtLeverancierTeleNew.Text = ""; //Telefoonnummer

            //Connectie
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                int leverancierID;
                //Kijken naar naam van leverancier
                string sql = "select tblleveranciers.id from tblleveranciers where tblleveranciers.naam like @naam;";
                MySqlCommand comm = new MySqlCommand(sql, con);
                comm.Parameters.AddWithValue("@naam", leverancierNaam);
                con.Open();

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
                    comm = new MySqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@leverancierNaam", leverancierNaam);
                    comm.Parameters.AddWithValue("@leverancierContact", leverancierContact);
                    comm.Parameters.AddWithValue("@leverancierEmail", leverancierEmail);
                    comm.Parameters.AddWithValue("@leverancierTelefoon", leverancierTelefoon);
                    leverancierID = Convert.ToInt16(comm.ExecuteScalar());
                    lblLeverancierNewInfo.Visible = true;
                    lblLeverancierNewInfo.Text = "Leverancier " + leverancierNaam + " successvol aangemaakt";
                    if (leverancierNaam != "")
                    {
                        DDLLeverancier.Items.Add(leverancierNaam);
                    }
                }
            }
        } //Voeg een leverancier toe

        protected void btnLocatieWerknemerNew_Click(object sender, EventArgs e)
        {
            //Locatie toevoegen (button)
            string omschrijving = txtLocatieWerknemerNew.Text;
            txtLocatieWerknemerNew.Text = "";

            if (omschrijving == "")
            {
                lblLocatieOmschrijvingError.Visible = true;
                lblLocatieOmschrijvingError.Text = "Gelieve een locatie in te vullen!";
                return;
            }
            lblLocatieOmschrijvingError.Visible = false;

            //Connectie
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {

                int locatieID;
                //Kijken naar 'omschrijving' van locatie
                string sql = "select tbllocatie.id from tbllocatie where tbllocatie.omschrijving like @omschrijving;";
                MySqlCommand comm = new MySqlCommand(sql, con);
                comm.Parameters.AddWithValue("@omschrijving", omschrijving);
                con.Open();

                //Locatie bestaat?, anders aanmaken:
                if (comm.ExecuteScalar() != null)
                {
                    locatieID = Convert.ToInt16(comm.ExecuteScalar());
                    lblLocatieNewInfo.Visible = true;
                    lblLocatieNewInfo.Text = "Locatie reeds aangemaakt";
                }
                else
                {
                    sql = "INSERT INTO tbllocatie(id, omschrijving, commentaar) VALUES(null,@omschrijving,'');SELECT LAST_INSERT_ID();";
                    comm = new MySqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@omschrijving", omschrijving);
                    locatieID = Convert.ToInt16(comm.ExecuteScalar());
                    lblLocatieNewInfo.Visible = true;
                    lblLocatieNewInfo.Text = "Locatie " + omschrijving + " successvol aangemaakt";
                    if (omschrijving != "")
                    {
                        DDLLocatie.Items.Add(omschrijving);
                    }
                }
            }
        } //Voeg een locatie toe

        protected void btnMaakActief_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                //ApparatenGridView Actief
                if (ApparatenGridView.Visible == true)
                {
                    int teller = 0;
                    foreach (GridViewRow row in ApparatenGridView.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("CBApparaten");
                        if (cb != null && cb.Checked)
                        {
                            using (MySqlCommand cmd = new MySqlCommand("UPDATE tblapparaat SET uitgebruik = null WHERE(id = @ID);"))
                            {
                                teller++;
                                int apparaatid = Convert.ToInt32(ApparatenGridView.DataKeys[row.RowIndex].Value);
                                cmd.Parameters.AddWithValue("@ID", apparaatid);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                    ApparatenGridView.EditIndex = -1;
                    this.BindGridApparaten();
                }

                //WerknemersGridView Actief
                if (WerknemersGridView.Visible == true)
                {
                    int teller = 0;
                    foreach (GridViewRow row in WerknemersGridView.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("cbWerknemers");
                        if (cb != null && cb.Checked)
                        {
                            using (MySqlCommand cmd = new MySqlCommand("UPDATE tblwerknemer SET status = 1 WHERE(id = @ID);"))
                            {
                                teller++;
                                int werknemerid = Convert.ToInt32(WerknemersGridView.DataKeys[row.RowIndex].Value);
                                cmd.Parameters.AddWithValue("@ID", werknemerid);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                    WerknemersGridView.EditIndex = -1;
                    WerknemersGridView.DataBind();
                }
                this.BindGridControls();
                this.BindGridWerknemer();
            }
        } //Maakt een Gebruiker of apparaat actief

        protected void btnMaakInactief_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                //ApparatenGridView Actief
                if (ApparatenGridView.Visible == true)
                {
                    int teller = 0;
                    foreach (GridViewRow row in ApparatenGridView.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("CBApparaten");
                        if (cb != null && cb.Checked)
                        {
                            using (MySqlCommand cmd = new MySqlCommand("UPDATE tblapparaat SET uitgebruik = CURDATE() WHERE(id = @ID);"))
                            {
                                teller++;
                                int apparaatid = Convert.ToInt32(ApparatenGridView.DataKeys[row.RowIndex].Value);
                                cmd.Parameters.AddWithValue("@ID", apparaatid);

                                //Connectie openen en uitvoeren
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                    ApparatenGridView.EditIndex = -1;
                    this.BindGridApparaten();

                }

                //WerknemersGridView Actief
                if (WerknemersGridView.Visible == true)
                {
                    int teller = 0;
                    foreach (GridViewRow row in WerknemersGridView.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("cbWerknemers");
                        if (cb != null && cb.Checked)
                        {
                            using (MySqlCommand cmd = new MySqlCommand("UPDATE tblwerknemer SET status = 0 WHERE(id = @ID);"))
                            {
                                teller++;
                                int werknemerid = Convert.ToInt32(WerknemersGridView.DataKeys[row.RowIndex].Value);
                                cmd.Parameters.AddWithValue("@ID", werknemerid);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                    WerknemersGridView.EditIndex = -1;
                    this.BindGridWerknemer();
                    this.BindGridControls();
                }
            }
        } //Maakt een Gebruiker of apparaat inactief

        protected void btnLaatInactiefZien_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                if (ApparatenGridView.Visible == true)
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat left outer join (SELECT apparaatID, MIN(ingebruik),werknemerID FROM tblhistoriek GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id left outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID left outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id left outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id left outer JOIN tbltype ON tblmodel.typeID = tbltype.id left outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE tblapparaat.id IS NOT NULL AND tblapparaat.uitgebruik IS NOT NULL UNION SELECT tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat right outer join (SELECT apparaatID, MIN(ingebruik),werknemerID FROM tblhistoriek GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id right outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID right outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id right outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id right outer JOIN tbltype ON tblmodel.typeID = tbltype.id right outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE tblapparaat.id IS NOT NULL AND tblapparaat.uitgebruik IS NOT NULL;"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ApparatenGridView.DataSource = dt;
                                ApparatenGridView.DataBind();
                            }
                        }
                    }
                }

                if (WerknemersGridView.Visible == true)
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT COALESCE(tblwerknemer.id,0) as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 'onbekend') as 'locatie',COALESCE(tblwerknemer.status, 0) as 'status', COALESCE(tblwerknemer.commentaar, 0) as 'commentaar' FROM tblwerknemer left outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE tblwerknemer.id IS NOT NULL AND status LIKE 0 UNION SELECT  COALESCE(tblwerknemer.id, 0) as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 'onbekend') as 'locatie', COALESCE(tblwerknemer.status, 0) as 'status', COALESCE(tblwerknemer.commentaar, 0) as 'commentaar' FROM tblwerknemer right outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE tblwerknemer.id IS NOT NULL AND status LIKE 0;"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                WerknemersGridView.DataSource = dt;
                                WerknemersGridView.DataBind();
                            }
                        }

                    }

                }
            }
        } //Voeg een apparaat toe

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            if (ApparatenGridView.Visible == true)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ExportApparaten.xls");
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        Table table = new Table();
                        table.GridLines = ApparatenGridView.GridLines;
                        /*if (ApparatenGridView.HeaderRow != null)
                        {
                            string lastHeaderText = ApparatenGridView.HeaderRow.Cells[ApparatenGridView.Columns.Count - 1].Text;
                            if (lastHeaderText == "Select")
                            {
                                ApparatenGridView.HeaderRow.Cells[ApparatenGridView.Columns.Count - 1].Text = "";
                                ApparatenGridView.HeaderRow.Cells[ApparatenGridView.Columns.Count - 2].Text = "";
                                ApparatenGridView.HeaderRow.Cells[ApparatenGridView.Columns.Count - 3].Text = "";
                            }
                            PrepareControlForExport(ApparatenGridView.HeaderRow);
                            table.Rows.Add(ApparatenGridView.HeaderRow);
                        }*/
                        foreach (GridViewRow row in ApparatenGridView.Rows)
                        {
                            CheckBox cb = (CheckBox)row.FindControl("CBApparaten");
                            if (cb != null && cb.Checked)
                            {
                                PrepareControlForExport(row);
                                table.Rows.Add(row);
                            }
                        }
                        table.RenderControl(htw);
                        HttpContext.Current.Response.Write(sw.ToString());
                        HttpContext.Current.Response.End();
                    }
                }
            }
            if (WerknemersGridView.Visible == true)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ExportWerknemers.xls");
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        Table table = new Table();
                        table.GridLines = WerknemersGridView.GridLines;
                        foreach (GridViewRow row in WerknemersGridView.Rows)
                        {
                            CheckBox cb = (CheckBox)row.FindControl("CBWerknemers");
                            if (cb != null && cb.Checked)
                            {
                                PrepareControlForExport(row);
                                table.Rows.Add(row);
                            }
                        }
                        table.RenderControl(htw);
                        HttpContext.Current.Response.Write(sw.ToString());
                        HttpContext.Current.Response.End();
                    }
                }
            }
            if (LeveranciersGridView.Visible == true)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ExportLeveranciers.xls");
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        Table table = new Table();
                        table.GridLines = LeveranciersGridView.GridLines;
                        foreach (GridViewRow row in LeveranciersGridView.Rows)
                        {
                            CheckBox cb = (CheckBox)row.FindControl("CBLeveranciers");
                            if (cb != null && cb.Checked)
                            {
                                PrepareControlForExport(row);
                                table.Rows.Add(row);
                            }
                        }
                        table.RenderControl(htw);
                        HttpContext.Current.Response.Write(sw.ToString());
                        HttpContext.Current.Response.End();
                    }
                }
            }
        } //export data to excel

        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is HiddenField)
                {
                    control.Controls.Remove(current);
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }
                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        } //maak de data gereed voor export

        /*--------------------------------------------------------------------------------------------------*/
        //Aparaten CODE
        /*--------------------------------------------------------------------------------------------------*/

        private void BindGridApparaten(string sortExpression = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat left outer join (select apparaatID,ingebruik,werknemerID FROM tblhistoriek join( SELECT apparaatID, MAX(ingebruik) ingebruik FROM tblhistoriek WHERE (ingebruik < now()) Group by apparaatID) as b using(ingebruik,apparaatID) GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id left outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID left outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id left outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id left outer JOIN tbltype ON tblmodel.typeID = tbltype.id left outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE tblapparaat.id IS NOT NULL AND tblapparaat.uitgebruik IS NULL"))
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
        } //Bind de gv aparaten

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
        } //Delete row functie > verwijderd ook alle historiek van het desbetreffende app

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
        } //Geen tijd om het te testen zonder dit. Wss onnodig

        protected void ApparatenGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ApparatenGridView.EditIndex = e.NewEditIndex;
            this.BindGridApparaten();
        } //Edit index functie

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
            string serienummer = (row.Cells[8].Controls[0] as TextBox).Text;
            //LEVERANCIER EDIT
            DropDownList LeverancierDL = ((DropDownList)row.FindControl("DDLLeverancierEdit"));
            string leverancier = LeverancierDL.SelectedItem.ToString();
            //SOL EDIT

            TextBox SOLTXT = ((TextBox)row.FindControl("txtSOLEdit"));
            string sol = SOLTXT.Text.ToString();
            //FACTUURNUMMER EDIT
            string factuurnummer = (row.Cells[10].Controls[0] as TextBox).Text;
            //Stock EDIT
            CheckBox stockCB = ((CheckBox)row.FindControl("cbStockEdit"));
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

                // werknemer ophalen
                cmd = new MySqlCommand("select tblwerknemer.id from tblwerknemer where tblwerknemer.naam like @naam;");
                cmd.Parameters.AddWithValue("@naam", gebruiker);
                cmd.Connection = con;
                werknemerID = Convert.ToInt16(cmd.ExecuteScalar());

                //het apparaat updaten
                cmd = new MySqlCommand("UPDATE tblapparaat SET modelID = @modelID, aankoopdatum = @sol, serienummer = @serienummer, leverancierID = @leverancier, stock = @stock, factuurnummer = @factuurnummer WHERE(id = @ID);");
                cmd.Parameters.AddWithValue("@modelID", modelID);
                cmd.Parameters.AddWithValue("@serienummer", serienummer);
                cmd.Parameters.AddWithValue("@leverancier", leverancierID);
                cmd.Parameters.AddWithValue("@stock", instock);
                cmd.Parameters.AddWithValue("@factuurnummer", factuurnummer);
                cmd.Parameters.AddWithValue("@ID", apparaatID);
                cmd.Parameters.AddWithValue("@gebruiker", gebruiker);
                cmd.Parameters.AddWithValue("@sol", Convert.ToDateTime(sol));
                cmd.Connection = con;
                cmd.ExecuteNonQuery();

                HiddenField hfgebruikerOLD = ((HiddenField)row.FindControl("hfGebruiker"));
                string gebruikerOLD = hfgebruikerOLD.Value.ToString();
                if (stockCB.Checked == false)
                {
                    if (gebruiker != gebruikerOLD)
                    {
                        //oude werknemer ophalen
                        cmd = new MySqlCommand("select tblwerknemer.id from tblwerknemer where tblwerknemer.naam like @naam;");
                        cmd.Parameters.AddWithValue("@naam", gebruikerOLD);
                        cmd.Connection = con;
                        werknemerID = Convert.ToInt16(cmd.ExecuteScalar());

                        // EOL VOOR DE OUDE MAKEN
                        cmd = new MySqlCommand("UPDATE tblhistoriek SET uitgebruik = CURDATE() WHERE werknemerid like @werknemer and apparaatID like @apparaat ORDER BY ingebruik desc LIMIT 1;");
                        cmd.Parameters.AddWithValue("@apparaat", apparaatID);
                        cmd.Parameters.AddWithValue("@werknemer", werknemerID);
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();

                        //werknemer ophalen
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
                else
                {
                    //werknemer ophalen
                    cmd = new MySqlCommand("select tblwerknemer.id from tblwerknemer where tblwerknemer.naam like @naam;");
                    cmd.Parameters.AddWithValue("@naam", gebruikerOLD);
                    cmd.Connection = con;
                    werknemerID = Convert.ToInt16(cmd.ExecuteScalar());

                    //UPDATE HISTORIEK - DE OUDE EEN EOL GEVEN / GEEN NIEUWE;
                    cmd = new MySqlCommand("UPDATE tblhistoriek SET uitgebruik = CURDATE() WHERE werknemerid like @werknemer and apparaatID like @apparaat ORDER BY ingebruik desc LIMIT 1;");
                    cmd.Parameters.AddWithValue("@apparaat", apparaatID);
                    cmd.Parameters.AddWithValue("@werknemer", werknemerID);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            ApparatenGridView.EditIndex = -1;
            this.BindGridApparaten();
        } //update row en refresh

        protected void ApparatenGridView_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.BindGridApparaten(e.SortExpression);
        } //Sorteer knop in header laten sorteren > sortingdirection()

        protected void ApparatenGridView_OnPaging(object sender, GridViewPageEventArgs e)
        {
            ApparatenGridView.PageIndex = e.NewPageIndex;
            this.BindGridApparaten();
        } //Paginering regelen

        protected void ApparatenGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ApparatenGridView.EditIndex = -1;
            this.BindGridApparaten();
        } //Cancel edit regelen

        protected void ApparatenGridView_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CommandHistoriek") //historiek van gebruikers van dat apparaat
            {
                GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int apparaatID = Convert.ToInt32(ApparatenGridView.DataKeys[row.RowIndex].Values[0]);
                string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT tblhistoriek.id as 'id', COALESCE(tblhistoriek.ingebruik, 'onbekend') as 'Ingebruik', COALESCE(tblhistoriek.uitgebruik, 'onbekend') as 'Uitgebruik', COALESCE(tblwerknemer.naam, 'onbekend') as 'Werknemer', COALESCE(tbltype.omschrijving, 'onbekend') as 'Type', COALESCE(tblmerk.omschrijving, 'onbekend') as 'Merk', COALESCE(tblmodel.omschrijving, 'onbekend') as 'Model', COALESCE(tblapparaat.serienummer, 'onbekend') as 'Serienummer' FROM tblhistoriek left outer JOIN tblwerknemer ON tblwerknemer.id = tblhistoriek.werknemerID left outer JOIN tblapparaat ON tblapparaat.id = tblhistoriek.apparaatID left outer JOIN tblmodel ON tblmodel.id = tblapparaat.modelID left outer JOIN tbltype ON tbltype.id = tblmodel.typeID left outer JOIN tblmerk ON tblmerk.id = tblmodel.merkID WHERE tblhistoriek.id IS NOT NULL AND tblapparaat.id like @apparaatid order by serienummer;"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@apparaatid", apparaatID);
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
            if (e.CommandName == "Meer_Informatie")
            {
                GridViewRow row = ApparatenGridView.Rows[Convert.ToInt32(e.CommandArgument)];
                int id = Convert.ToInt32(ApparatenGridView.DataKeys[row.RowIndex].Values[0]);
                string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT tblapparaat.commentaar as 'commentaar', tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat left outer join (SELECT apparaatID, MIN(ingebruik),werknemerID FROM tblhistoriek GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id left outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID left outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id left outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id left outer JOIN tbltype ON tblmodel.typeID = tbltype.id left outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE tblapparaat.id LIKE @apparaatid UNION SELECT tblapparaat.commentaar as 'commentaar', tblapparaat.id as id, COALESCE(tblapparaat.serienummer,'onbekend') as 'serie nummer', COALESCE(tblapparaat.stock,0) as 'stock', COALESCE(tbltype.omschrijving,'onbekend') as 'type', COALESCE(tblmodel.omschrijving,'onbekend') as 'model', COALESCE(tblmerk.omschrijving,'onbekend') as 'merk', tblapparaat.aankoopdatum as SOL, tblapparaat.uitgebruik as 'EOL', tblapparaat.factuurnummer as 'factuurnummer', tblLeveranciers.naam as 'Leverancier', tblwerknemer.naam as 'gebruiker' FROM tblapparaat right outer join (SELECT apparaatID, MIN(ingebruik),werknemerID FROM tblhistoriek GROUP BY apparaatID)tblhistoriekgrouped on tblhistoriekgrouped.apparaatid = tblapparaat.id right outer join tblwerknemer ON tblwerknemer.id = tblhistoriekgrouped.werknemerID right outer join tblleveranciers ON tblapparaat.leverancierID = tblleveranciers.id right outer JOIN tblmodel ON tblapparaat.modelID = tblmodel.id right outer JOIN tbltype ON tblmodel.typeID = tbltype.id right outer JOIN tblmerk ON tblmodel.merkID = tblmerk.id WHERE tblapparaat.id LIKE @apparaatid ;"))
                    {
                        DataTable dt = new DataTable();
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@apparaatid", id);
                        con.Open();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            txtcommentaarApparaatInfo.Text = dt.Rows[0]["commentaar"].ToString();
                            lblidapparaatInfo.Text = id.ToString();
                            lblserienummerApparaatInfo.Text = dt.Rows[0]["serie nummer"].ToString();
                            lbltypeApparaatInfo.Text = dt.Rows[0]["type"].ToString();
                            lblmodelApparaatInfo.Text = dt.Rows[0]["model"].ToString();
                            lblmerkApparaatInfo.Text = dt.Rows[0]["merk"].ToString();
                            lblsolApparaatInfo.Text = dt.Rows[0]["SOL"].ToString();
                            lbleolApparaatInfo.Text = dt.Rows[0]["EOL"].ToString();
                            lblfactuurnmbrApparaatInfo.Text = dt.Rows[0]["factuurnummer"].ToString();
                            lblleverancierApparaatInfo.Text = dt.Rows[0]["Leverancier"].ToString();
                            lblgebruikerApparaatInfo.Text = dt.Rows[0]["gebruiker"].ToString();
                        }
                        con.Close();
                        // NIEUWE MAKEN EN OPHALEN OF ALLE KOLOMMEN AL INLADEN EN ONZICHTBARE ZOALS COMMENTAAR OOK OPHALEN.
                    }
                }
                mpInfoApparaten.Show();
            }
        } //Command - Historiek en Meer info regelen en binden

        protected void modalpopup_close_submit(object sender, EventArgs e)
        {
            txtcommentaarApparaatInfo.ReadOnly = true;

            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("UPDATE tblapparaat set commentaar = @commentaar WHERE id = @id"))
                {

                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@id", lblidapparaatInfo.Text);
                    cmd.Parameters.AddWithValue("@commentaar", txtcommentaarApparaatInfo.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            mpInfoApparaten.Hide();
        } //Meer info popup closen en de commentaar opslagen

        protected void Insert_ApparaatCommentaar(object sender, EventArgs e)
        {
            txtcommentaarApparaatInfo.ReadOnly = false;
            mpInfoApparaten.Show();
        } //Commentaar op een apparaatn toestaan invoegen

        /*--------------------------------------------------------------------------------------------------*/
        //WERKNEMER CODE
        /*--------------------------------------------------------------------------------------------------*/
        protected void WerknemersGridView_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CommandHistoriek") //historiek van apparaten van deze gebruiker
            {
                GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int werknemerID = Convert.ToInt32(WerknemersGridView.DataKeys[row.RowIndex].Values[0]);
                string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT tblhistoriek.id as 'id', COALESCE(tblhistoriek.ingebruik, 'onbekend') as 'Ingebruik', COALESCE(tblhistoriek.uitgebruik, 'onbekend') as 'Uitgebruik', COALESCE(tblwerknemer.naam, 'onbekend') as 'Werknemer', COALESCE(tbltype.omschrijving, 'onbekend') as 'Type', COALESCE(tblmerk.omschrijving, 'onbekend') as 'Merk', COALESCE(tblmodel.omschrijving, 'onbekend') as 'Model', COALESCE(tblapparaat.serienummer, 'onbekend') as 'Serienummer' FROM tblhistoriek left outer JOIN tblwerknemer ON tblwerknemer.id = tblhistoriek.werknemerID left outer JOIN tblapparaat ON tblapparaat.id = tblhistoriek.apparaatID left outer JOIN tblmodel ON tblmodel.id = tblapparaat.modelID left outer JOIN tbltype ON tbltype.id = tblmodel.typeID left outer JOIN tblmerk ON tblmerk.id = tblmodel.merkID WHERE tblhistoriek.id IS NOT NULL AND tblwerknemer.id like @werknemerID order by serienummer;"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@werknemerID", werknemerID);
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
                WerknemersGridView.Visible = false;
                HistoriekGridView.Visible = true;
            }
        } //Command - Historiek binden

        protected void BindGridWerknemer(string sortExpression = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT COALESCE(tblwerknemer.id,0) as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 'onbekend') as 'locatie',COALESCE(tblwerknemer.status, 0) as 'status', COALESCE(tblwerknemer.commentaar, 0) as 'commentaar' FROM tblwerknemer left outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE tblwerknemer.id IS NOT NULL AND status LIKE 1 UNION SELECT  COALESCE(tblwerknemer.id, 0) as id, COALESCE(tblwerknemer.naam, 'onbekend') as 'naam', COALESCE(tbllocatie.omschrijving, 'onbekend') as 'locatie', COALESCE(tblwerknemer.status, 0) as 'status', COALESCE(tblwerknemer.commentaar, 0) as 'commentaar' FROM tblwerknemer right outer JOIN tbllocatie ON tblwerknemer.locatieID = tbllocatie.id WHERE tblwerknemer.id IS NOT NULL AND status LIKE 1;"))
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
        }  //Bind de gv Werknemer

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
        } //Delete row functie -> Gaat niet als er nog historiek is

        protected void WerknemersGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            WerknemersGridView.EditIndex = e.NewEditIndex;
            this.BindGridWerknemer();
        } //Edit index functie

        protected void WerknemersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = WerknemersGridView.Rows[e.RowIndex];
            int idWerknemer = Convert.ToInt32(WerknemersGridView.DataKeys[e.RowIndex].Values[0]);
            string naam = (row.Cells[4].Controls[0] as TextBox).Text;
            string commentaar = (row.Cells[7].Controls[0] as TextBox).Text;

            DropDownList locatieDDL = ((DropDownList)row.FindControl("DDLLocatieEdit"));
            string locatie = locatieDDL.SelectedItem.ToString();

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
                cmd = new MySqlCommand("UPDATE tblwerknemer SET locatieID = @locatieID, naam = @naam, commentaar = @commentaar WHERE(id = @ID);");
                cmd.Parameters.AddWithValue("@locatieID", locatieID);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@naam", naam);
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
            this.BindGridWerknemer();
        }

        protected void WerknemersGridView_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.BindGridWerknemer(e.SortExpression);
        }

        protected void btnFilterApparaten_Click(object sender, EventArgs e)
        {
            Filter_Apparaat();
        }

        /*--------------------------------------------------------------------------------------------------*/
        //Historiek CODE
        /*--------------------------------------------------------------------------------------------------*/

        protected void BindGridHistoriek(string sortExpression = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblhistoriek.id as 'id', COALESCE(tblhistoriek.ingebruik, 'onbekend') as 'Ingebruik', COALESCE(tblhistoriek.uitgebruik,'onbekend') as 'Uitgebruik', COALESCE(tblwerknemer.naam, 'onbekend') as 'Werknemer', COALESCE(tbltype.omschrijving, 'onbekend') as 'Type', COALESCE(tblmerk.omschrijving, 'onbekend') as 'Merk', COALESCE(tblmodel.omschrijving, 'onbekend') as 'Model', COALESCE(tblapparaat.serienummer, 'onbekend') as 'Serienummer' FROM tblhistoriek left outer JOIN tblwerknemer ON tblwerknemer.id = tblhistoriek.werknemerID left outer JOIN tblapparaat ON tblapparaat.id = tblhistoriek.apparaatID left outer JOIN tblmodel ON tblmodel.id = tblapparaat.modelID left outer JOIN tbltype ON tbltype.id = tblmodel.typeID left outer JOIN tblmerk ON tblmerk.id = tblmodel.merkID WHERE tblhistoriek.id IS NOT NULL order by serienummer;"))
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
        } //Bind de gv Historiek

        protected void HistoriekGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            HistoriekGridView.EditIndex = e.NewEditIndex;
            this.BindGridHistoriek();
        } //Edit index functie

        protected void HistoriekGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            HistoriekGridView.EditIndex = -1;
            this.BindGridHistoriek();
        }

        protected void HistoriekGridView_OnPaging(object sender, GridViewPageEventArgs e)
        {
            HistoriekGridView.PageIndex = e.NewPageIndex;
            this.BindGridHistoriek();
        }

        protected void HistoriekGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = HistoriekGridView.Rows[e.RowIndex];
            DropDownList werknemerEdit = ((DropDownList)row.FindControl("DDLWerknemer"));
            string werknemer = werknemerEdit.SelectedItem.ToString();
            TextBox ingebruikEdit = ((TextBox)row.FindControl("txtIngebruik"));
            string ingebruik = ingebruikEdit.Text;
            TextBox uitgebruikEdit = ((TextBox)row.FindControl("txtUitgebruik"));
            DateTime? uitgebruik = null;
            if (uitgebruikEdit.Text != "onbekend")
            {
                uitgebruik = Convert.ToDateTime(uitgebruikEdit.Text);
            }
            int id = Convert.ToInt32(HistoriekGridView.DataKeys[e.RowIndex].Values[0]);
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                //WerknemerID ophalen
                MySqlCommand cmd = new MySqlCommand("select tblwerknemer.id from tblwerknemer where tblwerknemer.naam like @werknemer;");
                cmd.Parameters.AddWithValue("@werknemer", werknemer);
                cmd.Connection = con;
                con.Open();
                int werknemerID = Convert.ToInt16(cmd.ExecuteScalar());

                cmd = new MySqlCommand("UPDATE tblhistoriek SET werknemerID = @werknemerID, ingebruik = @ingebruik, uitgebruik = @uitgebruik WHERE(id = @ID);");
                {
                    cmd.Parameters.AddWithValue("@werknemerID", werknemerID);
                    cmd.Parameters.AddWithValue("@ingebruik", Convert.ToDateTime(ingebruik));
                    cmd.Parameters.AddWithValue("@uitgebruik", uitgebruik);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                HistoriekGridView.EditIndex = -1;
                this.BindGridHistoriek();
            }
        }

        /*--------------------------------------------------------------------------------------------------*/
        //Leveranciers CODE
        /*--------------------------------------------------------------------------------------------------*/

        protected void BindGridLeveranciers(string sortExpression = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT tblleveranciers.id as 'id', COALESCE(tblleveranciers.naam, 'onbekend') as 'naam', COALESCE(tblleveranciers.contactpersoon, 'onbekend') as 'contactpersoon', COALESCE(tblleveranciers.email, 'onbekend') as 'email', COALESCE(tblleveranciers.telefoonnummer, 'onbekend') as 'telefoonnummer' FROM tblleveranciers WHERE tblleveranciers.id IS NOT NULL order by naam;"))
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
                                LeveranciersGridView.DataSource = dv;
                            }
                            else
                            {
                                LeveranciersGridView.DataSource = dt;
                            }
                            LeveranciersGridView.DataBind();
                        }
                    }
                }
            }
        } //Bind de gv Leverancier

        protected void LeveranciersGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            LeveranciersGridView.EditIndex = e.NewEditIndex;
            this.BindGridLeveranciers();
        }

        protected void LeveranciersGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            LeveranciersGridView.EditIndex = -1;
            this.BindGridLeveranciers();
        }

        protected void LeveranciersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = LeveranciersGridView.Rows[e.RowIndex];
            int idLeverancier = Convert.ToInt32(LeveranciersGridView.DataKeys[e.RowIndex].Values[0]);

            //Naam Edit
            TextBox naamEdit = ((TextBox)row.FindControl("txtNaamLeveranciersEdit"));
            string naam = naamEdit.Text;
            //Contactpersoon Edit
            TextBox contactEdit = ((TextBox)row.FindControl("txtContactpersoonLeveranciersEdit"));
            string contactpersoon = contactEdit.Text;
            //email Edit
            TextBox emailEdit = ((TextBox)row.FindControl("txtEmailLeveranciersEdit"));
            string email = emailEdit.Text;
            //Telefoonnummer Edit
            TextBox teleEdit = ((TextBox)row.FindControl("txtTelefoonnummerLeveranciersEdit"));
            string telefoonnummer = teleEdit.Text;

            int id = Convert.ToInt32(LeveranciersGridView.DataKeys[e.RowIndex].Values[0]);
            string constr = ConfigurationManager.ConnectionStrings["dbinventarisConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                //Leverancier updaten
                MySqlCommand cmd = new MySqlCommand("UPDATE tblleveranciers SET naam = @naam, contactpersoon = @contactpersoon, email = @email, telefoonnummer = @telefoonnummer WHERE(id = @ID);");
                cmd.Parameters.AddWithValue("@naam", naam);
                cmd.Parameters.AddWithValue("@contactpersoon", contactpersoon);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@telefoonnummer", telefoonnummer);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            LeveranciersGridView.EditIndex = -1;
            this.BindGridLeveranciers();
        }

        protected void LeveranciersGridView_OnPaging(object sender, GridViewPageEventArgs e)
        {
            LeveranciersGridView.PageIndex = e.NewPageIndex;
            this.BindGridLeveranciers();
        }
    }
}