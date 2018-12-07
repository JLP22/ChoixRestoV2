using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.Models
{
    //Via EntityFramework = classes "POCO" (Plain Old C# Object) : représente une classe minimale avec uniquement des propriétés pour contenir des valeurs associées à un objet
    //Modèle "Resto"

    //Décoration pour forcer EntityFramework à nommer la table (objet restau ayant été traduit en anglais au pluriel par resteos) 
    [Table("Restos")]
    public class Resto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom du restaurant doit être saisi")] //Validation coté BDD type Obligatoire et affichage message erreur précis 
        public string Nom { get; set; }
        //Force le nom du champ lors de l'affichage dans "@Html.TextBoxFor"
        [Display(Name = "Téléphone")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Le numéro de téléphone est incorrect")] //Validation coté BDD avec Expression reg et affichage message erreur précis 
        //(Utilisation dans un formulaire avec @Html.LabelFor(model => model.Telephone)
        //                                  @Html.TextBoxFor(model => model.Telephone)

        public string Telephone { get; set; }

        /*********************** MEMO VUE *******************************/
        /*
        @using(Html.BeginForm())
        {
        < fieldset >
            < legend > Ajouter un restaurant</ legend >
            < div >
                   @Html.LabelFor(model => model.Nom)
                @Html.TextBoxFor(model => model.Nom)
                @Html.ValidationMessageFor(model => model.Nom)
            </ div >
            < div >
                @Html.LabelFor(model => model.Telephone)
                @Html.TextBoxFor(model => model.Telephone)
                @Html.ValidationMessageFor(model => model.Telephone)
            </ div >
            < br />
            < input type = "submit" value = "Ajouter" />
        </ fieldset >
        /******************************************************************/
    }
}