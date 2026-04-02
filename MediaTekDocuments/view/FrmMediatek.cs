using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        // Actions Livres/Dvd/Revues
        private bool enCoursAjoutLivre = false;
        private bool enCoursEditionLivre = false;
        private bool enCoursAjoutDvd = false;
        private bool enCoursEditionDvd = false;
        private bool enCoursAjoutRevue = false;
        private bool enCoursEditionRevue = false;

        private Button btnLivresAjouter;
        private Button btnLivresModifier;
        private Button btnLivresSupprimer;

        private Button btnDvdAjouter;
        private Button btnDvdModifier;
        private Button btnDvdSupprimer;

        private Button btnRevuesAjouter;
        private Button btnRevuesModifier;
        private Button btnRevuesSupprimer;

        private DataGridView dgvLivresExemplairesListe;
        private DataGridView dgvDvdExemplairesListe;
        private ComboBox cbxLivresExemplaireEtat;
        private ComboBox cbxDvdExemplaireEtat;
        private Button btnLivresExemplaireChangerEtat;
        private Button btnLivresExemplaireSupprimer;
        private Button btnDvdExemplaireChangerEtat;
        private Button btnDvdExemplaireSupprimer;
        private Button btnRevuesModifier;
        private Button btnRevuesSupprimer;

        private DataGridView dgvLivresExemplairesListe;
        private DataGridView dgvDvdExemplairesListe;
        private ComboBox cbxLivresExemplaireEtat;
        private ComboBox cbxDvdExemplaireEtat;
        private Button btnLivresExemplaireChangerEtat;
        private Button btnLivresExemplaireSupprimer;
        private Button btnDvdExemplaireChangerEtat;
        private Button btnDvdExemplaireSupprimer;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            InitialiserCrudDocumentButtons();
            AfficherEtatsExemplaires();
            AfficherAlerteAbonnementsExpirant();
        }

        private void AfficherAlerteAbonnementsExpirant()
        {
            var abonnements = controller.GetAbonnementsRevueFinissantDans30Jours();
            if (abonnements != null && abonnements.Count > 0)
            {
                FrmAlerteAbonnements alerteForm = new FrmAlerteAbonnements(abonnements);
                alerteForm.ShowDialog();
            }
        }

        private void InitialiserCrudDocumentButtons()
        {
            // Livres
            btnLivresAjouter = new Button { Text = "Ajouter", Width = 80, Height = 30, Left = 630, Top = 20 };
            btnLivresModifier = new Button { Text = "Modifier", Width = 80, Height = 30, Left = 720, Top = 20 };
            btnLivresSupprimer = new Button { Text = "Supprimer", Width = 80, Height = 30, Left = 810, Top = 20 };
            btnLivresAjouter.Click += BtnLivresAjouter_Click;
            btnLivresModifier.Click += BtnLivresModifier_Click;
            btnLivresSupprimer.Click += BtnLivresSupprimer_Click;
            grpLivresRecherche.Controls.Add(btnLivresAjouter);
            grpLivresRecherche.Controls.Add(btnLivresModifier);
            grpLivresRecherche.Controls.Add(btnLivresSupprimer);

            // Dvd
            btnDvdAjouter = new Button { Text = "Ajouter", Width = 80, Height = 30, Left = 630, Top = 20 };
            btnDvdModifier = new Button { Text = "Modifier", Width = 80, Height = 30, Left = 720, Top = 20 };
            btnDvdSupprimer = new Button { Text = "Supprimer", Width = 80, Height = 30, Left = 810, Top = 20 };
            btnDvdAjouter.Click += BtnDvdAjouter_Click;
            btnDvdModifier.Click += BtnDvdModifier_Click;
            btnDvdSupprimer.Click += BtnDvdSupprimer_Click;
            grpDvdRecherche.Controls.Add(btnDvdAjouter);
            grpDvdRecherche.Controls.Add(btnDvdModifier);
            grpDvdRecherche.Controls.Add(btnDvdSupprimer);

            // Revues
            btnRevuesAjouter = new Button { Text = "Ajouter", Width = 80, Height = 30, Left = 630, Top = 20 };
            btnRevuesModifier = new Button { Text = "Modifier", Width = 80, Height = 30, Left = 720, Top = 20 };
            btnRevuesSupprimer = new Button { Text = "Supprimer", Width = 80, Height = 30, Left = 810, Top = 20 };
            btnRevuesAjouter.Click += BtnRevuesAjouter_Click;
            btnRevuesModifier.Click += BtnRevuesModifier_Click;
            btnRevuesSupprimer.Click += BtnRevuesSupprimer_Click;
            grpRevuesRecherche.Controls.Add(btnRevuesAjouter);
            grpRevuesRecherche.Controls.Add(btnRevuesModifier);
            grpRevuesRecherche.Controls.Add(btnRevuesSupprimer);

            var btnGestionCommandes = new Button { Text = "Gestion Commandes", Width = 150, Height = 30, Left = 10, Top = 260 };
            btnGestionCommandes.Click += BtnGestionCommandes_Click;
            this.Controls.Add(btnGestionCommandes);

            // Exemplaires Livres/Dvd
            dgvLivresExemplairesListe = new DataGridView { Left = 10, Top = 350, Width = 850, Height = 120, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows = false, AllowUserToDeleteRows = false };
            dgvLivresExemplairesListe.ColumnHeaderMouseClick += DgvLivresExemplairesListe_ColumnHeaderMouseClick;
            dgvLivresExemplairesListe.SelectionChanged += DgvLivresExemplairesListe_SelectionChanged;
            this.tabLivres.Controls.Add(dgvLivresExemplairesListe);

            dgvDvdExemplairesListe = new DataGridView { Left = 10, Top = 350, Width = 850, Height = 120, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows = false, AllowUserToDeleteRows = false };
            dgvDvdExemplairesListe.ColumnHeaderMouseClick += DgvDvdExemplairesListe_ColumnHeaderMouseClick;
            dgvDvdExemplairesListe.SelectionChanged += DgvDvdExemplairesListe_SelectionChanged;
            this.tabDvd.Controls.Add(dgvDvdExemplairesListe);

            cbxLivresExemplaireEtat = new ComboBox { Left = 10, Top = 475, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            this.tabLivres.Controls.Add(cbxLivresExemplaireEtat);
            btnLivresExemplaireChangerEtat = new Button { Text = "Changer état", Left = 170, Top = 473, Width = 100 };
            btnLivresExemplaireChangerEtat.Click += BtnLivresExemplaireChangerEtat_Click;
            this.tabLivres.Controls.Add(btnLivresExemplaireChangerEtat);
            btnLivresExemplaireSupprimer = new Button { Text = "Supprimer ex.", Left = 280, Top = 473, Width = 120 };
            btnLivresExemplaireSupprimer.Click += BtnLivresExemplaireSupprimer_Click;
            this.tabLivres.Controls.Add(btnLivresExemplaireSupprimer);

            cbxDvdExemplaireEtat = new ComboBox { Left = 10, Top = 475, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            this.tabDvd.Controls.Add(cbxDvdExemplaireEtat);
            btnDvdExemplaireChangerEtat = new Button { Text = "Changer état", Left = 170, Top = 473, Width = 100 };
            btnDvdExemplaireChangerEtat.Click += BtnDvdExemplaireChangerEtat_Click;
            this.tabDvd.Controls.Add(btnDvdExemplaireChangerEtat);
            btnDvdExemplaireSupprimer = new Button { Text = "Supprimer ex.", Left = 280, Top = 473, Width = 120 };
            btnDvdExemplaireSupprimer.Click += BtnDvdExemplaireSupprimer_Click;
            this.tabDvd.Controls.Add(btnDvdExemplaireSupprimer);
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();
        private readonly BindingSource bdgLivresExemplaires = new BindingSource();
        private List<Exemplaire> lesExemplairesLivres = new List<Exemplaire>();
        private List<Etat> lesEtats = new List<Etat>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
            ConfigurerModeLivres(true, false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                    AfficheLivresExemplaires(livre.Id);
                    AfficherEtatsExemplaires();
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
            if (dgvLivresExemplairesListe != null)
                dgvLivresExemplairesListe.DataSource = null;
        }

        private void AfficherEtatsExemplaires()
        {
            lesEtats = controller.GetAllEtats();
            if (cbxLivresExemplaireEtat != null)
            {
                cbxLivresExemplaireEtat.DataSource = null;
                cbxLivresExemplaireEtat.DataSource = lesEtats;
                cbxLivresExemplaireEtat.DisplayMember = "Libelle";
                cbxLivresExemplaireEtat.ValueMember = "Id";
            }
            if (cbxDvdExemplaireEtat != null)
            {
                cbxDvdExemplaireEtat.DataSource = null;
                cbxDvdExemplaireEtat.DataSource = new List<Etat>(lesEtats);
                cbxDvdExemplaireEtat.DisplayMember = "Libelle";
                cbxDvdExemplaireEtat.ValueMember = "Id";
            }
        }

        private void RemplirLivresExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires == null) exemplaires = new List<Exemplaire>();
            foreach (var ex in exemplaires)
            {
                var etat = lesEtats.Find(e => e.Id == ex.IdEtat);
                ex.EtatLibelle = etat?.Libelle ?? ex.IdEtat;
            }
            lesExemplairesLivres = exemplaires.OrderByDescending(x => x.DateAchat).ToList();
            bdgLivresExemplaires.DataSource = lesExemplairesLivres;
            if (dgvLivresExemplairesListe != null)
            {
                dgvLivresExemplairesListe.DataSource = bdgLivresExemplaires;
                if (dgvLivresExemplairesListe.Columns.Contains("idEtat")) dgvLivresExemplairesListe.Columns["idEtat"].Visible = false;
                if (dgvLivresExemplairesListe.Columns.Contains("id")) dgvLivresExemplairesListe.Columns["id"].Visible = false;
                if (dgvLivresExemplairesListe.Columns.Contains("photo")) dgvLivresExemplairesListe.Columns["photo"].Visible = false;
                if (dgvLivresExemplairesListe.Columns.Contains("etatLibelle")) dgvLivresExemplairesListe.Columns["etatLibelle"].HeaderText = "Etat";
                dgvLivresExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void AfficheLivresExemplaires(string idLivre)
        {
            var exs = controller.GetExemplairesLivre(idLivre);
            RemplirLivresExemplairesListe(exs);
        }

        private void RemplirDvdExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires == null) exemplaires = new List<Exemplaire>();
            foreach (var ex in exemplaires)
            {
                var etat = lesEtats.Find(e => e.Id == ex.IdEtat);
                ex.EtatLibelle = etat?.Libelle ?? ex.IdEtat;
            }
            lesExemplairesDvd = exemplaires.OrderByDescending(x => x.DateAchat).ToList();
            var bdgDvdExemplaires = new BindingSource();
            bdgDvdExemplaires.DataSource = lesExemplairesDvd;
            if (dgvDvdExemplairesListe != null)
            {
                dgvDvdExemplairesListe.DataSource = bdgDvdExemplaires;
                if (dgvDvdExemplairesListe.Columns.Contains("idEtat")) dgvDvdExemplairesListe.Columns["idEtat"].Visible = false;
                if (dgvDvdExemplairesListe.Columns.Contains("id")) dgvDvdExemplairesListe.Columns["id"].Visible = false;
                if (dgvDvdExemplairesListe.Columns.Contains("photo")) dgvDvdExemplairesListe.Columns["photo"].Visible = false;
                if (dgvDvdExemplairesListe.Columns.Contains("etatLibelle")) dgvDvdExemplairesListe.Columns["etatLibelle"].HeaderText = "Etat";
                dgvDvdExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void AfficheDvdExemplaires(string idDvd)
        {
            var exs = controller.GetExemplairesDvd(idDvd);
            RemplirDvdExemplairesListe(exs);
        }

        private void DgvLivresExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string col = dgvLivresExemplairesListe.Columns[e.ColumnIndex].DataPropertyName;
            if (col == "Numero") lesExemplairesLivres = lesExemplairesLivres.OrderByDescending(x => x.Numero).ToList();
            if (col == "DateAchat") lesExemplairesLivres = lesExemplairesLivres.OrderBy(x => x.DateAchat).ToList();
            if (col == "EtatLibelle") lesExemplairesLivres = lesExemplairesLivres.OrderBy(x => x.EtatLibelle).ToList();
            dgvLivresExemplairesListe.DataSource = null;
            dgvLivresExemplairesListe.DataSource = lesExemplairesLivres;
        }

        private void DgvDvdExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string col = dgvDvdExemplairesListe.Columns[e.ColumnIndex].DataPropertyName;
            if (col == "Numero") lesExemplairesDvd = lesExemplairesDvd.OrderByDescending(x => x.Numero).ToList();
            if (col == "DateAchat") lesExemplairesDvd = lesExemplairesDvd.OrderBy(x => x.DateAchat).ToList();
            if (col == "EtatLibelle") lesExemplairesDvd = lesExemplairesDvd.OrderBy(x => x.EtatLibelle).ToList();
            dgvDvdExemplairesListe.DataSource = null;
            dgvDvdExemplairesListe.DataSource = lesExemplairesDvd;
        }

        private void DgvLivresExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresExemplairesListe.CurrentRow != null)
            {
                var ex = (Exemplaire)dgvLivresExemplairesListe.CurrentRow.DataBoundItem;
                cbxLivresExemplaireEtat.SelectedValue = ex.IdEtat;
            }
        }

        private void DgvDvdExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdExemplairesListe.CurrentRow != null)
            {
                var ex = (Exemplaire)dgvDvdExemplairesListe.CurrentRow.DataBoundItem;
                cbxDvdExemplaireEtat.SelectedValue = ex.IdEtat;
            }
        }

        private void BtnLivresExemplaireChangerEtat_Click(object sender, EventArgs e)
        {
            if (dgvLivresExemplairesListe.CurrentRow == null) return;
            var ex = (Exemplaire)dgvLivresExemplairesListe.CurrentRow.DataBoundItem;
            string newEtat = (cbxLivresExemplaireEtat.SelectedItem as Etat)?.Id;
            if (string.IsNullOrEmpty(newEtat)) return;
            ex.IdEtat = newEtat;
            if (controller.ModifierExemplaire(ex))
            {
                MessageBox.Show("Etat exemplaire modifié", "Succès");
                AfficheLivresExemplaires(ex.Id);
            }
            else
            {
                MessageBox.Show("Erreur modification état", "Erreur");
            }
        }

        private void BtnLivresExemplaireSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvLivresExemplairesListe.CurrentRow == null) return;
            var ex = (Exemplaire)dgvLivresExemplairesListe.CurrentRow.DataBoundItem;
            if (MessageBox.Show("Supprimer cet exemplaire ?", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            if (controller.SupprimerExemplaire(ex.Id, ex.Numero))
            {
                MessageBox.Show("Exemplaire supprimé", "Succès");
                AfficheLivresExemplaires(ex.Id);
            }
            else
            {
                MessageBox.Show("Suppression impossible", "Erreur");
            }
        }

        private void BtnDvdExemplaireChangerEtat_Click(object sender, EventArgs e)
        {
            if (dgvDvdExemplairesListe.CurrentRow == null) return;
            var ex = (Exemplaire)dgvDvdExemplairesListe.CurrentRow.DataBoundItem;
            string newEtat = (cbxDvdExemplaireEtat.SelectedItem as Etat)?.Id;
            if (string.IsNullOrEmpty(newEtat)) return;
            ex.IdEtat = newEtat;
            if (controller.ModifierExemplaire(ex))
            {
                MessageBox.Show("Etat exemplaire modifié", "Succès");
                AfficheDvdExemplaires(ex.Id);
            }
            else
            {
                MessageBox.Show("Erreur modification état", "Erreur");
            }
        }

        private void BtnDvdExemplaireSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvDvdExemplairesListe.CurrentRow == null) return;
            var ex = (Exemplaire)dgvDvdExemplairesListe.CurrentRow.DataBoundItem;
            if (MessageBox.Show("Supprimer cet exemplaire ?", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            if (controller.SupprimerExemplaire(ex.Id, ex.Numero))
            {
                MessageBox.Show("Exemplaire supprimé", "Succès");
                AfficheDvdExemplaires(ex.Id);
            }
            else
            {
                MessageBox.Show("Suppression impossible", "Erreur");
            }
        }
        {
            txbLivresNumero.ReadOnly = !idEditable;
            txbLivresTitre.ReadOnly = lectureSeule;
            txbLivresAuteur.ReadOnly = lectureSeule;
            txbLivresCollection.ReadOnly = lectureSeule;
            txbLivresIsbn.ReadOnly = lectureSeule;
            txbLivresImage.ReadOnly = lectureSeule;
            txbLivresGenre.ReadOnly = lectureSeule;
            txbLivresPublic.ReadOnly = lectureSeule;
            txbLivresRayon.ReadOnly = lectureSeule;
            dgvLivresListe.Enabled = lectureSeule;
            btnLivresModifier.Enabled = lectureSeule;
            btnLivresSupprimer.Enabled = lectureSeule;
            btnLivresAjouter.Text = lectureSeule ? "Ajouter" : "Valider";
            if (lectureSeule)
            {
                enCoursAjoutLivre = false;
                enCoursEditionLivre = false;
            }
        }

        private bool ValiderFormulaireLivres(out Livre livre)
        {
            livre = null;
            if (string.IsNullOrWhiteSpace(txbLivresNumero.Text))
            {
                MessageBox.Show("Numéro de document obligatoire", "Erreur");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txbLivresTitre.Text) || string.IsNullOrWhiteSpace(txbLivresAuteur.Text)
                || string.IsNullOrWhiteSpace(txbLivresCollection.Text) || string.IsNullOrWhiteSpace(txbLivresIsbn.Text))
            {
                MessageBox.Show("Tous les champs livre (titre, auteur, collection, ISBN) sont obligatoires", "Erreur");
                return false;
            }
            var genre = cbxLivresGenres.SelectedItem as Categorie;
            var lePublic = cbxLivresPublics.SelectedItem as Categorie;
            var rayon = cbxLivresRayons.SelectedItem as Categorie;
            if (genre == null || lePublic == null || rayon == null)
            {
                MessageBox.Show("Sélectionnez un genre, un public et un rayon", "Erreur");
                return false;
            }
            livre = new Livre(txbLivresNumero.Text.Trim(), txbLivresTitre.Text.Trim(), txbLivresImage.Text.Trim(), txbLivresIsbn.Text.Trim(), txbLivresAuteur.Text.Trim(), txbLivresCollection.Text.Trim(),
                genre.Id, genre.Libelle, lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle);
            return true;
        }

        private void BtnLivresAjouter_Click(object sender, EventArgs e)
        {
            if (!enCoursAjoutLivre)
            {
                enCoursAjoutLivre = true;
                enCoursEditionLivre = false;
                VideLivresInfos();
                ConfigurerModeLivres(false, true);
                btnLivresModifier.Enabled = false;
                btnLivresSupprimer.Enabled = false;
                return;
            }

            if (!ValiderFormulaireLivres(out Livre nouveauLivre))
            {
                return;
            }

            if (controller.CreerLivre(nouveauLivre))
            {
                MessageBox.Show("Livre créé", "Succès");
                lesLivres = controller.GetAllLivres();
                RemplirLivresListeComplete();
                ConfigurerModeLivres(true, false);
            }
            else
            {
                MessageBox.Show("Impossible de créer le livre", "Erreur");
            }
        }

        private void BtnLivresModifier_Click(object sender, EventArgs e)
        {
            if (!enCoursEditionLivre)
            {
                if (string.IsNullOrWhiteSpace(txbLivresNumero.Text))
                {
                    MessageBox.Show("Sélectionnez un livre à modifier", "Information");
                    return;
                }
                enCoursEditionLivre = true;
                enCoursAjoutLivre = false;
                ConfigurerModeLivres(false, false);
                btnLivresAjouter.Enabled = false;
                btnLivresSupprimer.Enabled = false;
                return;
            }

            if (!ValiderFormulaireLivres(out Livre livreModifie))
            {
                return;
            }

            if (controller.ModifierLivre(livreModifie))
            {
                MessageBox.Show("Livre modifié", "Succès");
                lesLivres = controller.GetAllLivres();
                RemplirLivresListeComplete();
                ConfigurerModeLivres(true, false);
            }
            else
            {
                MessageBox.Show("Impossible de modifier le livre", "Erreur");
            }
        }

        private void BtnLivresSupprimer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbLivresNumero.Text))
            {
                MessageBox.Show("Sélectionnez un livre à supprimer", "Information");
                return;
            }
            if (MessageBox.Show("Confirmez-vous la suppression du livre ?", "Attention", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            if (controller.SupprimerLivre(txbLivresNumero.Text.Trim()))
            {
                MessageBox.Show("Livre supprimé", "Succès");
                lesLivres = controller.GetAllLivres();
                RemplirLivresListeComplete();
            }
            else
            {
                MessageBox.Show("Suppression impossible : le livre peut avoir des exemplaires ou des commandes", "Erreur");
            }
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
            ConfigurerModeDvd(true, false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                    AfficheDvdExemplaires(dvd.Id);
                    AfficherEtatsExemplaires();
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        private void ConfigurerModeDvd(bool lectureSeule, bool idEditable)
        {
            txbDvdNumero.ReadOnly = !idEditable;
            txbDvdTitre.ReadOnly = lectureSeule;
            txbDvdRealisateur.ReadOnly = lectureSeule;
            txbDvdSynopsis.ReadOnly = lectureSeule;
            txbDvdDuree.ReadOnly = lectureSeule;
            txbDvdImage.ReadOnly = lectureSeule;
            txbDvdGenre.ReadOnly = lectureSeule;
            txbDvdPublic.ReadOnly = lectureSeule;
            txbDvdRayon.ReadOnly = lectureSeule;
            dgvDvdListe.Enabled = lectureSeule;
            btnDvdModifier.Enabled = lectureSeule;
            btnDvdSupprimer.Enabled = lectureSeule;
            btnDvdAjouter.Text = lectureSeule ? "Ajouter" : "Valider";
            if (lectureSeule)
            {
                enCoursAjoutDvd = false;
                enCoursEditionDvd = false;
            }
        }

        private bool ValiderFormulaireDvd(out Dvd dvd)
        {
            dvd = null;
            if (string.IsNullOrWhiteSpace(txbDvdNumero.Text))
            {
                MessageBox.Show("Numéro de document obligatoire", "Erreur");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txbDvdTitre.Text) || string.IsNullOrWhiteSpace(txbDvdRealisateur.Text)
                || string.IsNullOrWhiteSpace(txbDvdSynopsis.Text) || string.IsNullOrWhiteSpace(txbDvdDuree.Text))
            {
                MessageBox.Show("Tous les champs DVD (titre, réalisateur, synopsis, durée) sont obligatoires", "Erreur");
                return false;
            }
            if (!int.TryParse(txbDvdDuree.Text.Trim(), out int duree) || duree <= 0)
            {
                MessageBox.Show("Durée du DVD invalide", "Erreur");
                return false;
            }
            var genre = cbxDvdGenres.SelectedItem as Categorie;
            var lePublic = cbxDvdPublics.SelectedItem as Categorie;
            var rayon = cbxDvdRayons.SelectedItem as Categorie;
            if (genre == null || lePublic == null || rayon == null)
            {
                MessageBox.Show("Sélectionnez un genre, un public et un rayon", "Erreur");
                return false;
            }
            dvd = new Dvd(txbDvdNumero.Text.Trim(), txbDvdTitre.Text.Trim(), txbDvdImage.Text.Trim(), duree,
                txbDvdRealisateur.Text.Trim(), txbDvdSynopsis.Text.Trim(), genre.Id, genre.Libelle,
                lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle);
            return true;
        }

        private void BtnDvdAjouter_Click(object sender, EventArgs e)
        {
            if (!enCoursAjoutDvd)
            {
                enCoursAjoutDvd = true;
                enCoursEditionDvd = false;
                VideDvdInfos();
                ConfigurerModeDvd(false, true);
                btnDvdModifier.Enabled = false;
                btnDvdSupprimer.Enabled = false;
                return;
            }
            if (!ValiderFormulaireDvd(out Dvd nouveauDvd))
            {
                return;
            }
            if (controller.CreerDvd(nouveauDvd))
            {
                MessageBox.Show("DVD créé", "Succès");
                lesDvd = controller.GetAllDvd();
                RemplirDvdListeComplete();
                ConfigurerModeDvd(true, false);
            }
            else
            {
                MessageBox.Show("Impossible de créer le DVD", "Erreur");
            }
        }

        private void BtnDvdModifier_Click(object sender, EventArgs e)
        {
            if (!enCoursEditionDvd)
            {
                if (string.IsNullOrWhiteSpace(txbDvdNumero.Text))
                {
                    MessageBox.Show("Sélectionnez un DVD à modifier", "Information");
                    return;
                }
                enCoursEditionDvd = true;
                enCoursAjoutDvd = false;
                ConfigurerModeDvd(false, false);
                btnDvdAjouter.Enabled = false;
                btnDvdSupprimer.Enabled = false;
                return;
            }
            if (!ValiderFormulaireDvd(out Dvd dvdModifie))
            {
                return;
            }
            if (controller.ModifierDvd(dvdModifie))
            {
                MessageBox.Show("DVD modifié", "Succès");
                lesDvd = controller.GetAllDvd();
                RemplirDvdListeComplete();
                ConfigurerModeDvd(true, false);
            }
            else
            {
                MessageBox.Show("Impossible de modifier le DVD", "Erreur");
            }
        }

        private void BtnDvdSupprimer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbDvdNumero.Text))
            {
                MessageBox.Show("Sélectionnez un DVD à supprimer", "Information");
                return;
            }
            if (MessageBox.Show("Confirmez-vous la suppression du DVD ?", "Attention", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            if (controller.SupprimerDvd(txbDvdNumero.Text.Trim()))
            {
                MessageBox.Show("DVD supprimé", "Succès");
                lesDvd = controller.GetAllDvd();
                RemplirDvdListeComplete();
            }
            else
            {
                MessageBox.Show("Suppression impossible : le DVD peut avoir des exemplaires ou des commandes", "Erreur");
            }
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
            ConfigurerModeRevues(true, false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        private void BtnGestionCommandes_Click(object sender, EventArgs e)
        {
            FrmCommandes formCommandes = new FrmCommandes(controller);
            formCommandes.ShowDialog();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        private void ConfigurerModeRevues(bool lectureSeule, bool idEditable)
        {
            txbRevuesNumero.ReadOnly = !idEditable;
            txbRevuesTitre.ReadOnly = lectureSeule;
            txbRevuesPeriodicite.ReadOnly = lectureSeule;
            txbRevuesDateMiseADispo.ReadOnly = lectureSeule;
            txbRevuesImage.ReadOnly = lectureSeule;
            txbRevuesGenre.ReadOnly = lectureSeule;
            txbRevuesPublic.ReadOnly = lectureSeule;
            txbRevuesRayon.ReadOnly = lectureSeule;
            dgvRevuesListe.Enabled = lectureSeule;
            btnRevuesModifier.Enabled = lectureSeule;
            btnRevuesSupprimer.Enabled = lectureSeule;
            btnRevuesAjouter.Text = lectureSeule ? "Ajouter" : "Valider";
            if (lectureSeule)
            {
                enCoursAjoutRevue = false;
                enCoursEditionRevue = false;
            }
        }

        private bool ValiderFormulaireRevues(out Revue revue)
        {
            revue = null;
            if (string.IsNullOrWhiteSpace(txbRevuesNumero.Text))
            {
                MessageBox.Show("Numéro de document obligatoire", "Erreur");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txbRevuesTitre.Text) || string.IsNullOrWhiteSpace(txbRevuesPeriodicite.Text)
                || string.IsNullOrWhiteSpace(txbRevuesDateMiseADispo.Text))
            {
                MessageBox.Show("Tous les champs revue (titre, périodicité, délai) sont obligatoires", "Erreur");
                return false;
            }
            if (!int.TryParse(txbRevuesDateMiseADispo.Text.Trim(), out int delai) || delai <= 0)
            {
                MessageBox.Show("Délai de mise à dispo invalide", "Erreur");
                return false;
            }
            var genre = cbxRevuesGenres.SelectedItem as Categorie;
            var lePublic = cbxRevuesPublics.SelectedItem as Categorie;
            var rayon = cbxRevuesRayons.SelectedItem as Categorie;
            if (genre == null || lePublic == null || rayon == null)
            {
                MessageBox.Show("Sélectionnez un genre, un public et un rayon", "Erreur");
                return false;
            }
            revue = new Revue(txbRevuesNumero.Text.Trim(), txbRevuesTitre.Text.Trim(), txbRevuesImage.Text.Trim(),
                genre.Id, genre.Libelle, lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle,
                txbRevuesPeriodicite.Text.Trim(), delai);
            return true;
        }

        private void BtnRevuesAjouter_Click(object sender, EventArgs e)
        {
            if (!enCoursAjoutRevue)
            {
                enCoursAjoutRevue = true;
                enCoursEditionRevue = false;
                VideRevuesInfos();
                ConfigurerModeRevues(false, true);
                btnRevuesModifier.Enabled = false;
                btnRevuesSupprimer.Enabled = false;
                return;
            }
            if (!ValiderFormulaireRevues(out Revue nouvelleRevue))
            {
                return;
            }
            if (controller.CreerRevue(nouvelleRevue))
            {
                MessageBox.Show("Revue créée", "Succès");
                lesRevues = controller.GetAllRevues();
                RemplirRevuesListeComplete();
                ConfigurerModeRevues(true, false);
            }
            else
            {
                MessageBox.Show("Impossible de créer la revue", "Erreur");
            }
        }

        private void BtnRevuesModifier_Click(object sender, EventArgs e)
        {
            if (!enCoursEditionRevue)
            {
                if (string.IsNullOrWhiteSpace(txbRevuesNumero.Text))
                {
                    MessageBox.Show("Sélectionnez une revue à modifier", "Information");
                    return;
                }
                enCoursEditionRevue = true;
                enCoursAjoutRevue = false;
                ConfigurerModeRevues(false, false);
                btnRevuesAjouter.Enabled = false;
                btnRevuesSupprimer.Enabled = false;
                return;
            }
            if (!ValiderFormulaireRevues(out Revue revueModifiee))
            {
                return;
            }
            if (controller.ModifierRevue(revueModifiee))
            {
                MessageBox.Show("Revue modifiée", "Succès");
                lesRevues = controller.GetAllRevues();
                RemplirRevuesListeComplete();
                ConfigurerModeRevues(true, false);
            }
            else
            {
                MessageBox.Show("Impossible de modifier la revue", "Erreur");
            }
        }

        private void BtnRevuesSupprimer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbRevuesNumero.Text))
            {
                MessageBox.Show("Sélectionnez une revue à supprimer", "Information");
                return;
            }
            if (controller.GetExemplairesRevue(txbRevuesNumero.Text.Trim()).Count > 0)
            {
                MessageBox.Show("Impossible de supprimer une revue avec des exemplaires existants", "Erreur");
                return;
            }
            if (MessageBox.Show("Confirmez-vous la suppression de la revue ?", "Attention", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            if (controller.SupprimerRevue(txbRevuesNumero.Text.Trim()))
            {
                MessageBox.Show("Revue supprimée", "Succès");
                lesRevues = controller.GetAllRevues();
                RemplirRevuesListeComplete();
            }
            else
            {
                MessageBox.Show("Suppression impossible : la revue peut avoir des commandes", "Erreur");
            }
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion
    }
}
