using Commons.Results;
using DataAccessLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IDataRepository
    {
        Task<IEnumerable<GenreVM>> GetGenresAsync();
        Task<IEnumerable<DeveloperVM>> GetDevelopersAsync();
        Task<IEnumerable<CatalogVM>> GetCatalogsAsync();
        Task<CatalogVM> GetCatalogByIdAsync(Guid Id);
        Task<Result<Guid>> StoreCatalogAsync(CatalogVM catalog);
        Task<Result<bool>> UpdateCatalogAsync(CatalogVM catalog);
    }
}