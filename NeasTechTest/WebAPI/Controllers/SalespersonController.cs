using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAPI.Controllers
{
    public class SalespersonController : ApiController
    {
        SalespersonDAO spDAO = new SalespersonDAO();

        public SalespersonController() { }

        public IHttpActionResult GetAllSalespersons()
        {
            List<Salesperson> salespersons = spDAO.GetAll() as List<Salesperson>;
            if (salespersons == null)
            {
                return NotFound();
            }
            return Ok(salespersons);
        }

        public IHttpActionResult GetSalesperson(int id)
        {
            var salesperson = spDAO.GetById(id);
            if (salesperson == null)
            {
                return NotFound();
            }
            return Ok(salesperson);
        }

        [HttpPost]
        [ResponseType(typeof(Salesperson))]
        public IHttpActionResult PostSalesperson([FromBody]Salesperson salesperson)
        {
            if (salesperson == null)
            {
                return BadRequest("Invalid passed data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newId = 0;
            try
            {
                newId = spDAO.Insert(salesperson);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtRoute("DefaultApi", new { id = newId }, salesperson);
        }

        [HttpPut]
        [ResponseType(typeof(Salesperson))]
        public IHttpActionResult PutSalesperson([FromBody]Salesperson salesperson)
        {
            if (salesperson == null)
            {
                return BadRequest("Invalid passed data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                spDAO.Update(salesperson);
            }
            catch (Exception e)
            {
                return BadRequest("e.message");
            }

            return Content(HttpStatusCode.Accepted, salesperson);
        }

    }
}

