using ChoixRestoV2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ChoixRestoV2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //Méthode exécutée au démarrage de l'application web
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            //Appel classe DropCreateDatabaseAlways pour vides la BDD
            IDatabaseInitializer<BddContext> init = new DropCreateDatabaseAlways<BddContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new BddContext());

            /*
            //Appel de la classe InitChoixResto pour initialiser la BDD avec des données par défaut
            IDatabaseInitializer<BddContext> init = new InitChoixResto();
            Database.SetInitializer(init);
            init.InitializeDatabase(new BddContext());*/

        }
    }
}
