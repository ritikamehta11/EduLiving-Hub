using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }


        //Book details
        public string Title { get; set; }
        public int PublishDate { get; set; }
        public string Plot { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //FOREIGN KEYS

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
    //DTO
    public class BookDto
    {

        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }
        public int PublishDate { get; set; }

        public string Plot { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public AuthorDto Author { get; set; }
        public GenreDto Genre { get; set; }




    }
}