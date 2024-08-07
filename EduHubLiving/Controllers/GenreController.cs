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
    public class GenreController : Controller
    {
        // GET: Genre
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static GenreController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44315/api/");
        }

        // GET: Genre/List
        public ActionResult List()
        {
            //objective: communicate with our Genre data api to retrieve a list of genres
            //curl https://localhost:44315/api/genredata/listgenres
            ShowCount ViewModel = new ShowCount();

            string url = "genredata/listgenres";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<GenreDto> genres = response.Content.ReadAsAsync<IEnumerable<GenreDto>>().Result;

            ViewModel.Genres = genres;


            url = "genredata/countGenres";
            response = client.GetAsync(url).Result;
            int genrecount = response.Content.ReadAsAsync<int>().Result;

            ViewModel.totalGenres = genrecount;
            return View(ViewModel);
        }

        public ActionResult Show(int id)
        {
            string url = "genredata/findgenre/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GenreDto genre = response.Content.ReadAsAsync<GenreDto>().Result;
            return View(genre);


        }


        // GET: Genre/Detail/5
        public ActionResult Detail(int id)
        {
            DetailGenre ViewModel = new DetailGenre();

            //objective: communicate with our Genre data api to retrieve one Genre
            //curl https://localhost:44324/api/Genredata/findgenre/{id}

            string url = "genredata/findGenre/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            GenreDto SelectedGenre = response.Content.ReadAsAsync<GenreDto>().Result;
            Debug.WriteLine("Genre received : ");
            Debug.WriteLine(SelectedGenre.Name);

            ViewModel.SelectedGenre = SelectedGenre;

            //show all books under the care of this genre
            url = "bookdata/listbooksforgenre/" + id;
            response = client.GetAsync(url).Result;
            List<BookDto> booksingenre = response.Content.ReadAsAsync<List<BookDto>>().Result;
            Debug.WriteLine(booksingenre);
            ViewModel.BooksInGenre = booksingenre;


            return View(ViewModel);
        }

        public ActionResult New() { return View(); }

        // POST: Genre/Create
        [HttpPost]
        public ActionResult Create(Genre Genre)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Genre.GenreName);
            //objective: add a new Genre into our system using the API
            //curl -H "Content-Type:application/json" -d @Genre.json https://localhost:44324/api/Genredata/addGenre 
            string url = "genredata/addgenre";


            string jsonpayload = jss.Serialize(Genre);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
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


        // GET : genre/deleteconfirm/{id}
        [Route("Genre/DeleteConfirm/{id}")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "genredata/findgenre/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GenreDto selectedgenre = response.Content.ReadAsAsync<GenreDto>().Result;
            return View(selectedgenre);
        }



        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "genredata/deletegenre/" + id;
            HttpContent content = new StringContent("");
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

        // GET: Genre/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "genredata/findgenre/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GenreDto selectedGenre = response.Content.ReadAsAsync<GenreDto>().Result;
            return View(selectedGenre);
        }

        // POST: Genre/Update/5
        [HttpPost]
        public ActionResult Update(int id, Genre Genre)
        {

            string url = "genredata/updategenre/" + id;
            string jsonpayload = jss.Serialize(Genre);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}