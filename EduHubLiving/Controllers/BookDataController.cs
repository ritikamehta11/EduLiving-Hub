using EduHubLiving.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace EduHubLiving.Controllers
{
    public class BookDataController : ApiController
    {


        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// to list all the books in the library system
        /// </summary>
        /// <returns>a list of books</returns>
        // get : api/bookdata/listbooks ->
        // {"title" : "harry potter and the philosopher's stone" , "publication year": 1997 , "author":"j. k. rowling" , "genre" : "fantasy"}
        // {"title" : "1984" , "publication year": 1948 , "author":"george orwell" , "genre" : "dystopian"}
        // {"title" : "pride and prejudice" , "publication year": 1813 , "author":"jane austen" , "genre" : "romance"}

        [HttpGet]
        [Route("api/bookdata/listbooks")]

        public List<BookDto> ListBooks()
        {

            List<Book> bookslist = db.Books.ToList();
            List<BookDto> bookdtos = new List<BookDto>();
            foreach (Book book in bookslist)
            {

                BookDto bookdto = new BookDto();
                bookdto.BookId = book.BookId;
                bookdto.Title = book.Title;
                bookdto.PublishDate = book.PublishDate;
                bookdto.Plot = book.Plot;

                if (book.Author != null)
                {
                    // Only access properties if Author is not null
                    bookdto.Author = new AuthorDto
                    {
                        AuthorId = book.Author.AuthorId,
                        Name = book.Author.Name,
                        Bio = book.Author.Bio
                    };
                }

                if (book.Genre != null)
                {
                    // Only access properties if Author is not null
                    bookdto.Genre = new GenreDto
                    {
                        GenreId = book.Genre.GenreId,
                        Name = book.Genre.Name,
                        
                    };
                }


                // bookdto.Genre.GenreId = book.GenreId;





                bookdtos.Add(bookdto);

            }
            return bookdtos;
        }


        ///// <summary>
        ///// To find a the book in the library system
        ///// </summary>
        ///// <returns>A book details</returns>
        //// GET :api/BookData/FindBook/1
        [HttpGet]
        [Route("api/BookData/FindBook/{id}")]
        public IHttpActionResult FindBook(int id)
        {
            Book book = db.Books.Find(id);
            BookDto bookdto = new BookDto();
            if (bookdto == null)
            {
                return NotFound();
            }
            bookdto.BookId = book.BookId;
            bookdto.Title = book.Title;

            bookdto.PublishDate = book.PublishDate;
            bookdto.Plot = book.Plot;
            bookdto.Author = new AuthorDto
            {
                AuthorId = book.Author.AuthorId,
                Bio = book.Author.Bio,

                Name = book.Author.Name

            };

            bookdto.Genre = new GenreDto
            {
                GenreId = book.Genre.GenreId,
                Name = book.Genre.Name

            };

            return Ok(bookdto);
        }


        /// <summary>
        /// To add a book in the library system
        /// </summary>
        /// <returns>the list of books with the newly added book</returns>
        // POST: api/BookData/AddBook
        [ResponseType(typeof(Book))]
        [HttpPost]
        [Route("api/bookdata/addbook")]
        public IHttpActionResult AddBook(Book book)
        {

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Debug.WriteLine(error.ErrorMessage);
                        if (error.Exception != null)
                        {
                            Debug.WriteLine(error.Exception.Message);
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            Debug.WriteLine("Entering AddBook method");

            db.Books.Add(book);
            Debug.WriteLine(book.Title);
            db.SaveChanges();

            Debug.WriteLine("Book added with ID: " + book.BookId);
            return Ok(book);


            //if (!ModelState.IsValid)
            //{
            //  Debug.WriteLine("Model state is invalid");
            //foreach (var modelState in ModelState.Values)
            //{
            //  foreach (var error in modelState.Errors)
            //{
            //  Debug.WriteLine(error.ErrorMessage);
            //if (error.Exception != null)
            //{
            //  Debug.WriteLine(error.Exception.Message);
            //}
            //}
            // }
            //   return BadRequest(ModelState);
            //}

            //db.Books.Add(book);
            //db.SaveChanges();

            //Debug.WriteLine("Book added with ID: " + book.BookId);
            //return Ok();
            // return CreatedAtRoute("DefaultApi", new { controller = "BookData", id = book.BookId }, book);
        }


        /// <summary>
        /// To deletea book from the library system
        /// </summary>
        /// <returns>list of books with a book removed</returns>

        // POST : api/BookData/DeleteBook/{id}
        [ResponseType(typeof(Book))]
        [HttpPost]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok();
        }



        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/bookdata/updatebook/{id}")]
        public IHttpActionResult UpdateBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.BookId)
            {

                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;



            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }



        /// <summary>
        /// Gathers information about books related to a particular author
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all books in the database, including their associated genre that match to a particular author id
        /// </returns>
        /// <param name="id">Author ID.</param>
        /// <example>
        /// GET: api/BookData/ListBooksForAuthor/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(BookDto))]
        [Route("api/bookdata/listbooksforauthor/{id}")]
        public IHttpActionResult ListBooksForAuthor(int id)
        {
            //SQL equivalent:
            //select books.*, authorbooks.* from books INNER JOIN 
            //authorbooks on books.authorid = author.authorid
            //where authorbooks.authorid={AuthorID}

            //all books that have authors which match with our ID
            /* List<Book> Books = db.Books.Where(
                 book => book.AuthorId.Any(

                     author => author.AuthorId == id)
                 ).ToList();*/


            var Books = db.Books
        .Where(b => b.AuthorId == id)
        .ToList();
            List<BookDto> BookDtos = new List<BookDto>();

            Debug.WriteLine(Books);
            Debug.WriteLine(id);


            foreach (Book book in Books)
            {

                BookDto bookdto = new BookDto();
                bookdto.BookId = book.BookId;
                bookdto.Title = book.Title;
                bookdto.PublishDate = book.PublishDate;
                bookdto.Plot = book.Plot;
                bookdto.Author = new AuthorDto
                {
                    AuthorId = book.Author.AuthorId,
                    Bio = book.Author.Bio,

                    Name = book.Author.Name

                };
                if (book.Genre != null)
                {
                    // Only access properties if Genre is not null
                    bookdto.Genre = new GenreDto
                    {
                        GenreId = book.Genre.GenreId,
                        Name = book.Genre.Name,
                      
                    };
                }

                BookDtos.Add(bookdto);
            }
            Debug.WriteLine(BookDtos);
            return Ok(BookDtos);






        }



        [HttpGet]
        [ResponseType(typeof(BookDto))]
        [Route("api/bookdata/listbooksforgenre/{id}")]
        public IHttpActionResult ListBooksForGenre(int id)
        {
            //SQL equivalent:
            //select books.*, authorbooks.* from books INNER JOIN 
            //authorbooks on books.authorid = author.authorid
            //where authorbooks.authorid={AuthorID}

            //all books that have authors which match with our ID
            /* List<Book> Books = db.Books.Where(
                 book => book.AuthorId.Any(

                     author => author.AuthorId == id)
                 ).ToList();*/


            var Books = db.Books
        .Where(b => b.GenreId == id)
        .ToList();
            List<BookDto> BookDtos = new List<BookDto>();

            Debug.WriteLine(Books);
            Debug.WriteLine(id);


            foreach (Book book in Books)
            {

                BookDto bookdto = new BookDto();
                bookdto.BookId = book.BookId;
                bookdto.Title = book.Title;
                bookdto.PublishDate = book.PublishDate;
                bookdto.Plot = book.Plot;
                bookdto.Author = new AuthorDto
                {
                    AuthorId = book.Author.AuthorId,
                    Bio = book.Author.Bio,

                    Name = book.Author.Name

                };
                if (book.Genre != null)
                {
                    // Only access properties if Genre is not null
                    bookdto.Genre = new GenreDto
                    {
                        GenreId = book.Genre.GenreId,
                        Name = book.Genre.Name,
                        
                    };
                }

                BookDtos.Add(bookdto);
            }
            Debug.WriteLine(BookDtos);
            return Ok(BookDtos);






        }


        [HttpGet]
        [ResponseType(typeof(int))]
        [Route("api/bookdata/countbooks")]
        public IHttpActionResult CountBooks()
        {
            int bookCount = db.Books.Count();
            Debug.WriteLine(bookCount);
            return Ok(bookCount);
        }
    }

}
