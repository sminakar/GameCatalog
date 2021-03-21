using AutoMapper;
using Commons.Results;
using DataAccess.DataContext;
using DataAccessLibrary.Models.Entities;
using DataAccessLibrary.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class DataRepository : IDataRepository
    {

        protected readonly CatalogContext _context;

        public DataRepository(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GenreVM>> GetGenresAsync()
        {
            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Genre, GenreVM>();
            }));

            return mapper.Map<IEnumerable<GenreVM>>(await _context.Genres
                .OrderBy(o => o.Title)
                .AsNoTracking()
                .ToListAsync());
        }

        public async Task<IEnumerable<DeveloperVM>> GetDevelopersAsync()
        {
            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Developer, DeveloperVM>();
            }));

            return mapper.Map<IEnumerable<DeveloperVM>>(await _context.Developers
                .OrderBy(o => o.CompanyName)
                .AsNoTracking()
                .ToListAsync());
        }

        public async Task<IEnumerable<CatalogVM>> GetCatalogsAsync()
        {
            var catalogs = await _context.Catalogs
                .Include(g => g.Genre)
                .Include(c => c.Company)
                .OrderBy(o => o.Title)
                .AsNoTracking()
                .ToListAsync();

            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Catalog, CatalogVM>()
                .ForPath(dest => dest.CompanyName, member => member.MapFrom(source => source.Company.CompanyName))
                .ForPath(dest => dest.CompanyID, member => member.MapFrom(source => source.Company.ID))
                .ForPath(dest => dest.GenreID, member => member.MapFrom(source => source.Genre.ID))
                .ForPath(dest => dest.GenereTitle, member => member.MapFrom(source => source.Genre.Title));
            }));

            return mapper.Map<IEnumerable<CatalogVM>>(catalogs);
        }

        public async Task<CatalogVM> GetCatalogByIdAsync(Guid Id)
        {
            var catalog = await _context.Catalogs
             .Include(g => g.Genre)
             .Include(c => c.Company)
             .AsNoTracking()
             .FirstOrDefaultAsync(c => c.ID == Id);


            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Catalog, CatalogVM>()
                .ForPath(dest => dest.CompanyName, member => member.MapFrom(source => source.Company.CompanyName))
                .ForPath(dest => dest.CompanyID, member => member.MapFrom(source => source.Company.ID))
                .ForPath(dest => dest.GenreID, member => member.MapFrom(source => source.Genre.ID))
                .ForPath(dest => dest.GenereTitle, member => member.MapFrom(source => source.Genre.Title));
            }));

            return mapper.Map<CatalogVM>(catalog);
        }

        public async Task<Result<Guid>> StoreCatalogAsync(CatalogVM catalog)
        {
            string errSource = "SotreCatalogAsync";

            Genre genre = await _context.Genres.FindAsync(catalog.GenreID);
            if (genre is null)
            {
                return new ErrorResult<Guid>(
                    code: ErrorCode.NotFound,
                    message: $"Could not find any Genre with Id {catalog.GenreID}",
                    errorSource: errSource);
            }

            Developer developer = await _context.Developers.FindAsync(catalog.CompanyID);
            if (developer is null)
            {
                return new ErrorResult<Guid>(
                    code: ErrorCode.NotFound,
                    message: $"Could not find any Game developer company with Id {catalog.CompanyID}",
                    errorSource: errSource);
            }

            Guid catalogID = Guid.NewGuid();

            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CatalogVM, Catalog>()
                .AfterMap((src, des) =>
                {
                    des.ID = catalogID;
                    des.Company = developer;
                    des.Genre = genre;
                });

            }));

            await _context.AddAsync<Catalog>(mapper.Map<Catalog>(catalog));
            await _context.SaveChangesAsync();

            return new Result<Guid>() { Data = catalogID };
        }

        public async Task<Result<bool>> UpdateCatalogAsync(CatalogVM catalogVM)
        {
            string errSource = "UpdateCatalogAsync";
    

            var dbCatalog = await _context.Catalogs
                .Include(g => g.Genre)
                .Include(c => c.Company)
                .FirstOrDefaultAsync(c => c.ID == catalogVM.ID);

            if (dbCatalog is null)
            {
                return new ErrorResult<bool>(
                    code: ErrorCode.NotFound,
                    message: $"Could not find any Game Catalog to update. CatalogID: {catalogVM.ID}",
                    errorSource: errSource);
            }

            Genre dbGenre = dbCatalog.Genre;
            Developer dbDevloper = dbCatalog.Company;


            if (dbCatalog.Company.ID != catalogVM.CompanyID)      // developer company changed
            {
                dbDevloper = await _context.FindAsync<Developer>(catalogVM.CompanyID);
                
                if (dbDevloper is null)
                {
                    return new ErrorResult<bool>(
                        code: ErrorCode.NotFound,
                        message: $"Could not find updated game developer company  ID: {catalogVM.CompanyID}",
                        errorSource: errSource);
                }

                dbCatalog.Company = dbDevloper;
            }

            if (dbCatalog.Genre.ID != catalogVM.GenreID)      // Catalog Genre changed
            {
                dbGenre = await _context.FindAsync<Genre>(catalogVM.GenreID);

                if (dbGenre is null)
                {
                    return new ErrorResult<bool>(
                        code: ErrorCode.NotFound,
                        message: $"Could not find updated game's Genre  ID: {catalogVM.GenreID}",
                        errorSource: errSource);
                }

                dbCatalog.Genre = dbGenre;
            }

            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CatalogVM, Catalog>()
                .AfterMap((src, des) =>
                { 
                    des.Company = dbDevloper;
                    des.Genre = dbGenre;
                    des.ModifyDate = DateTime.UtcNow;
                });
            }));

            var x = mapper.Map<Catalog>(catalogVM);
            _context.Entry(dbCatalog).CurrentValues.SetValues(x);

            return new Result<bool>() { Data = await _context.SaveChangesAsync() > 0 };
        }
    }
}
