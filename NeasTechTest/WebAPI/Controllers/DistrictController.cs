using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAPI.Controllers
{
    public class DistrictController : ApiController
    {
        DistrictDAO dDAO = new DistrictDAO();

        public DistrictController() { }

        public IHttpActionResult GetAllDistricts()
        {
            List<District> districts = dDAO.GetAll() as List<District>;
            if(districts == null)
            {
                return NotFound();
            }
            return Ok(districts);
        }

        public IHttpActionResult GetDistrict(int id)
        {
            var district = dDAO.GetByIdDataSet(id);
            if(district == null)
            {
                return NotFound();
            }
            return Ok(district);
        }

        [HttpPost]
        [ResponseType(typeof(District))]
        public IHttpActionResult PostDistrict([FromBody]District district)
        {
            if (district == null)
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
                newId = dDAO.Insert(district);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtRoute("DefaultApi", new { id = newId }, district);
        }

        [HttpPut]
        [ResponseType(typeof(District))]
        public IHttpActionResult PutDistrict([FromBody]District district)
        {
            if (district == null)
            {
                return BadRequest("Invalid passed data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int rowsAffected = 0;
            try
            {
                rowsAffected = dDAO.UpdateDS(district);
                if(district.Salespersons != null || district.Salespersons.Count() != 0)
                {
                    rowsAffected += dDAO.UpdateSalespersonsList(district);
                }                
            }
            catch (Exception e)
            {
                return BadRequest("e.message");
            }

            return Content(HttpStatusCode.Accepted, district);
        }

    }
}
