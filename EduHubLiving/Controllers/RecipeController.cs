using EduHubLiving.Models;
using System;
using EduHubLiving.Models.ViewModels;
using EduHubLiving.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EduHubLiving.Controllers
{
    public class RecipeController : Controller
    {
        // GET: Recipe
       
        private static readonly HttpClient client = new HttpClient();
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RecipeController()
        {
            client.BaseAddress = new Uri("https://localhost:44336/api/");
        }

        // GET: Recipe/List
        //
        public ActionResult List()
        {
            string url = "RecipeData/ListRecipe";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<RecipeDto> recipes = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;
                return View(recipes);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Recipe/Details/5
        public ActionResult Details(int id)
        {
            DetailsRecipe ViewModel = new DetailsRecipe(); // Ensure DetailsRecipe exists and is correctly implemented
            string url = "RecipeData/FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                RecipeDto selectedRecipe = response.Content.ReadAsAsync<RecipeDto>().Result;
                return View(selectedRecipe);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Recipe/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        public ActionResult Create(Recipe recipe)
        {
            string url = "RecipeData/AddRecipe";

            string jsonpayload = jss.Serialize(recipe);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        // GET: Recipe/Edit/5
        public ActionResult Edit(int id)
        {
            string url = $"RecipeData/FindRecipe/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                RecipeDto selectedRecipe = response.Content.ReadAsAsync<RecipeDto>().Result;
                return View(selectedRecipe);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Recipe/Update/5
        [HttpPost]
        public ActionResult Update(int id, Recipe recipe)
        {
            string url = $"RecipeData/UpdateRecipe/{id}";

            string jsonPayload = jss.Serialize(recipe);
            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Recipe/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = $"RecipeData/FindRecipe/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                RecipeDto selectedRecipe = response.Content.ReadAsAsync<RecipeDto>().Result;
                return View(selectedRecipe);
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        // POST: Recipe/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = $"RecipeData/DeleteRecipe/{id}";
            HttpResponseMessage response = client.PostAsync(url, null).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // Handle other actions or errors here
        public ActionResult Error()
        {
            return View();
        }

    }
}