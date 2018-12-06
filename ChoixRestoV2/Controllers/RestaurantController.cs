using ChoixRestoV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixRestoV2.Controllers
{
    public class RestaurantController : Controller
    {
        //Pour eviter des using dans le corps de l'action (factorise dal)
        private IDal dal;

        //Surcharge le controleur (si aucun paramètre lui est filé, on file par défaut new dal() à RestaurantController(IDal dalIoc))
        //Utile pour le cas normal
        public RestaurantController() : this(new Dal())
        {
        }

        //Si un Idal est passé en paramètre au constructeur, on l'utilise 
        //utile pour les tests DalEnDur
        public RestaurantController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            List<Resto> listeDesRestaurants = dal.ObtientTousLesRestaurants();
            return View(listeDesRestaurants);
        }

        //Apel de la vue (get)
        public ActionResult CreerRestaurant()
        {
            return View();
        }

        //envoi du formulaire à la vue (post)
        [HttpPost]
        public ActionResult CreerRestaurant(Resto resto)
        {
            //teste par le nom si restaurant existe déjà
            if (dal.RestaurantExiste(resto.Nom))
            {
                //Renvoi message d'erreur
                ModelState.AddModelError("Nom", "Ce nom de restaurant existe déjà");
                return View(resto);
            }
            //teste si numéro de tel est valide, si non msg erreur
            if (!ModelState.IsValid)
                return View(resto);
            //Création du restaurant
            dal.CreerRestaurant(resto.Nom, resto.Telephone);
            //renvoi sur la vue Index
            return RedirectToAction("Index");
        }

        public ActionResult ModifierRestaurant(int? id)
        {
            if (id.HasValue)
            {
                Resto restaurant = dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
                if (restaurant == null)
                    return View("Error");
                return View(restaurant);
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ModifierRestaurant(Resto resto)
        {
            if (!ModelState.IsValid)
                return View(resto);
            dal.ModifierRestaurant(resto.Id, resto.Nom, resto.Telephone);
            return RedirectToAction("Index");
        }
    }
}