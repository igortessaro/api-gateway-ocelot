using CustomerApi.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IList<Customer> _customers = new List<Customer>()
        {
            new Customer(1, "Fulano", "Silva", new DateTime(1992, 10, 2)),
            new Customer(2, "Beltrano", "Santos", new DateTime(1990, 1, 10)),
            new Customer(3, "Ciclano", "Martins", new DateTime(1988, 12, 24)),
            new Customer(4, "Joao", "Cunha", new DateTime(1991, 5, 20)),
            new Customer(5, "Jose", "Machado", new DateTime(1999, 9, 5))
        };
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            this._logger = logger;
        }

        // GET: api/<CustomerController>
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return this._customers;
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public Customer? Get(int id)
        {
            return this._customers.FirstOrDefault(x => x.id == id);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public Customer Post([FromBody] Customer value)
        {
            var lastId = _customers.Max(x => x.id);
            var newCustomer = new Customer(lastId++, value.firstName, value.lastName, value.birthday);
            this._customers.Add(newCustomer);

            return newCustomer;
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public Customer? Put(int id, [FromBody] Customer value)
        {
            var customerRemove = this._customers.FirstOrDefault(x => x.id == id);

            if (customerRemove is null) { return null; }

            this._customers.Remove(customerRemove);
            var newCustomer = new Models.Customer(id, value.firstName, value.lastName, value.birthday);
            this._customers.Add(newCustomer);

            return newCustomer;
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var customerRemove = this._customers.FirstOrDefault(x => x.id == id);

            if (customerRemove is not null)
            {
                this._customers.Remove(customerRemove);
            }
        }
    }
}
