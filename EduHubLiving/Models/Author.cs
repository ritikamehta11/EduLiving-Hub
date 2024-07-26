using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //an author can have many books written by him/her
        public ICollection<Book> Books { get; set; }
    }
    public class AuthorDto
    {
        [Key]
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public ICollection<Book> Books { get; set; }

    }
}