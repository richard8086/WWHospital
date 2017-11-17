using Hospital.Implementation;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hospital.Controllers
{
    public class ConsultationsController : ApiController
    {
        public ConsultationsController(IHospitalResource resource)
        {
            _resource = resource;
        }

        // GET: api/Consultations
        public IEnumerable<Consultation> Get()
        {
            return _resource.Consultations;
        }

        //// GET: api/Consultations/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Consultations
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Consultations/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Consultations/5
        //public void Delete(int id)
        //{
        //}
        private IHospitalResource _resource;
    }
}
