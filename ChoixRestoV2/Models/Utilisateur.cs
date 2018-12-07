using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.Models
{
    //Via EntityFramework = classes "POCO" (Plain Old C# Object) : représente une classe minimale avec uniquement des propriétés pour contenir des valeurs associées à un objet
    //Modèle "Utilisateur"
    public class Utilisateur
    {
        public int Id { get; set; }
        //annotation pour rendre une propriété obligatoire (rendre le champ de la base de données en "not null")
        [Required]
        public string Prenom { get; set; }
        [Required]
        public string MotDePasse { get; set; }
    }
}