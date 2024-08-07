using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models.ViewModels
{
    public class DetailBook
    {
        public BookDto SelectedBook { get; set; }
        public IEnumerable<AuthorDto> Writer { get; set; }

        public IEnumerable<GenreDto> Type { get; set; }
    }
}