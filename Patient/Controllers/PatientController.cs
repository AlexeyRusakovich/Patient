using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Patient.Api.Helpers;
using Patient.Api.Models;
using Patient.Data.Interfaces;

namespace Patient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IRepository<Data.Models.Patient> _patientRepository;
        private readonly IByExpressionSearcher<Data.Models.Patient> _patientByDateSearcher;
        private readonly IMapper _mapper;

        public PatientController(
            IRepository<Data.Models.Patient> patientRepository,
            IByExpressionSearcher<Data.Models.Patient> patientByDateSearcher,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _patientByDateSearcher = patientByDateSearcher;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<PatientDto>>(patients));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            var patient = await _patientRepository.GetById(id);
            var mapped = _mapper.Map<PatientDto>(patient);
            return Ok(mapped);
        }

        [HttpGet("searchByDate")]
        public async Task<IActionResult> GetPatientsByDate([FromQuery][FHIRDate] string[] date)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var searchRequest = new DateSearchRequest { Date1 = date[0], Date2 = date.Length > 1 ? date[1] : null };
            var fhirDates = new[] { searchRequest.Date1, searchRequest.Date2 }
                .Select(x => x?.ToFhirDate())
                .Where(x => x != null);

            var fhirExpressions = fhirDates.Select(x => x.ToExpression()).ToList();

            var result = await _patientByDateSearcher.GetAllByDateExpressions(fhirExpressions);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient(PatientDto patient)
        {
            var mapped = _mapper.Map<Data.Models.Patient>(patient);
            await _patientRepository.Create(mapped);
            await _patientRepository.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePatient(PatientDto patient)
        {
            var mapped = _mapper.Map<Data.Models.Patient>(patient);
            await _patientRepository.Update(mapped);
            await _patientRepository.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            await _patientRepository.Delete(id);
            await _patientRepository.SaveChanges();
            return Ok();
        }
    }
}
