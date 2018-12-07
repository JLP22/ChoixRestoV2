using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.ViewModels
{
    // View-modele livré à la vue 
    // View-modele portant RestaurantCheckBoxViewModel
    // Utile car implémentation d'une validation personnalisée

    // Pour implémenter une validation particulière, la classe doit implémenter l'interface IValidatableObject
    public class RestaurantVoteViewModel : IValidatableObject
    {
        public List<RestaurantCheckBoxViewModel> ListeDesResto { get; set; }

        //Méthode Validate à implémenter (contrat interface)
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Au moins un restaurant doit être sélectionné
            if (!ListeDesResto.Any(r => r.EstSelectionne))
                yield return new ValidationResult("Vous devez choisir au moins un restaurant", new[] { "ListeDesResto" });
        }
    }
}