using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.Models
{
    //Via EntityFramework = classes "POCO" (Plain Old C# Object) : représente une classe minimale avec uniquement des propriétés pour contenir des valeurs associées à un objet
    //Modèle "Resultats"
    public class Resultats
    {
        public string Nom { get; set; }
        public string Telephone { get; set; }
        public int NombreDeVotes { get; set; }
    }
}