using EduHubLiving.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;

namespace EduHubLiving.Controllers
{
    public class RecipeDataController : ApiController
    {
        // GET: RecipeData
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Lists the recipe details in the database

        /// </summary>
        /// <returns> An array of recipe objects dtos/returns>
        /// <example>
        /// <returns></returns>
        //GET: api/RecipeData/ListRecipe->
        ///</example>
        [HttpGet]
        [Route("api/RecipeData/ListRecipe")]
        public List<RecipeDto> ListRecipe()
        {
            //SELECT * FROM RECIPE
            List<Recipe> RecipesData = db.Recipes.ToList();

            List<RecipeDto> RecipeDtos = new List<RecipeDto>();

            foreach (Recipe Recipe in RecipesData)
            {
                RecipeDto Dto = new RecipeDto();

                Dto.RecipeName = Recipe.RecipeName;
                Dto.RecipeType = Recipe.RecipeType;
                Dto.RecipeDescription = Recipe.RecipeDescription;
                Dto.RecipeInstructions = Recipe.RecipeInstructions;

                RecipeDtos.Add(Dto);

            }

            return RecipeDtos;


        }


        /// <summary>
        /// Retrieves a specific recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to retrieve.</param>
        /// <returns>A recipe DTO if found, NotFound if not found.</returns>
        /// <example>
        /// GET: api/RecipeData/FindRecipe/2
        /// </example>


        [ResponseType(typeof(Recipe))]
        [HttpGet]
        public IHttpActionResult FindRecipe(int id)
        {
            Recipe Recipe = db.Recipes.Find(id);
            RecipeDto RecipeDto = new RecipeDto()
            {
                RecipeID = Recipe.RecipeID,
                RecipeName = Recipe.RecipeName,
                RecipeType = Recipe.RecipeType,
                RecipeDescription = Recipe.RecipeDescription,
                RecipeInstructions = Recipe.RecipeInstructions

            };
            if (Recipe == null)
            {
                return NotFound();
            }

            return Ok(RecipeDto);
        }


        /// <summary>
        /// Updates an existing recipe.
        /// </summary>
        /// <param name="id">The ID of the recipe to update.</param>
        /// <param name="recipe">The updated recipe object.</param>
        /// <returns>NoContent if successful, NotFound if the recipe doesn't exist, BadRequest if the ID doesn't match the recipe.</returns>
        /// <example>
        /// POST: api/RecipeData/UpdateRecipe/2
        /// </example>



        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRecipe(int id, Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipe.RecipeID)
            {

                return BadRequest();
            }

            db.Entry(recipe).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool RecipeExists(int id)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Adds a new recipe to the database.
        /// </summary>
        /// <param name="recipe">The recipe object to add.</param>
        /// <returns>OK if successful, BadRequest if the model state is invalid.</returns>
        /// <example>
        /// POST: api/RecipeData/AddRecipe
        /// </example>

        // POST: api/RecipeData/AddRecipe
        [ResponseType(typeof(Recipe))]
        [HttpPost]
        [Route("api/RecipeData/AddRecipe")]
        public IHttpActionResult AddRecipe(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Recipes.Add(recipe);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recipe.RecipeID }, recipe);
        }


        /// <summary>
        /// Deletes a recipe from the database.
        /// </summary>
        /// <param name="id">The ID of the recipe to delete.</param>
        /// <returns>OK if successful, NotFound if the recipe doesn't exist.</returns>
        /// <example>
        /// POST: api/RecipeData/DeleteRecipe/2
        /// </example>


        [ResponseType(typeof(Recipe))]
        [HttpPost]
        public IHttpActionResult DeleteRecipe(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            db.Recipes.Remove(recipe);
            db.SaveChanges();

            return Ok();
        }

    }
}