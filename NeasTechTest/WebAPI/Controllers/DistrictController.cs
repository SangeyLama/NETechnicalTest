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
        List<District> districts = new List<District>();
        DistrictDAO dDAO = new DistrictDAO();

        public DistrictController() { }

        public DistrictController(List<District> districts)
        {
            this.districts = districts;
        }

        public IEnumerable<District> GetAllDistricts()
        {
            return dDAO.GetAll();
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
            //if(district == null)
            //{
            //    return BadRequest("Invalid passed data");
            //}

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            int newId = 0;
            try
            {
                newId = dDAO.Insert(district);
            }
            catch (Exception e)
            {
                return BadRequest("e.message");
            }

            return CreatedAtRoute("DefaultApi", new { id = newId }, district);
        }

    }
}
