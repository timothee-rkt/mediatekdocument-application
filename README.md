# MediatekDocuments - Extensions et Nouvelles Fonctionnalités
Ce dépôt est une version étendue de l'application originale, disponible ici :(https://github.com/CNED-SLAM/MediaTekDocuments.git). Le README du dépôt d'origine contient la présentation initiale du projet et de son contexte.
# ✨ Fonctionnalités Ajoutées
Dans le cadre de cet atelier, plusieurs missions ont été réalisées pour enrichir l'application et sécuriser son fonctionnement :
1. Gestion Étendue du Catalogue
Administration des documents : Possibilité d'ajouter, modifier et supprimer des livres, DVD et revues
.
Sécurité des données : L'identifiant d'un document est désormais verrouillé en modification. La suppression est bloquée si le document possède des exemplaires ou des commandes rattachées
.
2. Système de Commandes et Abonnements
Suivi des commandes (Livres/DVD) : Gestion complète du cycle de vie d'une commande : en cours, livrée, réglée, et relancée
.
Automatisation : Génération automatique des exemplaires en base de données dès qu'une commande passe au stade "livrée"
.
Abonnements Revues : Gestion des abonnements et renouvellements. Une alerte automatique s'affiche désormais au démarrage pour les abonnements expirant dans moins de 30 jours
.
3. Gestion Physique des Exemplaires
Suivi de l'état : Mise en place d'un contrôle de l'état d'usure des documents physiques (neuf, usagé, détérioré, inutilisable)
.
4. Sécurité et Authentification
Contrôle d'accès par service : Authentification sécurisée différenciant les droits entre le service Administratif (accès complet), le service Prêts (consultation uniquement) et le service Culture (accès refusé)
.
Protection de l'API : Masquage des identifiants de connexion API (précédemment en clair dans le code) et sécurisation de l'accès à l'arborescence des fichiers de l'API via le navigateur
.
Traçabilité : Intégration de journaux d'événements (logs) pour toutes les actions de la couche d'accès aux données
.
 # 💻 Installation et Utilisation en Local
Suivez ces étapes pour configurer l'environnement de développement sur votre poste :
Prérequis
IDE : Visual Studio 2019 ou supérieur.
SGBD : MySQL (WAMP).
Serveur Web : Apache (pour l'API PHP).
Outils : Extensions Specflow et Newtonsoft.json pour Visual Studio, et Postman pour les tests API
.
Étape 1 : Base de données
Importez le script SQL de la base de données mediatek86 fourni dans les ressources.
Assurez-vous que l'utilisateur MySQL a les droits nécessaires sur cette base.
Étape 2 : API REST
Déposez le dossier de l'API PHP (rest_mediatekdocuments) dans le répertoire racine de votre serveur web (ex: htdocs ou www).
Configurez le fichier de connexion à la base de données de l'API avec vos accès locaux.
Étape 3 : Configuration de l'Application C#
Ouvrez la solution .sln dans Visual Studio.
Ouvrez le fichier App.config à la racine du projet.
Modifiez les clés de configuration pour faire pointer l'application vers votre API locale (ex: http://localhost/rest_mediatekdocuments/) et renseignez les identifiants d'accès sécurisés
.
Étape 4 : Lancement
Compilez la solution (Nettoyer puis Re-générer).
Lancez l'application. Utilisez vos identifiants définis en base de données pour vous connecter selon votre service d'affectation.

