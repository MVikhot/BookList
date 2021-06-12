using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookList.Models
{
    public class Book
    {
         public string book_id { get; set; }
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(0, 999999999)]
        public string ISBN { get; set; }
        [Required]
        public Category BookCategory { get; set; }
    }
}
    public enum Category
    {
        Fantasy,
        Horror,
        Detective
    }



