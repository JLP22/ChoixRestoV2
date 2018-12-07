using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.Models
{
    //Via EntityFramework = classes "POCO" (Plain Old C# Object) : représente une classe minimale avec uniquement des propriétés pour contenir des valeurs associées à un objet
    //Modèle "Vote"
    public class Vote
    {
        public int Id { get; set; }
        public virtual Resto Resto { get; set; }
        public virtual Utilisateur Utilisateur { get; set; }
    }
}