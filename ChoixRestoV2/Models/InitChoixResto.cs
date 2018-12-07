using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.Models
{
    //Initialisateur personnalisé pour alimenter la BDD avec des données (par défaut si appelée via "Global.asax.cs / Application_Start())

    //La classe doit hériter de DropCreateDatabaseAlways
    public class InitChoixResto : DropCreateDatabaseAlways<BddContext>
    {
        //La méthode Seed doit être substituée par une alimentation personnalisée
        protected override void Seed(BddContext context)
        {
            context.Restos.Add(new Resto { Id = 1, Nom = "Resto pinambour", Telephone = "123" });
            context.Restos.Add(new Resto { Id = 2, Nom = "Resto pinière", Telephone = "456" });
            context.Restos.Add(new Resto { Id = 3, Nom = "Resto toro", Telephone = "789" });

            base.Seed(context);
        }
    }   
}