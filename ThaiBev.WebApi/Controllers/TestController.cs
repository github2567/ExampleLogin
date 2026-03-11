using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaiBev.DAL.Common;
using ThaiBev.DAL.Data;
using ThaiBev.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ThaiBev.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly CustomerDAL _customer;
        public TestController(CustomerDAL customer)
        {
            _customer = customer;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetTestToken(int id)
        {
            try
            {
                var result = "Success";

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Customer")]
        public async Task<ActionResult<List<Customers>>> GetTestCustomer()
        {
            try
            {
                var customer = await _customer.GetCustomerAllList();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}