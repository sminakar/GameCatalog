using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Models.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ClusterID { get; set; }
    }
}
