using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models.ViewModels
{
    public class DetailsIngredient
    {
        //the ingredients itself that we want to display
        public IngredientsDto SelectedIngredients { get; set; }

        //all of the related animals to that particular species
        public IEnumerable<RecipeDto> RelatedRecipes { get; set; }
    }
}