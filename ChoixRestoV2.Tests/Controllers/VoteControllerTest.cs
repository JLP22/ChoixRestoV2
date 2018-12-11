using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChoixRestoV2.Controllers;
using ChoixRestoV2.Models;
using ChoixRestoV2.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ChoixRestoV2.Tests.Controllers
{
    [TestClass]
    public class VoteControllerTests
    {
        private IDal dal;
        private int idSondage;
        private VoteController controleur;


        [TestInitialize]
        public void Init()
        {
            //Génère donnée par défaut
            dal = new DalEnDur();
            idSondage = dal.CreerUnSondage();
            /* Moq = frameworks de simulacre (pour bouchonner le code = remplacer une vraie fonctionnalité d'une application par une fonctionnalité simulée et simpliste)
             * On utilise l’objet générique Mock pour créer un faux objet du type de notre interface. 
             * On utilise la méthode Setup  à travers une expression lambda pour indiquer que la propriété HttpContext.Request.Browser.Browser retournera "1" via Returns()
             * Pour utiliser Moq, clic droit sur projet, gérer package nuget et installer Moq
             */
             
            //bouchonner propriété Browser par défaut
            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(p => p.HttpContext.Request.Browser.Browser).Returns("1");

            controleur = new VoteController(dal);
            controleur.ControllerContext = controllerContext.Object;
        }

        [TestCleanup]
        public void Clean()
        {
            //nettoyer la mémoire entre chaques tests
            dal.Dispose();
        }

        //tester l’action Index : obtient view-model cohérent : sans avoir créé d’utilisateur (bouchonnage fournira un id qui n’existe pas dans la liste des utilisateurs)
        [TestMethod]
        public void Index_AvecSondageNormalMaisSansUtilisateur_RenvoiLeBonViewModelEtAfficheLaVue()
        {
            ViewResult view = (ViewResult)controleur.Index(idSondage);

            RestaurantVoteViewModel viewModel = (RestaurantVoteViewModel)view.Model;
            Assert.AreEqual(3, viewModel.ListeDesResto.Count);
            Assert.AreEqual(1, viewModel.ListeDesResto[0].Id);
            Assert.IsFalse(viewModel.ListeDesResto[0].EstSelectionne);
            Assert.AreEqual("Resto pinambour (0102030405)", viewModel.ListeDesResto[0].NomEtTelephone);
        }

        //tester l’action Index : obtient view-model cohérent : avec un utilisateur n’ayant pas voté
        [TestMethod]
        public void Index_AvecSondageNormalAvecUtilisateurNayantPasVote_RenvoiLeBonViewModelEtAfficheLaVue()
        {
            dal.AjouterUtilisateur("Nico", "1234");
            dal.AjouterUtilisateur("Jérémie", "1234");

            ViewResult view = (ViewResult)controleur.Index(idSondage);

            RestaurantVoteViewModel viewModel = (RestaurantVoteViewModel)view.Model;
            Assert.AreEqual(3, viewModel.ListeDesResto.Count);
            Assert.AreEqual(1, viewModel.ListeDesResto[0].Id);
            Assert.IsFalse(viewModel.ListeDesResto[0].EstSelectionne);
            Assert.AreEqual("Resto pinambour (0102030405)", viewModel.ListeDesResto[0].NomEtTelephone);
        }

        //Test utilisateur a déjà réalisé un vote et qu’il doit être redirigé vers l’affichage des résultats
        [TestMethod]
        public void Index_AvecSondageNormalMaisDejaVote_EffectueLeRedirectToAction()
        {
            dal.AjouterUtilisateur("Nico", "1234");
            dal.AjouterUtilisateur("Jérémie", "1234");
            dal.AjouterVote(idSondage, 1, 1);

            RedirectToRouteResult resultat = (RedirectToRouteResult)controleur.Index(idSondage);

            Assert.AreEqual("AfficheResultat", resultat.RouteValues["action"]);
            Assert.AreEqual(idSondage, resultat.RouteValues["id"]);
        }

        //vérifier POST : un view-model invalide renvoie bien la vue par défaut avec le même view-model
        [TestMethod]
        public void IndexPost_AvecViewModelInvalide_RenvoiLeBonViewModelEtAfficheLaVue()
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = new List<RestaurantCheckBoxViewModel>
            {
                new RestaurantCheckBoxViewModel { EstSelectionne = false, Id = 2, NomEtTelephone = "Resto pinière (0102030405)"},
                new RestaurantCheckBoxViewModel { EstSelectionne = false, Id = 3, NomEtTelephone = "Resto toro (0102030405)"},
            }
            };
            controleur.ValideLeModele(viewModel);

            ViewResult view = (ViewResult)controleur.Index(viewModel, idSondage);

            viewModel = (RestaurantVoteViewModel)view.Model;
            Assert.AreEqual(2, viewModel.ListeDesResto.Count);
            Assert.AreEqual(2, viewModel.ListeDesResto[0].Id);
            Assert.IsFalse(viewModel.ListeDesResto[0].EstSelectionne);
            Assert.AreEqual("Resto pinière (0102030405)", viewModel.ListeDesResto[0].NomEtTelephone);
        }

        //view-model est valide mais que l’utilisateur n’est pas trouvé
        [TestMethod]
        public void IndexPost_AvecViewModelValideMaisPasDutilisateur_RenvoiUneHttpUnauthorizedResult()
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = new List<RestaurantCheckBoxViewModel>
            {
                new RestaurantCheckBoxViewModel { EstSelectionne = true, Id = 2, NomEtTelephone = "Resto pinière (0102030405)"},
                new RestaurantCheckBoxViewModel { EstSelectionne = false, Id = 3, NomEtTelephone = "Resto toro (0102030405)"},
            }
            };
            controleur.ValideLeModele(viewModel);

            HttpUnauthorizedResult view = (HttpUnauthorizedResult)controleur.Index(viewModel, idSondage);

            Assert.AreEqual(401, view.StatusCode);
        }

        //Vérifier tout se passe bien avec un view-model valide et un utilisateur présent 
        [TestMethod]
        public void IndexPost_AvecViewModelValideEtUtilisateur_AppelleBienAjoutVoteEtRenvoiBonneAction()
        {
            Mock<IDal> mock = new Mock<IDal>();
            mock.Setup(m => m.ObtenirUtilisateur("1")).Returns(new Utilisateur { Id = 1, Prenom = "Nico" });

            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(p => p.HttpContext.Request.Browser.Browser).Returns("1");
            controleur = new VoteController(mock.Object);
            controleur.ControllerContext = controllerContext.Object;

            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = new List<RestaurantCheckBoxViewModel>
                {
                    new RestaurantCheckBoxViewModel { EstSelectionne = true, Id = 2, NomEtTelephone = "Resto pinière (0102030405)"},
                    new RestaurantCheckBoxViewModel { EstSelectionne = false, Id = 3, NomEtTelephone = "Resto toro (0102030405)"},
                }
            };
            controleur.ValideLeModele(viewModel);

            RedirectToRouteResult resultat = (RedirectToRouteResult)controleur.Index(viewModel, idSondage);

            mock.Verify(m => m.AjouterVote(idSondage, 2, 1));
            Assert.AreEqual("AfficheResultat", resultat.RouteValues["action"]);
            Assert.AreEqual(idSondage, resultat.RouteValues["id"]);
        }

        //tester la méthode d’affichage des résultats sans avoir voté
        [TestMethod]
        public void AfficheResultat_SansAvoirVote_RenvoiVersIndex()
        {
            RedirectToRouteResult resultat = (RedirectToRouteResult)controleur.AfficheResultat(idSondage);

            Assert.AreEqual("Index", resultat.RouteValues["action"]);
            Assert.AreEqual(idSondage, resultat.RouteValues["id"]);
        }
        //tester la méthode d’affichage des résultats en ayant voté
        [TestMethod]
        public void AfficheResultat_AvecVote_RenvoiLesResultats()
        {
            dal.AjouterUtilisateur("Nico", "1234");
            dal.AjouterUtilisateur("Jérémie", "1234");
            dal.AjouterVote(idSondage, 1, 1);

            ViewResult view = (ViewResult)controleur.AfficheResultat(idSondage);

            List<Resultats> model = (List<Resultats>)view.Model;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Resto pinambour", model[0].Nom);
            Assert.AreEqual(1, model[0].NombreDeVotes);
            Assert.AreEqual("0102030405", model[0].Telephone);
        }
    }
}
