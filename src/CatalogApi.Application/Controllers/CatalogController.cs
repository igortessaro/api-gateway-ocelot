using CatalogApi.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CatalogApi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IList<Catalog> _catalogs = new List<Catalog>()
        {
            new Catalog(1, "Catálogo 1"),
            new Catalog(2, "Catálogo 2"),
            new Catalog(3, "Catálogo 3"),
            new Catalog(4, "Catálogo 4"),
            new Catalog(5, "Catálogo 5")
        };
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ILogger<CatalogController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        public IEnumerable<Catalog> Get()
        {
            return this._catalogs;
        }

        [HttpGet("{id}")]
        public Catalog? Get(int id)
        {
            return this._catalogs.FirstOrDefault(x => x.Id == id);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public Catalog Post([FromBody] Catalog value)
        {
            var lastId = _catalogs.Max(x => x.Id);
            var newCustomer = new Catalog(lastId++, value.Name);
            this._catalogs.Add(newCustomer);

            return newCustomer;
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public Catalog? Put(int id, [FromBody] Catalog value)
        {
            var catolgToRemove = this._catalogs.FirstOrDefault(x => x.Id == id);

            if (catolgToRemove is null)
            {
                return null;
            }

            this._catalogs.Remove(catolgToRemove);
            var newCustomer = new Catalog(id, value.Name);
            this._catalogs.Add(newCustomer);

            return newCustomer;
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var catolgToRemove = this._catalogs.FirstOrDefault(x => x.Id == id);

            if (catolgToRemove is not null)
            {
                this._catalogs.Remove(catolgToRemove);
            }
        }

        [HttpGet("customer")]
        public async Task<IActionResult> GetAsync()
        {
            this._logger.LogInformation("Getting customers from customer-api");

            try
            {
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://customer-api");

                this._logger.LogInformation("Getting customers from customer-api {url}", httpClient.BaseAddress);

                var message = await httpClient.GetAsync("api/customer");
                var response = await message.Content.ReadAsStringAsync();

                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error getting customers from customer-api");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("customer/{port}")]
        public async Task<IActionResult> GetCustomAsync(int port, [FromBody] ApiConfig apiConfig)
        {
            this._logger.LogInformation("Getting customers from customer-api {port} and {apiConfig}", port, apiConfig);

            try
            {
                var handler = new HttpClientHandler();

                handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, errors) => true;
                using var httpClient = new HttpClient(handler);
                var protocol = apiConfig.IsHttps ? "https" : "http";
                var host = apiConfig.UsePort ? $"{apiConfig.Host}:{apiConfig.Port}" : apiConfig.Host;
                httpClient.BaseAddress = new Uri($"{protocol}://{host}");

                this._logger.LogInformation("Getting customers from customer-api {url}", httpClient.BaseAddress);

                var message = await httpClient.GetAsync(apiConfig.EndPoint);
                var response = await message.Content.ReadAsStringAsync();

                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error getting customers from customer-api");
                return BadRequest(ex.Message);
            }
        }
    }

    public sealed class ApiConfig
    {
        public ApiConfig()
        {
            this.Port = 80;
            this.IsHttps = false;
            this.Host = "localhost";
            this.EndPoint = "api/customer";
            this.UsePort = false;
        }

        public int Port { get; set; }
        public bool IsHttps { get; set; }
        public string Host { get; set; }
        public string EndPoint { get; set; }
        public bool UsePort { get; set; }
    }
}
