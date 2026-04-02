using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    public class FrmCommandes : Form
    {
        private readonly FrmMediatekController controller;
        private ComboBox cbxTypeDocument;
        private TextBox txbDocumentNumero;
        private Button btnChercherDocument;
        private DataGridView dgvCommandes;
        private DateTimePicker dtpDateCommande;
        private DateTimePicker dtpDateFin;
        private TextBox txbMontant;
        private TextBox txbQuantite;
        private ComboBox cbxSuivi;
        private Button btnAjouterCommande;
        private Button btnModifierSuivi;
        private Button btnSupprimerCommande;
        private Label lblDocumentInfo;

        private List<CommandeDocument> commandes = new List<CommandeDocument>();

        public FrmCommandes(FrmMediatekController controller)
        {
            this.controller = controller;
            InitializeComponent();
            InitialiserDonnees();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestion des commandes";
            this.Width = 900;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterParent;

            cbxTypeDocument = new ComboBox { Left = 20, Top = 20, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cbxTypeDocument.Items.AddRange(new string[] { "Livre", "Dvd" });
            cbxTypeDocument.SelectedIndex = 0;
            this.Controls.Add(cbxTypeDocument);

            txbDocumentNumero = new TextBox { Left = 150, Top = 20, Width = 120 };
            this.Controls.Add(txbDocumentNumero);

            btnChercherDocument = new Button { Text = "Rechercher", Left = 280, Top = 18, Width = 100 };
            btnChercherDocument.Click += BtnChercherDocument_Click;
            this.Controls.Add(btnChercherDocument);

            lblDocumentInfo = new Label { Left = 20, Top = 55, Width = 860, Height = 24, Text = "Document :" };
            this.Controls.Add(lblDocumentInfo);

            dgvCommandes = new DataGridView { Left = 20, Top = 85, Width = 860, Height = 260, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoGenerateColumns = false, MultiSelect = false };
            dgvCommandes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = "Id", Width = 80 });
            dgvCommandes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date commande", DataPropertyName = "DateCommande", Width = 120 });
            dgvCommandes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date fin abonnement", DataPropertyName = "DateFinAbonnement", Width = 120 });
            dgvCommandes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Montant", DataPropertyName = "Montant", Width = 80 });
            dgvCommandes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Quantité", DataPropertyName = "Quantite", Width = 80 });
            dgvCommandes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Suivi", DataPropertyName = "Suivi", Width = 120 });
            dgvCommandes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Document", DataPropertyName = "IdDocument", Width = 120 });
            dgvCommandes.ColumnHeaderMouseClick += DgvCommandes_ColumnHeaderMouseClick;
            dgvCommandes.SelectionChanged += DgvCommandes_SelectionChanged;
            this.Controls.Add(dgvCommandes);

            GroupBox grpSaisie = new GroupBox { Left = 20, Top = 360, Width = 860, Height = 180, Text = "Nouvelle commande" };
            this.Controls.Add(grpSaisie);

            var labelDate = new Label { Text = "Date commande", Left = 20, Top = 30, Width = 100 };
            grpSaisie.Controls.Add(labelDate);
            dtpDateCommande = new DateTimePicker { Left = 130, Top = 25, Width = 150, Format = DateTimePickerFormat.Short };
            grpSaisie.Controls.Add(dtpDateCommande);

            var labelMontant = new Label { Text = "Montant", Left = 300, Top = 30, Width = 80 };
            grpSaisie.Controls.Add(labelMontant);
            txbMontant = new TextBox { Left = 380, Top = 25, Width = 100 };
            grpSaisie.Controls.Add(txbMontant);

            var labelQuantite = new Label { Text = "Quantité", Left = 500, Top = 30, Width = 80 };
            grpSaisie.Controls.Add(labelQuantite);
            txbQuantite = new TextBox { Left = 580, Top = 25, Width = 60 };
            grpSaisie.Controls.Add(txbQuantite);

            var labelDateFin = new Label { Text = "Date fin abonnement", Left = 660, Top = 30, Width = 110 };
            grpSaisie.Controls.Add(labelDateFin);
            dtpDateFin = new DateTimePicker { Left = 780, Top = 25, Width = 120, Format = DateTimePickerFormat.Short };
            grpSaisie.Controls.Add(dtpDateFin);

            var labelSuivi = new Label { Text = "Suivi", Left = 20, Top = 70, Width = 80 };
            grpSaisie.Controls.Add(labelSuivi);
            cbxSuivi = new ComboBox { Left = 80, Top = 68, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cbxSuivi.Items.AddRange(new string[] { "en cours", "relancée", "livrée", "réglée" });
            cbxSuivi.SelectedIndex = 0;
            grpSaisie.Controls.Add(cbxSuivi);

            btnAjouterCommande = new Button { Text = "Ajouter commande", Left = 20, Top = 70, Width = 150 };
            btnAjouterCommande.Click += BtnAjouterCommande_Click;
            grpSaisie.Controls.Add(btnAjouterCommande);

            btnModifierSuivi = new Button { Text = "Modifier suivi", Left = 180, Top = 70, Width = 150 };
            btnModifierSuivi.Click += BtnModifierSuivi_Click;
            grpSaisie.Controls.Add(btnModifierSuivi);

            btnSupprimerCommande = new Button { Text = "Supprimer commande", Left = 340, Top = 70, Width = 170 };
            btnSupprimerCommande.Click += BtnSupprimerCommande_Click;
            grpSaisie.Controls.Add(btnSupprimerCommande);
        }

        private void InitialiserDonnees()
        {
            // Initialisation simple
        }

        private void BtnChercherDocument_Click(object sender, EventArgs e)
        {
            string type = cbxTypeDocument.SelectedItem.ToString();
            string idDocument = txbDocumentNumero.Text.Trim();
            if (string.IsNullOrEmpty(idDocument))
            {
                MessageBox.Show("Saisissez un numéro de document.", "Information");
                return;
            }
            lblDocumentInfo.Text = $"Document ({type}) recherché : {idDocument}";
            if (type == "Livre")
            {
                commandes = controller.GetCommandesLivre(idDocument);
            }
            else if (type == "Dvd")
            {
                commandes = controller.GetCommandesDvd(idDocument);
            }
            else
            {
                commandes = controller.GetCommandesRevue(idDocument);
            }
            commandes = commandes.OrderByDescending(c => c.DateCommande).ToList();
            dgvCommandes.DataSource = commandes;
        }

        private void BtnAjouterCommande_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txbMontant.Text.Trim(), out decimal montant) || montant < 0)
            {
                MessageBox.Show("Montant invalide", "Erreur");
                return;
            }
            if (!int.TryParse(txbQuantite.Text.Trim(), out int quantite) || quantite <= 0)
            {
                MessageBox.Show("Quantité invalide", "Erreur");
                return;
            }
            string type = cbxTypeDocument.SelectedItem.ToString();
            string idDocument = txbDocumentNumero.Text.Trim();
            if (string.IsNullOrEmpty(idDocument))
            {
                MessageBox.Show("Saisissez un numéro de document.", "Erreur");
                return;
            }

            DateTime? dateFin = null;
            if (type == "Revue")
            {
                dateFin = dtpDateFin.Value.Date;
                if (dateFin <= dtpDateCommande.Value.Date)
                {
                    MessageBox.Show("La date de fin d'abonnement doit être postérieure à la date de commande.", "Erreur");
                    return;
                }
            }

            var commande = new CommandeDocument(null, idDocument, dtpDateCommande.Value.Date, dateFin, montant, quantite, "00001", "en cours", type);
            if (controller.CreerCommandeDocument(commande))
            {
                MessageBox.Show("Commande enregistrée (suivi en cours).", "Succès");
                BtnChercherDocument_Click(null, null);
            }
            else
            {
                MessageBox.Show("Impossible d'enregistrer la commande.", "Erreur");
            }
        }

        private void DgvCommandes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandes.CurrentRow == null) return;
            var commande = dgvCommandes.CurrentRow.DataBoundItem as CommandeDocument;
            if (commande != null)
            {
                cbxSuivi.SelectedItem = commande.Suivi;
                dtpDateCommande.Value = commande.DateCommande;
                dtpDateFin.Value = commande.DateFinAbonnement ?? dtpDateCommande.Value;
                txbMontant.Text = commande.Montant.ToString("F2");
                txbQuantite.Text = commande.Quantite.ToString();
            }
        }

        private void BtnModifierSuivi_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.CurrentRow == null)
            {
                MessageBox.Show("Sélectionnez une commande.", "Information");
                return;
            }
            var commande = dgvCommandes.CurrentRow.DataBoundItem as CommandeDocument;
            if (commande == null) return;
            string nouvelleEtape = cbxSuivi.SelectedItem.ToString();
            // Règles métier : pas de retour arrière après livrée ou réglée
            if ((commande.Suivi == "livrée" || commande.Suivi == "réglée") && (nouvelleEtape == "en cours" || nouvelleEtape == "relancée"))
            {
                MessageBox.Show("Impossible de revenir en arrière après livraison ou règlement.", "Erreur");
                return;
            }
            // ne peut être réglée que si livrée
            if (nouvelleEtape == "réglée" && commande.Suivi != "livrée")
            {
                MessageBox.Show("Une commande ne peut être réglée que si elle est livrée.", "Erreur");
                return;
            }
            commande.Suivi = nouvelleEtape;
            if (controller.ModifierCommandeDocument(commande))
            {
                MessageBox.Show("Étape de suivi mise à jour.", "Succès");
                BtnChercherDocument_Click(null, null);
            }
            else
            {
                MessageBox.Show("Impossible de changer l'étape de suivi.", "Erreur");
            }
        }

        private void BtnSupprimerCommande_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.CurrentRow == null)
            {
                MessageBox.Show("Sélectionnez une commande.", "Information");
                return;
            }
            var commande = dgvCommandes.CurrentRow.DataBoundItem as CommandeDocument;
            if (commande == null) return;
            if (commande.Suivi == "livrée" || commande.Suivi == "réglée")
            {
                MessageBox.Show("Une commande livrée ou réglée ne peut pas être supprimée.", "Erreur");
                return;
            }
            if (commande.TypeDocument == "Revue" && commande.DateFinAbonnement.HasValue)
            {
                List<Exemplaire> exemplaires = controller.GetExemplairesRevue(commande.IdDocument);
                bool aParution = exemplaires.Any(e => controller.ParutionDansAbonnement(commande.DateCommande, commande.DateFinAbonnement.Value, e.DateAchat));
                if (aParution)
                {
                    MessageBox.Show("Impossible de supprimer : des parutions existent dans la période d'abonnement.", "Erreur");
                    return;
                }
            }
            if (MessageBox.Show("Confirmer suppression de la commande ?", "Attention", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            if (controller.SupprimerCommandeDocument(commande.Id))
            {
                MessageBox.Show("Commande supprimée.", "Succès");
                BtnChercherDocument_Click(null, null);
            }
            else
            {
                MessageBox.Show("Suppression impossible.", "Erreur");
            }
        }

        private void DgvCommandes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var colName = dgvCommandes.Columns[e.ColumnIndex].DataPropertyName;
            if (colName == "DateCommande") commandes = commandes.OrderByDescending(c => c.DateCommande).ToList();
            if (colName == "Montant") commandes = commandes.OrderBy(c => c.Montant).ToList();
            if (colName == "Quantite") commandes = commandes.OrderBy(c => c.Quantite).ToList();
            if (colName == "Suivi") commandes = commandes.OrderBy(c => c.Suivi).ToList();
            dgvCommandes.DataSource = null;
            dgvCommandes.DataSource = commandes;
        }
    }
}
