namespace MediaTekDocuments.model
{
    public class Suivi
    {
        public string Id { get; set; }
        public string Libelle { get; set; }

        public Suivi(string id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }
    }
}