﻿using ChoixRestoV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixRestoV2.Controllers
{
    public class AccueilController : Controller
    {
        private IDal dal;

        public AccueilController() : this(new Dal())
        {
        }

        public AccueilController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        // GET: Accueil
        public ActionResult Index()
        {
            return View();
        }
        // POST: Accueil
        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost()
        {
            int idSondage = dal.CreerUnSondage();
            return View("Index", "Vote", new { id = idSondage });
        }
    }   
}