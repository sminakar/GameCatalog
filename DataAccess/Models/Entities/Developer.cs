using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Models.Entities
{
    public sealed class Developer : BaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string CompanyName { get; set; }

        [MaxLength(25)]
        public string Country { get; set; }
        
        public int? EstablishYear { get; set; }

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifyDate { get; set; }
    }
}
