using System;

namespace DataAccessLibrary.Models.ViewModels
{
    public class CatalogVM
    {
        public Guid ID { get; set; }

        public long ClusterID { get; set; }

        public string Title { get; set; }

        public int GenreID { get; set; }

        public string GenereTitle { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public Guid CompanyID { get; set; }

        public string CompanyName { get; set; }

        public decimal Price { get; set; }

        public int? Rate { get; set; }
    }
}
