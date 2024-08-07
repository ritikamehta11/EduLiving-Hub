using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models.ViewModels
{
    public class UpdateBook
    {
        public BookDto SelectedBook { get; set; }


        // all species to choose from when updating this animal

        public IEnumerable<AuthorDto> AuthorOptions { get; set; }
        public IEnumerable<GenreDto> GenreOptions { get; set; }
    }
}