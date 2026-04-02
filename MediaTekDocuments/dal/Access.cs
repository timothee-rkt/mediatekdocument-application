using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;
using System.Diagnostics;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update

        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);

                // Configuration des logs
                string logFilePath = "logs.txt";
                TextWriterTraceListener listener = new TextWriterTraceListener(logFilePath);
                Trace.Listeners.Add(listener);
                Trace.AutoFlush = true;
                Trace.TraceInformation("Initialisation de la classe Access.");
            }
            catch (Exception e)
            {
                Trace.TraceError("Erreur lors de l'initialisation de Access : " + e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/revue/" + idDocument, null);
            return lesExemplaires;
        }

        public List<Exemplaire> GetExemplairesLivre(string idLivre)
        {
            if (string.IsNullOrWhiteSpace(idLivre)) return new List<Exemplaire>();
            return TraitementRecup<Exemplaire>(GET, "exemplaire/livre/" + idLivre, null);
        }

        public List<Exemplaire> GetExemplairesDvd(string idDvd)
        {
            if (string.IsNullOrWhiteSpace(idDvd)) return new List<Exemplaire>();
            return TraitementRecup<Exemplaire>(GET, "exemplaire/dvd/" + idDvd, null);
        }

        public List<Etat> GetAllEtats()
        {
            return TraitementRecup<Etat>(GET, "etat", null);
        }

        public bool ModifierExemplaire(Exemplaire exemplaire)
        {
            if (exemplaire == null) return false;
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            return TraitementEcriture("PUT", "exemplaire", "champs=" + jsonExemplaire);
        }

        public bool SupprimerExemplaire(string idDocument, int numero)
        {
            if (string.IsNullOrWhiteSpace(idDocument) || numero <= 0) return false;
            return TraitementEcriture("DELETE", $"exemplaire/{idDocument}/{numero}", null);
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ecriture simple vers l'API (POST, PUT, DELETE)
        /// </summary>
        /// <param name="methode"></param>
        /// <param name="message"></param>
        /// <param name="parametres"></param>
        /// <returns></returns>
        private bool TraitementEcriture(String methode, String message, String parametres)
        {
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                String code = (String)retour["code"];
                return code != null && code.Equals("200");
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + e.Message);
            }
            return false;
        }

        public bool CreerLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre, new CustomDateTimeConverter());
            return TraitementEcriture(POST, "livre", "champs=" + jsonLivre);
        }

        public bool ModifierLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre, new CustomDateTimeConverter());
            return TraitementEcriture("PUT", "livre", "champs=" + jsonLivre);
        }

        public bool SupprimerLivre(string idLivre)
        {
            if (string.IsNullOrWhiteSpace(idLivre)) return false;
            return TraitementEcriture("DELETE", "livre/" + idLivre, null);
        }

        public bool CreerDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd, new CustomDateTimeConverter());
            return TraitementEcriture(POST, "dvd", "champs=" + jsonDvd);
        }

        public bool ModifierDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd, new CustomDateTimeConverter());
            return TraitementEcriture("PUT", "dvd", "champs=" + jsonDvd);
        }

        public bool SupprimerDvd(string idDvd)
        {
            if (string.IsNullOrWhiteSpace(idDvd)) return false;
            return TraitementEcriture("DELETE", "dvd/" + idDvd, null);
        }

        public bool CreerRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue, new CustomDateTimeConverter());
            return TraitementEcriture(POST, "revue", "champs=" + jsonRevue);
        }

        public bool ModifierRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue, new CustomDateTimeConverter());
            return TraitementEcriture("PUT", "revue", "champs=" + jsonRevue);
        }

        public bool SupprimerRevue(string idRevue)
        {
            if (string.IsNullOrWhiteSpace(idRevue)) return false;
            return TraitementEcriture("DELETE", "revue/" + idRevue, null);
        }

        public List<CommandeDocument> GetCommandesLivre(string idLivre)
        {
            if (string.IsNullOrWhiteSpace(idLivre)) return new List<CommandeDocument>();
            return TraitementRecup<CommandeDocument>(GET, "commande/livre/" + idLivre, null);
        }

        public List<CommandeDocument> GetCommandesDvd(string idDvd)
        {
            if (string.IsNullOrWhiteSpace(idDvd)) return new List<CommandeDocument>();
            return TraitementRecup<CommandeDocument>(GET, "commande/dvd/" + idDvd, null);
        }

        public List<CommandeDocument> GetCommandesRevue()
        {
            return TraitementRecup<CommandeDocument>(GET, "commande/revue", null);
        }

        public bool CreerCommandeDocument(CommandeDocument commande)
        {
            if (commande == null) return false;
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            return TraitementEcriture(POST, "commande", "champs=" + jsonCommande);
        }

        public bool ModifierCommandeDocument(CommandeDocument commande)
        {
            if (commande == null) return false;
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            return TraitementEcriture("PUT", "commande", "champs=" + jsonCommande);
        }

        public bool SupprimerCommandeDocument(string idCommande)
        {
            if (string.IsNullOrWhiteSpace(idCommande)) return false;
            return TraitementEcriture("DELETE", "commande/" + idCommande, null);
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

    }
}
