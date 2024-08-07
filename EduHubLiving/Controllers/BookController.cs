using EduHubLiving.Models;
using EduHubLiving.Models.ViewModels;
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
    public class BookController : Controller
    {
        // GET: Book

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44315/api/");
        }


        public ActionResult List()
        {
            //objective: communicate with our Book data api to retrieve a list of books
            //curl https://localhost:44315/api/bookdata/listbooks
            ShowCount ViewModel = new ShowCount();

            string url = "bookdata/listbooks";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<BookDto> books = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;
            ViewModel.Books = books;

            url = "bookdata/countbooks";
            response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            int booksCount = response.Content.ReadAsAsync<int>().Result;
            ViewModel.totalBooks = booksCount;


            return View(ViewModel);
        }

        public ActionResult Show(int id)
        {
            string url = "bookdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookDto book = response.Content.ReadAsAsync<BookDto>().Result;
            return View(book);


        }
        public ActionResult Create(Book book)
        {

            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(book.bookName);
            //objective: add a new book into our system using the API
            //curl -H "Content-Type:application/json" -d @book.json https://localhost:44324/api/bookdata/addbook 


            string url = "bookdata/addbook";

            Debug.WriteLine(book);
            string jsonpayload = jss.Serialize(book);
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

        public ActionResult New()
        {
            return View();
        }


        // GET : book/deleteconfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            string url = "bookdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookDto selectedbook = response.Content.ReadAsAsync<BookDto>().Result;
            return View(selectedbook);
        }



        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "bookdata/deletebook/" + id;
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
        public ActionResult Edit(int id)
        {
            UpdateBook ViewModel = new UpdateBook();

            // Get the existing book information
            string url = "bookdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookDto SelectedBook = response.Content.ReadAsAsync<BookDto>().Result;
            ViewModel.SelectedBook = SelectedBook;

            // Get all genres to choose from when updating this book
            url = "Genredata/listgenres/";
            response = client.GetAsync(url).Result;
            IEnumerable<GenreDto> GenreOptions = response.Content.ReadAsAsync<IEnumerable<GenreDto>>().Result;
            ViewModel.GenreOptions = GenreOptions;

            // Get all authors to choose from when updating this book
            url = "Authordata/listauthors/";
            response = client.GetAsync(url).Result;
            IEnumerable<AuthorDto> AuthorOptions = response.Content.ReadAsAsync<IEnumerable<AuthorDto>>().Result;
            ViewModel.AuthorOptions = AuthorOptions;

            return View(ViewModel);
        }


        // POST: Book/Update/5
        [HttpPost]
        public ActionResult Update(int id, Book book)
        {

            string url = "bookdata/updatebook/" + id;
            string jsonpayload = jss.Serialize(book);
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