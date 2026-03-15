LesReken 🎓

LesReken est une application web progressive (PWA) conçue pour la gestion simplifiée de cours particuliers. Développée spécifiquement pour une utilisation fluide sur mobile, elle automatise le calcul complexe des tarifs de groupe au prorata.

🚀 Concept & Business Logic

L'application répond à un besoin spécifique de facturation dynamique :

Tarif fixe par groupe : 25€ / heure.

Calcul au prorata : Le coût est automatiquement divisé par le nombre d'élèves présents.

Exemple : 1h à 2 personnes = 12,50€ par personne.

Exemple : 30mn à 5 personnes = 2,50€ par personne.

Gestion des paiements : Support des paiements partiels et suivi en temps réel du solde restant dû par chaque élève.

🛠 Tech Stack

Ce projet est une démonstration de compétences modernes en développement Web :

Frontend : Angular utilisant les Signals pour une gestion d'état réactive et performante.

Styling : Tailwind CSS avec une approche Mobile-First et des composants inspirés du design iOS (Glassmorphism, Safe Area).

Backend : Architecture ASP.NET Core 10 avec Entity Framework Core et SQLite.

Stockage local : Persistance via localStorage (version actuelle) avec synchronisation prévue vers une API REST.

✨ Fonctionnalités clés

Saisie Rapide : Interface optimisée pour enregistrer une séance en moins de 10 secondes après le cours.

Gestion d'Élèves : Distinction entre les élèves permanents et ponctuels.

Journal de Bord : Historique complet des sessions avec possibilité d'annulation.

Comptabilité simplifiée : Vue d'ensemble des revenus perçus et des créances par élève.

Optimisation Mobile : Design adapté et navigation tactile facilitée.

📱 Installation (PWA)

Pour utiliser LesReken sur votre iPhone comme une application native :

Ouvrez l'URL du projet dans Safari.

Appuyez sur le bouton Partager.

Sélectionnez "Sur l'écran d'accueil".

Développé avec passion ❤️️
