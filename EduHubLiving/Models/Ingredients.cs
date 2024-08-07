using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models
{
    public class Ingredients
    {

        [Key]
        public int IngredientID { get; set; }

        public string IngredientName { get; set; }
    }

    public class IngredientsDto
    {
        public int IngredientID { get; set; }
        public string IngredientName { get; set; }
    }
}