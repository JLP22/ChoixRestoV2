using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.Models
{
    //Via EntityFramework = classes "POCO" (Plain Old C# Object) : représente une classe minimale avec uniquement des propriétés pour contenir des valeurs associées à un objet
    //Modèle "Sondage"
    public class Sondage
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual List<Vote> Votes { get; set; }
    }
}