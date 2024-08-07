using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models.ViewModels
{
    public class DetailsRecipe
    {
        public RecipeDto SelectedRecipe { get; set; }
        public IEnumerable<IngredientsDto> IngredientsOptions { get; set; }
    }
}