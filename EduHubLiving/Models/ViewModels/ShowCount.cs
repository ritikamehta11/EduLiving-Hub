using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models.ViewModels
{
    public class ShowCount
    {
        public IEnumerable<BookDto> Books { get; set; }
        public int totalBooks { get; set; }

        public IEnumerable<AuthorDto> Authors { get; set; }
        public int totalAuthors { get; set; }

        public IEnumerable<GenreDto> Genres { get; set; }

        public int totalGenres { get; set; }
    }
}