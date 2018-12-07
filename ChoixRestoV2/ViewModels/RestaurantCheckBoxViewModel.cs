using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixRestoV2.ViewModels
{
    // Page de validation du vote contient CB (Html.CheckBoxFor)
    // Coté BDD dispose liste restaurant mais pas de booleen (liés à CB)
    // Pour bénéficier du binding de modèle : création d'un view-modele s'apuyant sur resto (info à afficher + booleen)
    public class RestaurantCheckBoxViewModel
    {
        public int Id { get; set; }
        public string NomEtTelephone { get; set; }
        public bool EstSelectionne { get; set; }
    }
}