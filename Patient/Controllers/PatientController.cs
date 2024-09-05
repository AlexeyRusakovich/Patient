using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Patient.Api.Models;
using Patient.Data.Interfaces;

namespace Patient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IRepository<Data.Models.Patient> _patientRepository;
        private readonly IMapper _mapper;

        public PatientController(IRepository<Data.Models.Patient> patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
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
