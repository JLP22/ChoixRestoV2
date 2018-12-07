using ChoixRestoV2.Models;
using ChoixRestoV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixRestoV2.Controllers
{
    public class VoteController : Controller
    {
        //Pour eviter des using dans le corps de l'action (factorise dal)
        private IDal dal;

        //Surcharge le controleur (si aucun paramètre lui est filé, on file par défaut new dal() à RestaurantController(IDal dalIoc))
        //Utile pour le cas normal
        public VoteController() : this(new Dal())
        {
        }

        //Si un Idal est passé en paramètre au constructeur, on l'utilise 
        //utile pour les tests DalEnDur
        public VoteController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        //GET (affichage vue) : Action Index
        public ActionResult Index(int id)
        {
            //Création du view-modele de liste des Restaurant avec CB
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = dal.ObtientTousLesRestaurants().Select(r => new RestaurantCheckBoxViewModel { Id = r.Id, NomEtTelephone = string.Format("{0} ({1})", r.Nom, r.Telephone) }).ToList()
            };
            //Si l'utilisateur a déjà voté, affichage des résultats
            if (dal.ADejaVote(id, Request.Browser.Browser))
            {
                return RedirectToAction("AfficheResultat", new { id = id });
            }
            //Affichage de la vue vote avec view modele de liste des restaurants avec CB
            return View(viewModel);
        }

        //POST (renvoi formulaire) : Action Index
        [HttpPost]
        public ActionResult Index(RestaurantVoteViewModel viewModel, int id)
        {
            //Si modèle non respecté (ex pas de CB de cochée)
            if (!ModelState.IsValid)
                //Reaffichage page avec liste des restaurants avec CB
                return View(viewModel);

            //Teste utilisateur valide
            Utilisateur utilisateur = dal.ObtenirUtilisateur(Request.Browser.Browser);
            if (utilisateur == null)
                return new HttpUnauthorizedResult();
            //Pour chaque CB cochée, ajoute vote pour le restaurant concerné
            foreach (RestaurantCheckBoxViewModel restaurantCheckBoxViewModel in viewModel.ListeDesResto.Where(r => r.EstSelectionne))
            {
                dal.AjouterVote(id, restaurantCheckBoxViewModel.Id, utilisateur.Id);
            }
            //Rtoute vers l'action AfficheResultat
            return RedirectToAction("AfficheResultat", new { id = id });
        }

        //GET (affichage vue) : Action AfficheResultat
        public ActionResult AfficheResultat(int id)
        {
            //Si l'utitilisateur n'a pas encore voté, route vers l'action Index (= vue Vote/Index)
            if (!dal.ADejaVote(id, Request.Browser.Browser))
            {
                return RedirectToAction("Index", new { id = id });
            }
            //Affichage des résultat
            List<Resultats> resultats = dal.ObtenirLesResultats(id);
            //Route vers vue "AfficheResultat" en passant en paramètre la table Resultats
            return View(resultats.OrderByDescending(r => r.NombreDeVotes).ToList());
        }
    }
}