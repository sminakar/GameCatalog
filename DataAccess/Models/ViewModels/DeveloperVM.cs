using System;

namespace DataAccessLibrary.Models.ViewModels
{
    public class DeveloperVM
    {
        public long ClusterID { get; set; }

        public Guid ID { get; set; }

        public string CompanyName { get; set; }

        public string Country { get; set; }

        public int? EstablishYear { get; set; }
    }
}
