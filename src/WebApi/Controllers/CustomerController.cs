using System;
using System.Linq;
using System.Net.Sockets;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("customers")]
    public class CustomerController : Controller
    {
        [HttpGet("{id:long}")]   
        public  IActionResult GetCustomerAsync([FromRoute] long id)
        {
            Customer customer = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                customer = db.Customers.Where(customer=>customer.Id==id).FirstOrDefault();

            }
            if (customer != null)
            {
                return  Ok(customer);
            }

            return NotFound();
        }

        [HttpPost("")]
        public IActionResult CreateCustomerAsync([FromBody] Customer customer)
        {
            var newCustomer = customer;
            using (ApplicationContext db = new ApplicationContext())
            {
                //if(db.Customers.Any(customerBd => customerBd.Id == customer.Id))
                //{
                //    return Ok(customer.Id);
                //}
                newCustomer = new Customer { Id = new Random().Next(0, int.MaxValue), Firstname = customer.Firstname, Lastname = customer.Lastname };
                db.Customers.Add(newCustomer);
                db.SaveChanges();

            }
            return Ok(newCustomer.Id.ToString());
            //return Conflict();

        }
      
    }
}