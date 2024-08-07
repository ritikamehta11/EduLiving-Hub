using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduHubLiving.Models.ViewModels
{
    public class CreateBook
    {
        public IEnumerable<AuthorDto> AuthorOptions { get; set; }
        public IEnumerable<GenreDto> GenreOptions { get; set; }
    }
}