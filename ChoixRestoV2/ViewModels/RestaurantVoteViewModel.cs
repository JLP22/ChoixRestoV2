using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.ViewModels
{
    public class RestaurantVoteViewModel : IValidatableObject
    {
        public List<RestaurantCheckBoxViewModel> ListeDesResto { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ListeDesResto.Any(r => r.EstSelectionne))
                yield return new ValidationResult("Vous devez choisir au moins un restaurant", new[] { "ListeDesResto" });
        }
    }
}