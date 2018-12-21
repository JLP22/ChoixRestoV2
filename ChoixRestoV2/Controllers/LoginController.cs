using ChoixRestoV2.Models;
using ChoixRestoV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ChoixRestoV2.Controllers
{
    public class LoginController : Controller
    {
        private IDal dal;

        public LoginController() : this(new Dal())
        {

        }

        private LoginController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            //Vérifie que l'utilisateur est authentifié
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //Récupère utilisateur via la DAL à travers un viewmodel pour préparer les données  à présenter dans la vue
                //Identifiant de l’utilisateur dans la propriété Name car c'est L'id utilisé lors de l'appel (FormsAuthentication.SetAuthCookie(id.ToString(), false);)
                viewModel.Utilisateur = dal.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            }
            return View(viewModel);
        }
        //Si l'utilisateur n'est pas authentifié, la vue proposera un formulaire d'authentification
        //2 paramètre : viewmodel de l'utilisateur pour traiter contenu du formulaire 
        //+ chaine permettant d'attraper l'url visé précédemment mais interdite le cas échéant (ex : /Login/Index?ReturnUrl=%2fVote%2fIndex%2f1)
        // et permettra de rediriger l’utilisateur vers la ressource qu’il essayait d’obtenir
        [HttpPost]
        public ActionResult Index(UtilisateurViewModel viewModel, string returnUrl)
        {
            //Modèle fournit valide (info user correctement rempli - prénom + mdp bien renseignés ?)
            if (ModelState.IsValid)
            {
                //Authentification de l'utilisateur
                Utilisateur utilisateur = dal.Authentifier(viewModel.Utilisateur.Prenom, viewModel.Utilisateur.MotDePasse);
                if (utilisateur != null)
                {
                    //Si authentification OK : authentifier sur ASP.NET l’utilisateur en générant un cookie
                    //2ème param = false permet de faire en sorte que l’authentification n’ait qu’une durée de vie limitée à la session
                    FormsAuthentication.SetAuthCookie(utilisateur.Id.ToString(), false);
                    //Si URL page préalablement demandé existe, renvoie vers elle
                    //"IsLocalUrl" : Vérifie que le paramètre ReturnUrl contient bien un lien vers notre site (éviter toute tentative de redirection vers un site exterieur)
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    //Sinon renvoie page accueil
                    return Redirect("/");
                }
                //Si authentification KO : ajouter erreur au ModelState afin d’informer la vue
                //Champ en erreur = Utilisateur.Prenom car est chemin d’accès via le view-model
                ModelState.AddModelError("Utilisateur.Prenom", "Prénom et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }

        //Créer un compte : affiche simplement la vue
        public ActionResult CreerCompte()
        {
            return View();
        }

        //Créer un compte : traite le formulaire et appelle la DAL pour créer le compte
        [HttpPost]
        public ActionResult CreerCompte(Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                int id = dal.AjouterUtilisateur(utilisateur.Prenom, utilisateur.MotDePasse);
                FormsAuthentication.SetAuthCookie(id.ToString(), false);
                return Redirect("/");
            }
            return View(utilisateur);
        }

        //Déconnexion en vidant le cookie d'authentification
        public ActionResult Deconnexion()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}