using System;
using System.Collections.Generic;
using System.Linq;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        public List<Exemplaire> GetExemplairesLivre(string idLivre)
        {
            return access.GetExemplairesLivre(idLivre);
        }

        public List<Exemplaire> GetExemplairesDvd(string idDvd)
        {
            return access.GetExemplairesDvd(idDvd);
        }

        public List<Etat> GetAllEtats()
        {
            return access.GetAllEtats();
        }

        public bool ModifierExemplaire(Exemplaire exemplaire)
        {
            return access.ModifierExemplaire(exemplaire);
        }

        public bool SupprimerExemplaire(string idDocument, int numero)
        {
            return access.SupprimerExemplaire(idDocument, numero);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        // Livres
        public bool CreerLivre(Livre livre)
        {
            return access.CreerLivre(livre);
        }

        public bool ModifierLivre(Livre livre)
        {
            return access.ModifierLivre(livre);
        }

        public bool SupprimerLivre(string idLivre)
        {
            return access.SupprimerLivre(idLivre);
        }

        // Dvd
        public bool CreerDvd(Dvd dvd)
        {
            return access.CreerDvd(dvd);
        }

        public bool ModifierDvd(Dvd dvd)
        {
            return access.ModifierDvd(dvd);
        }

        public bool SupprimerDvd(string idDvd)
        {
            return access.SupprimerDvd(idDvd);
        }

        // Revues
        public bool CreerRevue(Revue revue)
        {
            return access.CreerRevue(revue);
        }

        public bool ModifierRevue(Revue revue)
        {
            return access.ModifierRevue(revue);
        }

        public bool SupprimerRevue(string idRevue)
        {
            return access.SupprimerRevue(idRevue);
        }

        // Commandes documents
        public List<CommandeDocument> GetCommandesLivre(string idLivre)
        {
            return access.GetCommandesLivre(idLivre);
        }

        public List<CommandeDocument> GetCommandesDvd(string idDvd)
        {
            return access.GetCommandesDvd(idDvd);
        }

        public List<CommandeDocument> GetCommandesRevue(string idRevue)
        {
            if (string.IsNullOrWhiteSpace(idRevue)) return new List<CommandeDocument>();
            return access.GetCommandesRevue().Where(c => c.IdDocument == idRevue).ToList();
        }

        public bool CreerCommandeDocument(CommandeDocument commande)
        {
            return access.CreerCommandeDocument(commande);
        }

        public bool ModifierCommandeDocument(CommandeDocument commande)
        {
            return access.ModifierCommandeDocument(commande);
        }

        public bool SupprimerCommandeDocument(string idCommande)
        {
            return access.SupprimerCommandeDocument(idCommande);
        }

        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }

        public List<CommandeDocument> GetAbonnementsRevueFinissantDans30Jours()
        {
            var abonnements = access.GetCommandesRevue();
            var aujourdHui = DateTime.Today;
            return abonnements
                .Where(c => c.DateFinAbonnement.HasValue)
                .Where(c => c.DateFinAbonnement.Value >= aujourdHui && c.DateFinAbonnement.Value <= aujourdHui.AddDays(30))
                .OrderBy(c => c.DateFinAbonnement.Value)
                .ToList();
        }
    }
}
