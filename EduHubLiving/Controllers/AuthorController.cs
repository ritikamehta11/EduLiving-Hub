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
    public class AuthorController : Controller
    {
        // GET: Author
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AuthorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44315/api/");
        }

        // GET: Author/List
        public ActionResult List()
        {
            //objective: communicate with our Author data api to retrieve a list of authors
            //curl https://localhost:44315/api/authordata/listauthors

            ShowCount ViewModel = new ShowCount();
            string url = "authordata/listauthors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<AuthorDto> authors = response.Content.ReadAsAsync<IEnumerable<AuthorDto>>().Result;

            ViewModel.Authors = authors;


            url = "authordata/countAuthors";
            response = client.GetAsync(url).Result;
            int authorcount = response.Content.ReadAsAsync<int>().Result;

            ViewModel.totalAuthors = authorcount;
            return View(ViewModel);




        }

        public ActionResult Show(int id)
        {
            string url = "authordata/findauthor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AuthorDto author = response.Content.ReadAsAsync<AuthorDto>().Result;
            return View(author);


        }


        // GET: Author/Detail/5
        public ActionResult Detail(int id)
        {
            DetailAuthor ViewModel = new DetailAuthor();

            //objective: communicate with our Author data api to retrieve one Author
            //curl https://localhost:44324/api/Authordata/findauthor/{id}

            string url = "authordata/findAuthor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            AuthorDto SelectedAuthor = response.Content.ReadAsAsync<AuthorDto>().Result;
            Debug.WriteLine("Author received : ");
            Debug.WriteLine(SelectedAuthor.Name);

            ViewModel.SelectedAuthor = SelectedAuthor;

            //show all books under the care of this author
            url = "bookdata/listbooksforauthor/" + id;
            response = client.GetAsync(url).Result;
            List<BookDto> bookswritten = response.Content.ReadAsAsync<List<BookDto>>().Result;
            Debug.WriteLine(bookswritten);
            ViewModel.BooksWritten = bookswritten;


            return View(ViewModel);
        }

        public ActionResult New() { return View(); }

        // POST: Author/Create
        [HttpPost]
        public ActionResult Create(Author Author)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Author.AuthorName);
            //objective: add a new Author into our system using the API
            //curl -H "Content-Type:application/json" -d @Author.json https://localhost:44324/api/Authordata/addAuthor 
            string url = "authordata/addauthor";


            string jsonpayload = jss.Serialize(Author);
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


        // GET : author/deleteconfirm/{id}
        [Route("Author/DeleteConfirm/{id}")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "authordata/findauthor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AuthorDto selectedauthor = response.Content.ReadAsAsync<AuthorDto>().Result;
            return View(selectedauthor);
        }



        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "authordata/deleteauthor/" + id;
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

        // GET: Author/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "authordata/findauthor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AuthorDto selectedAuthor = response.Content.ReadAsAsync<AuthorDto>().Result;
            return View(selectedAuthor);
        }

        // POST: Author/Update/5
        [HttpPost]
        public ActionResult Update(int id, Author Author)
        {

            string url = "authordata/updateauthor/" + id;
            string jsonpayload = jss.Serialize(Author);
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