using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        [ForeignKey(nameof(User))]
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public List<User> Receivers { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public string Filepath { get; set; }
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