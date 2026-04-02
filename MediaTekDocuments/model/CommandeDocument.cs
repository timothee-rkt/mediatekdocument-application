using System;

namespace MediaTekDocuments.model
{
    public class CommandeDocument
    {
        public string Id { get; set; }
        public string IdDocument { get; set; }
        public DateTime DateCommande { get; set; }
        public DateTime? DateFinAbonnement { get; set; }
        public decimal Montant { get; set; }
        public int Quantite { get; set; }
        public string IdSuivi { get; set; }
        public string Suivi { get; set; }
        public string TypeDocument { get; set; } // "Livre", "Dvd" ou "Revue"

        public CommandeDocument(string id, string idDocument, DateTime dateCommande, DateTime? dateFinAbonnement, decimal montant, int quantite,
            string idSuivi, string suivi, string typeDocument)
        {
            Id = id;
            IdDocument = idDocument;
            DateCommande = dateCommande;
            DateFinAbonnement = dateFinAbonnement;
            Montant = montant;
            Quantite = quantite;
            IdSuivi = idSuivi;
            Suivi = suivi;
            TypeDocument = typeDocument;
        }
    }
}