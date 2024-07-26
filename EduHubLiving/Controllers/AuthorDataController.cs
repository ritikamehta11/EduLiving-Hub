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
    public class AuthorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [Route("api/AuthorData/ListAuthors")]
        public List<AuthorDto> ListAuthors()
        {

            List<Author> AuthorsList = db.Authors.ToList();

            List<AuthorDto> authordtos = new List<AuthorDto>();

            foreach (Author author in AuthorsList)
            {

                AuthorDto authorDto = new AuthorDto();
                authorDto.AuthorId = author.AuthorId;
                authorDto.Name = author.Name;
                authorDto.Bio = author.Bio;
                authordtos.Add(authorDto);

            }

            return authordtos;
        }


        // GET :api/AuthorData/FindAuthor/1
        [HttpGet]
        [Route("api/AuthorData/FindAuthor/{id}")]
        public IHttpActionResult FindAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            AuthorDto authordto = new AuthorDto();
            if (authordto == null)
            {
                return NotFound();
            }
            authordto.AuthorId = id;
            authordto.Name = author.Name;
            authordto.Bio = author.Bio;


            return Ok(authordto);
        }



        /// <summary>
        /// To add an author in the library system
        /// </summary>
        /// <returns>the list of authors </returns>
        // POST: api/BookData/AddBook
        [ResponseType(typeof(Author))]
        [HttpPost]
        [Route("api/authordata/addauthor")]
        public IHttpActionResult AddAuthor(Author author)
        {
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
                Debug.WriteLine("Entering AddAuthor method");

                db.Authors.Add(author);
                Debug.WriteLine(author.Name);
                db.SaveChanges();
                Debug.WriteLine("author added with ID: " + author.Name);
                return Ok(author);
            }

        }
        [ResponseType(typeof(Author))]
        [HttpPost]
        public IHttpActionResult DeleteAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            db.Authors.Remove(author);
            db.SaveChanges();

            return Ok();
        }



        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/AuthorData/UpdateAuthor/{id}")]
        public IHttpActionResult UpdateAuthor(int id, Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.AuthorId)
            {

                return BadRequest();
            }

            db.Entry(author).State = EntityState.Modified;



            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }


        [HttpGet]
        [ResponseType(typeof(int))]
        [Route("api/authordata/countauthors")]
        public IHttpActionResult CountAuthors()
        {
            int authorCount = db.Authors.Count();
            Debug.WriteLine(authorCount);
            return Ok(authorCount);
        }
    }
}
