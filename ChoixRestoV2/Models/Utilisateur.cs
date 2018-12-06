using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public string MotDePasse { get; set; }
    }
}