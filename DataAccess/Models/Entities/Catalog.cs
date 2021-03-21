using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataAccessLibrary.Models.Entities
{
    public sealed class Catalog : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        public Developer Company { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(18, 2)")]
        public decimal Price { get; set; }

        [Range(minimum:1, maximum:5)]
        [Column(TypeName = "tinyint")]
        public int? Rate { get; set; }

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifyDate { get; set; }
    }
}
