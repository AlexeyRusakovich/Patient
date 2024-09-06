using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Patient.Api.Helpers;
using Patient.Api.Models;
using Patient.Data.Interfaces;
using System.Net;

namespace Patient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IRepository<Data.Models.Patient> _patientRepository;
        private readonly IByDateSearcher<Data.Models.Patient> _patientByDateSearcher;
        private readonly IMapper _mapper;

        public PatientController(
            IRepository<Data.Models.Patient> patientRepository,
            IByDateSearcher<Data.Models.Patient> patientByDateSearcher,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _patientByDateSearcher = patientByDateSearcher;
            _mapper = mapper;
        }

        /// <summary>
        /// You can get all patients
        /// </summary>
        /// <returns>List of all patients</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PatientDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<PatientDto>>(patients));
        }

        /// <summary>
        /// You can search patient by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns patient by id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PatientDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            var patient = await _patientRepository.GetById(id);
            var mapped = _mapper.Map<PatientDto>(patient);
            return Ok(mapped);
        }

        /// <summary>
        /// Search patients by birthday using dates in fhir date format
        /// </summary>
        /// <remarks>
        /// You should pass at least one and maximum two date parameters
        /// 
        /// I.e. query: ?date=gt2024-09-01
        /// </remarks>
        /// <param name="date"></param>
        /// <returns>List of patients</returns>
        [HttpGet("searchByDate")]
        [ProducesResponseType(typeof(IEnumerable<PatientDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPatientsByDate([FromQuery][FHIRDates] string[] date)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fhirDates = date
                .Where(x => x != null)
                .Select(x => x.ToFhirDate());

            var result = await _patientByDateSearcher.GetAllByDates(fhirDates);
            var mapped = _mapper.Map<IEnumerable<PatientDto>>(result);
            return Ok(mapped);
        }

        /// <summary>
        /// Create new patient
        /// </summary>
        /// <remarks>
        /// Do nothing if patient already exists.
        /// </remarks>
        /// <param name="patient"></param>
        /// <returns>200 code</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePatient(PatientDto patient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mapped = _mapper.Map<Data.Models.Patient>(patient);
            await _patientRepository.Create(mapped);
            await _patientRepository.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Updates patient if exists
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>200 code</returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdatePatient(PatientDto patient)
        {
            var mapped = _mapper.Map<Data.Models.Patient>(patient);
            await _patientRepository.Update(mapped);
            await _patientRepository.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Deletes patient if exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            await _patientRepository.Delete(id);
            await _patientRepository.SaveChanges();
            return Ok();
        }
    }
}
