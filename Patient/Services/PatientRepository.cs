﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Patient.Data.Context;
using Patient.Data.Interfaces;

namespace Patient.Api.Services
{
    public class PatientRepository : IRepository<Data.Models.Patient>
    {
        private readonly PatientContext _context;
        private readonly IMapper _mapper;

        public PatientRepository(PatientContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Create(Data.Models.Patient entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!await _context.Patients.AnyAsync(x => x.Id == entity.Id))
            {
                _context.Patients.Add(entity);
            }
        }

        public async Task Delete(Guid id)
        {
            var result = await _context.Patients.FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                _context.Patients.Remove(result);
            }
        }

        public async Task<IEnumerable<Data.Models.Patient>> GetAll()
        {
            return await _context.Patients
                .AsNoTracking()
                .Include(x => x.GivenNames)
                .ToListAsync();
        }

        public async Task Update(Data.Models.Patient entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = await _context.Patients
                .Include(x => x.GivenNames)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (result != null)
            {
                _mapper.Map(entity, result);
            }
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;  
        }

        public async Task<Data.Models.Patient> GetById(Guid id)
        {
            return await _context.Patients
                .AsNoTracking()
                .Include(x => x.GivenNames)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
