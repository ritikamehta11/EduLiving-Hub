using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models.ViewModels
{
    public class UpdateRecipe
    {

        //This viewmodel is a class which stores information that we need to present to /Recipe/Update/{}

        //the existing recipe information

        public RecipeDto SelectedRecipe { get; set; }

        // all ingredients to choose when updating a recipe

        public IEnumerable<IngredientsDto> IngredientsOptions { get; set; }
    }
}