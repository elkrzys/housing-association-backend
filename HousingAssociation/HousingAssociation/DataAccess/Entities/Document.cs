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
        
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public List<User> Receivers { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
        [Required] public string Filepath { get; set; }
        [Required] public string Md5 { get; set; }
        public int? DaysToExpire { get; set; }
        public DateTime? RemovesAt
        {
            get
            {
                if (DaysToExpire != null)
                {
                    return CreatedAt.AddDays(DaysToExpire.Value);
                }
                return null;
            }   
        }
  
    }
}