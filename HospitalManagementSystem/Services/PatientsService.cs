﻿using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Services
{
    public class PatientsService
    {
        private readonly HospitalDbContext _context;

        public PatientsService()
        {
            _context = new HospitalDbContext();
        }

        public List<Patient> GetPatients(string search = "")
        {
            var query = _context.Patients
                .Include(x => x.Appointments)
                .ThenInclude(a => a.Doctor)
                .Include(x => x.Appointments)
                .ThenInclude(x => x.Visit)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FirstName.Contains(search) ||
                    x.LastName.Contains(search) ||
                    x.PhoneNumber.Contains(search));
            }

            return query.ToList();
        }

        public Patient? GetPatientById(int id)
            => _context.Patients.FirstOrDefault(x => x.Id == id);

        public void Create(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

        public void Delete(Patient patient)
        {
            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }
    }
}
