using Microsoft.AspNetCore.Mvc;
using SubscriptionApi.Infrastructure.Data;
using SubscriptionApi.Models;

namespace SubscriptionApi.Controllers
{
    /// <summary>
    /// Controller for managing services in the subscription API.
    /// This controller exposes endpoints for retrieving available services.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicesController"/> class.
        /// </summary>
        /// <param name="context">The database context used for accessing the services data.</param>
        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all available services from the database.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Service"/> objects representing the available services.
        /// Returns a 200 OK response with the list of services.
        /// </returns>
        [HttpGet("services")]
        public ActionResult<Service[]> GetServices()
        {
            var services = _context.Services.ToArray();
            return Ok(services);
        }
    }
}
