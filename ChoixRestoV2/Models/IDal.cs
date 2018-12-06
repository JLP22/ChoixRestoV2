using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoixRestoV2.Models
{
    public interface IDal : IDisposable
    {
        void CreerRestaurant(string nom, string telephone);
        void ModifierRestaurant(int id, string nom, string telephone);
        List<Resto> ObtientTousLesRestaurants();
        bool RestaurantExiste(string nom);
        int AjouterUtilisateur(string nom, string motDePasse);
        Utilisateur Authentifier(string nom, string motDePasse);
        Utilisateur ObtenirUtilisateur(int id);
        Utilisateur ObtenirUtilisateur(string idStr);
        int CreerUnSondage();
        void AjouterVote(int idSondage, int idResto, int idUtilisateur);
        List<Resultats> ObtenirLesResultats(int idSondage);
        bool ADejaVote(int idSondage, string idStr);
    }
}
