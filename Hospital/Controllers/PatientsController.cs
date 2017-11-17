using Hospital.Models;
using Hospital.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hospital.Controllers
{
    public class PatientsController : ApiController
    {
        public PatientsController(IHospitalResource resource)
        {
            _resource = resource;
        }
        // GET: api/Patients
        public IEnumerable<Patient> Get()
        {
            return _resource.Patients;
        }

        // GET: api/Patients/5
        public Patient Get(string name)
        {
            return _resource.GetPatient(name);
        }

        // POST: api/Patients
        public Patient Post([FromBody]Patient patient)
        {
            return _resource.AddPatient(patient);
        }

        //// PUT: api/Patients/5
        //public Patient Put(string name, [FromBody]Patient patient)
        //{
        //    return _resource.AddPatient(patient);
        //}

        // DELETE: api/Patients/5
        public Patient Delete(string name)
        {
            return _resource.DeletePatient(name);
        }

        private IHospitalResource _resource;
    }
}
