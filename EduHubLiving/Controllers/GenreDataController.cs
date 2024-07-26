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
    public class GenreDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [Route("api/GenreData/ListGenres")]
        public List<GenreDto> Listgenres()
        {

            List<Genre> genresList = db.Genres.ToList();

            List<GenreDto> genredtos = new List<GenreDto>();

            foreach (Genre genre in genresList)
            {

                GenreDto genredto = new GenreDto();
                genredto.GenreId = genre.GenreId;
                
                genredto.Name = genre.Name;


                genredtos.Add(genredto);

            }

            return genredtos;
        }


        // GET :api/GenreData/FindGenre/1
        [HttpGet]
        [Route("api/GenreData/FindGenre/{id}")]
        public IHttpActionResult FindGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            GenreDto genredto = new GenreDto();
            if (genredto == null)
            {
                return NotFound();
            }
            genredto.GenreId = genre.GenreId;
            genredto.Name = genre.Name;
            


            return Ok(genredto);
        }



        [ResponseType(typeof(Genre))]
        [HttpPost]
        [Route("api/genredata/addgenre")]
        public IHttpActionResult AddGenre(Genre genre)
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
                Debug.WriteLine("Entering AddGenre method");

                db.Genres.Add(genre);
                Debug.WriteLine(genre.Name);
                db.SaveChanges();
                Debug.WriteLine("Genre added with ID: " + genre.Name);
                return Ok(genre);
            }

        }

        [ResponseType(typeof(Genre))]
        [HttpPost]
        [Route("api/GenreData/DeleteGenre/{id}")]
        public IHttpActionResult DeleteGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }
            db.Genres.Remove(genre);
            db.SaveChanges();
            return Ok();

        }

        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/GenreData/UpdateGenre/{id}")]
        public IHttpActionResult UpdateGenre(int id, Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != genre.GenreId)
            {

                return BadRequest();
            }

            db.Entry(genre).State = EntityState.Modified;



            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [ResponseType(typeof(int))]
        [Route("api/genredata/countgenres")]
        public IHttpActionResult CountGenres()
        {
            int genreCount = db.Genres.Count();
            Debug.WriteLine(genreCount);
            return Ok(genreCount);
        }
    }
}
