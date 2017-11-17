using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models;

namespace Hospital.Interfaces
{
    /// <summary>
    /// Hospital Data Repository
    /// </summary>
    public interface IHospitalResource
    {
        /// <summary>
        /// Get list of registred patients
        /// </summary>
        IEnumerable<Patient> Patients { get;}
        /// <summary>
        /// Get list of consulations
        /// </summary>
        IEnumerable<Consultation> Consultations { get; }

        /// <summary>
        /// Get patient
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Patient GetPatient(string name);
        /// <summary>
        /// Register a patient
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        Patient AddPatient(Patient patient);
        /// <summary>
        /// Delete a patient
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Patient DeletePatient(string name);
    }
}
