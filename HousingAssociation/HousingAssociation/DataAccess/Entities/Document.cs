using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Entities
{
    public class Document
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; }
        [ForeignKey("Author")] public int AuthorId { get; set; }
        [Required] public DateTimeOffset Created { get; set; }
        [Required] public string Filepath { get; set; }
        [Required] public string Md5 { get; set; }
        public DateTimeOffset? Removes { get; set; }
        public User Author { get; set; }
        public List<User> Receivers { get; set; }
    }
}