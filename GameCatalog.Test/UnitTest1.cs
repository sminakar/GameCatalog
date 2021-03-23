//using System.Text.Json;
using DataAccess.DataContext;
using DataAccessLibrary;
using DataAccessLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Xunit;
using System.Text;

namespace GameCatalog.Test
{
    public class ApiUnitTest
    {
        private readonly HttpClient _client;
        private readonly string BaseURL;
        private readonly IDataRepository _repository;
        public ApiUnitTest()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient();
            BaseURL = "http://localhost:5165/api/v1/Catalog/";

            AppContextFactory _contextFactory = new AppContextFactory();
            CatalogContext _context = _contextFactory.CreateDbContext();
            _repository = new DataRepository(_context);
        }

        [Fact]
        public void RetriveAllGenresByAPI_ShouldReturn_AllGenres_InDB()
        {
            APIClient client = new APIClient(BaseURL + "Genres", string.Empty, "json", _client);
            client.CallApi();

            string result = client.Result.Result;
            JObject resObject = JObject.Parse(result);

            var apiResults = from d in resObject["data"]
                             select d;

            var dbGenres = _repository.GetGenresAsync().Result.ToList();

            int expected = dbGenres.Count();
            int actual = apiResults.Count();

            Assert.Equal(expected, actual);

            if (apiResults.Any())
            {
                foreach (var item in apiResults)
                {
                    GenreVM actualDta = JsonConvert.DeserializeObject<GenreVM>(item.ToString());
                    GenreVM expectedDta = dbGenres.FirstOrDefault(f => f.ID == actualDta.ID);

                    Assert.Equal(expectedDta.Title, actualDta.Title);
                }
            }
        }


        [Fact]
        public void RetriveAllDevelopersByAPI_ShouldReturn_AllMatchDevelopers_InDB()
        {
            PropertyInfo[] pinfo = (typeof(DeveloperVM)).GetProperties();
            APIClient client = new APIClient(BaseURL + "Companies", string.Empty, "json", _client);
            client.CallApi();

            string result = client.Result.Result;
            JObject resObject = JObject.Parse(result);

            var apiResults = from d in resObject["data"]
                             select d;

            var dbDevelopers = _repository.GetDevelopersAsync().Result.ToList();

            int expected = dbDevelopers.Count();
            int actual = apiResults.Count();

            Assert.Equal(expected, actual);

            if (apiResults.Any())
            {
                foreach (var item in apiResults)
                {
                    DeveloperVM actualDta = JsonConvert.DeserializeObject<DeveloperVM>(item.ToString());
                    DeveloperVM expectedDta = dbDevelopers.FirstOrDefault(f => f.ID == actualDta.ID);

                    foreach (var prop in pinfo)
                    {
                        var actualVal = prop.GetValue(actualDta);
                        var expectedVal = prop.GetValue(expectedDta);

                        Assert.Equal(expectedVal, actualVal);
                    }
                }
            }
        }

        [Fact]
        public void RetriveAllCatalogsByAPI_ShouldMatch_AllCatalog_InDB()
        {
            PropertyInfo[] pinfo = (typeof(CatalogVM)).GetProperties();
            APIClient client = new APIClient(BaseURL + "Catalogs", string.Empty, "json", _client);
            client.CallApi();

            string result = client.Result.Result;
            JObject resObject = JObject.Parse(result);

            var apiResults = from d in resObject["data"]
                             select d;

            var dbCatalogs = _repository.GetCatalogsAsync().Result.ToList();

            int expected = dbCatalogs.Count();
            int actual = apiResults.Count();

            Assert.Equal(expected, actual);

            if (apiResults.Any())
            {
                foreach (var item in apiResults)
                {
                    CatalogVM actualDta = JsonConvert.DeserializeObject<CatalogVM>(item.ToString());
                    CatalogVM expectedDta = dbCatalogs.FirstOrDefault(f => f.ID == actualDta.ID);

                    foreach (var prop in pinfo)
                    {
                        var actualVal = prop.GetValue(actualDta);
                        var expectedVal = prop.GetValue(expectedDta);

                        Assert.Equal(expectedVal, actualVal);
                    }
                }
            }
        }


        [Fact]
        public async void RetriveSingleCatalogsByAPI_WithoutID_Should_FaceWith404Async()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                BaseURL + "Catalog");

            HttpResponseMessage response = await _client.SendAsync(request);

            Assert.False(response.IsSuccessStatusCode, "Wrong Address");
        }

        [Fact]
        public async void RetriveSingleCatalogsByAPI_ShouldMatch_TheSameCatalog_InDbAsync()
        {
            PropertyInfo[] pinfo = (typeof(CatalogVM)).GetProperties();

            // select a random catalog record from database
            var dbCatalogs = _repository.GetCatalogsAsync().Result.ToList();
            int selectedIndex = new Random().Next(0, dbCatalogs.Count - 1);
            var expected = dbCatalogs[selectedIndex];

            // pass Id of above selected record as api parameter
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{BaseURL}Catalog/{expected.ID}");

            HttpResponseMessage response = await _client.SendAsync(request);

            Assert.True(response.IsSuccessStatusCode, "Wrong Response");

            var catalogDta = await response.Content.ReadAsStringAsync();
            dynamic catalogObj = JsonConvert.DeserializeObject(catalogDta);
            var catalogObj_data = catalogObj.data;

            CatalogVM actual = JsonConvert
                .DeserializeObject<CatalogVM>(catalogObj_data.ToString());

            foreach (var prop in pinfo)
            {
                var actualVal = prop.GetValue(actual);
                var expectedVal = prop.GetValue(expected);

                Assert.Equal(expectedVal, actualVal);
            }
        }

        [Fact]
        public async void RetriveSingleCatalogsByWrongParameter_ShouldResponse404DbAsync()
        {
            PropertyInfo[] pinfo = (typeof(CatalogVM)).GetProperties();

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{BaseURL}Catalog/{Guid.NewGuid()}");

            HttpResponseMessage response = await _client.SendAsync(request);

            Assert.False(response.IsSuccessStatusCode, "Wrong Parameter");
        }

        [Fact]
        public async void AddCatalogByAPI_ShouldAddADummyRecordAndThenDeleteItAsync()
        {
            // Store Dummy to database by API
            var catCompany = (await _repository.GetDevelopersAsync())
                    .First(c => c.ClusterID == 1);

            var catGenre = (await _repository.GetGenresAsync())
                .First(g => g.ID == 1);

            Guid id = Guid.NewGuid();

            CatalogVM catToAdd = new CatalogVM()
            {
                ID = id,
                Title = $"DummyTitle {id}",
                CompanyID = catCompany.ID,
                CompanyName = catCompany.CompanyName,
                GenreID = catGenre.ID,
                GenereTitle = catGenre.Title,
                Price = 10.0M,
                Rate = 5,
                ReleaseDate = DateTime.UtcNow
            };

            var catToAddJson = JsonConvert.SerializeObject(catToAdd);
            var content = new StringContent(catToAddJson,
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync($"{BaseURL}AddCatalog", content);

            System.Net.HttpStatusCode actual = response.StatusCode;
            Assert.Equal(System.Net.HttpStatusCode.OK, actual);

            string result = response.Content.ReadAsStringAsync().Result;
            string addedCatalogId = (JObject.Parse(result))["CId"].ToString();

            // Retrieve directly stored Dummy from db 
            CatalogVM dbCatalog = await _repository.GetCatalogByIdAsync(Guid.Parse(addedCatalogId));

            Assert.NotNull(dbCatalog);

            // Delete added Dummy from db
            _repository.DeleteCatalogByIdAsync(Guid.Parse(addedCatalogId));
        }


        [Fact]
        public async void AddCatalogByAPI_WithWronData_ShoulReturnBadRequestAsync()
        {
            // Store Dummy to database by API with Wrong Company Id

            var catCompany = (await _repository.GetDevelopersAsync())
                .First(c => c.ClusterID == 1);

            var catGenre = (await _repository.GetGenresAsync())
                .First(g => g.ID == 1);

            Guid id = Guid.NewGuid();

            CatalogVM catToAdd = new CatalogVM()
            {
                ID = id,
                Title = $"DummyTitle {id}",
                CompanyID = Guid.Empty,          // wrong Id
                CompanyName = catCompany.CompanyName,
                GenreID = catGenre.ID,
                GenereTitle = catGenre.Title,
                Price = 10.0M,
                Rate = 5,
                ReleaseDate = DateTime.UtcNow
            };

            var catToAddJson = JsonConvert.SerializeObject(catToAdd);
            var content = new StringContent(catToAddJson,
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync($"{BaseURL}AddCatalog", content);

            System.Net.HttpStatusCode actual = response.StatusCode;
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, actual);

            catToAdd.CompanyID = catCompany.ID;
            catToAdd.CompanyName = catCompany.CompanyName;

            catToAdd.GenreID = 0; // this time set request with wron GenreId

            response = await _client.PostAsync($"{BaseURL}AddCatalog", content);

            actual = response.StatusCode;
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, actual);

            catToAdd.GenreID = catGenre.ID;

            catToAdd.Title = String.Empty;  // this time send the request with wrong game title

            response = await _client.PostAsync($"{BaseURL}AddCatalog", content);

            actual = response.StatusCode;
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, actual);
        }


        [Fact]
        public async void TryToUpdateACatalogWithWrongCompanyIDByAPI_ShouldReturnBadRequestAsync()
        {
            var catalogToUpdate = _repository.GetCatalogsAsync()
                .Result
                .FirstOrDefault(c => c.ClusterID == 1);     // retrieve a catalog

            catalogToUpdate.ID = Guid.Empty;        // assign a wrong ID

            var catToUpdateJson = JsonConvert.SerializeObject(catalogToUpdate);
            var content = new StringContent(catToUpdateJson,
                Encoding.UTF8,
                "application/json");

            var response = await _client.PutAsync($"{BaseURL}UpdateCatalog", content);

            System.Net.HttpStatusCode actual = response.StatusCode;
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, actual);
        }


        [Theory]
        [InlineData("title", "Title to Update")]
        [InlineData("rate", 3)]
        [InlineData("price", 12.66)]
        [InlineData("genreID", 1)]
        [InlineData("companyID", "EED48690-A564-4208-9FC6-16610270BE8E")]
        
        public async void UpdateACatalogsByAPI_ShouldReturnOkAsync(string fieldName, object newFieldValue)
        {
          
            PropertyInfo[] pinfo = (typeof(CatalogVM)).GetProperties();

            // select a random catalog record from database
            var dbCatalogs = _repository.GetCatalogsAsync().Result.ToList();
            int selectedIndex = new Random().Next(0, dbCatalogs.Count - 1);
            var catToUpdate = dbCatalogs[selectedIndex];
            object storedFieldValue = null;

            foreach(var property in pinfo)
            {
                if (property.Name.ToUpper() == fieldName.ToUpper())
                {
                    PropertyInfo prop = catToUpdate.GetType().GetProperty((property.Name));
                    storedFieldValue = prop.GetValue(catToUpdate);    // keep the current field value
                    
                    switch (prop.PropertyType.Name)
                    {
                        case "Decimal":
                            prop.SetValue(catToUpdate, decimal.Parse(newFieldValue.ToString()));
                            break;

                        case "Guid":
                            prop.SetValue(catToUpdate, Guid.Parse(newFieldValue.ToString()));
                            break;

                        default:
                            prop.SetValue(catToUpdate, newFieldValue);
                            break;
                    }
                }
            }

            var catToUpdateJson = JsonConvert.SerializeObject(catToUpdate);
            var content = new StringContent(catToUpdateJson,
                Encoding.UTF8,
                "application/json");

            var response = await _client.PutAsync($"{BaseURL}UpdateCatalog", content);

            System.Net.HttpStatusCode actual = response.StatusCode;
            Assert.Equal(System.Net.HttpStatusCode.OK, actual);

            // check if database recored is changed
            var dbfCatalog = await _repository.GetCatalogByIdAsync(catToUpdate.ID);
            var expected = newFieldValue;
            object actualInDb = null;

            foreach (var property in pinfo)
            {
                if (property.Name.ToUpper() == fieldName.ToUpper())
                {
                    actualInDb = property.GetValue(dbfCatalog);
                    Assert.Equal(expected.ToString().ToUpper(), actualInDb.ToString().ToUpper());

                    break;
                }
            }

            // Restore field value to its original
            foreach (var property in pinfo)
            {
                if (property.Name.ToUpper() == fieldName.ToUpper())
                {
                    switch (property.PropertyType.Name)
                    {
                        case "Decimal":
                            property.SetValue(catToUpdate, decimal.Parse(storedFieldValue.ToString()));
                            break;

                        case "Guid":
                            property.SetValue(catToUpdate, Guid.Parse(storedFieldValue.ToString()));
                            break;

                        default:
                            property.SetValue(catToUpdate, newFieldValue);
                            break;
                    }

                    break;
                }
            }

            catToUpdateJson = JsonConvert.SerializeObject(catToUpdate);
            content = new StringContent(catToUpdateJson,
                Encoding.UTF8,
                "application/json");

            await _client.PutAsync($"{BaseURL}UpdateCatalog", content);
        }
    }
}

