using Commons.Results;
using DataAccessLibrary;
using DataAccessLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Controllers.API.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class CatalogController : ControllerBase
    {
        protected readonly IDataRepository _repository;
        public CatalogController(IDataRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("Genres")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGenresAsync()
        {
            IEnumerable<GenreVM> genres = await _repository.GetGenresAsync();

            if (genres is null)
            {
                return NotFound(new Commons.Results.ErrorResult<IActionResult>(
                    ErrorCode.NullValue,
                    "Could not retrieve any Genre data from database.",
                    "Genres API-v1.0"));
            }


            return Ok(new Result<IEnumerable<GenreVM>> { Data = genres });
        }

        [HttpGet("Companies")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDevelopersAsync()
        {
            IEnumerable<DeveloperVM> developers = await _repository.GetDevelopersAsync();

            if (developers is null)
            {
                return NotFound(new ErrorResult<IActionResult>(
                    ErrorCode.NullValue,
                    "Could not retrieve any Game developer data from database.",
                    "Copmanies API-v1.0"));
            }

            return Ok(new Result<IEnumerable<DeveloperVM>> { Data = developers });
        }

        [HttpGet("Catalogs")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCatalogsAsync()
        {
            IEnumerable<CatalogVM> catalogs = await _repository.GetCatalogsAsync();

            if (catalogs is null)
            {
                return NotFound(new ErrorResult<IActionResult>(
                    ErrorCode.NullValue,
                    "Could not retrieve any Game Catalog data from database.",
                    "Catalogs API-v1.0"));
            }

            return Ok(new Result<IEnumerable<CatalogVM>> { Data = catalogs });
        }

        [HttpGet("Catalog/{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCatalogsAsync(Guid Id)
        {
            CatalogVM catalog = await _repository.GetCatalogByIdAsync(Id);

            if (catalog is null)
            {
                return NotFound(new ErrorResult<IActionResult>(
                    ErrorCode.NotFound,
                    $"Could not find any Game Catalog data with ID {Id} .",
                    "Catalogs API-v1.0"));
            }

            return Ok(new Result<CatalogVM> { Data = catalog });
        }

        [HttpPost("AddCatalog")]
        [MapToApiVersion("1.0")]
        public IActionResult AddCatalogsAsync([FromBody]CatalogVM catalog)
        {
            Guid companyId;

            if (String.IsNullOrEmpty(catalog.Title) ||
                catalog.GenreID == 0 ||
                !Guid.TryParse(catalog.CompanyID.ToString(), out companyId) ||
                catalog.Price <= 0)
            {
                return new BadRequestResult();
            }

            var addResult = _repository.StoreCatalogAsync(catalog).Result;
            
            if (addResult.IsFailure)
            {
                // logger should log the error here

                return new BadRequestResult();
            }

            return Ok(addResult.Data);
        }

        [HttpPut("UpdateCatalog")]
        [MapToApiVersion("1.0")]
        public IActionResult UpdateCatalogsAsync([FromBody] CatalogVM catalog)
        {
            var updateResult = _repository.UpdateCatalogAsync(catalog).Result;

            if (updateResult.IsFailure)
            {
                // logger should log the error here

                return new BadRequestResult();
            }

            return Ok();
        }
    }
}
